using System;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;
using Should;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class FileRecordStorageTests
    {
        private AccountID m_accountID;
        private readonly AccountID m_accountIDFixed = new AccountID(new Guid("a10726c2-c6aa-4a1a-9f64-892626bdd7c0"));
        private IRecordStorageConfig m_storageConfig;
        private IRecordStorageFactory m_factory;

        [SetUp]
        public void Setup()
        {
            m_accountID = new AccountID(Guid.NewGuid());
            m_storageConfig = new FileRecordStorageConfig();
            m_factory = new FileRecordStorageFactory(m_storageConfig);
        }

        [Test]
        public void file_record_storage_factory_should_create_file_storage()
        {
            // arrange

            // act
            var storage = m_factory.GetOrCreateStorage(m_accountID);

            // assert
            storage.ShouldNotBeNull();
        }

        [Test]
        public void file_record_storage_should_append_new()
        {
            // arrange

            // act
            var storage = m_factory.GetOrCreateStorage(m_accountID);
            var account = new AccountCreated(m_accountID);

            // assert
            storage.TryAppend(new object[]{ account });
        }

        [Test]
        public void file_record_storage_should_append_new_and_fetch()
        {
            // arrange
            var storage = m_factory.GetOrCreateStorage(m_accountID);
            var account = new AccountCreated(m_accountID);

            storage.TryAppend(new object[] { account });

            // act
            var fetch = storage.GetRecords(0, int.MaxValue);

            // assert
            fetch.ShouldNotBeEmpty();
        }

        [Test]
        public void file_record_storage_should_append_sequence()
        {
            // arrange

            // act
            var storage = m_factory.GetOrCreateStorage(m_accountIDFixed);

            // assert
            storage.TryAppend(new object[]
            {
                new AccountCreated(m_accountIDFixed),
                new AccountSuspended(m_accountIDFixed),
                new AccountEnabled(m_accountIDFixed),
                new AccountSuspended(m_accountIDFixed),
                new AccountEnabled(m_accountIDFixed),
                new AccountSuspended(m_accountIDFixed)
            });
        }
    }

    public class FileRecordStorageConfig : IRecordStorageConfig
    {
        #region Implementation of IRecordStorageConfig

        public object BuildConfiguration()
        {
            return "test-records";
        }

        #endregion
    }
}
