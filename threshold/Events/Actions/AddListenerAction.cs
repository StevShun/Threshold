using threshold.Events.Conduit;

namespace threshold.Events.Actions
{
    public class AddListenerAction : IAction
    {
        private IEventListener EventListener;

        public AddListenerAction(IEventListener eventListener)
        {
            EventListener = eventListener;
        }

        public IEventListener GetEventListener()
        {
            return EventListener;
        }

        public ActionType GetActionType()
        {
            return ActionType.AddListener;
        }
    }
}
