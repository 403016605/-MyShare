using System.Threading.Tasks;
using MyShare.Kernel.Base.Commands;
using MyShare.Kernel.Domain;
using MyShare.Sample.Domain;

namespace MyShare.Sample.Commands.Handlers
{
    public class BookCommandHandlers : ICommandHandler<CreateBookCommand>,
        ICommandHandler<RemoveBookCommand>,
        ICommandHandler<RestoreBookCommand>
    {
        private readonly ISession _session;

        public BookCommandHandlers(ISession session)
        {
            _session = session;
        }
        public async Task Handle(CreateBookCommand message)
        {
            var item = new Book(message.Id, message.Name);
            await _session.Add(item);
            await _session.Commit();
        }

        public async Task Handle(RemoveBookCommand message)
        {
            var item = await _session.Get<Book>(message.Id, message.ExpectedVersion);
            item.Remove(message.Id);
            await _session.Commit();
        }

        public async Task Handle(RestoreBookCommand message)
        {
            var item = await _session.Get<Book>(message.Id);
            await _session.Add(item);
            await _session.Commit();
        }
    }
}
