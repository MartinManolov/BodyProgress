using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
using BodyProgress.Web.ViewModels;
using BodyProgress.Web.ViewModels.ViewInputModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BodyProgress.Services.Data.Tests
{
    public class WorkoutsServiceTests
    {
        [Fact]
        public async Task CreateWorkoutTest()
        {
            var testWorkoutsData = new List<Workout>();
            var testExercisesData = new List<Exercise>();
            var testSetsData = new List<Set>();

            var workoutsRepo = new Mock<IDeletableEntityRepository<Workout>>();
            var exercisesRepo = new Mock<IDeletableEntityRepository<Exercise>>();
            var setsRepo = new Mock<IDeletableEntityRepository<Set>>();

            exercisesRepo.Setup(r => r.All())
                .Returns(testExercisesData.AsQueryable);
            exercisesRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>()))
                .Callback(() => testExercisesData.Add(It.IsAny<Exercise>()));

            setsRepo.Setup(r => r.AddAsync(It.IsAny<Set>()))
                .Callback(() => testSetsData.Add(It.IsAny<Set>()));

            workoutsRepo.Setup(r => r.AddAsync(It.IsAny<Workout>()))
                .Callback(() => testWorkoutsData.Add(It.IsAny<Workout>()));

            var input = new WorkoutInputModel
            {
                WorkoutName = "Chest",
                Date = DateTime.UtcNow,
                Sets = new List<SetInputModel>
                {
                    new SetInputModel
                    {
                        ExerciseName = "Bench press",
                        Reps = 8,
                        Weight = 100,
                    },
                },
            };

            var service = new WorkoutsService(workoutsRepo.Object, exercisesRepo.Object, setsRepo.Object);
            await service.Create(input, "user1");

            Assert.Single(testWorkoutsData);
            Assert.Single(testExercisesData);
            Assert.Single(testSetsData);
        }

        [Fact]
        public void AllWorkoutByUserIdTest()
        {
            var testWorkoutsData = new List<Workout>
            {
                new Workout
                {
                    Id = "workoutId",
                    Name = "Chest",
                    Date = DateTime.UtcNow,
                    OwnerId = "ownerId",
                    Sets = new List<Set>
                    {
                        new Set
                        {
                            WorkoutId = "workoutId",
                            Exercise = new Exercise
                            {
                                Name = "bench press",
                            },
                            Reps = 10,
                            Weight = 100,
                        },
                    },
                },
            };
            var workoutsRepo = new Mock<IDeletableEntityRepository<Workout>>();
            var exercisesRepo = new Mock<IDeletableEntityRepository<Exercise>>();
            var setsRepo = new Mock<IDeletableEntityRepository<Set>>();

            workoutsRepo.Setup(r => r.AllAsNoTracking())
                            .Returns(testWorkoutsData.AsQueryable);

            var service = new WorkoutsService(workoutsRepo.Object, exercisesRepo.Object, setsRepo.Object);
            var workouts1 = service.All("ownerId");
            var workouts2 = service.All("someId");

            Assert.Single(workouts1);
            Assert.Empty(workouts2);
        }

        [Fact]
        public async Task DeleteWorkoutByIdTest()
        {
            var testWorkoutsData = new List<Workout>
            {
                new Workout
                {
                    Id = "workoutId",
                    Name = "Chest",
                    Date = DateTime.UtcNow,
                    OwnerId = "ownerId",
                    Sets = new List<Set>
                    {
                        new Set
                        {
                            WorkoutId = "workoutId",
                            Exercise = new Exercise
                            {
                                Name = "bench press",
                            },
                            Reps = 10,
                            Weight = 100,
                        },
                    },
                },
            };
            var testSetsData = new List<Set>();
            var workoutsRepo = new Mock<IDeletableEntityRepository<Workout>>();
            var exercisesRepo = new Mock<IDeletableEntityRepository<Exercise>>();
            var setsRepo = new Mock<IDeletableEntityRepository<Set>>();

            workoutsRepo.Setup(r => r.All())
                            .Returns(testWorkoutsData.AsQueryable);
            workoutsRepo.Setup(r => r.Delete(It.IsAny<Workout>()))
                .Callback(() => testWorkoutsData[0].IsDeleted = true);
            setsRepo.Setup(r => r.All())
                            .Returns(testSetsData.AsQueryable);
            setsRepo.Setup(r => r.Delete(It.IsAny<Set>()))
                .Callback(() => testSetsData[0].IsDeleted = true);

            var service = new WorkoutsService(workoutsRepo.Object, exercisesRepo.Object, setsRepo.Object);
            await service.Delete("workoutId");

            Assert.True(testWorkoutsData[0].IsDeleted);
        }

    }
}
