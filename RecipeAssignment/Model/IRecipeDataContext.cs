using System;
using System.Data.Linq;

namespace RecipeAssignment.Model
{
    public interface IRecipeDataContext : IDisposable
    {
        ITable<Recipe> Recipes();

        void ExecuteCommand(string command, params object[] parameters);

        void SubmitChanges();
    }

    public class RecipeDataContext : IRecipeDataContext
    {
        private readonly DataContext _dataContext;

        public RecipeDataContext(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ITable<Recipe> Recipes()
        {
            return _dataContext.GetTable<Recipe>();
        }

        public void ExecuteCommand(string command, params object[] parameters)
        {
            _dataContext.ExecuteCommand(command, parameters);
        }

        public void SubmitChanges()
        {
            _dataContext.SubmitChanges();
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        ITable<Recipe> IRecipeDataContext.Recipes()
        {
            throw new NotImplementedException();
        }
    }
}