using Google.Apis.Customsearch.v1;
using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VideoWriter.Struct;
using static Google.Apis.Customsearch.v1.CseResource.SiterestrictResource.ListRequest;

namespace VideoWriter.Robots
{
    public class ImageBot
    {

        public ImageBot() { }

        public async Task FetchImagesFromGoogle(Post post)
        {
            string apiKey = ConfigurationSettings.AppSettings["GOOGLE_SEARCH_API_KEY"];
            string searchEngineId = ConfigurationSettings.AppSettings["GOOGLE_SEARCH_ENGINE_ID"];

            using (var searchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer { ApiKey = apiKey }))
            {
                var listRequest = searchService.Cse.List(post.Keyword);
                listRequest.Cx = searchEngineId;
                listRequest.SearchType = (int)SearchTypeEnum.Image;
                listRequest.Num = 1;
                listRequest.ImgSize = CseResource.ListRequest.ImgSizeEnum.Xxlarge;
                listRequest.Rights = "cc_publicdomain";

                var search = listRequest.Execute();

                foreach (var image in search.Items.ToList())
                {
                    if (image.Mime == "image/jpeg")
                    {
                        post.Images.Add(new Image()
                        {
                            Width = (int)image.Image.Width,
                            Height = (int)image.Image.Height,
                            URI = image.Link
                        });
                    }
                }
            }
        }

        public async Task DownloadImages(Post post)
        {
            string _directoryTempGhost = ConfigurationSettings.AppSettings["DIRECTORY_TEMP"];  

            if (!Directory.Exists(_directoryTempGhost))
                Directory.CreateDirectory(_directoryTempGhost);

            using (WebClient client = new WebClient())
            {
                foreach (Image image in post.Images)
                {
                    image.Path = _directoryTempGhost + Guid.NewGuid() + ".jpg";
                    client.DownloadFile(new Uri(image.URI), image.Path);
                }
            }
        }

        public async Task DeleteFileImages(Post post)
        {
            foreach (Image image in post.Images)
            {
                if (File.Exists(image.Path))
                    File.Delete(image.Path);

                if (File.Exists(image.PathResize))
                    File.Delete(image.PathResize);
            }
        }

        public async Task ScaleImages(Post post)
        {
            string _directoryTempGhost = ConfigurationSettings.AppSettings["DIRECTORY_TEMP"];

            int maxWidth = 1080;
            int maxHeight = 1080;

            foreach (var imageItem in post.Images)
            {
                imageItem.PathResize = _directoryTempGhost + Guid.NewGuid() + ".jpg";

                System.Drawing.Image image = System.Drawing.Image.FromFile(imageItem.Path);

                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new System.Drawing.Bitmap(maxWidth, maxHeight, PixelFormat.Format24bppRgb);

                using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                { 
                    int y = (maxHeight / 2) - newHeight / 2;
                    int x = (maxWidth / 2) - newWidth / 2;
                    graphics.Clear(System.Drawing.Color.White); 

                    graphics.DrawImage(image, x, y, newWidth, newHeight);
                }
                newImage.Save(imageItem.PathResize, ImageFormat.Jpeg); 
            }
        }
         
    }
}
