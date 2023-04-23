using Moq;
using NUnit.Framework;
using RecipeAssignment.Model;
using System;
using System.Collections.Generic;

namespace RecipeAssignmentTests2
{
    [TestFixture]
    public class DataClasses1DataContextTests
    {
        private Mock<IRecipeDataContext> _mockRecipeDataContext;
        private IDataContextWrapper _stubDataContextWrapper;
        private IRecipeRepository _recipeRepository;

        [SetUp]
        public void Setup()
        {
            _mockRecipeDataContext = new Mock<IRecipeDataContext>();
            _stubDataContextWrapper = new StubDataContextWrapper(_mockRecipeDataContext.Object);
            _recipeRepository = new RecipeRepository(_stubDataContextWrapper);

            var recipe1 = new Recipe
            {
                recipe_id = 1,
                recipe_name = "Potato",
                cooking_time = new TimeSpan(00, 30, 00),
                ingredients = "Potato",
                description = "Cook",
                is_favorite = false
            };
            var recipe2 = new Recipe
            {
                recipe_id = 2,
                recipe_name = "Chicken",
                cooking_time = new TimeSpan(00, 20, 00),
                ingredients = "Chicken",
                description = "Boil",
                is_favorite = false
            };
            var recipe3 = new Recipe
            {
                recipe_id = 3,
                recipe_name = "Salad",
                cooking_time = new TimeSpan(00, 15, 00),
                ingredients = "Cucumber - 1\nTomato - 1",
                description = "Cut",
                is_favorite = false
            };

            var recipes = new List<Recipe> { recipe1, recipe2, recipe3 };
            _mockRecipeDataContext.Setup(x => x.Recipes()).Returns(new StubRecipeTable(recipes));
        }

        [Test]
        public void AddRecipeShouldAddNewRecipeWhenDoesNotExist()
        {
            _recipeRepository.AddRecipe(new Recipe
            {
                recipe_name = "Pavlova cake",
                cooking_time = new TimeSpan(00, 10, 00),
                ingredients = "Cake",
                description = "Freeze",
                is_favorite = false
            });
            Assert.AreEqual(4, _recipeRepository.GetAllRecipes().Count);
        }

        [Test]
        public void GetAllRecipeByIdShouldReturnRecipeWhenIdExists()
        {
            var results = _recipeRepository.GetAllRecipes();
            Assert.AreEqual(3, results.Count);
        }

        [Test]
        public void GetRecipeByRecipeNameShouldReturnRecipeWhenRecipeNameMatches()
        {
            var results = _recipeRepository.GetRecipeByRecipeName("Chicken");
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        public void GetRecipeByIngredientShouldReturnRecipeWhenIngredientMatches()
        {
            var results = _recipeRepository.GetRecipeByIngredient("Tomato");
            Assert.AreEqual(1, results.Count);
        }
    }
}