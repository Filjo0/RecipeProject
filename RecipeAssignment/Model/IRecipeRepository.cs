using System.Collections.Generic;
using System.Linq;

namespace RecipeAssignment.Model
{
    public interface IRecipeRepository
    {
        Recipe AddRecipe(Recipe recipe);
        Recipe GetRecipe(int id);
        List<Recipe> GetAllRecipes();
        List<Recipe> GetRecipeByRecipeName(string recipeName);
        List<Recipe> GetRecipeByIngredient(string ingredient);
    }

    public class RecipeRepository : IRecipeRepository
    {
        private readonly IDataContextWrapper _dataContextWrapper;

        public RecipeRepository(IDataContextWrapper dataContextWrapper)
        {
            _dataContextWrapper = dataContextWrapper;
        }

        public Recipe AddRecipe(Recipe recipe)
        {
            using var db = _dataContextWrapper.CreateDataContext();
            db.Recipes().InsertOnSubmit(recipe);
            db.SubmitChanges();
            return recipe;
        }

        public Recipe GetRecipe(int id)
        {
            using var db = _dataContextWrapper.CreateDataContext();
            return (from recipe in db.Recipes() where recipe.recipe_id == id select recipe).SingleOrDefault();
        }

        public List<Recipe> GetAllRecipes()
        {
            using var db = _dataContextWrapper.CreateDataContext();
            return (from recipe in db.Recipes() select recipe).ToList();
        }

        public List<Recipe> GetRecipeByRecipeName(string recipeName)
        {
            using var db = _dataContextWrapper.CreateDataContext();
            return (from recipe in db.Recipes() where recipe.recipe_name == recipeName select recipe).ToList();
        }

        public List<Recipe> GetRecipeByIngredient(string ingredient)
        {
            using var db = _dataContextWrapper.CreateDataContext();
            return (from recipe in db.Recipes() where recipe.ingredients.Contains(ingredient) select recipe).ToList();
        }
    }
}