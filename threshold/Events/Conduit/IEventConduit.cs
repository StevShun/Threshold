using threshold.Events.Types;

namespace threshold.Events.Conduit
{
    public interface IEventConduit
    {
        void Start();

        void Stop();

        void SendEvent(IEvent anEvent);

        void AddEventListener(IEventListener eventListener);

        void RemoveEventListener(IEventListener eventListener);
    }
}
