using System.Linq;
using RWP.Data.Contracts;

namespace RWP.Data.Filters
{
  /// <summary>
  /// Base class of DB filter
  /// </summary>
  public abstract class FilterBase<TEntity>
    where TEntity : IEntityWithId
  {
    private const int DEFAULT_SKIP = 0;
    private const int DEFAULT_TAKE = 20;
    protected const char LIKE = '%';
    protected const string FULL_NAME_FORMAT = "{0} {1} {2}";

    protected FilterBase()
    {
      Skip = DEFAULT_SKIP;
      Take = DEFAULT_TAKE;
    }

    #region Properties

    /// <summary>
    /// Number of entries to skip
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// Number of entries to take
    /// </summary>
    public int Take { get; set; }

    #endregion Properties

    /// <summary>
    /// Apply filter to queryable result
    /// </summary>
    public IQueryable<TEntity> Apply(IQueryable<TEntity> source, bool bounding = true)
    {
      var result = DoApply(source);

      if (bounding)
      {
        // Orderding
        result = result.OrderByDescending(p => p.Id);

        // Bounding
        result = result.Skip(this.Skip).Take(this.Take);
      }

      return result;
    }

    /// <summary>
    /// Apply operation implementation
    /// </summary>
    protected abstract IQueryable<TEntity> DoApply(IQueryable<TEntity> source);

  }
}