﻿using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Domain.Uow;

using LiteDB;

namespace DynamicTranslator.LiteDb.LiteDb.Uow
{
    public class LiteDbUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        public LiteDatabase Database { get; set; }

        public LiteTransaction Transaction { get; private set; }

        public LiteDbUnitOfWork(IConnectionStringResolver connectionStringResolver, IUnitOfWorkDefaultOptions defaultOptions)
            : base(connectionStringResolver, defaultOptions) {}

        public override void SaveChanges()
        {
            Transaction.Commit();
        }

        public override Task SaveChangesAsync()
        {
            SaveChanges();
            return Task.FromResult(0);
        }

        protected override void BeginUow()
        {
            Transaction = Database.BeginTrans();
        }

        protected override void CompleteUow()
        {
            Transaction.Commit();
        }

        protected override Task CompleteUowAsync()
        {
            CompleteUow();
            return Task.FromResult(0);
        }

        protected override void DisposeUow()
        {
            Transaction?.Dispose();
            Transaction = null;
        }
    }
}