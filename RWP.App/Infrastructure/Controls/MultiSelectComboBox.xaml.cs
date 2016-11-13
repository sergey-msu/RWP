using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace RWP.App.Infrastructure.Controls
{
  /// <summary>
  /// Interaction logic for MultiSelectComboBox.xaml
  /// </summary>
  public partial class MultiSelectComboBox : UserControl
  {
    static MultiSelectComboBox()
    {
      BackgroundProperty.OverrideMetadata(typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(Brushes.White));
    }

    public MultiSelectComboBox()
    {
      InitializeComponent();

      this.PreviewMouseWheel += OnPreviewMouseWheel;
    }

    private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (!e.Handled)
      {
        e.Handled = true;
        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArg.RoutedEvent = UIElement.MouseWheelEvent;
        eventArg.Source = sender;
        var parent = ((Control)sender).Parent as UIElement;
        parent.RaiseEvent(eventArg);
      }
    }

    /// <summary>
    /// Коллекция элементов
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
      "ItemsSource",
      typeof(IEnumerable),
      typeof(MultiSelectComboBox),
      new FrameworkPropertyMetadata(null, OnItemsSourceChanged));

    /// <summary>
    /// Выбранные элементы
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
      "SelectedItems",
      typeof(IList),
      typeof(MultiSelectComboBox),
      new FrameworkPropertyMetadata(new ObservableCollection<object>(), OnSelectedItemsChanged));

    /// <summary>
    /// Путь к отображаемому свойству
    /// </summary>
    public static readonly DependencyProperty DisplayMemberPathProperty =
      DependencyProperty.Register(
        "DisplayMemberPath",
        typeof(string),
        typeof(MultiSelectComboBox),
        new FrameworkPropertyMetadata(default(string), DisplayMemberPathPropertyChangedCallback));

    /// <summary>
    /// Шаблон выбранного элемента
    /// </summary>
    public static readonly DependencyProperty SelectionBoxItemTemplateProperty = DependencyProperty.Register(
        "SelectionBoxItemTemplate",
        typeof(DataTemplate),
        typeof(MultiSelectComboBox),
        new PropertyMetadata(default(DataTemplate)));

    /// <summary>
    /// Шаблон элемента в списке
    /// </summary>
    public static readonly DependencyProperty ItemTemplateProperty =
      DependencyProperty.Register(
        "ItemTemplate",
        typeof(DataTemplate),
        typeof(MultiSelectComboBox),
        new PropertyMetadata(default(DataTemplate)));

    /// <summary>
    /// Шаблон контента кнопки удаления элемента из выбранных
    /// </summary>
    public static readonly DependencyProperty RemoveItemButtonContentTemplateProperty =
      DependencyProperty.Register(
        "RemoveItemButtonContentTemplate",
        typeof(DataTemplate),
        typeof(MultiSelectComboBox));

    /// <summary>
    /// Стиль кнопки удаления элемента из выбранных
    /// </summary>
    public static readonly DependencyProperty RemoveItemButtonStyleProperty =
      DependencyProperty.Register(
        "RemoveItemButtonStyle",
        typeof(Style),
        typeof(MultiSelectComboBox));


    /// <summary>
    /// Обработчик изменения DisplayMemberPath
    /// </summary>
    private static void DisplayMemberPathPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var control = (MultiSelectComboBox)d;
      if (control == null) return;
      control.OnDisplayMemberPathPropertyChanged();
    }

    /// <summary>
    /// Изменились выбранные элементы
    /// </summary>
    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var control = (MultiSelectComboBox)d;
      control.RefreshSource();
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var control = (MultiSelectComboBox)d;
      control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
    }

    /// <summary>
    /// Шаблон выбранного элемента
    /// </summary>
    public DataTemplate SelectionBoxItemTemplate
    {
      get { return (DataTemplate)GetValue(SelectionBoxItemTemplateProperty); }
      set { SetValue(SelectionBoxItemTemplateProperty, value); }
    }

    /// <summary>
    /// Шаблон контента кнопки удаления элемента из выбранных
    /// </summary>
    public DataTemplate RemoveItemButtonContentTemplate
    {
      get { return (DataTemplate)GetValue(RemoveItemButtonContentTemplateProperty); }
      set { SetValue(RemoveItemButtonContentTemplateProperty, value); }
    }

    /// <summary>
    /// Стиль кнопки удаления элемента из выбранных
    /// </summary>
    public Style RemoveItemButtonStyle
    {
      get { return (Style)GetValue(RemoveItemButtonStyleProperty); }
      set { SetValue(RemoveItemButtonStyleProperty, value); }
    }

    /// <summary>
    /// Путь к отображаемому свойству
    /// </summary>
    public string DisplayMemberPath
    {
      get { return (string)GetValue(DisplayMemberPathProperty); }
      set { SetValue(DisplayMemberPathProperty, value); }
    }

    /// <summary>
    /// Источник элементов
    /// </summary>
    public IEnumerable ItemsSource
    {
      get { return (IEnumerable)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Шаблон элемента в списке
    /// </summary>
    public DataTemplate ItemTemplate
    {
      get { return (DataTemplate)GetValue(ItemTemplateProperty); }
      set { SetValue(ItemTemplateProperty, value); }
    }

    /// <summary>
    /// Выбранные элементы
    /// </summary>
    public IList SelectedItems
    {
      get { return (IList)GetValue(SelectedItemsProperty); }
      set { SetValue(SelectedItemsProperty, value); }
    }

    /// <summary>
    /// Коллекция элементов комбобокса
    /// </summary>
    private ObservableCollection<object> ComboBoxSource
    {
      get { return (ObservableCollection<object>)ComboBox.ItemsSource; }
      set { ComboBox.ItemsSource = value; }
    }

    /// <summary>
    /// Обработчик изменения DisplayMemberPath
    /// </summary>
    private void OnDisplayMemberPathPropertyChanged()
    {
      if (ItemTemplate == null)
        ItemTemplate = GetDefaultTemplate(DisplayMemberPath);
      if (SelectionBoxItemTemplate == null)
        SelectionBoxItemTemplate = GetDefaultTemplate(DisplayMemberPath);
    }

    /// <summary>
    /// Выбран новый элемент
    /// </summary>
    private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (SelectedItems == null) return;
      foreach (var addedItem in e.AddedItems)
      {
        SelectedItems.Add(addedItem);
        ChangeComboBoxSource(() => ComboBoxSource.Remove(addedItem));
        ComboBox.SelectedItem = null;
      }
    }

    /// <summary>
    /// Получить дефолтный шаблон элемента
    /// Дефолтный шаблон ориентирован на использование DisplayMemberPath
    /// </summary>
    /// <param name="displayMemberPath"></param>
    /// <returns></returns>
    private DataTemplate GetDefaultTemplate(string displayMemberPath)
    {
      var textBlock = new FrameworkElementFactory(typeof(TextBlock));
      textBlock.SetBinding(TextBlock.TextProperty, new Binding(displayMemberPath));
      return new DataTemplate { VisualTree = textBlock };
    }

    /// <summary>
    /// Источник изменился
    /// </summary>
    private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
      var oldCollection = oldValue as INotifyCollectionChanged;
      if (oldCollection != null)
        oldCollection.CollectionChanged -= OnItemsSourceCollectionChanged;

      RefreshSource();

      var newCollection = newValue as INotifyCollectionChanged;
      if (newCollection != null)
        newCollection.CollectionChanged += OnItemsSourceCollectionChanged;
    }


    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
      RefreshSource();
    }


    private void RefreshSource()
    {
      if (SelectedItems != null && ItemsSource != null)
      {
        ChangeComboBoxSource(
          () =>
          ComboBoxSource =
          new ObservableCollection<object>(ItemsSource.OfType<object>().Where(i => !SelectedItems.Contains(i))));
      }
      else if (ItemsSource != null)
      {
        ChangeComboBoxSource(
          () =>
          ComboBoxSource =
          new ObservableCollection<object>(ItemsSource.OfType<object>()));
      }
      else
      {
        ChangeComboBoxSource(
          () =>
          ComboBoxSource = null);
      }

    }

    /// <summary>
    /// Обработка удаления выбранного элемента (сделать невыбранным)
    /// </summary>
    private void OnDeleteItemButtonClick(object sender, RoutedEventArgs e)
    {
      if (SelectedItems == null) return;
      var button = sender as Button;
      if (button == null) return;

      ChangeComboBoxSource(() => ComboBoxSource.Add(button.DataContext));
      SelectedItems.Remove(button.DataContext);
    }

    /// <summary>
    /// Изменить источник элементов комбобокса
    /// </summary>
    private void ChangeComboBoxSource(Action action)
    {
      ComboBox.SelectionChanged -= OnComboBoxSelectionChanged;
      action.Invoke();
      ComboBox.SelectedItem = null;
      ComboBox.SelectionChanged += OnComboBoxSelectionChanged;
    }
  }
}
