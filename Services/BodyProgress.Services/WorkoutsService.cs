using System.Security.Cryptography.X509Certificates;

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
        private readonly IDeletableEntityRepository<Workout> _workoutRepository;
        private readonly IDeletableEntityRepository<Exercise> _exerciseRepository;
        private readonly IDeletableEntityRepository<Set> _setsRepository;

        public WorkoutsService(IDeletableEntityRepository<Workout> workoutRepository,
            IDeletableEntityRepository<Exercise> exerciseRepository,
            IDeletableEntityRepository<Set> setsRepository)
        {
            this._workoutRepository = workoutRepository;
            this._exerciseRepository = exerciseRepository;
            this._setsRepository = setsRepository;
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

                var exercise = this._exerciseRepository.All()
                    .FirstOrDefault(x => x.Name == set.ExerciseName);
                if (exercise == null)
                {
                    exercise = new Exercise() { Name = set.ExerciseName };
                    await this._exerciseRepository.AddAsync(exercise);
                }

                var serie = new Set()
                {
                    WorkoutId = workout.Id,
                    ExerciseId = exercise.Id,
                    Weight = set.Weight,
                    Reps = set.Reps,
                };
                workout.Sets.Add(serie);
                await this._setsRepository.AddAsync(serie);
            }

            await this._workoutRepository.AddAsync(workout);
            await this._exerciseRepository.SaveChangesAsync();
            await this._setsRepository.SaveChangesAsync();
            await this._workoutRepository.SaveChangesAsync();
        }

        public ICollection<WorkoutViewModel> All(string userId)
        {
            var workoutsFromDb = this._workoutRepository.AllAsNoTracking()
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
            var workout = this._workoutRepository.All().FirstOrDefault(x => x.Id == workoutId);
            if (workout == null)
            {
                return;
            }

            this._workoutRepository.Delete(workout);
            var sets = this._setsRepository.All().Where(x => x.WorkoutId == workoutId).ToList();
            foreach (var set in sets)
            {
                this._setsRepository.Delete(set);
            }

            await this._workoutRepository.SaveChangesAsync();
            await this._setsRepository.SaveChangesAsync();
        }
    }
}
