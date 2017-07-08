using threshold.Events.Types;

namespace threshold.Events.Conduit
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
    public interface IEventConduit
    {
        void Start();

        void Stop();

        void SendEvent(IEvent anEvent);

        void AddEventListener(IEventListener eventListener);

        void RemoveEventListener(IEventListener eventListener);
    }
}
