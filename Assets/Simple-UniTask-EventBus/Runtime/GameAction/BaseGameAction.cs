using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Base class for game actions
/// </summary>
public abstract class BaseGameAction
{
    /// <summary>
    /// Execute the game action
    /// </summary>
    /// <returns></returns>
    public abstract UniTask Execute();
}

