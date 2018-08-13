# Amusoft.EventManagement

Available on NuGet: [WeakEvent](https://www.nuget.org/packages/WeakEvent/)

Events are the most common source of memory leaks in .NET applications. The lifetime of the subscriber is extended to that of the publisher, unless you unsubscribe from the event. That's because the publisher maintains a strong reference to the subscriber, via the delegate, which prevents garbage collection of the subscriber.

This library provides a generic weak event that can be used, to publish events without affecting the lifetime of the subscriber.
In other words, if there is no other reference to the subscriber, the fact that it has subscribed to the event doesn't prevent it
from being garbage collected.

## How to use it

Instead of declaring your event like this:

```csharp
public event EventHandler<int> OnChanged;
```

You can declare it like this:

```csharp
private readonly WeakEvent<EventHandler<int>> _onChanged = new WeakEvent<EventHandler<int>>();
public event EventHandler<int> OnChanged
{
    add { _onChanged.Add(value); }
    remove { _onChanged.Remove(value); }
}
```

The event is then raised like this:

```csharp
private void RaiseOnChanged(int e)
{
    _onChanged.Invoke(this, e);
}
```

That's it, you have a weak event! Client code can subscribe to it as usual, this is completely transparent from the subscriber's
point of view.

The advantage of this library compared to others is that it also works with custom delegate types.
The only restriction for a delegate is, that its return type must be either void or Task.

If your architecture requirement is consuming the result of events i would recommend to have a look at https://github.com/dotnet/reactive. Because this library is based around dealing with any sort of publish+consumer scenario. You can still use this library with custom handlers to bypass that restriction however.

## Supported .NET runtimes:

| Platform | Supported |
| --- | --- |
| .NET 4.0+ | :heavy_check_mark: |
| netstandard1.1 | :heavy_check_mark: |
