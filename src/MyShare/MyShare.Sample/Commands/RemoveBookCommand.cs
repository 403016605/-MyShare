using System;
using MyShare.Kernel.Base.Commands;

namespace MyShare.Sample.Commands
{
    public class RemoveBookCommand : Command
    {
        public RemoveBookCommand(Guid id, int originalVersion)
        {
            Id = id;
            ExpectedVersion = originalVersion;
        }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public Guid Id { get; }
    }
}