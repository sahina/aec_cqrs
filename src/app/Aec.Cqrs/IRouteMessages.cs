namespace Aec.Cqrs
{
    public interface IRouteMessages
    {
        void Route(IMessage message);
    }
}