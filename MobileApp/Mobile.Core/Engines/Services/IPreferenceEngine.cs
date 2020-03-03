using CoreEngine.Model.DBModel;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface IPreferenceEngine
    {
        string GetSetting(string key, string value);
        void SetSetting(string key, string value);
        Task<DBFile> PickFile();
    }
}
