using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlazorApp12.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Назва повинна містити від 3 до 50 символів.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Опис є обов'язковим.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Опис повинен містити від 10 до 500 символів.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Категорія є обов'язковою.")]
        [Range(1, int.MaxValue, ErrorMessage = "Виберіть валідну категорію.")]
        public int? CollectionCategoryId { get; set; }

        public CollectionCategory CollectionCategory { get; set; }

        public byte[]? Photo { get; set; }

        public List<CollectionRecipe> CollectionRecipes { get; set; } = new List<CollectionRecipe>();
    }
}