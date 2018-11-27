namespace AppBee.Persistence
{
    public interface IAppBeeConsumer
    {
        void SendMessage(string requestBody);
    }
}
