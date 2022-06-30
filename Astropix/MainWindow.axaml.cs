using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Astropix
{
    public partial class MainWindow : Window
    {
        #region Properties

        private const string API_ENDPOINT = "https://api.nasa.gov/planetary/apod?api_key=ggjnISEJS5wleVXRnZrO4HOxn5n8FenTPBsgZyfu";

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            PopulateView();
        }

        private async void PopulateView()
        {
            APIResponse apiResponse = await GetAPIData();
            if (apiResponse != null)
            {
                UpdateTitle(apiResponse.title);
                UpdateDescription(apiResponse.explanation);
                SetAPODImage(apiResponse.hdurl);
                return;
            }
            Environment.Exit(0);
        }

        private void UpdateTitle(string content)
        {
            this.FindControl<TextBlock>("titleTextBlock").Text = content;
        }

        private void UpdateDescription(string content)
        {
            this.FindControl<TextBlock>("descriptionTextBlock").Text = content;
        }

        private async void SetAPODImage(string imageUrl)
        {
            byte[] imageBuffer = new byte[0];

            await Task.Run(() => 
            {
                imageBuffer = DownloadImage(imageUrl);
            });

            Stream stream = new MemoryStream(imageBuffer);
            Bitmap? image = new Bitmap(stream);
            this.FindControl<Image>("image").Source = image;
        }

        private byte[] DownloadImage(string imageUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] result = webClient.DownloadData(new Uri(imageUrl));
                return result;
            }
        }

        private async Task<APIResponse> GetAPIData()
        {
            WebResponse webResponse = await WebRequest.Create(API_ENDPOINT).GetResponseAsync();
            string json = string.Empty;

            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                json = await streamReader.ReadToEndAsync();
            }

#pragma warning disable CS8603 // Possible null reference return.
            return JsonConvert.DeserializeObject<APIResponse>(json);
#pragma warning restore CS8603 // Possible null reference return.
        }

        #region Designer Code

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion
    }

    public class APIResponse
    {
        private const string FAILED_TO_FETCH_DATA = "Failed to fetch data.";

        public string copyright = FAILED_TO_FETCH_DATA;
        public string date = FAILED_TO_FETCH_DATA;
        public string explanation = FAILED_TO_FETCH_DATA;
        public string hdurl = FAILED_TO_FETCH_DATA;
        public string media_type = FAILED_TO_FETCH_DATA;
        public string service_version = FAILED_TO_FETCH_DATA;
        public string title = "Failed To Fetch Data";
        public string url = FAILED_TO_FETCH_DATA;
    }
}
