# Simple UniTask EventBus

This project implements a lightweight, strongly-typed, and asynchronous Event Bus in Unity using [UniTask](https://github.com/Cysharp/UniTask). It allows different systems in your game to communicate with each other without tight coupling, while fully supporting asynchronous operations (like animations, server requests, or waiting for delays).

## 📦 Installation

You can install this package using the **Unity Package Manager** with a Git URL.

1. Open **Window -> Package Manager**.
2. Click the **+** button in the top-left corner and select **"Add package from git URL..."**.
3. **First, install UniTask** by pasting this URL and clicking **Add**:
   ```text
   https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
   ```
4. Once UniTask is installed, click **"Add package from git URL..."** again.
5. Paste the EventBus URL:
   ```text
   https://github.com/dewa99/Simple-UniTask-EventBus.git?path=/Packages/com.dewz.simple-unitask-eventbus
   ```
6. Click **Add**.

*Note: UniTask MUST be installed first, otherwise Unity will throw a "Package cannot be found" error when installing this package.*

---

## 🧠 Core Concept

Unlike a standard synchronous Event Bus (where firing an event instantly executes all listeners and continues), this Event Bus is **Asynchronous**. 

When an event is published, the publisher can `await` the event. The Event Bus will sequentially run through every subscribed listener's async task, waiting for each to finish completely before moving on. This is extremely useful when you want an action (like clicking a button) to wait for a sequence of visual effects or animations to finish before continuing.

---

## 🛠️ How It Works (Step-by-Step)

The system is built on four main components, demonstrated in the provided example files:

### 1. The Event Bus (`EventBus.cs`)
The `EventBus<T>` is a static generic class. By making it generic (`where T : struct`), C# automatically creates a unique, separate event bus for every different struct type you pass into it.
* **Subscription:** Systems can register or unregister their asynchronous methods (`Func<T, UniTask>`) to listen for events.
* **Publishing:** When `PublishAsync` is called, it iterates through all subscribers and `await`s their tasks sequentially. 

### 2. The Event Payload (`Animation.GameAction.cs`)
Events are defined as structs. This avoids garbage collection (GC) allocation when creating new events. Structs can also contain parameters to pass data to subscribers.
```csharp
public struct PlaySquareAnimation : IGameEvent
{
    public int Loop;
    public async UniTask Publish()
    {
        // Publishes itself to its specific Event Bus
        await EventBus<PlaySquareAnimation>.PublishAsync(this);
    }
}
```
*In this demo, the struct itself contains a convenience `Publish()` method to easily trigger the event bus, and a `Loop` parameter to tell the animation how many times to play.*

### 3. The Subscriber / Listener (`SquareView.cs`)
The subscriber is the object that reacts to the event. It subscribes to the specific struct type it cares about.
* In `Start()`, it subscribes its `PlayAnimation` method to `EventBus<PlaySquareAnimation>`.
* In `OnDestroy()`, it properly unsubscribes to prevent memory leaks.
* The `PlayAnimation` method returns a `UniTask`. In the demo, it plays a visual animation loop using native `Vector3.Lerp` and `UniTask.Yield()`. The task finishes only when the animation completes.

### 4. The Publisher (`PlayButton.cs`)
The publisher is what triggers the event. 
* When the UI button is clicked, it calls `async void OnClick()`.
* It disables the button to prevent spam-clicking.
* It instantiates the event struct with parameters and `await`s its `Publish()` method: 
  ```csharp
  await new PlaySquareAnimation() { Loop = loop }.Publish();
  ```
* **Because it `await`s the publish method**, the code execution here pauses until `SquareView` completely finishes its animation.
* Once the animation is done, the code resumes and the button becomes interactable again.

---

## 🔄 The Demo Execution Flow

Here is exactly what happens when you press the "Play" button in the demo:

1. **User clicks PlayButton.**
2. `PlayButton.OnClick()` fires. The button becomes non-interactable.
3. `new PlaySquareAnimation() { Loop = loop }.Publish()` is called and **awaited**.
4. Inside `Publish()`, the `EventBus<PlaySquareAnimation>.PublishAsync(this)` is called and **awaited**.
5. The Event Bus loops through its subscribers (in this case, just `SquareView`).
6. The Event Bus calls `SquareView.PlayAnimation()` and **awaits** the UniTask it returns.
7. `SquareView` loops through the visual sequence (jumping up, spinning, and dropping down) for the specified `Loop` amount using native Unity math.
8. The animation plays. Meanwhile, the Event Bus and the `PlayButton` are patiently waiting.
9. After the sequence loops finish, the `UniTask` in `SquareView` completes.
10. The Event Bus sees the task is complete, finishes its loop, and completes its own `UniTask`.
11. The `Publish()` method completes.
12. `PlayButton.OnClick()` resumes on the next line. The button becomes interactable again.

## 💡 Key Benefits
* **Type Safety:** You don't use strings or enums to identify events. You use the struct type itself.
* **Zero Allocation Events:** Because events are `structs`, publishing an event does not create garbage.
* **Async Control Flow:** You have absolute guarantee that when an `await EventBus.PublishAsync()` finishes, all visual effects, API calls, or loading screens tied to that event are 100% complete.
