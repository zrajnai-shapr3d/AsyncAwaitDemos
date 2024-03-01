#region Main 

using var input = new FileStream("input.txt", FileMode.Open);
using var output = new FileStream("output.txt", FileMode.Create);

Console.WriteLine($"Main thread Starting... {Thread.CurrentThread.ManagedThreadId}");

var task = CopyStreamToStreamAsync(input, output);

Console.WriteLine($"Main thread working... {Thread.CurrentThread.ManagedThreadId}");

task.Wait();

Console.WriteLine($"Main thread exiting. {Thread.CurrentThread.ManagedThreadId}");

#endregion

static Task CopyStreamToStreamAsync(Stream source, Stream destination) {
    static IEnumerable<Task> GetTasks(Stream source, Stream destination) {
        var buffer = new byte[50];
        while (true) {
            Task<int> read = source.ReadAsync(buffer, 0, buffer.Length);
            yield return read;

            int numRead = read.Result;
            if (numRead <= 0) {
                break;
            }

            Task write = destination.WriteAsync(buffer, 0, numRead);
            yield return write;
        }
    }

    static Task CombineTasks(IEnumerable<Task> tasks) {
        var tcs = new TaskCompletionSource(); // produces a task and allows to set its result

        IEnumerator<Task> e = tasks.GetEnumerator();

        void Process() {
            Console.WriteLine($"Background Processing... {Thread.CurrentThread.ManagedThreadId}");
            if (e.MoveNext()) {
                // If there are more tasks to process
                // Set the next tasks continuation to call Process again
                e.Current.ContinueWith(_ => Process());
                return;
            }

            // If there are no more tasks to process
            tcs.SetResult();
            Console.WriteLine($"Background Processing done. {Thread.CurrentThread.ManagedThreadId}");
        }

        Process();

        return tcs.Task;
    }

    return CombineTasks(GetTasks(source, destination));
}
