namespace BlazorApp12.Models
{
    public class RecipeMealTime
    {
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public int MealTimeId { get; set; }
        public MealTime? MealTime { get; set; }
    }
}