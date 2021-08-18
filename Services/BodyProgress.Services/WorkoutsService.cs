namespace BodyProgress.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Services.Contracts;
    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public class WorkoutsService : IWorkoutsService
    {
        private readonly IDeletableEntityRepository<Workout> workoutRepository;
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;
        private readonly IDeletableEntityRepository<Set> setsRepository;

        public WorkoutsService(IDeletableEntityRepository<Workout> workoutRepository,
            IDeletableEntityRepository<Exercise> exerciseRepository,
            IDeletableEntityRepository<Set> setsRepository)
        {
            this.workoutRepository = workoutRepository;
            this.exerciseRepository = exerciseRepository;
            this.setsRepository = setsRepository;
        }

        public async Task Create(WorkoutInputModel input, string userId)
        {
            var workout = new Workout()
            {
                Name = input.WorkoutName,
                Date = input.Date,
                OwnerId = userId,
            };

            foreach (var set in input.Sets)
            {
                if (set.ExerciseName == null)
                {
                    continue;
                }

                var exercise = this.exerciseRepository.All()
                    .FirstOrDefault(x => x.Name == set.ExerciseName);
                if (exercise == null)
                {
                    exercise = new Exercise() { Name = set.ExerciseName };
                    await this.exerciseRepository.AddAsync(exercise);
                }

                var serie = new Set()
                {
                    WorkoutId = workout.Id,
                    ExerciseId = exercise.Id,
                    Weight = set.Weight,
                    Reps = set.Reps,
                };
                workout.Sets.Add(serie);
                await this.setsRepository.AddAsync(serie);
            }

            await this.workoutRepository.AddAsync(workout);
            await this.exerciseRepository.SaveChangesAsync();
            await this.setsRepository.SaveChangesAsync();
            await this.workoutRepository.SaveChangesAsync();
        }

        public ICollection<WorkoutViewModel> All(string userId)
        {
            var workoutsFromDb = this.workoutRepository.AllAsNoTracking()
                .Where(x => x.OwnerId == userId)
                .OrderByDescending(x => x.Date);

            return workoutsFromDb.Select(workoutDb => new WorkoutViewModel()
                {
                    Id = workoutDb.Id,
                    Name = workoutDb.Name,
                    Date = workoutDb.Date,
                    Sets = workoutDb.Sets.Select(x => new SetViewModel()
                    {
                        ExerciseName = x.Exercise.Name,
                        Reps = x.Reps,
                        Weight = x.Weight,
                    }).ToList(),
                    AllWeightForWorkout = workoutDb.Sets.Select(x => x.Reps * x.Weight).Sum(),
                })
                .ToList();
        }

        public async Task Delete(string workoutId)
        {
            var workout = this.workoutRepository.All().FirstOrDefault(x => x.Id == workoutId);
            if (workout == null)
            {
                return;
            }

            this.workoutRepository.Delete(workout);
            var sets = this.setsRepository.All().Where(x => x.WorkoutId == workoutId).ToList();
            foreach (var set in sets)
            {
                this.setsRepository.Delete(set);
            }

            await this.workoutRepository.SaveChangesAsync();
            await this.setsRepository.SaveChangesAsync();
        }
    }
}
