using Cysharp.Threading.Tasks;
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
        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + new Vector3(0, 2f, 0);
        Quaternion startRot = Quaternion.Euler(0, 0, 0);
        Quaternion peakRot = Quaternion.Euler(0, 0, 180);

        for (int i = 0; i < animation.Loop; i++)
        {
            float elapsed = 0f;
            float duration = 0.5f;

            // Phase 1: Move up & rotate 180 degrees
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                
                // OutQuad easing
                float easeT = 1f - (1f - t) * (1f - t);
                
                transform.position = Vector3.Lerp(startPos, peakPos, easeT);
                transform.rotation = Quaternion.Lerp(startRot, peakRot, t); // Linear
                
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            transform.position = peakPos;
            transform.rotation = peakRot;

            elapsed = 0f;

            // Phase 2: Move down & rotate back to 0 degrees
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                
                // InQuad easing
                float easeT = t * t;
                
                transform.position = Vector3.Lerp(peakPos, startPos, easeT);
                transform.rotation = Quaternion.Lerp(peakRot, startRot, t); // Linear
                
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            transform.position = startPos;
            transform.rotation = startRot;
        }
    }
}
