using threshold.Events.Actions;

namespace threshold.Events
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
    public interface IAction
    {
        ActionType GetActionType();
    }
}
