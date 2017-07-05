using threshold.Events.Conduit;

namespace threshold.Events.Actions
{
    public class RemoveListenerAction : IAction
    {
        private IEventListener EventListener;

        public RemoveListenerAction(IEventListener eventListener)
        {
            EventListener = eventListener;
        }

        public ActionType GetActionType()
        {
            return ActionType.RemoveListener;
        }

        public IEventListener GetEventListener()
        {
            return EventListener;
        }
    }
}
