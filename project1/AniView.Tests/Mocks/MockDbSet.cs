using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace AniView.Tests.Mocks; 
// mock the behavior of the DBSet 
public class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class {
    public MockDbSet(List<TEntity> dataSource = null) {
        var data = (dataSource ?? new List<TEntity>());
        var queryable = data.AsQueryable();

        this.As<IQueryable<TEntity>>().Setup(e => e.Provider).Returns(queryable.Provider);
        this.As<IQueryable<TEntity>>().Setup(e => e.Expression).Returns(queryable.Expression);
        this.As<IQueryable<TEntity>>().Setup(e => e.ElementType).Returns(queryable.ElementType);
        this.As<IQueryable<TEntity>>().Setup(e => e.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        //Mocking the insertion of entities
        this.Setup(dbSet => dbSet.Add(It.IsAny<TEntity>())).Returns((TEntity arg) => {
            data.Add(arg);
            return  arg as EntityEntry<TEntity>;
        });

        this.Setup(dbSet => dbSet.Remove(It.IsAny<TEntity>())).Returns((TEntity arg) => 
        {
            data.Remove(arg); 
            return arg as EntityEntry<TEntity>; 
        });

        // this.Setup(dbSet => dbSet.Update(It.IsAny<TEntity>() )).Returns((TEntity arg) => 
        // {
            
        // });

    
    }
}