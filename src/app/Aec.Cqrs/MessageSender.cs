using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Aec.Cqrs
{
    public sealed class MessageSender
    {
        readonly Random m_random = new Random();
        readonly IQueueWriter[] m_queues;
        readonly Func<string> m_idGenerator;

        #region Ctors

        public MessageSender(IQueueWriter[] queues, Func<string> idGenerator = null)
        {
            m_queues = queues;
            m_idGenerator = idGenerator ?? (() => Guid.NewGuid().ToString());

            if (queues.Length == 0)
                throw new InvalidOperationException("There should be at least one queue");
        }

        #endregion

        public void SendOne(object content)
        {
            InnerSendBatch(cb => { }, new[] { content });
        }

        public void SendOne(object content, Action<EnvelopeBuilder> configure)
        {
            InnerSendBatch(configure, new[] { content });
        }


        public void SendBatch(object[] content)
        {
            if (content.Length == 0)
                return;

            InnerSendBatch(cb => { }, content);
        }

        public void SendBatch(object[] content, Action<EnvelopeBuilder> builder)
        {
            InnerSendBatch(builder, content);
        }

        public void SendControl(Action<EnvelopeBuilder> builder)
        {
            InnerSendBatch(builder, new object[0]);
        }

        public void SendEnvelope(ImmutableEnvelope envelope)
        {
            var queue = GetOutboundQueue();

            if (Transaction.Current == null)
            {
                SystemObserver.Notify(
                    new EnvelopeSent(queue.Name, envelope.EnvelopeId, false,
                                     envelope.Items.Select(x => x.MappedType.Name).ToArray(), envelope.GetAllAttributes()));
                
                queue.PutMessage(envelope);
            }
            else
            {
                var action = new CommitActionEnlistment(() =>
                {
                    SystemObserver.Notify(
                        new EnvelopeSent(
                            queue.Name,
                            envelope.EnvelopeId,
                            true,
                            envelope.Items.Select(x => x.MappedType.Name).ToArray(), envelope.GetAllAttributes()
                            ));

                    queue.PutMessage(envelope);
                });

                Transaction.Current.EnlistVolatile(action, EnlistmentOptions.None);
            }
        }

        #region Private Methods

        private IQueueWriter GetOutboundQueue()
        {
            if (m_queues.Length == 1)
                return m_queues[0];
            
            var random = m_random.Next(m_queues.Length);

            return m_queues[random];
        }

        private void InnerSendBatch(Action<EnvelopeBuilder> configure, IEnumerable<object> messageItems)
        {
            var id = m_idGenerator();

            var builder = new EnvelopeBuilder(id);
            foreach (var item in messageItems)
            {
                builder.AddItem(item);
            }

            configure(builder);
            var envelope = builder.Build();

            SendEnvelope(envelope);
        }

        #endregion

        sealed class CommitActionEnlistment : IEnlistmentNotification
        {
            readonly Action m_commit;

            public CommitActionEnlistment(Action commit)
            {
                m_commit = commit;
            }

            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                preparingEnlistment.Prepared();
            }

            public void Commit(Enlistment enlistment)
            {
                m_commit();
                enlistment.Done();
            }

            public void Rollback(Enlistment enlistment)
            {
                enlistment.Done();
            }

            public void InDoubt(Enlistment enlistment)
            {
                enlistment.Done();
            }
        }
    }
}