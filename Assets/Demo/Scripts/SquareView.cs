using Cysharp.Threading.Tasks;
using PrimeTween;
using System;
using UnityEngine;

public class SquareView : MonoBehaviour
{

    void Start()
    {
        EventBus<PlaySquareAnimation>.Subscribe(PlayAnimation);
    }

    void OnDestroy()
    {
        EventBus<PlaySquareAnimation>.Unsubscribe(PlayAnimation);
    }

    public async UniTask PlayAnimation(PlaySquareAnimation animation)
    {

        for(int i = 0; i < animation.Loop; i++)
        {
            await Sequence.Create().Group(Tween.PositionY(transform, transform.position.y + 2f, 0.5f, Ease.OutQuad))
            .Group(Tween.Rotation(transform, Quaternion.Euler(0, 0, 180), 0.5f, Ease.Linear))
            .Chain(Tween.PositionY(transform, transform.position.y, 0.5f, Ease.InQuad))
            .Group(Tween.Rotation(transform, Quaternion.Euler(0, 0, 0), 0.5f, Ease.Linear));
        }
        
              
    }
}
