namespace TT.Core.Application.Notifications;

public interface INotifier
{
    List<Notification> GetAllNotifications();
    void Handle(Notification notification);
    bool HaveNotification();
}
