using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using MauiIcons.Core;
using MauiIcons.Fluent;
using Plugin.Maui.Audio;

namespace Skadi;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.AddAudio();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiIconsCore(x =>
            {
                x.SetDefaultFontOverride(true);
            })
            .UseFluentMauiIcons()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); 
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}