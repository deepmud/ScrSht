using CaptureScreenShots;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

///DI,Logging,Settings
namespace ScrSht
{
	partial class Program
	{

		static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder();
			BuildConfig(builder);
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Build())
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			Log.Logger.Information("Application Starting");
			var host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) => 
				{
					services.AddTransient<IDoItServices, DoItServices>();
					services.AddTransient<IScreenShotService, ScreenShotService>();
				})
				.UseSerilog()
				.Build();
			var svc = ActivatorUtilities.CreateInstance<DoItServices>(host.Services);
			svc.Run();

			//var aTimer = new System.Timers.Timer();
			//aTimer.Interval = 30000;

			//// Hook up the Elapsed event for the timer. 
			//aTimer.Elapsed += OnTimedEvent;

			//// Have the timer fire repeated events (true is the default)
			//aTimer.AutoReset = true;

			//// Start the timer
			//aTimer.Enabled = true;


			//Console.WriteLine("Press the Enter key to exit the program at any time... ");
			//Console.ReadLine();
		}

		



		static void BuildConfig(IConfigurationBuilder builder)
		{
			builder.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"apppsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
				.AddEnvironmentVariables(); 
		}
	}
}
