using System.Windows;

namespace WpfProgressBarDemo;

public partial class MainWindow : Window
{
    private CancellationTokenSource _tokenSource;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void ButtonStart_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            _tokenSource = new CancellationTokenSource();
            
            var progress = new Progress<int>();
            progress.ProgressChanged += (s, args) => Progress.Value = args;
            var token = _tokenSource.Token;
            await DemoWork(progress, token);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
            
            _tokenSource.Dispose();
        }
    }
    
    private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
    {
        _tokenSource.Cancel();
    }

    private async Task DemoWork(IProgress<int> progress, CancellationToken cancellationToken = default)
    {
        for (int i = 0; i < 100; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            progress.Report(i + 1);
            await Task.Delay(500, cancellationToken);
        }
    }
}