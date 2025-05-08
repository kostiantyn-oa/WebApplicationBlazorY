namespace WebApplicationW.Models
{
    public class CollectionRecipe
    {
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}