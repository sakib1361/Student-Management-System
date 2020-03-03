
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace MobileApp.Droid
{
    [Activity(Label = "IIT",
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        Icon = "@drawable/iitlogo",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            StartActivity(mainActivityIntent);
            Finish();
        }
    }
}