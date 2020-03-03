using CoreEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface IDialogService : IToastService
    {

        Task<bool> ShowConfirmation(string title, string meaage);
        Task<string> ShowAction(string title, string cancel, params string[] actions);
        void ShowAction(string title, string cancel, Dictionary<string, Action> actions);
    }
}
