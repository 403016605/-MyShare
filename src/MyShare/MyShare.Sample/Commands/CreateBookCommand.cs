#region using

using System;
using MyShare.Kernel.Commands;

#endregion

namespace MyShare.Sample.Commands
{
    public class CreateBookCommand : Command
    {
        public readonly string Name;

        public CreateBookCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public Guid Id { get;  }
    }

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