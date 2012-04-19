using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class CommandSenderTests : TestBase
    {
        [Test]
        public void command_sender_should_send_command()
        {
            // arrange
            var cmd = new CreateAccount(Identifier);

            // act
            Sender.SendOne(cmd);

            // assert
        }

        [Test]
        public void command_sender_should_send_multiple_commands()
        {
            // arrange
            var cmd1 = new CreateAccount(Identifier);
            var cmd2 = new CreateAccount(Identifier);
            var cmd3 = new CreateAccount(Identifier);

            var batch = new object[] { cmd1, cmd2, cmd3 };

            // act
            Sender.SendBatch(batch);

            // assert
        }

        [Test]
        public void command_sender_should_send_envelope()
        {
            // arrange
            var builder = new EnvelopeBuilder("env");
            builder.AddItem(new CreateAccount(Identifier));
            builder.AddItem(new CreateAccount(Identifier));
            builder.AddItem(new CreateAccount(Identifier));

            // act
            Sender.SendEnvelope(builder.Build());

            // assert
        }

        [Test]
        public void command_sender_should_send_control()
        {
            // arrange

            // act
            Sender.SendControl(b => b.AddItem(new CreateAccount(Identifier)));

            // assert
        }
    }
}
