using System;
using System.Data.Linq;

namespace RecipeAssignment.Model
{
    public interface IDataContextWrapper
    {
        IRecipeDataContext CreateDataContext();
    }

    public class DataContextWrapper : IDataContextWrapper
    {
        private readonly string _connectionString;

        public DataContextWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IRecipeDataContext CreateDataContext()
        {
            return new RecipeDataContext(new DataContext(_connectionString));
        }

        IRecipeDataContext IDataContextWrapper.CreateDataContext()
        {
            throw new NotImplementedException();
        }
    }
}