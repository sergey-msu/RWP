using System;
using System.Data.Linq;
using RWP.Data.Contracts;

namespace RWP.Data
{
  public partial class Attachment : IEntityWithId, IUpdatable<Attachment>
  {
    void IUpdatable.UpdateFrom(object other, System.Data.Linq.DataContext dataContext)
    {
      UpdateFrom((Attachment)other, dataContext);
    }

    public void UpdateFrom(Attachment other, System.Data.Linq.DataContext dataContext)
    {
      this.Name = other.Name;
      this.Type = other.Type;
      this.Note = other.Note;
    }
  }
}
