using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Core.ViewModels
{
    public class StudentsViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;
        public List<DBUser> CurrentStudents { get; set; }

        public StudentsViewModel(IMemberHandler userHandler)
        {
            _memberHandler = userHandler;
        }

        public override void OnAppear(params object[] args)
        {
            RefreshAction();
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            await LoadCurrentStudents();
            IsRefreshisng = false;
        }

        private async Task LoadCurrentStudents()
        {
            IsBusy = true;
            CurrentStudents = await _memberHandler.GetCurrentBatchUsers();
            IsBusy = false;
        }
    }
}
