using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class Customer : IEntityWithId, IUpdatable<Customer>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((Customer)other, dataContext);
    }

    public void UpdateFrom(Customer other, System.Data.Linq.DataContext dataContext)
    {
      this.Name = other.Name;
      this.Address = other.Address;
      this.ResearchPlace = other.ResearchPlace;
      this.ContactName = other.ContactName;
      this.ContactEMail = other.ContactEMail;
      this.Note = other.Note;
    }
  }
}
