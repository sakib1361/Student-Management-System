using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;

namespace Mobile.Core.ViewModels
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;
        public bool HasResult { get; set; }
        public List<SemesterData> SemesterDatas { get; set; }
        public GradesViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }
        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            IsRefreshisng = true;
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            SemesterDatas = await _courseHandler.GetResult();
            HasResult = SemesterDatas.Count > 0;
            IsRefreshisng = false;
        }
    }
}
