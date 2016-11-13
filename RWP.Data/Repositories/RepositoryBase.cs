using System.Collections.Generic;
using System.Linq;
using RWP.Data.Contracts;
using RWP.Data.Filters;

namespace RWP.Data.Repositories
{
  /// <summary>
  /// Repository abstaraction
  /// </summary>
  /// <typeparam name="TEntity">Entity type</typeparam>
  public abstract class RepositoryBase<TEntity>
    where TEntity : class, IEntityWithId, IUpdatable<TEntity>, new()
  {
    internal RepositoryBase()
    {
    }

    internal DataContext CreateDataContext()
    {
      var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
      return new DataContext(connectionString);
    }

    /// <summary>
    /// Fetch all entities of desired type
    /// </summary>
    public virtual IEnumerable<TEntity> GetEntities()
    {
      using (var context = CreateDataContext())
      {
        return context.GetTable<TEntity>().ToList();
      }
    }

    /// <summary>
    /// Fetch all entities of desired type with filter
    /// </summary>
    public virtual IEnumerable<TEntity> GetEntities(FilterBase<TEntity> filter)
    {
      using (var context = CreateDataContext())
      {
        var table = context.GetTable<TEntity>();
        var result = (filter != null) ? filter.Apply(table) : table;

        return result.ToList();
      }
    }

    /// <summary>
    /// Get entity by ID
    /// </summary>
    public virtual TEntity GetById(int id)
    {
      using (var context = CreateDataContext())
      {
        return DataUtils.GetById<TEntity>(context, id);
      }
    }

    /// <summary>
    /// Insert new entity in DB context
    /// </summary>
    public virtual int Insert(TEntity entity)
    {
      using (var context = CreateDataContext())
      {
        context.GetTable<TEntity>().InsertOnSubmit(entity);
        context.SubmitChanges();

        return entity.Id;
      }
    }

    /// <summary>
    /// Update stored value of entity
    /// </summary>
    public virtual void Update(TEntity entity)
    {
      using (var context = CreateDataContext())
      {
        var persisted = DataUtils.GetById<TEntity>(context, entity.Id);
        if (persisted == null)
          return;

        persisted.UpdateFrom(entity, context);
        context.SubmitChanges();
      }
    }

    /// <summary>
    /// Update/insert entity
    /// </summary>
    public virtual int Upsert(TEntity entity)
    {
      using (var context = CreateDataContext())
      {
        var table = context.GetTable<TEntity>();
        TEntity persisted;
        if (entity.Id == 0)
        {
          persisted = new TEntity();
          persisted.UpdateFrom(entity, context);
          table.InsertOnSubmit(persisted);
        }
        else
        {
          persisted = DataUtils.GetById<TEntity>(context, entity.Id);
          if (persisted == null)
            return 0;

          persisted.UpdateFrom(entity, context);
        }

        context.SubmitChanges();

        return persisted.Id;
      }
    }

    /// <summary>
    /// Delete entity from DB
    /// </summary>
    public virtual void Delete(TEntity entity)
    {
      using (var context = CreateDataContext())
      {
        var persisted = DataUtils.GetById<TEntity>(context, entity.Id);
        if (persisted == null)
          return;

        context.GetTable<TEntity>().DeleteOnSubmit(persisted);
        context.SubmitChanges();
      }
    }
  }
}