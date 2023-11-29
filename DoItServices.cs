using CaptureScreenShots;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

///DI,Logging,Settings
namespace ScrSht
{
	
		public class DoItServices : IDoItServices
		{
			private readonly ILogger<DoItServices> _log;
			private readonly IConfiguration _configuration;
		      private readonly IScreenShotService _screenShotService;
			public DoItServices(ILogger<DoItServices> log, IConfiguration configuration,IScreenShotService screenShotService)
			{
				_log = log;
				_configuration = configuration;
			_screenShotService = screenShotService;
		    }


			public void Run()
			{
			//for (var x = 0; x < _configuration.GetValue<int>("loop"); x++)
			//{
			//	_log.LogInformation("Run number {runNumber}", x);
			//}
				_screenShotService.Button1_Click();
				_screenShotService.Page_Load();
				
			var aTimer = new System.Timers.Timer();
				aTimer.Interval = 100000;

				// Hook up the Elapsed event for the timer. 
				aTimer.Elapsed += OnTimedEvent;

				// Have the timer fire repeated events (true is the default)
				aTimer.AutoReset = true;

				// Start the timer
				aTimer.Enabled = true;

			
				_log.LogInformation("Press the Enter key to exit the program at any time... ");
				Console.ReadLine();
			}
	    private  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
		{

			_screenShotService.Button1_Click();
			_screenShotService.Page_Load();

			Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
		}
	}
}

