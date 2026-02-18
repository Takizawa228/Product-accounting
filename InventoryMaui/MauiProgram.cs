using Syncfusion.Maui.Core.Hosting;
using InventoryMaui.Service;
using Microsoft.Extensions.Logging;


namespace InventoryMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<ApiServiceClients>();
            builder.Services.AddSingleton<ApiServiceClientsDTO>();
            builder.Services.AddSingleton<ApiServiceWorker>();
            builder.Services.AddSingleton<ApiServiceProducts>();
            builder.Services.AddSingleton<ApiServiceTransactions>();

#if DEBUG
            builder.Logging.AddDebug();
           
#endif

            return builder.Build();
        }
    }
}
