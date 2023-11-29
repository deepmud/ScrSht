namespace CaptureScreenShots
{
	public interface IScreenShotService
	{
		void Button1_Click();
		void Capture(System.String capturedFilePath, System.String pathString);
		void Page_Load();
	}
}