using threshold.Events.Conduit;

namespace threshold.Events.Actions
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
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
