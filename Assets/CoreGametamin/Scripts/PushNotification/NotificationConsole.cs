
using System;
using System.Collections;
using Gametamin.Core;

namespace NotificationSamples
{
    public partial class NotificationConsole : Gametamin.Core.SingletonBehaviour<NotificationConsole>
    {
        protected GameNotificationsManager _manager;
        protected GameNotificationsManager manager => _manager ??= gameObject.GetOrAddComponentSafe<GameNotificationsManager>();
        public static bool CanSendNotification { get; set; }
        public static void Init(Action callack = null)
        {
#if UNITY_EDITOR
            callack?.Invoke();
#else
            Instance.InitNotification(callack);
#endif
        }
        void InitNotification(Action callack = null)
        {
            StartCoroutine(InitCoroutine(callack));
        }
        IEnumerator InitCoroutine(Action callack)
        {
            yield return manager.Initialize();
            manager.DismissAllNotifications();
            callack?.Invoke();
        }
        public void SendNotification(string title, string body, DateTime deliveryTime)
        {
            SendNotification(title, body, deliveryTime);
        }
        public void SendNotification(string title, string body, DateTime deliveryTime, int id)
        {
            SendNotification(title, body, deliveryTime, null, id);
        }
        public void SendNotification(string title, string body, DateTime deliveryTime, int? badgeNumber = null, int? id = null,
            bool reschedule = false)
        {
            GameNotification notification = manager.CreateNotification();

            if (notification == null)
            {
                return;
            }
            notification.Title = title;
            notification.Body = body;
            if (badgeNumber != null)
            {
                notification.BadgeNumber = badgeNumber.Value;
            }
            if (id != null)
            {
                notification.Id = id.Value;
            }
            PendingNotification notificationToDisplay = manager.ScheduleNotification(notification, deliveryTime);
            notificationToDisplay.Reschedule = reschedule;
        }

        /// <summary>
        /// Cancel a given pending notification
        /// </summary>
        public void CancelPendingNotificationItem(PendingNotification itemToCancel)
        {
            manager.CancelNotification(itemToCancel.Notification.Id.Value);
        }

        public static void OpenSettings()
        {
            Unity.Notifications.NotificationCenter.OpenNotificationSettings();
        }
    }
}
