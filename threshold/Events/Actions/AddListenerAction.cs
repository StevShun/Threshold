using threshold.Events.Conduit;

namespace threshold.Events.Actions
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
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
