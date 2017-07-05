using threshold.Events.Actions;

namespace threshold.Events
{
    public interface IAction
    {
        ActionType GetActionType();
    }
}
