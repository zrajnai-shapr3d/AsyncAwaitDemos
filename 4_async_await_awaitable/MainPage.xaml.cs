namespace _4_async_await_awaitable;

public sealed partial class MainPage {
  public MainPage() {
    InitializeComponent();
  }

  private async void OnTapped(object sender, TappedRoutedEventArgs e) {
    MyTextBlock.Text = "Starting.";

    MyAwaitable myAwaitable = new("First");

    MyTextBlock.Text = await myAwaitable;

    MyTextBlock.Text += ".. Done";
  }
}