using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class EnvelopeSerializerTests
    {
        private AccountID m_id;
        private IEnvelopeSerializer m_serializer;
        private ImmutableEnvelope m_envelope;
        private EnvelopeBuilder m_envelopeBuilder;

        [SetUp]
        public void Setup()
        {
            m_id = new AccountID(Guid.NewGuid());
            m_serializer = new EnvelopeSerializer();

            m_envelopeBuilder = new EnvelopeBuilder("1");
            m_envelopeBuilder.AddItem(new CreateAccount(m_id));
            m_envelopeBuilder.AddItem(new DisableAccount(m_id));
            m_envelopeBuilder.AddItem(new EnableAccount(m_id));
            m_envelope = m_envelopeBuilder.Build();
        }

        [Test]
        public void should_serialize_envelope_to_stream()
        {
            // arrange

            // act
            var stream = m_serializer.SerializeToStream(m_envelope);

            // assert
            Assert.Inconclusive("should_serialize_envelope_to_stream");
        }

        [Test]
        public void should_serialize_envelope_to_bytes()
        {
            // arrange

            // act

            // assert
            Assert.Inconclusive("should_serialize_envelope_to_bytes");
        }

        [Test]
        public void should_deserialize_envelope_from_stream()
        {
            // arrange

            // act

            // assert
            Assert.Inconclusive("should_deserialize_envelope_from_stream");
        }

        [Test]
        public void should_deserialize_envelope_from_bytes()
        {
            // arrange

            // act

            // assert
            Assert.Inconclusive("should_deserialize_envelope_from_bytes");
        }
    }
}
