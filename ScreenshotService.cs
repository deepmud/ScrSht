using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScrSht;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CaptureScreenShots
{

	public class ScreenShotService : IScreenShotService
    {

		readonly ILogger<DoItServices> _log;
		private IConfiguration _configuration;

		public ScreenShotService(IConfiguration configuration, ILogger<DoItServices> log)
		{
			_configuration = configuration;
			_log = log;
			//_configuration = configuration;
			//Button1_Click();
			//Page_Load();


		}
		
		public void Page_Load()
		{
			_log.LogInformation("going for fresh screenshot////////////////////////// " + DateTime.Now.ToString());
		}

		public void Button1_Click()
		{
			var folder = "ScreenShot" + DateTime.Now.ToString("MMMM");
			string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath,folder );

			string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
		
			if (Directory.Exists(pathString))
			{
				Directory.Delete(pathString, true);
				_log.LogInformation("folder and files deleted " );
			}

			Directory.CreateDirectory(pathString);

			var screenshotFile = uploadsFolder + "\\" + Guid.NewGuid().ToString() + ".bmp";
			Capture(screenshotFile, pathString);//path to Save Captured files  

		}



		public async void Capture(string capturedFilePath, string pathString)
		{
			using var bitmap = new Bitmap(1920, 1080);
			using (var g = Graphics.FromImage(bitmap))
			{
				g.CopyFromScreen(0, 0, 0, 0,
				bitmap.Size, CopyPixelOperation.SourceCopy);
			}

			bitmap.Save(capturedFilePath, ImageFormat.Jpeg);
			_log.LogInformation("screenshot taken " + DateTime.Now.ToString());
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			//HttpClient client = new HttpClient(clientHandler);
			using (var client2 = new HttpClient(clientHandler))
			{
				client2.BaseAddress = new Uri(_configuration.GetValue<string>("baseUrl2"));
				var contents = new ByteArrayContent(File.ReadAllBytesAsync(capturedFilePath).Result);
				//new ByteArrayContent(File.ReadAllBytesAsync(capturedFilePath).Result);
				contents.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
				var poSuppliers = JsonConvert.SerializeObject(contents);
				HttpContent data = new StringContent(poSuppliers, Encoding.UTF8, "multipart/form-data");







				using (var content = new MultipartFormDataContent())
				{
					//content.Add(new StreamContent(new MemoryStream(File.(capturedFilePath).Result)));
					content.Add(data);
					var poSupplier = JsonConvert.SerializeObject(content);


					//var message = await client2.PostAsync("ConsumeScreenshot2", poSupplier);
					var message = await client2.GetAsync("ConsumeScreenshot2?data=" + poSupplier);


					var input = await message.Content.ReadAsStringAsync();
					_log.LogInformation(input);
				}
				//return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;

			}



			//using var form = new MultipartFormDataContent();
			//using var fileContent = new ByteArrayContent(File.ReadAllBytesAsync(capturedFilePath).Result);
			//fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
			//var pocontent=JsonConvert.SerializeObject(fileContent);
			//HttpContent data = new StringContent(pocontent);
			////form.Add(fileContent, "file", Path.GetFileName(capturedFilePath));
			//HttpClientHandler clientHandler = new HttpClientHandler();
			//clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			//HttpClient client = new HttpClient(clientHandler);
			//var _httpClient = new HttpClient(clientHandler);
			//_httpClient.BaseAddress =new  Uri(_configuration.GetValue<string>("baseUrl2"));
			////var _url = _configuration.GetValue<string>("baseUrl2");
			////var dd =$"{_url}/ConsumeScreenshot?file={fileContent}"
			//var serverImage = new HttpRequestMessage(HttpMethod.Post, "ConsumeScreenshot");
			//serverImage.Content = data;
			////var response = await _httpClient.PostAsync("ConsumeScreenshot/", fileContent);
			//var response = await _httpClient.SendAsync(serverImage);

			//response.EnsureSuccessStatusCode();
			//var responseContent = await response.Content.ReadAsStringAsync();
			//var result = JsonSerializer.Deserialize<FileUploadResult>(responseContent);
			//_logger.LogInformation("Uploading is complete.");
			//return result.Guid;
			



			//var streams = File.OpenRead(capturedFilePath);
			//var stream = File.ReadAllBytes(capturedFilePath);

			//var content = new MultipartFormDataContent();

			//var file_content = new ByteArrayContent(new StreamContent(streams).ReadAsByteArrayAsync().Result);
			////var oo =Convert.ToInt64(stream);

			//file_content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			//file_content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
			//{
			//	FileName = "screenshot.jpeg",
			//	Name = "foo",
			//};
			//content.Add(file_content);

			////var d = new ReceiveScreenShot();
			////Console.WriteLine(d.ConsumeScreenshot(file_content) + DateTime.Now.ToString());
			////d.ConsumeScreenshot(file_content);
			//var pocontent=JsonConvert.SerializeObject(file_content);
			//HttpContent data = new StringContent(pocontent, Encoding.UTF8, "application/json");

			//HttpClientHandler clientHandler = new HttpClientHandler();
			//clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			//HttpClient client = new HttpClient(clientHandler);
			//HttpClient client2 = new HttpClient(clientHandler);

			////var endpointsn = "login?" + login;
			////var j = "aniemeka@hotmail.com";
			////var k = "Code2Day!";
			////var endpointsss = "Login?email=" + j + "&pass=" + k;

			//client.BaseAddress = new Uri(_configuration.GetValue<string>("baseUrl2"));

			//var response = await client.GetAsync(_configuration.GetValue<string>("login"));

			//_log.LogInformation("file sent to server at " + " /" + response.StatusCode.ToString() + response.RequestMessage.RequestUri.ToString() + " /  "+ response.Content.ReadAsStringAsync().Result + DateTime.Now.ToString());
			//var df = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
			//ApplicationUser dd = new ApplicationUser();
			//// var nn = df.ToString() ;
			////var ddd = nn[2].n;

			//var endpoint="";
			//client2.BaseAddress = new Uri(_configuration.GetValue<string>("baseUrl2"));
			//var serverImage = new HttpRequestMessage(HttpMethod.Post,endpoint );
			//var s = JsonConvert.SerializeObject(serverImage.Content = data);
			//var jj = _configuration.GetValue<string>("userId");
			//var sl = JsonConvert.SerializeObject(stream);
			//var ffd = Convert.ToBase64String(stream);
			////var ddsd = Convert.ToUInt64(stream);
			////var sdf = ddsd.ToString();
			//HttpContent datas = new StringContent(ffd, Encoding.UTF8, "application/json");
			////endpoint = "ConsumeScreenshot?userid="+ jj +"&contents="+ datas.ReadAsStringAsync().Result;
			//endpoint = "ConsumeScreenshot?userid="+ jj +"&contents="+ stream;
			////endpoint = "ConsumeScreenshot?contents=" + datas.ReadAsStringAsync().Result;


			////serverImage.Headers.Add = file_content.Headers;
			// var responses = await client2.GetAsync(endpoint);
			//	_log.LogInformation("file sent to server at " + " /" + response.StatusCode.ToString() + response.RequestMessage.RequestUri.ToString() + " /  "  + DateTime.Now.ToString());
			
			//var response = await client.GetAsync(endpoint);
			
			//_log.LogInformation("file sent to server at " + " /" /*+ response..ToString()*/ + " /  " + DateTime.Now.ToString());
			//streams.Dispose();

			Directory.Delete(pathString, true);

			_log.LogInformation("folder deleted at " + DateTime.Now.ToString());
		}



		public class ApplicationUser 
		{


			public string status { get; set; }


			public  string data { get; set; }

		}

	}
}

//[HttpGet]
//public void ConsumeScreenshot2(string data)
//{

//	var folder = "ScreenShotR" + DateTime.Now.ToString("MMMM");
//	string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
//	if (!Directory.Exists(pathString))
//	{
//		Directory.CreateDirectory(pathString);
//	}
//	var screenshotFile = pathString + "\\" + Guid.NewGuid().ToString() + ".jpg";
//	// byte[] array = null;
//	var datas = new MultipartFormDataContent();
//	datas = JsonConvert.DeserializeObject<MultipartFormDataContent>(data);

//	var byteArray = datas.ReadAsByteArrayAsync().Result;
//	var stream = new MemoryStream(byteArray);
//	Image image = Image.FromStream(stream, true); ;
//	image.Save(screenshotFile, ImageFormat.Jpeg);
//	stream.Dispose();
//}
