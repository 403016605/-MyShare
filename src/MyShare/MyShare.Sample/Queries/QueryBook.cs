using System;
using System.Collections.Generic;
using System.Linq;
using MyShare.Sample.Infrastructure;
using MyShare.Sample.Infrastructure.Entities;

namespace MyShare.Sample.Queries
{
    internal class QueryBook : IQueryBook
    {

        public IEnumerable<BookEntity> GetList()
        {
            return InMemoryDatabase.BookSet.Values.ToList();
        }

        public BookEntity Get(Guid id)
        {
            return InMemoryDatabase.BookSet[id];
        }
    }
}