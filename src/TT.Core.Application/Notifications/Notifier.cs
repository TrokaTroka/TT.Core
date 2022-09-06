namespace TT.Core.Application.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications;

    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public List<Notification> GetAllNotifications()
    {
        return _notifications;
    }

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public bool HaveNotification()
    {
        return _notifications.Any();
    }
}
