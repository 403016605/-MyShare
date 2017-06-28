using System.Threading.Tasks;
using MyShare.Kernel.Events;
using MyShare.Sample.Infrastructure;
using MyShare.Sample.Infrastructure.Entities;

namespace MyShare.Sample.Events.Handlers
{
    public class BookEventHandlers : IEventHandler<BookCreatedEvent>,
        IEventHandler<BookRemoveEvent>
    {
        public Task Handle(BookCreatedEvent message)
        {
            InMemoryDatabase.BookSet.Remove(message.Id);
            InMemoryDatabase.BookSet.Add(message.Id, new BookEntity(message.Id, message.Name, message.Version));
            return Task.CompletedTask;
        }

        public Task Handle(BookRemoveEvent message)
        {
            InMemoryDatabase.BookSet.Remove(message.Id);
            return Task.CompletedTask;
        }
    }
}
