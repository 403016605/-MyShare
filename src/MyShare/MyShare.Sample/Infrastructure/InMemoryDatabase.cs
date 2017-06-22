using System;
using System.Collections.Generic;
using MyShare.Sample.Infrastructure.Entities;

namespace MyShare.Sample.Infrastructure
{
    public static class InMemoryDatabase
    {
        public static readonly Dictionary<Guid, Book> BookSet = new Dictionary<Guid, Book>();
    }
}
