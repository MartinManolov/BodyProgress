using System.Security.Cryptography.X509Certificates;

namespace BodyProgress.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BodyProgress.Data.Common.Repositories;
    using BodyProgress.Data.Models;
    using BodyProgress.Web.ViewModels;
    using BodyProgress.Web.ViewModels.ViewInputModels;

    public class WorkoutsService : IWorkoutsService
    {
        private readonly IDeletableEntityRepository<Workout> _workoutRepository;
        private readonly IDeletableEntityRepository<Exercise> _exerciseRepository;
        private readonly IDeletableEntityRepository<Set> _setsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> _usersRepository;

        public WorkoutsService(IDeletableEntityRepository<Workout> workoutRepository,
            IDeletableEntityRepository<Exercise> exerciseRepository,
            IDeletableEntityRepository<Set> setsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this._workoutRepository = workoutRepository;
            this._exerciseRepository = exerciseRepository;
            this._setsRepository = setsRepository;
            this._usersRepository = usersRepository;
        }

        public async Task Create(AddWorkoutInputModel input, string userId)
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
            var user = this._usersRepository.All().FirstOrDefault(x => x.Id == userId);
            user.Workouts.Add(workout);

            await this._exerciseRepository.SaveChangesAsync();
            await this._setsRepository.SaveChangesAsync();
            await this._workoutRepository.SaveChangesAsync();
            await this._usersRepository.SaveChangesAsync();
        }

        public ICollection<WorkoutViewModel> All(string userId)
        {
            var workoutsFromDb = this._workoutRepository.AllAsNoTracking()
                .Where(x => x.OwnerId == userId)
                .OrderByDescending(x => x.Date);

            return workoutsFromDb.Select(workoutDb => new WorkoutViewModel()
                {
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
    }
}
