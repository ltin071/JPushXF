
using Android.App;
using Android.Content.PM;
using Android.OS;
using CN.Jpush.Android.Api;
using Android.Content;

namespace JPush.Droid
{
    [Activity(Label = "JPush", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            initPushNotification();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        private void initPushNotification()
        {
            IntentFilter filter = new IntentFilter();
            filter.AddAction(JPushInterface.ActionNotificationOpened);
            filter.AddAction(JPushInterface.ActionNotificationReceived);
            filter.AddAction(JPushInterface.ActionMessageReceived);
            filter.AddAction(JPushInterface.ActionRegistrationId);
            filter.AddAction(JPushInterface.ActionConnectionChange);
            NotificationReceiver receiver = new NotificationReceiver();
            RegisterReceiver(receiver, filter);
            JPushInterface.SetDebugMode(true);
            JPushInterface.Init(this.ApplicationContext);
        }
    }
}