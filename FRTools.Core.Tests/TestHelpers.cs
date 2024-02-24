using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FRTools.Core.Tests
{
    public static class TestHelpers
    {
        public static DbSet<TEntity> CreateFakeDbSet<TEntity>(IQueryable<TEntity>? data = null) where TEntity : class
        {
            data ??= Enumerable.Empty<TEntity>().AsQueryable();
            var fakeDbSet = A.Fake<DbSet<TEntity>>(x =>
            {
                x.Implements<IQueryable<TEntity>>();
                x.Implements<IInfrastructure<IServiceProvider>>();
                x.Implements<IListSource>();
            });

            A.CallTo(() => ((IQueryable<TEntity>)fakeDbSet).GetEnumerator()).Returns(data.GetEnumerator());
            A.CallTo(() => ((IQueryable<TEntity>)fakeDbSet).Provider).Returns(data.Provider);
            A.CallTo(() => ((IQueryable<TEntity>)fakeDbSet).Expression).Returns(data.Expression);

            return fakeDbSet;
        }
    }
}
