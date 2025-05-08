using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplicationW.Models
{
    public class CollectionDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CollectionCategoryId { get; set; }

        [JsonPropertyName("photo")]
        public string? Photo { get; set; } // Позначено як необов’язкове

        public List<CollectionRecipeDto> CollectionRecipes { get; set; }
    }

    public class CollectionRecipeDto
    {
        public int RecipeId { get; set; }
    }
}