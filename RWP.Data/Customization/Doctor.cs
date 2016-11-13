using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class Doctor : IEntityWithId, IUpdatable<Doctor>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((Doctor)other, dataContext);
    }

    public void UpdateFrom(Doctor other, System.Data.Linq.DataContext dataContext)
    {
      this.FirstName = other.FirstName;
      this.MiddleName = other.MiddleName;
      this.LastName = other.LastName;
      this.Position = other.Position;
      this.Print = other.Print;
      this.Note = other.Note;
    }
  }
}
