using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Services;

namespace Archimedes.Patterns.Notifications
{
    /// <summary>
    /// Holds n Notifications, mostly used to show them to the user.
    /// </summary>
    public class NotificationCollection
    {
        List<UserNotification> _notifications = new List<UserNotification>();

        public NotificationCollection() { }

        public NotificationCollection(UserNotification notification) { _notifications.Add(notification); }

        public NotificationCollection(IEnumerable<UserNotification> notifications) { _notifications.AddRange(notifications); }

        #region Public Methods

        public IEnumerable<UserNotification> GetAllNotifications() {
            return new List<UserNotification>(_notifications);
        }

        public void Add(UserNotification n) {
            _notifications.Add(n);
        }

        public void Merge(NotificationCollection other) {
            foreach (var n in other.GetAllNotifications()) {
                if(!_notifications.Contains(n))
                    this.Add(n);
            }
        }

        public string FormatToMessage() {
            string err = "";
            foreach (var error in _notifications) {
                err += error.FormatToMessage() + Environment.NewLine;
            }
            return err;
        }

        public void Clear() {
            _notifications.Clear();
        }

        #endregion

        #region Properties

        public bool HasError {
            get {
                return _notifications.FindAll(x => (x.Type == NotificationType.Error)).Any();
            }
        }

        public bool HasMessages {
            get {
                return _notifications.Any();
            }
        }

        #endregion

    }
}
