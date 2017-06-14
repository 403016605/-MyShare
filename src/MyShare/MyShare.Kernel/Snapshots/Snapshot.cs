#region using

using System;

#endregion

namespace MyShare.Kernel.Snapshots
{
    /// <summary>
    /// 快照抽象类
    /// </summary>
    public abstract class Snapshot
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}