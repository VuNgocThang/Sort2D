using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif
using System;
public class NotificationController : MonoBehaviour
{
    void Start()
    {
        InitializeNotifications();
    }
    private void InitializeNotifications()
    {
#if UNITY_ANDROID
        InitializeAndroidNotifications();
#elif UNITY_IOS
        StartCoroutine(RequestAuthorizationIOS());
#endif
    }
#if UNITY_ANDROID
    // Initialize Android notifications
    private void InitializeAndroidNotifications()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        SendNotif();
    }
#endif
#if UNITY_IOS
    private IEnumerator RequestAuthorizationIOS()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
        var request = new AuthorizationRequest(authorizationOption, true);
        while (!request.IsFinished)
        {
            yield return null;
        }
        if (request.Granted)
        {
            SendNotif();
            Debug.Log("iOS Notification Authorization granted.");
        }
        else
        {
            Debug.Log($"iOS Notification Authorization denied. Error: {request.Error}");
        }
    }
#endif
    public void SendNotif()
    {
        DateTime am12 = DateTime.Today.AddHours(12);
        DateTime pm21 = DateTime.Today.AddHours(21);
        DateTime now = DateTime.Now;
        int hour12 = 12 - now.Hour;
        int hour21 = 21 - now.Hour;
        if (hour12 >= 4)
        {
            ScheduleNotification(am12);
        }
        else
        if (hour21 >= 4)
        {
            ScheduleNotification(pm21);
        }
        for (int i = 0; i < 7; i++)
        {
            int dayBonus = i + 1;
            ScheduleNotification(DateTime.Today.AddDays(dayBonus).AddHours(12));
            ScheduleNotification(DateTime.Today.AddDays(dayBonus).AddHours(21));
        }
        ScheduleNotification(DateTime.Today.AddDays(14).AddHours(12));
        ScheduleNotification(DateTime.Today.AddDays(14).AddHours(21));
        ScheduleNotification(DateTime.Today.AddDays(28).AddHours(12));
        ScheduleNotification(DateTime.Today.AddDays(28).AddHours(21));
    }
    public void ScheduleNotification(DateTime dataTimeShow)
    //public void ScheduleNotification(int secondsFromNow)
    {
        double secondsFromNow = (dataTimeShow - DateTime.Now).TotalSeconds;
        //Debug.Log(secondsFromNow+" ------------------  ");   
        string title = "";
        string message = "";
        int index = UnityEngine.Random.Range(1, 4);
        if (index == 1)
        {
            title = "The new day has come🌞";
            message = "Claim your money💸";
        }
        else if (index == 2)
        {
            title = "Time to feed the cat 🐈🙀";
            message = "Let's go now, fishy! 🐾🐾";
        }
        else if (index == 3)
        {
            title = "Busy hour! Help me 🥺️📓";
            message = "Buzz buzz ❣️💌";
        }
#if UNITY_ANDROID
        var notification = new AndroidNotification
        {
            Title = title,
            Text = message,
            SmallIcon = "icon_0", // replace with your own icon
            FireTime = DateTime.Now.AddSeconds(secondsFromNow),
        };
        //notification.IntentData = "UnityActivity";
        AndroidNotificationCenter.SendNotification(notification, "default_channel");
#elif UNITY_IOS
        var notification = new iOSNotification
        {
            Identifier = "notification_" + Guid.NewGuid().ToString(),
            Title = title,
            Body = message,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
            Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, secondsFromNow),
                Repeats = false,
            }
        };
        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }
    public void ClearAllNotifications()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
    }
}