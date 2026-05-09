using Cysharp.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Marker interface for all game events.
/// Implementing structs can publish themselves directly to their EventBus
/// without needing a separate GameAction wrapper.
/// </summary>
public interface IGameEvent
{
    /// <summary>
    /// Publishes this event to all subscribers on its EventBus channel.
    /// </summary>
    UniTask Publish();
}

