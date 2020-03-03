using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class TodoAddViewModel : BaseViewModel
    {
        private readonly ITodoItemHandler _todoHandler;
        private readonly IMemberHandler _memberHandler;

        public List<DBUser> AllUsers { get; set; }
        public ToDoItem CurrentItem { get; private set; }
        public DateTime CurrentDate { get; set; }
        public TimeSpan CurrentTime { get; set; }

        public TodoAddViewModel(ITodoItemHandler todoItemHandler, IMemberHandler memberHandler)
        {
            _todoHandler = todoItemHandler;
            _memberHandler = memberHandler;
            CurrentDate = DateTime.Today;
            CurrentTime = TimeSpan.FromHours(15);
        }

        

        public async override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args.Length > 0 && args[0] is ToDoItem toDo)
            {
                var data = await _memberHandler.GetCurrentBatchUsers();
                if(data == null)
                {
                    AllUsers = new List<DBUser>();
                }
                else
                {
                    data.Remove(data.FirstOrDefault(x => x.Id == AppService.CurrentUser.Id));
                    AllUsers = data;
                }
                CurrentItem = toDo;
                if (CurrentItem.Id > 0)
                {
                    CurrentDate = CurrentItem.EventTime.Date ;
                    CurrentTime = CurrentItem.EventTime.TimeOfDay;
                }
            }
            else GoBack();
        }

        public ICommand SaveCommand => new RelayCommand(SaveAction);
        public ICommand UserSelectCommand => new RelayCommand<DBUser>(UserSelectAction);

        private void UserSelectAction(DBUser obj)
        {
            obj.IsChecked = !obj.IsChecked;
            obj.NotifyChange(nameof(DBUser.IsChecked));
        }

        private async void SaveAction()
        {
            if (string.IsNullOrWhiteSpace(CurrentItem.Title))
                _dialog.ShowMessage("Error", "Invalid Title");
            else if (string.IsNullOrWhiteSpace(CurrentItem.Message))
                _dialog.ShowMessage("Error", "Invalid Message");
            else
            {
                CurrentItem.EventTime = CurrentDate.Add(CurrentTime);
                CurrentItem.ParticementUserIds = AllUsers.Where(x => x.IsChecked).Select(m => m.Id).ToList();
                CurrentItem.ParticementUserIds.Add(AppService.CurrentUser.Id);
                CurrentItem.OwnerId = AppService.CurrentUser.Id;
                IsBusy = true;
                if(CurrentItem.Id == 0)
                {
                    var resp = await _todoHandler.AddToDoItem(CurrentItem);
                    ShowResponse(resp, true);
                }
                else
                {
                    var resp = await _todoHandler.UpdateToDoItem(CurrentItem);
                    ShowResponse(resp, true);
                }
                IsBusy = false;
            }
        }
    }
}
