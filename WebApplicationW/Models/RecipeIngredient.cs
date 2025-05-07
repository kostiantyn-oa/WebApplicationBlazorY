using System.ComponentModel.DataAnnotations;

namespace WebApplicationW.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Кількість є обов'язковою.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Кількість повинна містити від 1 до 50 символів.")]
        public string Quantity { get; set; }

        public Recipe? Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}