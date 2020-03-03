namespace CoreEngine.Helpers
{
    public interface IToastService
    {
        void ShowToastMessage(string message);
        void ShowMessage(string title, string message);
    }
}
