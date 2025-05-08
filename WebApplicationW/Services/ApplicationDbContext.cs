using Microsoft.EntityFrameworkCore;
using WebApplicationW.Models;

namespace WebApplicationW.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<DifficultyLevel> DifficultyLevel { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<RecipeCategory> RecipeCategory { get; set; }
        public DbSet<MealTime> MealTime { get; set; }
        public DbSet<RecipeMealTime> RecipeMealTime { get; set; }
        public DbSet<Collection> Collection { get; set; }
        public DbSet<CollectionCategory> CollectionCategory { get; set; }
        public DbSet<CollectionRecipe> CollectionRecipe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Existing configurations...

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany()
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeCategory>()
                .HasKey(rc => new { rc.RecipeId, rc.CategoryId });

            modelBuilder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Recipe)
                .WithMany(r => r.RecipeCategories)
                .HasForeignKey(rc => rc.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Category)
                .WithMany()
                .HasForeignKey(rc => rc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeMealTime>()
                .HasKey(rm => new { rm.RecipeId, rm.MealTimeId });

            modelBuilder.Entity<RecipeMealTime>()
                .HasOne(rm => rm.Recipe)
                .WithMany(r => r.RecipeMealTimes)
                .HasForeignKey(rm => rm.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeMealTime>()
                .HasOne(rm => rm.MealTime)
                .WithMany()
                .HasForeignKey(rm => rm.MealTimeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.DifficultyLevel)
                .WithMany()
                .HasForeignKey(r => r.DifficultyLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // New configurations for Collections
            modelBuilder.Entity<Collection>()
                .HasOne(c => c.CollectionCategory)
                .WithMany()
                .HasForeignKey(c => c.CollectionCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CollectionRecipe>()
                .HasKey(cr => new { cr.CollectionId, cr.RecipeId });

            modelBuilder.Entity<CollectionRecipe>()
                .HasOne(cr => cr.Collection)
                .WithMany(c => c.CollectionRecipes)
                .HasForeignKey(cr => cr.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CollectionRecipe>()
                .HasOne(cr => cr.Recipe)
                .WithMany()
                .HasForeignKey(cr => cr.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}