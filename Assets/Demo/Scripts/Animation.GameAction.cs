using Cysharp.Threading.Tasks;
using UnityEngine;

public struct PlaySquareAnimation : IGameEvent
{
    public int Loop;
    public async UniTask Publish()
    {
        await EventBus<PlaySquareAnimation>.PublishAsync(this);
    }
}
