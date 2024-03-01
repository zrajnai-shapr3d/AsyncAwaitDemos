using System.Threading;

namespace _1_SynchronizationContext {
    public sealed partial class MainPage : Page {
        public MainPage() {
            InitializeComponent();
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e) {
            // We're on the UI thread here
            var syncContext = SynchronizationContext.Current;

            ThreadPool.QueueUserWorkItem(state => {
                // We're on a thread pool thread here
                Thread.Sleep(TimeSpan.FromSeconds(5));
                syncContext.Post(o => {
                    // We're back on the UI thread here
                    MyTextBlock.Text = "Done!";
                }, null);
                // Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { MyTextBlock.Text = "Done!"; });
            });

            MyTextBlock.Text = "Working...";
        }
    }
}
