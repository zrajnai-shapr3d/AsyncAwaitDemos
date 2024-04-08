#region Main 

using var input = new FileStream("input.txt", FileMode.Open);
using var output = new FileStream("output.txt", FileMode.Create);

Console.WriteLine($"Main thread Starting... {Thread.CurrentThread.ManagedThreadId}");

var task = CopyStreamToStreamAsync(input, output);

Console.WriteLine($"Main thread working... {Thread.CurrentThread.ManagedThreadId}");

task.Wait();

Console.WriteLine($"Main thread exiting. {Thread.CurrentThread.ManagedThreadId}");

#endregion

static IEnumerable<Task> GetTasks(Stream source, Stream destination) {
    var buffer = new byte[50];
    while (true) {
        Task<int> readTask = source.ReadAsync(buffer, 0, buffer.Length);
        yield return readTask;

        int numRead = readTask.Result;
        if (numRead <= 0) {
            break;
        }

        Task writeTask = destination.WriteAsync(buffer, 0, numRead);
        yield return writeTask;
    }
}

static void Process(IEnumerator<Task> tasks, TaskCompletionSource tcs) {
    Console.WriteLine($"Processing... {Thread.CurrentThread.ManagedThreadId}");
    if (tasks.MoveNext()) {
        // If there are more tasks to process
        // Set the next tasks continuation to call Process again
        tasks.Current.ContinueWith(_ => Process(tasks, tcs));
        return;
    }

    // If there are no more tasks to process
    tcs.SetResult();
    Console.WriteLine($"Processing done. {Thread.CurrentThread.ManagedThreadId}");
}

static Task CopyStreamToStreamAsync(Stream source, Stream destination) {
    var tcs = new TaskCompletionSource(); // produces a task and allows to set its result

    IEnumerable<Task> tasks = GetTasks(source, destination);
    Process(tasks.GetEnumerator(), tcs);

    return tcs.Task;
}
