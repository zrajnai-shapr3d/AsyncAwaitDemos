using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace _5_task_examples;

public sealed partial class MainPage : Page {
    public MainPage() {
        InitializeComponent();
    }

    private void OnTapped(object sender, TappedRoutedEventArgs e) {
        MyTextBlock.Text = "Starting.";
        MyTextBlock.TextWrapping = TextWrapping.Wrap;

        var task = GetStringAsync();

        MyTextBlock.Text = task.Result;
    }

    public static async Task<string> GetStringAsync() {
        await Task.Delay(3000);
        return "Result";
    }
}