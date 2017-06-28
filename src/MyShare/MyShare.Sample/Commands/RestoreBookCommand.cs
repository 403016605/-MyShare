using System;
using MyShare.Kernel.Commands;

namespace MyShare.Sample.Commands
{
    public class RestoreBookCommand : ICommand
    {
        public RestoreBookCommand(Guid id)
        {
            Id = id;
            FromVersion = 0;
        }

        public RestoreBookCommand(Guid id ,int fromVersion)
        {
            Id = id;
            FromVersion = fromVersion;
        }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public Guid Id { get; }

        public int FromVersion { get; }

        /// <summary>
        /// 聚合的预期的版本
        /// </summary>
        public int ExpectedVersion { get; set; }
    }
}