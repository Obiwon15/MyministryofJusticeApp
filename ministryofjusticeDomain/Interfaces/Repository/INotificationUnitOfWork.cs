namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface INotificationUnitOfWork
    {
        IUserManagerRepo UserManagerRepo { get; }
        INotificationRepo NotificationRepo { get; }
    }
}