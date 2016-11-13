using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class Patient : IEntityWithId, IUpdatable<Patient>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((Patient)other, dataContext);
    }

    public void UpdateFrom(Patient other, System.Data.Linq.DataContext dataContext)
    {
      this.FirstName = other.FirstName;
      this.MiddleName = other.MiddleName;
      this.LastName = other.LastName;
      this.DOB = other.DOB;
      this.Sex = other.Sex;
      this.Note = other.Note;
    }
  }
}
