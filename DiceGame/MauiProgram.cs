/*
 * DiceGame MauiProgram.cs
 * Generated code to launch MauiApp
 * Course- IST440
 * Author- Cameron Grande
 * Date Developed- 9/26/22
 * Last Changed- 9/26/22
 * Rev: 1
 */

using MauiSpeechToTextSample.Platforms;
using MauiSpeechToTextSample;
using Microsoft.Extensions.Logging;

namespace DiceGame;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MetalMania-Regular.ttf", "MetalMania");
            });

        builder.Services.AddTransient<MainPage>();
		builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();

        return builder.Build();
	}
}
