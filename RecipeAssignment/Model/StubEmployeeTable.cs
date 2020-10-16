using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace RecipeAssignment.Model
{
    public class StubRecipeTable : ITable<Recipe>
    {
        private readonly List<Recipe> _internalList;

        public StubRecipeTable(List<Recipe> list)
        {
            _internalList = list;
        }

        public void Attach(Recipe entity)
        {
        }

        public void DeleteOnSubmit(Recipe entity)
        {
            _internalList.Remove(entity);
        }

        public void InsertOnSubmit(Recipe entity)
        {
            _internalList.Add(entity);
        }

        public IEnumerator<Recipe> GetEnumerator() => _internalList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType => _internalList.AsQueryable().ElementType;

        public Expression Expression => _internalList.AsQueryable().Expression;

        public IQueryProvider Provider => _internalList.AsQueryable().Provider;
    }
}