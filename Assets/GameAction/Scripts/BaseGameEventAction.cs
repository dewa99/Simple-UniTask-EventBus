using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A generic base class for GameActions that simply publish a single IGameEvent.
/// Subclasses only need to define their data fields and implement BuildEvent()
/// — no Execute() boilerplate required.
/// </summary>
public abstract class BaseGameEventAction<TEvent> : BaseGameAction
    where TEvent : struct, IGameEvent
{
    /// <summary>
    /// Constructs the event payload from this action's data fields.
    /// </summary>
    protected abstract TEvent BuildEvent();

    public override UniTask Execute() => BuildEvent().Publish();
}

