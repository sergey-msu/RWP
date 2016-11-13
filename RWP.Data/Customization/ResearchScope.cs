using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class ResearchScope : IEntityWithId, IUpdatable<ResearchScope>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((ResearchScope)other, dataContext);
    }

    public void UpdateFrom(ResearchScope other, System.Data.Linq.DataContext dataContext)
    {
      this.Name = other.Name;
    }
  }
}
