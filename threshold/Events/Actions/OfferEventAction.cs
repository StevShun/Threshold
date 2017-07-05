using threshold.Events.Types;

namespace threshold.Events.Actions
{
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
