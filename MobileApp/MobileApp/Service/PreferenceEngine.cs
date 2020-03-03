using CoreEngine.Model.DBModel;
using Mobile.Core.Engines.Services;
using Plugin.FilePicker;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileApp.Service
{
    public class PreferenceEngine : IPreferenceEngine
    {
        public string GetSetting(string key, string value)
        {
            return Preferences.Get(key, value);
        }

        public void SetSetting(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public async Task<DBFile> PickFile()
        {
            var fileData = await CrossFilePicker.Current.PickFile();
            if (fileData == null)
            {
                return null;
            }
            else
            {
                return new DBFile(fileData.GetStream(), fileData.FileName);
            }
        }
    }
}
