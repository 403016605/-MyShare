#region using

using System;
using MyShare.Kernel.Base.Commands;

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
}