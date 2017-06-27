using System;
using System.Collections.Generic;
using MyShare.Sample.Infrastructure.Entities;

namespace MyShare.Sample.Queries
{
    public interface IQueryBook
    {
        IEnumerable<BookEntity> GetList();

        BookEntity Get(Guid id);
    }
}
