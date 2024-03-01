namespace _4_async_await;

public sealed partial class MainPage {
  public MainPage() {
    InitializeComponent();
  }

  private async void OnTapped(object sender, TappedRoutedEventArgs e) {
    MyTextBlock.Text = "Starting.";

    MyAwaitable firstAwaitable = new("First");
    MyAwaitable secondAwaitable = new("Second");

    MyTextBlock.Text = await firstAwaitable;
    MyTextBlock.Text = await secondAwaitable;
  }
}