using System.Threading;
using System.Threading.Tasks;

namespace _2_Tasks;

public sealed partial class TasksMainPage : Page {
    public TasksMainPage() {
        InitializeComponent();
    }

    private void OnTapped(object sender, TappedRoutedEventArgs e) {
        var backgroundScheduler = TaskScheduler.Default;
        var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        // Demonstrating that continuations can be scheduled after the task has started

        // Sleeps on a background thread
        var task1 = Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(3)); });

        // Then update the UI on the UI thread
        var task2 = task1.ContinueWith(_ => { MyTextBlock.Text = "Almost done..."; }, uiScheduler);

        // Then sleep on the background thread
        var task3 = task2.ContinueWith(_ => { Thread.Sleep(TimeSpan.FromSeconds(3)); }, backgroundScheduler);

        // Then update the UI on the UI thread
        task3.ContinueWith(_ => { MyTextBlock.Text = "Done!"; }, uiScheduler);

        MyTextBlock.Text = "Working...";
    }
}