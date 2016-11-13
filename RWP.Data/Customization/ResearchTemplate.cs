using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class ResearchTemplate : IEntityWithId, IUpdatable<ResearchTemplate>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((ResearchTemplate)other, dataContext);
    }

    public void UpdateFrom(ResearchTemplate other, System.Data.Linq.DataContext dataContext)
    {
      this.Name = other.Name;
      this.Content = other.Content;
    }
  }
}
