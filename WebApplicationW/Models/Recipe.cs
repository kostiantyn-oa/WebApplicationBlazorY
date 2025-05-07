using System.ComponentModel.DataAnnotations;

namespace WebApplicationW.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Назва повинна містити від 3 до 50 символів.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Опис є обов'язковим.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Опис повинен містити від 10 до 500 символів.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Інструкції є обов'язковими.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Інструкції повинні містити від 10 до 2000 символів.")]
        public string Instructions { get; set; }

        [Required(ErrorMessage = "Час приготування є обов'язковим.")]
        [Range(1, 1000, ErrorMessage = "Час приготування має бути від 1 до 1000 хвилин.")]
        public int CookingTime { get; set; }

        [Required(ErrorMessage = "Рівень складності є обов'язковим.")]
        [Range(1, int.MaxValue, ErrorMessage = "Виберіть валідний рівень складності.")]
        public int? DifficultyLevelId { get; set; }

        public DifficultyLevel? DifficultyLevel { get; set; } // Явно необов'язкове

        [Range(0, 10000, ErrorMessage = "Калорії мають бути від 0 до 10000.")]
        public int Calories { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public List<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
        public List<RecipeMealTime> RecipeMealTimes { get; set; } = new List<RecipeMealTime>();
    }
}