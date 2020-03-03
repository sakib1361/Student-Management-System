using Mobile.Core.Engines.Services;
using System.Runtime.CompilerServices;

namespace Mobile.Core.Worker
{
    public class SettingService
    {
        private readonly IPreferenceEngine _preference;

        public SettingService(IPreferenceEngine preferenceEngine)
        {
            _preference = preferenceEngine;
        }

        public string Token
        {
            get => _preference.GetSetting(CallName(), "");
            set => _preference.SetSetting(CallName(), value);
        }
        public string Subscription
        {
            get => _preference.GetSetting(CallName(), "");
            set => _preference.SetSetting(CallName(), "");
        }

        private string CallName([CallerMemberName] string name = "")
        {
            return name;
        }
    }
}
