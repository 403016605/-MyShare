using System;
using MyShare.Kernel.Base.Commands;

namespace MyShare.Sample.Commands
{
    public class RestoreBookCommand : Command
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
    }
}