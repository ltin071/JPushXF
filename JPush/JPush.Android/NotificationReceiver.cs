using Android.App;
using Android.Content;
using Android.OS;
using CN.Jpush.Android.Api;
using CN.Jpush.Android.Service;
using Plugin.LocalNotifications;

namespace JPush.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "cn.jpush.android.intent.REGISTRATION" }, Categories = new string[] { "com.companyname.JPush" })]
    [IntentFilter(new string[] { "cn.jpush.android.intent.MESSAGE_RECEIVED" }, Categories = new string[] { "com.companyname.JPush" })]
    [IntentFilter(new string[] { "cn.jpush.android.intent.NOTIFICATION_RECEIVED" }, Categories = new string[] { "com.companyname.JPush" })]
    [IntentFilter(new string[] { "cn.jpush.android.intent.NOTIFICATION_OPENED" }, Categories = new string[] { "com.companyname.JPush" })]
    [IntentFilter(new string[] { "cn.jpush.android.intent.CONNECTION" }, Categories = new string[] { "com.companyname.JPush" })]
    class NotificationReceiver : PushReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);
            if (intent.Action == JPushInterface.ActionNotificationReceived)
            {
                //When user tap the notification on notification center
                Bundle bundle = intent.Extras;
                string notificationData = bundle.GetString(JPushInterface.ExtraAlert);
                CrossLocalNotifications.Current.Show("JPush", notificationData, 101);
            }
            if (intent.Action == JPushInterface.ActionRegistrationId)
            {
                //Only call when first launch, get the registrationID
                string regID = JPushInterface.GetRegistrationID(context);
            }

        }
    }
}