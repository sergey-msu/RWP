using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RWP.App.Resources
{
  public partial class Styles : ResourceDictionary
  {
    public Styles()
    {
      InitializeComponent();
    }

    private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      var tabItem = e.Source as TabItem;

      if (tabItem == null)
        return;

      if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
      {
        DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
      }
    }


    private void TabItem_Drop(object sender, DragEventArgs e)
    {
      var tabItemTarget = e.Source as TabItem;

      var formats = e.Data.GetFormats();
      if (formats == null || !formats.Any())
        return;

      var format = formats[0];
      var tabItemSource = e.Data.GetData(format) as TabItem;

      if (tabItemTarget != null && !tabItemTarget.Equals(tabItemSource))
      {
        var tabControl = tabItemTarget.Parent as TabControl;
        int sourceIndex = tabControl.Items.IndexOf(tabItemSource);
        int targetIndex = tabControl.Items.IndexOf(tabItemTarget);

        tabControl.Items.Remove(tabItemSource);
        tabControl.Items.Insert(targetIndex, tabItemSource);

        tabControl.Items.Remove(tabItemTarget);
        tabControl.Items.Insert(sourceIndex, tabItemTarget);

        tabControl.SelectedItem = tabItemSource;
      }
    }
  }
}
