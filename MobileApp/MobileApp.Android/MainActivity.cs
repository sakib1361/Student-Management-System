using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Java.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mobile.Core.Engines.Services;
using MobileApp.Droid.Services;
using System;
using Xamarin.Forms;

namespace MobileApp.Droid
{
    //yoKOOpnaexb8JyrClG6ts00MiYU=
    //sakib.buet51@gmail.com, 25 Apr, 2019, PrivateKeyEntry,
    //Certificate fingerprint(SHA1): CA:82:8E:3A:99:DA:7B:16:FC:27:2A:C2:94:6E:AD:B3:4D:0C:89:85

    [Activity(Label = "IIT", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            //global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            FormsMaterial.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CreateNotificationChannel();

            //PrintId();
            LoadApplication(App.Init(ConfigureServices));
        }

        private void PrintId()
        {
            try
            {
                PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());

                    var finalId = Convert.ToBase64String(md.Digest());
                    System.Diagnostics.Debug.WriteLine(finalId);
                }
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        internal const string CHANNEL_ID = "IIT_notification";
        internal const string CHANNEL_NAME = "IITNotificationChannel";
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }
            var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default)
            {
                Description = CHANNEL_NAME
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var platform = new PlatformService(this);
            services.AddSingleton<IPlatformService>(platform);
        }
    }
}