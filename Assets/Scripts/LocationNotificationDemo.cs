using System;
using System.Collections;
using Unity.Notifications.iOS;
using UnityEngine;

namespace Gringlas.UnityIOsPushnotificationsDemo
{

    public class LocationNotificationDemo : MonoBehaviour
    {
        IEnumerator Start()
        {
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
                yield break;

            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            }

            // Stop service if there is no need to query location updates continuously
            //Input.location.Stop();

            using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };

                string result = "\n RequestAuthorization: \n";
                result += "\n finished: " + req.IsFinished;
                result += "\n granted :  " + req.Granted;
                result += "\n error:  " + req.Error;
                result += "\n deviceToken:  " + req.DeviceToken;
                Debug.Log(result);

            }

        }

            // Update is called once per frame
        void Update()
        {
            sendLocationNotification();
        }


        public void sendTimeIntervalNotifcation()
        {
            var timeIntervalTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, 5),
                Repeats = false
            };
            var notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be 
                // used to cancel the notification, if you don't set one, a unique 
                // string will be generated automatically.
                Title = "TimeInterval",
                Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
                Subtitle = "This is a subtitle, something, something important...",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeIntervalTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void sendLocationNotification()
        {
            Debug.Log("sendLocationNotification");
            Debug.Log("My Location: " + Input.location.lastData.latitude + "/" + Input.location.lastData.longitude);
            var locationTrigger = new iOSNotificationLocationTrigger()
            {
                Center = new Vector2(2.294498f, 48.858263f),
                Radius = 250f,
                NotifyOnEntry = true,
                NotifyOnExit = true
            };
            var notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be 
                // used to cancel the notification, if you don't set one, a unique 
                // string will be generated automatically.
                Title = "Location",
                Body = "Wir sind in Paris",
                Subtitle = "Wie kommen wir denn hier hin :D",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread2",
                Trigger = locationTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

    }

}