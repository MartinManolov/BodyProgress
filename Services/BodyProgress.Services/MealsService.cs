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

    public class MealsService : IMealsService
    {
        private readonly IDeletableEntityRepository<Meal> mealsRepository;
        private readonly IDeletableEntityRepository<FoodsMealsQuantity> foodsMealsQuantityRepository;
        private readonly IDeletableEntityRepository<Food> foodRepository;

        public MealsService(IDeletableEntityRepository<Meal> mealsRepository,
            IDeletableEntityRepository<FoodsMealsQuantity> foodsMealsQuantityRepository,
            IDeletableEntityRepository<Food> foodRepository)
        {
            this.mealsRepository = mealsRepository;
            this.foodsMealsQuantityRepository = foodsMealsQuantityRepository;
            this.foodRepository = foodRepository;
        }

        public async Task Create(MealInputModel input, string userId)
        {
            var meal = new Meal()
            {
                Name = input.MealName,
                Date = input.Date,
                OwnerId = userId,
            };

            List<FoodsMealsQuantity> foodsMealsList = new List<FoodsMealsQuantity>();
            foreach (var foodMealsQuantity in input.FoodsWithQuantities)
            {
                Food food = this.foodRepository.All().FirstOrDefault(x => x.Name == foodMealsQuantity.FoodName);
                if (food == null)
                {
                    food = new Food()
                    {
                        Name = foodMealsQuantity.FoodName,
                        KcalPer100G = foodMealsQuantity.KcalPer100g,
                        CarbsPer100G = foodMealsQuantity.CarbsPer100g,
                        ProteinPer100G = foodMealsQuantity.ProteinPer100g,
                        FatPer100G = foodMealsQuantity.FatPer100g,
                    };

                    await this.foodRepository.AddAsync(food);
                }

                var foodMeals = new FoodsMealsQuantity()
                {
                    MealId = meal.Id,
                    FoodId = food.Id,
                    Quantity = foodMealsQuantity.Quantity,
                };
                await this.foodsMealsQuantityRepository.AddAsync(foodMeals);

                food.FoodsMealsQuantities.Add(foodMeals);
                meal.FoodsMealsQuantities.Add(foodMeals);
            }

            await this.mealsRepository.AddAsync(meal);
            await this.foodRepository.SaveChangesAsync();
            await this.foodRepository.SaveChangesAsync();
            await this.foodsMealsQuantityRepository.SaveChangesAsync();
        }

        public ICollection<MealViewModel> All(string userId)
        {
           return this.mealsRepository.AllAsNoTracking()
                .Where(x => x.OwnerId == userId)
                .OrderByDescending(x => x.Date)
                .Select(x => new MealViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Date = x.Date,
                    FoodsMealQuantities = x.FoodsMealsQuantities.Select(x => new FoodMealQuantityViewModel()
                    {
                        FoodName = x.Food.Name,
                        Quantity = x.Quantity,
                        KcalPer100g = x.Food.KcalPer100G,
                        CarbsPer100g = x.Food.CarbsPer100G,
                        ProteinPer100g = x.Food.ProteinPer100G,
                        FatsPer100g = x.Food.FatPer100G,
                    }).ToList(),
                    KcalPerMeal = x.FoodsMealsQuantities.Select(k => (k.Food.KcalPer100G * k.Quantity) / 100).Sum(),
                    CarbsPerMeal = x.FoodsMealsQuantities.Select(c => (c.Food.CarbsPer100G * c.Quantity) / 100).Sum(),
                    ProteinPerMeal = x.FoodsMealsQuantities.Select(p => (p.Food.ProteinPer100G * p.Quantity) / 100).Sum(),
                    FatsPerMeal = x.FoodsMealsQuantities.Select(f => (f.Food.FatPer100G * f.Quantity) / 100).Sum(),
                }).ToList();
        }

        public async Task Delete(string mealId)
        {
            var meal = this.mealsRepository.All().FirstOrDefault(x => x.Id == mealId);
            if (meal == null)
            {
                return;
            }

            this.mealsRepository.Delete(meal);

            var foodsMeal = this.foodsMealsQuantityRepository.All()
                .Where(x => x.MealId == mealId).ToList();
            foreach (var foodMeal in foodsMeal)
            {
                this.foodsMealsQuantityRepository.Delete(foodMeal);
            }

            await this.mealsRepository.SaveChangesAsync();
            await this.foodsMealsQuantityRepository.SaveChangesAsync();
        }
    }
}
