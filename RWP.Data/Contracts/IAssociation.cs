namespace RWP.Data.Contracts
{
  /// <summary>
  /// Represents entity of association table
  /// </summary>
  public interface IAssociation<TFirst, TSecond>
    where TFirst : IEntityWithId
    where TSecond : IEntityWithId
  {
    /// <summary>
    /// First part entity of association
    /// </summary>
    TFirst First { get; set; }

    /// <summary>
    /// Second part entity of association
    /// </summary>
    TSecond Second { get; set; }
  }
}
