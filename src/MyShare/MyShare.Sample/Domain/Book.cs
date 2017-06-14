using System;
using System.Collections.Generic;
using System.Text;
using MyShare.Kernel.Domain;
using MyShare.Sample.Events;

namespace MyShare.Sample.Domain
{
    public class Book:AggregateRoot
    {
        private Book()
        {
        }

        public Book(Guid id, string name)
        {
            Id = id;
            ApplyChange(new BookCreatedEvent(id, name));
        }

        public void Remove(Guid id)
        {
            ApplyChange(new BookRemoveEvent(Id));
        }
    }
}
