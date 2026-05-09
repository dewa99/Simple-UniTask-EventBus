using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// A lightweight, strongly-typed Event Bus designed for asynchronous (awaitable) events.
/// Every unique struct passed as 'T' creates its own separate event bus.
/// Handlers must return an Awaitable, allowing the publisher to wait for all subscribers to finish their asynchronous tasks.
/// </summary>
public static class EventBus<T> where T : struct
{
    private static readonly List<Func<T, Awaitable>> subscribers = new();

    /// <summary>
    /// Subscribes an asynchronous handler to the event.
    /// </summary>
    public static void Subscribe(Func<T, Awaitable> handler)
    {
        if (!subscribers.Contains(handler))
        {
            subscribers.Add(handler);
            Debug.Log($"[EventBus<{typeof(T).Name}>] Subscribed: {handler.Method.Name}. Total: {subscribers.Count}");
        }
    }

    /// <summary>
    /// Unsubscribes a handler from the event.
    /// </summary>
    public static void Unsubscribe(Func<T, Awaitable> handler)
    {
        subscribers.Remove(handler);
    }

    /// <summary>
    /// Publishes the event to all subscribers sequentially. 
    /// Use 'await' on this method to wait until every subscribed system has completely finished its async task.
    /// Note: This awaits subscribers sequentially. The next subscriber will not start until the current one finishes.
    /// </summary>
    public static async Awaitable PublishAsync(T eventData)
    {
        Debug.Log($"[EventBus<{typeof(T).Name}>] Publishing to {subscribers.Count} subscribers.");
        for (int i = 0; i < subscribers.Count; i++)
        {
            await subscribers[i].Invoke(eventData);
        }
    }
}

