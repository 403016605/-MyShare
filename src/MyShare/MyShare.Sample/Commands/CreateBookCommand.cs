#region using

using System;
using MyShare.Kernel.Commands;

#endregion

namespace MyShare.Sample.Commands
{
    public class CreateBookCommand : ICommand
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

        /// <summary>
        /// 聚合的预期的版本
        /// </summary>
        public int ExpectedVersion { get; set; }
    }
}