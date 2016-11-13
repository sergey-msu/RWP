using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq.Expressions;
using RWP.Data.Contracts;

namespace RWP.Data
{
  /// <summary>
  /// Data utilities
  /// </summary>
  public static class DataUtils
  {
    /// <summary>
    /// Fetch entity by integer ID
    /// </summary>
    public static TEntity GetById<TEntity>(System.Data.Linq.DataContext dataContext, int id)
      where TEntity : class
    {
      var itemParameter = Expression.Parameter(typeof(TEntity), "e");
      var whereExpression =
            Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Property(itemParameter, "Id"), Expression.Constant(id)),
                new[] { itemParameter }
              );

      return dataContext.GetTable<TEntity>().Where(whereExpression).FirstOrDefault();
    }

    /// <summary>
    /// Update associative table entities
    /// </summary>
    public static void UpdateAssociations<TAssociation, TFirst, TSecond>(
                            System.Data.Linq.DataContext dataContext,
                            EntitySet<TAssociation> oldAssociations,
                            EntitySet<TAssociation> newAssociations)
      where TAssociation : class, IAssociation<TFirst, TSecond>
      where TFirst : class, IEntityWithId
      where TSecond : class, IEntityWithId
    {
      var associationsToDelete = oldAssociations.Where(ms => newAssociations.All(e => e.Second.Id != ms.Second.Id)).ToList();
      foreach (var association in associationsToDelete)
        dataContext.GetTable<TAssociation>().DeleteOnSubmit(association);

      var associationsToUpdate = oldAssociations.Where(ms => newAssociations.Any(e => e.Second.Id == ms.Second.Id)).ToList();
      foreach (var association in associationsToUpdate)
      {
        var updated = newAssociations.First(ms => ms.First.Id == association.First.Id);
        if (association is IUpdatable)
          ((IUpdatable)association).UpdateFrom(updated, dataContext);
      }

      var associationToAdd = newAssociations.Where(ms => oldAssociations.All(e => e.Second.Id != ms.Second.Id)).ToList();
      foreach (var association in associationToAdd)
      {
        association.Second = DataUtils.GetById<TSecond>(dataContext, association.Second.Id);
        oldAssociations.Add(association);
      }
    }

    /// <summary>
    /// Renew entity from a collection with or without update
    /// </summary>
    public static TEntity Rejuvenate<TEntity>(IEnumerable<TEntity> fromColl, TEntity item, bool update = true, System.Data.Linq.DataContext dataContext = null)
      where TEntity : class, IEntityWithId, IUpdatable<TEntity>
    {
      if (item == null)
        return null;

      if (fromColl == null || !fromColl.Any())
        return item;

      var source = fromColl.FirstOrDefault(e => e.Id == item.Id);
      if (source == null)
        return item;

      if (update)
        source.UpdateFrom(item, dataContext);

      return source;
    }
  }
}
