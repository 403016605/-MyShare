namespace MyShare.Kernel.Base.Commands
{
    public abstract class Command : ICommand
    {
        /// <summary>
        ///     聚合的预期的版本
        /// </summary>
        public int ExpectedVersion { get; set; }
    }
}