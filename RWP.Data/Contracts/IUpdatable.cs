namespace RWP.Data.Contracts
{
  /// <summary>
  /// Represents entity which can be updated from other
  /// </summary>
  public interface IUpdatable
  {
    void UpdateFrom(object other, System.Data.Linq.DataContext dataContext);
  }

  /// <summary>
  /// Represents entity which can be updated from other
  /// </summary>
  public interface IUpdatable<T> : IUpdatable
  {
    void UpdateFrom(T other, System.Data.Linq.DataContext dataContext);
  }
}
