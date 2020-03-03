using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class TodoItemsViewModel : BaseViewModel
    {
        private readonly ITodoItemHandler _todoItemHandler;
        public ObservableCollection<ToDoItem> ToDoItems { get; set; }
        private int page = 0;
        private bool CanLoadMore;

        public TodoItemsViewModel(ITodoItemHandler todoItemHandler)
        {
            _todoItemHandler = todoItemHandler;
            ToDoItems = new ObservableCollection<ToDoItem>();
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            IsRefreshisng = true;
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            page = 0;
            ToDoItems.Clear();
            await LoadItems();
            IsRefreshisng = false;
        }

        private async Task LoadItems()
        {
            IsBusy = true;
            var resp = await _todoItemHandler.GetToDoItems(page);
            if (resp == null)
            {
                CanLoadMore = false;
            }
            else
            {
                CanLoadMore = resp.Count > 5;
                page++;
                foreach(var item in resp)
                {
                    item.Update();
                    ToDoItems.Add(item);
                }
            }
        }

        public ICommand AddCommand => new RelayCommand(AddAction);
        public ICommand SelectItemCommand => new RelayCommand<ToDoItem>(SelectItemAction);
        public ICommand LoadMoreCommand => new RelayCommand(LoadMoreAction);

        private async void LoadMoreAction()
        {
            if (CanLoadMore && !IsBusy)
            {
                await LoadItems();
            }
        }

        private void SelectItemAction(ToDoItem obj)
        {
            var actions = new Dictionary<string, Action>
            {
                { "Edit Item", () => EditItem(obj) },
                { "Delete", () => DeleteItem(obj) }
            };
            _dialog.ShowAction("Select Action", "Cancel", actions);
        }

        private async void DeleteItem(ToDoItem obj)
        {
            if (obj.OwnerId == AppService.CurrentUser.Id)
            {
                var confirm = await _dialog.ShowConfirmation("Confirm?", "Are you sure to delete this item?");
                if (confirm)
                {
                    var resp = await _todoItemHandler.DeleteToDoItem(obj);
                    if (resp != null && resp.Actionstatus)
                    {
                        ToDoItems.Remove(obj);
                    }

                    ShowResponse(resp);
                }
            }
        }

        private void EditItem(ToDoItem obj)
        {
            _nav.NavigateTo<TodoAddViewModel>(obj);
        }

        private void AddAction()
        {
            _nav.NavigateTo<TodoAddViewModel>(new ToDoItem());
        }
    }
}
