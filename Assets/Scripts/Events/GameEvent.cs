public class GameEvent
{
    public string EventName { get; }
    public object EventData { get; }

    public GameEvent(string eventName, object eventData = null)
    {
        EventName = eventName;
        EventData = eventData;
    }
}