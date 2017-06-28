using System;
using MyShare.Kernel.Commands;

namespace MyShare.Sample.Commands
{
    public class RemoveBookCommand : ICommand
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

        /// <summary>
        /// 聚合的预期的版本
        /// </summary>
        public int ExpectedVersion { get; set; }
    }
}