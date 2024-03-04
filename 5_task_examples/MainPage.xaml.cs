using System.Threading;
using System.Threading.Tasks;

namespace _5_task_examples;

public sealed partial class MainPage : Page {
    public MainPage() {
        InitializeComponent();
    }

    private void OnTapped(object sender, TappedRoutedEventArgs e) {
        MyTextBlock.Text = "Starting.";

        var s = LoadStringAsync();

        MyTextBlock.Text = s.Result;
    }
    private async Task<string> LoadStringAsync() {
        var firstName = await GetFirstNameAsync();
        return "Hello " + firstName;
    }

    private Task<string> GetFirstNameAsync() {
        return Task.Run(() => {
            Thread.Sleep(100);
            return "Zoltan";
        });
    }
}

