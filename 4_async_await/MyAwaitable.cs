using System.Runtime.CompilerServices;
using System.Threading;

namespace _4_async_await {
  public class MyAwaitable(string value) {
    public MyAwaiter GetAwaiter() => new(value);

    public class MyAwaiter : INotifyCompletion {
      private readonly string _value;
      private SynchronizationContext _synchronizationContext;
      private Action _continuation;

      public MyAwaiter(string value) {
        _value = value;
        ThreadPool.QueueUserWorkItem(TheWork);
      }

      public bool IsCompleted { get; private set; }

      public string GetResult() => $"{_value}";

      public void OnCompleted(Action continuation) {
        _synchronizationContext = SynchronizationContext.Current;
        _continuation = continuation;
      }

      private void TheWork(object _) {
        Thread.Sleep(TimeSpan.FromSeconds(3));
        IsCompleted = true;
        _synchronizationContext?.Post(_ => { _continuation?.Invoke(); }, null);
      }
    }
  }
}
