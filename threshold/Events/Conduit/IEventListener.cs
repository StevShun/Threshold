using System.Collections.Generic;
using threshold.Events.Types;

namespace threshold.Events.Conduit
{
    public interface IEventListener
    {
        void OnEvent(IEvent _event);

        List<EventType> GetNotifyTypes();
    }
}
