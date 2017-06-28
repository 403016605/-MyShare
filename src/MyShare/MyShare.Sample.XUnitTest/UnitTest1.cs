using System;
using MyShare.Kernel.Common;
using MyShare.Kernel.Common.Impl;
using MyShare.Sample.Events;
using Xunit;

namespace MyShare.Sample.XUnitTest
{


    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ISerializer serializer=new Serializer();

            var @event = new BookCreatedEvent(Guid.NewGuid(), "Test")
            {
                Version = 1,
                TimeStamp = DateTimeOffset.UtcNow.Ticks
            };

            var bytes = serializer.Serialize(@event);

            var obj = serializer.Deserialize<BookCreatedEvent>(bytes);

            Assert.Equal(obj.Id, @event.Id);
            Assert.Equal(obj.Name, @event.Name);
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
