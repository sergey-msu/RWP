using System.Windows;
using RWP.App.Infrastructure;
using RWP.App.Infrastructure.Mvvm;

namespace RWP.App.Views
{
  public abstract partial class ItemListTabBase : RwpTabBase
  {
    public ItemListTabBase(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();

      if (DetailsView != null)
      {
        _details.Content = DetailsView;
      }
    }

    public virtual FrameworkElement DetailsView { get { return null; } }
  }

  public abstract partial class ItemListTabBase<TView> : ItemListTabBase
    where TView : FrameworkElement, new()
  {
    public ItemListTabBase(TabManager manager, string id) : base(manager, id)
    {
      InitializeComponent();
    }

    private TView _detailsView;
    public override FrameworkElement DetailsView
    {
      get
      {
        if (_detailsView == null)
          _detailsView = new TView();

        return _detailsView;
      }
    }
  }
}
