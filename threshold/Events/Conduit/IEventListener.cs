using System.Collections.Generic;
using threshold.Events.Types;

namespace threshold.Events.Conduit
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
    public interface IEventListener
    {
        void OnEvent(IEvent _event);

        List<EventType> GetNotifyTypes();
    }
}
