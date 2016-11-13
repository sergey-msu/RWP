using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using RWP.Data.Contracts;
using RWP.Data.Repositories;
using RWP.App.Views;
using RWP.Data.Filters;

namespace RWP.App.Infrastructure.Mvvm
{
  public abstract class ItemListViewModelBase<TEntity, TItem, TRepository> : ViewModelBase
    where TEntity : class, IEntityWithId, IUpdatable<TEntity>, new()
    where TItem : ModelBase<TEntity>, INamed
    where TRepository : RepositoryBase<TEntity>, new()
  {
    private readonly string DELETE_ITEM_MESSAGE = Utils.GetResource("AreYouSureToDeleteItemMessage");
    protected readonly string ITEM_IS_SYSTEM_MESSAGE = Utils.GetResource("ItemIsSystemMessage");
    private const int FILTER_TIMESPAN = 1;

    private DispatcherTimer _filterTimer;


    private readonly TRepository _repository;

    public ItemListViewModelBase()
    {
      _repository = new TRepository();

      CreateItemCommand = new DelegateCommand(CreateItem);
      DeleteItemCommand = new DelegateCommand(DeleteItem, CanDeleteItem);
      EditItemCommand = new DelegateCommand(EditItem, CanEditItem);
      SaveCommand = new DelegateCommand(Save);
      CancelCommand = new DelegateCommand(Cancel);
      ClearFilterCommand = new DelegateCommand(ClearFilter);

      InitFilter();

      ItemsOnScreen = Constants.ITEMS_ON_SCREEN.First();

      Refresh();
    }

    #region Properties

    public abstract string Header { get; }

    private bool _isEdit;
    public bool IsEdit
    {
      get { return _isEdit; }
      set
      {
        _isEdit = value;
        OnPropertyChanged("IsEdit");
      }
    }

    private List<TItem> _items;
    public List<TItem> Items
    {
      get { return _items; }
      set
      {
        _items = value;
        OnPropertyChanged("Items");
      }
    }

    private TItem _selectedItem;
    public TItem SelectedItem
    {
      get { return _selectedItem; }
      set
      {
        BeforeSelectedItemChanged();
        _selectedItem = value;
        RaiseCanCommandsExecute();
        OnPropertyChanged("SelectedItem");
      }
    }

    private string _filter;
    public string Filter
    {
      get { return _filter; }
      set
      {
        _filter = value;
        OnFilterChanged();
        OnPropertyChanged("Filter");
      }
    }

    private int _itemsOnScreen;
    public int ItemsOnScreen
    {
      get { return _itemsOnScreen; }
      set
      {
        _itemsOnScreen = value;
        ExecFilter();
        OnPropertyChanged("ItemsOnScreen");
      }
    }

    protected virtual TRepository ItemRepository { get { return _repository; } }

    #endregion Properties

    #region ClearFilterCommand

    public ICommand ClearFilterCommand { get; private set; }

    private void ClearFilter()
    {
      Filter = null;
      ExecFilter();
    }

    #endregion ClearFilterCommand

    #region CreateItemCommand

    public ICommand CreateItemCommand { get; private set; }

    protected virtual void CreateItem()
    {
      try
      {
        var entity = new TEntity();
        var model = CreateItem(entity);
        SelectedItem = model;

        IsEdit = true;
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    #endregion CreateItemCommand

    #region EditItemCommand

    public DelegateCommand EditItemCommand { get; private set; }

    protected virtual void EditItem()
    {
      try
      {
        EditItemImpl();
        IsEdit = true;
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    protected virtual void EditItemImpl()
    {
      if (SelectedItem == null)
        throw new InvalidOperationException("Incorrect mode: no selected Item");

      IsEdit = true;
    }

    protected virtual bool CanEditItem()
    {
      return SelectedItem != null;
    }

    #endregion EditItemCommand

    #region DeleteItemCommand

    public DelegateCommand DeleteItemCommand { get; private set; }

    private void DeleteItem()
    {
      try
      {
        string message;
        if (!CheckCanDelete(out message))
        {
          RwpMessageBox.Show(message);
          return;
        }

        var result = RwpMessageBox.Show(DELETE_ITEM_MESSAGE, buttons: MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes)
          return;

        DeleteItemImpl();
        Refresh();
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    protected virtual void DeleteItemImpl()
    {
      ItemRepository.Delete(SelectedItem.Entity);
    }

    protected virtual bool CanDeleteItem()
    {
      return SelectedItem != null;
    }

    protected virtual bool CheckCanDelete(out string message)
    {
      message = null;
      return true;
    }

    #endregion DeleteItemCommand

    #region SaveCommand

    public DelegateCommand SaveCommand { get; private set; }

    protected void Save()
    {
      try
      {
        if (SelectedItem == null || !IsEdit)
          throw new RwpException("Incorrect mode: no selected doctor or mode is not edit");

        if (!SelectedItem.Validator.IsValid)
        {
          RwpMessageBox.Show(VALIDATION_ERRORS_MESSAGE);
          return;
        }

        var id = SaveImpl();
        Refresh();

        SelectedItem = Items.FirstOrDefault(t => t.Entity.Id == id);
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    protected virtual int SaveImpl()
    {
      SelectedItem.SaveToSource();
      int id = ItemRepository.Upsert(SelectedItem.Entity);
      return id;
    }

    #endregion SaveCommand

    #region CancelCommand

    public DelegateCommand CancelCommand { get; private set; }

    protected virtual void Cancel()
    {
      try
      {
        if (SelectedItem == null || !IsEdit)
          throw new InvalidOperationException("Incorrect mode: no selected doctor or mode is not edit");

        CancelImpl();
        IsEdit = false;
        SelectedItem = Items.FirstOrDefault(t => t.Entity.Id == SelectedItem.Entity.Id);
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    protected virtual void CancelImpl()
    {
      SelectedItem.ResetFromSource();
    }

    #endregion CancelCommand

    #region Overrides

    public override bool Execute(string command, params object[] args)
    {
      if (command == Constants.COMMAND_REFRESH)
      {
        Refresh();
        return true;
      }

      return true;
    }

    public override bool OnNavigatedFrom()
    {
      if (IsEdit)
      {
        RwpRoot.Show<RwpTabBase>(this.HostID);
        var result = RwpMessageBox.Show(EDIT_IN_PROGRESS_MESSAGE, buttons: MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes)
          return false;
      }

      if (SelectedItem != null)
      {
        if (SelectedItem.IsDirty)
        {
          RwpRoot.Show<RwpTabBase>(this.HostID);
          var result = RwpMessageBox.Show(EDIT_IN_PROGRESS_MESSAGE, buttons: MessageBoxButton.YesNo);
          if (result != MessageBoxResult.Yes)
            return false;
        }

        SelectedItem.ResetFromSource();
      }

      return base.OnNavigatedFrom();
    }

    #endregion Overrides

    #region Protected

    protected virtual TItem CreateItem(TEntity entity)
    {
      return (TItem)Activator.CreateInstance(typeof(TItem), new object[] { entity });
    }

    protected virtual void Refresh()
    {
      try
      {
        if (SelectedItem != null && SelectedItem.IsDirty)
        {
          RwpRoot.Show<RwpTabBase>(this.HostID);
          var result = RwpMessageBox.Show(EDIT_IN_PROGRESS_MESSAGE, buttons: MessageBoxButton.YesNo);
          if (result != MessageBoxResult.Yes)
            return;
        }

        var selected = SelectedItem;

        LoadItems();

        if (selected != null)
        {
          SelectedItem = Items.FirstOrDefault(e => e.Entity.Id == selected.Entity.Id);
        }
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    protected virtual void LoadItems()
    {
      var items = new List<TItem>();

      var filter = GetFilter();
      filter.Take = ItemsOnScreen;
      var entities = ItemRepository.GetEntities(filter);
      foreach (var item in entities)
      {
        var model = CreateItem(item);
        items.Add(model);
      }

      Items = items;
    }

    protected abstract FilterBase<TEntity> GetFilter();

    protected virtual void BeforeSelectedItemChanged()
    {
      try
      {
        IsEdit = false;
        if (SelectedItem != null)
          SelectedItem.ResetFromSource();
      }
      catch (Exception ex)
      {
        RwpRoot.ErrorHandler.Handle(ex);
      }
    }

    protected virtual void RaiseCanCommandsExecute()
    {
      if (DeleteItemCommand != null) DeleteItemCommand.RaiseCanExecuteChanged();
      if (EditItemCommand != null) EditItemCommand.RaiseCanExecuteChanged();
    }

    #endregion Protected

    #region Private

    private void InitFilter()
    {
      _filterTimer = new DispatcherTimer
      {
        Interval = TimeSpan.FromSeconds(FILTER_TIMESPAN),
        IsEnabled = false
      };
      _filterTimer.Tick += FilterTimerTick;
    }

    private void ExecFilter()
    {
      _filterTimer.Tick -= FilterTimerTick;
      LoadItems();
    }

    private void OnFilterChanged()
    {
      _filterTimer.Stop();
      _filterTimer.Tick -= FilterTimerTick;
      _filterTimer.Tick += FilterTimerTick;
      _filterTimer.Start();
    }

    private void FilterTimerTick(object sender, EventArgs e)
    {
      ExecFilter();
    }


    #endregion Private
  }
}
