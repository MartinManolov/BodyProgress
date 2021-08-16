using BodyProgress.Data.Common.Repositories;
using BodyProgress.Data.Models;
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
    public class MealsServiceTests
    {
        [Fact]
        public async Task CreateMealTest()
        {
            var testFoodsData = new List<Food>();
            var testfoodsMealsQuantityData = new List<FoodsMealsQuantity>();
            var testMealsData = new List<Meal>();

            var mealsRepo = new Mock<IDeletableEntityRepository<Meal>>();
            var foodsMealsQuantityRepo = new Mock<IDeletableEntityRepository<FoodsMealsQuantity>>();
            var foodRepo = new Mock<IDeletableEntityRepository<Food>>();

            foodRepo.Setup(r => r.All())
                .Returns(testFoodsData.AsQueryable);
            foodRepo.Setup(r => r.AddAsync(It.IsAny<Food>()))
                .Callback(() => testFoodsData.Add(It.IsAny<Food>()));

            foodsMealsQuantityRepo.Setup(r => r.AddAsync(It.IsAny<FoodsMealsQuantity>()))
                .Callback(() => testfoodsMealsQuantityData.Add(It.IsAny<FoodsMealsQuantity>()));

            mealsRepo.Setup(r => r.AddAsync(It.IsAny<Meal>()))
                .Callback(() => testMealsData.Add(It.IsAny<Meal>()));

            MealInputModel input = new MealInputModel
            {
                Date = DateTime.UtcNow,
                MealName = "test",
                FoodsWithQuantities = new List<FoodWithQuantityInputModel>
                {
                    new FoodWithQuantityInputModel
                    {
                        FoodName = "Pizza",
                        Quantity = 300,
                        KcalPer100g = 400,
                        CarbsPer100g = 25,
                        ProteinPer100g = 4,
                        FatPer100g = 6,
                    },
                },
            };

            var service = new MealsService(mealsRepo.Object, foodsMealsQuantityRepo.Object, foodRepo.Object);
            await service.Create(input, "testId");

            Assert.Single(testFoodsData);
            Assert.Single(testfoodsMealsQuantityData);
            Assert.Single(testMealsData);
        }

        [Fact]
        public void AllMealsByUserIdTest()
        {
            var testMealsData = new List<Meal>
            {
                new Meal
                {
                    Id = "Id1",
                    Name = "test1",
                    Date = DateTime.UtcNow,
                    OwnerId = "owner1",
                    FoodsMealsQuantities = new List<FoodsMealsQuantity>
                    {
                        new FoodsMealsQuantity
                        {
                           MealId = "Id1",
                           Quantity = 400,
                           Food = new Food
                           {
                               Name = "pizza",
                               KcalPer100G = 100,
                               CarbsPer100G = 30,
                               ProteinPer100G = 5,
                               FatPer100G = 5,
                           },
                        },
                    },
                },
            };

            var mealsRepo = new Mock<IDeletableEntityRepository<Meal>>();
            var foodsMealsQuantityRepo = new Mock<IDeletableEntityRepository<FoodsMealsQuantity>>();
            var foodRepo = new Mock<IDeletableEntityRepository<Food>>();

            mealsRepo.Setup(r => r.AllAsNoTracking())
                .Returns(testMealsData.AsQueryable);

            var service = new MealsService(mealsRepo.Object, foodsMealsQuantityRepo.Object, foodRepo.Object);
            var meals1 = service.All("owner1");
            var meals2 = service.All("testId");

            Assert.Single(meals1);
            Assert.Equal(0, meals2.Count);
        }

        [Fact]
        public async Task DeleteMealByIdTest()
        {
            var testMealsData = new List<Meal>
            {
                new Meal
                {
                    Id = "Id1",
                    Name = "test1",
                    Date = DateTime.UtcNow,
                    OwnerId = "owner1",
                    FoodsMealsQuantities = new List<FoodsMealsQuantity>
                    {
                        new FoodsMealsQuantity
                        {
                           MealId = "Id1",
                           Quantity = 400,
                           Food = new Food
                           {
                               Name = "pizza",
                               KcalPer100G = 100,
                               CarbsPer100G = 30,
                               ProteinPer100G = 5,
                               FatPer100G = 5,
                           },
                        },
                    },
                },
            };

            var mealsRepo = new Mock<IDeletableEntityRepository<Meal>>();
            var foodsMealsQuantityRepo = new Mock<IDeletableEntityRepository<FoodsMealsQuantity>>();
            var foodRepo = new Mock<IDeletableEntityRepository<Food>>();

            mealsRepo.Setup(r => r.All())
                .Returns(testMealsData.AsQueryable);
            mealsRepo.Setup(r => r.Delete(It.IsAny<Meal>()))
                .Callback(() => testMealsData[0].IsDeleted = true);

            var service = new MealsService(mealsRepo.Object, foodsMealsQuantityRepo.Object, foodRepo.Object);
            await service.Delete("Id1");

            Assert.True(testMealsData[0].IsDeleted);
        }
    }
}
