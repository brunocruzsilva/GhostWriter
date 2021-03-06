﻿using Google.Apis.Customsearch.v1;
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
using TMDbLib.Objects.People;
using VideoWriter.Struct;
using static Google.Apis.Customsearch.v1.CseResource.SiterestrictResource.ListRequest;

namespace VideoWriter.Robots
{
    public class ImageBot
    {
        private string _apiKey { get; set; }
        private string _searchEngineId { get; set; }
        public ImageBot()
        {
            dynamic credentialJson = JsonConvert.DeserializeObject(File.ReadAllText("Credentials.json"));
            _apiKey = credentialJson["GOOGLE_SEARCH_API_KEY"];
            _searchEngineId = credentialJson["GOOGLE_SEARCH_ENGINE_ID"];
        }

        public async Task FetchImagesFromGoogleWithTvRole(Post post)
        {
            using (var searchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer { ApiKey = _apiKey }))
            {
                foreach (TvRole tvRole in post.TvCredits.Cast)
                {
                    if (post.Images.Count < post.MaxNumberImages)
                    {
                        var listRequest = searchService.Cse.List(post.Keyword + " " + tvRole.Name);
                        listRequest.Cx = _searchEngineId;
                        listRequest.SearchType = (int)SearchTypeEnum.Image;
                        listRequest.Num = 5;
                        listRequest.ImgSize = CseResource.ListRequest.ImgSizeEnum.Xxlarge;
                        listRequest.Rights = "cc_publicdomain";

                        var search = listRequest.Execute();

                        if (search.Items != null)
                        {
                            foreach (var image in search.Items)
                            {
                                if (image.Mime == "image/jpeg" && !post.Images.Any(x => x.URI == image.Link))
                                {
                                    post.Images.Add(new Image()
                                    {
                                        Width = (int)image.Image.Width,
                                        Height = (int)image.Image.Height,
                                        URI = image.Link
                                    });
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task FetchImagesFromGoogle(Post post)
        {
            using (var searchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer { ApiKey = _apiKey }))
            {
                var listRequest = searchService.Cse.List(post.Keyword);
                listRequest.Cx = _searchEngineId;
                listRequest.SearchType = (int)SearchTypeEnum.Image;
                listRequest.Num = 5;
                listRequest.ImgSize = CseResource.ListRequest.ImgSizeEnum.Xxlarge;

                var search = listRequest.Execute();

                foreach (var image in search.Items.Take(post.MaxNumberImages).ToList())
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
