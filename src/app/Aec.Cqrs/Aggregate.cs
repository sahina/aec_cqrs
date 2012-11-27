namespace Aec.Cqrs
{
    public abstract class Aggregate<TIdentity> : IAggregate<TIdentity> where TIdentity : IIdentity
    {
        #region Implementation of IAggregate<in TIdentity>

        public void Execute(ICommand<TIdentity> c)
        {
            ((dynamic)this).When((dynamic)c);
        }

        #endregion
    }
}