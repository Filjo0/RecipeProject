namespace RecipeAssignment.Model
{
    public class StubDataContextWrapper : IDataContextWrapper
    {
        private readonly IRecipeDataContext _dataContext;

        public StubDataContextWrapper(IRecipeDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IRecipeDataContext CreateDataContext()
        {
            return _dataContext;
        }
    }
}