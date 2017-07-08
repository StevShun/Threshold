using threshold.Events.Types;

namespace threshold.Events.Actions
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
    public class OfferEventAction : IAction
    {
        private IEvent Event;

        public OfferEventAction(IEvent _event)
        {
            Event = _event;
        }

        public ActionType GetActionType()
        {
            return ActionType.OfferEvent;
        }

        public IEvent GetEvent()
        {
            return Event;
        }
    }
}
