using System;
using Archimedes.Patterns.Notifications;
using Archimedes.Patterns;
using Archimedes.Services.WPF.WorkBenchServices;
using Archimedes.Services.WPF.WorkBenchServices.MessageBox;
using Archimedes.Patterns.Services;


namespace Archimedes.Services.WPF.Notifications
{
    public static class NotificationPresenter
    {

        /// <summary>
        /// Show this Notification in the current std dispatcher thread
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="title"></param>
        public static void PopulateNotificationsSync(UserNotification nc, string title = "") {
            PopulateNotificationsSync(new NotificationCollection(nc), title);
        }

        /// <summary>
        /// Show All Notifications in the current std dispatcher thread
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="title"></param>
        public static void PopulateNotificationsSync(NotificationCollection nc, string title = "") {
            if (nc.HasMessages) {
                var _workbenchService = ServiceLocator.Instance.Resolve<IWorkBenchService>();
                _workbenchService.STADispatcher.Invoke(new Action<NotificationCollection, string>(PopulateNotifications), nc, title);
            }
        }

        /// <summary>
        /// Show this Notification
        /// </summary>
        /// <param name="nc">Notification Collection to use</param>
        /// <param name="title"></param>
        public static void PopulateNotifications(UserNotification nc, string title = "") {
            PopulateNotifications(new NotificationCollection(nc), title);
        }

        /// <summary>
        /// Show all Notifications
        /// </summary>
        /// <param name="nc">Notification Collection to use</param>
        /// <param name="title"></param>
        public static void PopulateNotifications(NotificationCollection nc, string title = "") {
            var _workbenchService = ServiceLocator.Instance.Resolve<IWorkBenchService>();
            foreach (var msg in nc.GetAllNotifications()) {
                if (msg == null) continue;
                _workbenchService.MessageBox(msg.Message, msg.Detail, title, GetIconFromType(msg.Type));
            }
        }


        /// <summary>
        /// Translate Notification Types to Messagebox Types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MessageBoxType GetIconFromType(NotificationType type) {
            switch (type) {

                case NotificationType.Ok:
                return MessageBoxType.Ok;

                case NotificationType.Info:
                return MessageBoxType.Information;

                case NotificationType.Warning:
                return MessageBoxType.Warning;

                case NotificationType.Error:
                return MessageBoxType.Error;

                default:
                return MessageBoxType.None;
            }
        }
    }
}
