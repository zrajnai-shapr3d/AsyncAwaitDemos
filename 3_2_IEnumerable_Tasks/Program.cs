#region Main 

using var input = new FileStream("input.txt", FileMode.Open);
using var output = new FileStream("output.txt", FileMode.Create);

Console.WriteLine("Main thread Starting...");

var task = CopyStreamToStreamAsync(input, output);

Console.WriteLine("Main thread working...");

task.Wait();

Console.WriteLine("Main thread exiting.");
#endregion

#region Implementation

static Task IterateAsync(IEnumerable<Task> tasks) {
    var tcs = new TaskCompletionSource();

    IEnumerator<Task> e = tasks.GetEnumerator();

    void Process() {
        Console.WriteLine("Background Processing...");
        try {
            if (e.MoveNext()) { // If there are more tasks to process
                e.Current.ContinueWith(_ => Process()); // Set the next tasks continuation to call Process again
                return;
            }
        }
        catch (Exception e) {
            tcs.SetException(e);
            return;
        }
        tcs.SetResult();
        Console.WriteLine("Background Processing done.");
    }

    Process();

    return tcs.Task;
}

static Task CopyStreamToStreamAsync(Stream source, Stream destination) {
    return IterateAsync(Impl(source, destination));

    static IEnumerable<Task> Impl(Stream source, Stream destination) {
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
            write.Wait();
        }
    }
}
#endregion