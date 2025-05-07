using System;
using System.Collections.Generic;
using WebApplicationW.Models;

namespace WebApplicationW.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int CookingTime { get; set; }
        public int? DifficultyLevelId { get; set; }
        public int Calories { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Photo { get; set; } // Base64 string
        public List<RecipeIngredientDto> RecipeIngredients { get; set; } = new List<RecipeIngredientDto>();
        public List<RecipeCategoryDto> RecipeCategories { get; set; } = new List<RecipeCategoryDto>();
        public List<RecipeMealTimeDto> RecipeMealTimes { get; set; } = new List<RecipeMealTimeDto>();
    }

    public class RecipeIngredientDto
    {
        public int IngredientId { get; set; }
        public string Quantity { get; set; }
    }

    public class RecipeCategoryDto
    {
        public int CategoryId { get; set; }
    }

    public class RecipeMealTimeDto
    {
        public int MealTimeId { get; set; }
    }
}