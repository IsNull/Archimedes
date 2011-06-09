using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Notifications
{
    /// <summary>
    /// Represents a single User Notification. 
    /// Usefull if you can't notify the user in the current class and wan't to pass the message resuslts to the invoker.
    /// </summary>
    public class UserNotification
    {
        public UserNotification(NotificationType type) { Type = type; }

        public NotificationType Type { get; set; }

        /// <summary>
        /// This Field holds a small Summary of the Error occured.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// This Fields hols a large Description and further Dbug informations
        /// </summary>
        public string Detail { get; set; }

        public string FormatToMessage() {
            return Message;
        }

    }

    public enum NotificationType {
        Ok,
        Info,
        Warning,
        Error
    }
}
