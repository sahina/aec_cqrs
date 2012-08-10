using System;
using Aec.Cqrs.Client.Events;

namespace Aec.Cqrs.WebUI.MessageHandlers
{
    public class UserCreatedHandler : IEventHandler<UserCreated>
    {
        #region Implementation of IEventHandler<in UserCreated>

        public void Handle(UserCreated theEvent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}