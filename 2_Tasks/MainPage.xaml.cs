using System.Threading;
using System.Threading.Tasks;

namespace _2_Tasks;

public sealed partial class MainPage : Page {
    public MainPage() {
        InitializeComponent();
    }

    private void OnTapped(object sender, TappedRoutedEventArgs e) {
        var backgroundScheduler = TaskScheduler.Default;
        var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        // Demonstrating that continuations can be scheduled after the task has started
        var task1 = Task.Factory.StartNew(_ => { Thread.Sleep(TimeSpan.FromSeconds(3)); }, backgroundScheduler);
        var task2 = task1.ContinueWith(_ => { MyTextBlock.Text = "Almost done..."; }, uiScheduler);
        var task3 = task2.ContinueWith(_ => { Thread.Sleep(TimeSpan.FromSeconds(3)); }, backgroundScheduler);
        task3.ContinueWith(_ => { MyTextBlock.Text = "Done!"; }, uiScheduler);
        MyTextBlock.Text = "Working...";
    }
}