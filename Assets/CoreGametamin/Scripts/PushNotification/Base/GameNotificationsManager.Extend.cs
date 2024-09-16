using UnityEngine;
using Unity.Notifications;

namespace NotificationSamples
{
    /// <summary>
    /// Global notifications manager that serves as a wrapper for multiple platforms' notification systems.
    /// </summary>
    public partial class GameNotificationsManager : MonoBehaviour
    {
        public static void OpenSettings()
        {
            NotificationCenter.OpenNotificationSettings();
        }
    }
}
