namespace threshold.Events.Actions
{
    /// <summary>
    /// Credit for the EventConduit code and pattern goes to Sean Horton.
    /// https://github.com/sean-horton
    /// His pattern provides an excellent example of a "roll your own"
    /// event bus.
    /// </summary>
    public enum ActionType
    {
        OfferEvent,
        AddListener,
        RemoveListener
    }
}
