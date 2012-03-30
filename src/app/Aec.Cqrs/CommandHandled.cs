using System;

namespace Aec.Cqrs
{
    public class CommandHandled : ISystemEvent
    {
        public readonly object Command;

        public CommandHandled(object command)
        {
            Command = command;
        }

        public override string ToString()
        {
            return "Handled Command: " + Command + " at " + DateTime.UtcNow + " UTC";
        }
    }
}