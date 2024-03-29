﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aec.Cqrs.Tests.Unit.Fakes;
using NUnit.Framework;
using Should;

namespace Aec.Cqrs.Tests.Unit
{
    [TestFixture]
    public class DocumentTests
    {
        private IDocumentStrategy m_strategy;
        private AccountView m_account;
        private AccountID m_accountID;
        private FileDocumentStore m_store;
        private const string FOLDER_PATH = "test-docs";

        [SetUp]
        public void Setup()
        {
            m_strategy = new FileJsonDocumentStrategy();
            m_store = new FileDocumentStore(FOLDER_PATH, m_strategy);
            m_account = new AccountView
            {
                AccountID = Guid.NewGuid(),
                Name = "Test Account"
            };
            m_accountID = new AccountID(m_account.AccountID);
            m_store.ResetAll();
        }

        [Test]
        public void document_strategy_should_serialize_and_deserialize()
        {
            // arrange
            var stream = new MemoryStream();

            // act
            m_strategy.Serialize(m_account, stream);
            
            // serialize method closes the stream writer and underlying memory stream. make a new one.
            var newStream = new MemoryStream(stream.ToArray());
            var daccount = m_strategy.Deserialize<AccountView>(newStream);

            // assert
            daccount.ShouldBeType<AccountView>();
            daccount.AccountID.ShouldEqual(m_account.AccountID);
        }

        [Test]
        public void document_strategy_should_get_bucket_name()
        {
            // arrange

            // act
            var name = m_strategy.GetEntityBucket<AccountView>();

            // assert
            name.ShouldNotBeNull();
        }

        [Test]
        public void document_strategy_should_get_location()
        {
            // arrange

            // act
            var location = m_strategy.GetEntityLocation(m_account.GetType(), m_account.AccountID);

            // assert
            location.ShouldNotBeNull();
        }

        [Test]
        public void document_storage_should_delete()
        {
            // arrange
            var storage = new DocumentStorage(m_store);
            var bucket = m_strategy.GetEntityBucket<AccountView>();

            // act
            storage.AddEntity(m_accountID, m_account);
            storage.TryDeleteEntity<AccountView>(m_accountID);

            // assert
            m_store.EnumerateContents(bucket).ShouldBeEmpty();
        }

        [Test]
        public void document_storage_should_update()
        {
            // arrange
            AccountView fetch;
            var storage = new DocumentStorage(m_store);
            storage.AddEntity(m_accountID, m_account);

            // act
            storage.UpdateEntity<AccountView>(m_accountID, a => a.Name = "New Name");
            storage.TryGetEntity(m_accountID, out fetch);

            // assert
            fetch.Name.ShouldEqual("New Name");
        }

        [Test]
        public void document_storage_should_add()
        {
            // arrange
            var storage = new DocumentStorage(m_store);
            var bucket = m_strategy.GetEntityBucket<AccountView>();

            // act
            storage.AddEntity(m_accountID, m_account);

            // assert
            m_store.EnumerateContents(bucket).ShouldNotBeEmpty();
        }

        [Test]
        public void document_storage_should_get_all_type_items()
        {
            // arrange
            IEnumerable<AccountView> entities;
            var storage = new DocumentStorage(m_store);
            storage.AddEntity(new AccountID(Guid.NewGuid()), new AccountView());
            storage.AddEntity(new AccountID(Guid.NewGuid()), new AccountView());
            storage.AddEntity(new AccountID(Guid.NewGuid()), new AccountView());
            storage.AddEntity(new AccountID(Guid.NewGuid()), new AccountView());
            // act
            storage.TryGetAllEntities(out entities);
            var accountViews = entities as List<AccountView> ?? entities.ToList();

            // assert
            accountViews.ShouldNotBeEmpty();
            accountViews.Count().ShouldEqual(4);
        }
    }
}
