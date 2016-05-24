using Formular.Notifications.Models;

namespace Formular.Notifications.Interfaces
{
    interface INotification
    {
        bool SendNotification(Notification notification);
    }
}
