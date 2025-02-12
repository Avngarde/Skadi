using MauiIcons.Core;

namespace Skadi;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        _ = new MauiIcon();
    }
}