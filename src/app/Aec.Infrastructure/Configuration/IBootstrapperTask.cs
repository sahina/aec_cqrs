namespace Aec.Infrastructure.Configuration
{
    /// <summary>
    /// Contract to mark a bootstrap task.
    /// </summary>
    public interface IBootstrapperTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        void Execute();
    }
}
