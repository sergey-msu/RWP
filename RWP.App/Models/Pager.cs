using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App.Models
{
  public class Pager : NotifierBase
  {
    private readonly string TEXT_FORMAT = Utils.GetResource("PagerTextFormat");

    public Pager()
    {
      Page = 1;
    }

    private int _total;
    public int Total
    {
      get { return _total; }
      set
      {
        _total = value;
        Refresh();
      }
    }

    private int _step;
    public int Step
    {
      get { return _step; }
      set
      {
        _step = value;
        Page = 1;
        Refresh();
      }
    }

    public int Page { get; private set; }

    public bool CanNext
    {
      get { return Page * Step < Total; }
    }

    public bool CanBack
    {
      get { return Page > 1; }
    }

    public int TotalPages
    {
      get { return Step == 0 ? int.MaxValue : Total / Step + 1; }
    }

    public string Text
    {
      get { return string.Format(TEXT_FORMAT, Page, TotalPages); }
    }


    public void Next()
    {
      if (!CanNext)
        return;

      Page++;
      Refresh();
    }

    public void Back()
    {
      if (!CanBack)
        return;

      Page--;
      Refresh();
    }

    public void Reset()
    {
      Page = 1;
      Refresh();
    }

    private void Refresh()
    {
      OnPropertyChanged("Text");
      OnPropertyChanged("CanNext");
      OnPropertyChanged("CanBack");
    }
  }
}
