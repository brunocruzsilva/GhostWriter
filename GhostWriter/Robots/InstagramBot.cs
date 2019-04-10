﻿using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoWriter.Struct;

namespace VideoWriter.Robots
{
    public class InstagramBot
    {
        const string stateFile = "state.bin";
        private UserSessionData _userSessionData { get; set; }
        private IInstaApi _instaApi { get; set; }

        public InstagramBot()
        {
            _userSessionData = new UserSessionData
            {
                UserName = "elencoemdetalhes",
                Password = "05141631"
            };

            var delay = RequestDelay.FromSeconds(2, 2);

            _instaApi = InstaApiBuilder.CreateBuilder()
                                       .SetUser(_userSessionData)
                                       .UseLogger(new DebugLogger(LogLevel.Exceptions))
                                       .SetRequestDelay(delay)
                                       .Build();

            try
            {
                if (File.Exists(stateFile))
                {
                    using (var fs = File.OpenRead(stateFile))
                    {
                        _instaApi.LoadStateDataFromStream(fs);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task LoginAsync()
        {
            var delay = RequestDelay.FromSeconds(2, 2);

            if (!_instaApi.IsUserAuthenticated)
            {
                Console.WriteLine($"Logging in as {_userSessionData.UserName}");
                delay.Disable();
                var logInResult = await _instaApi.LoginAsync();
                delay.Enable();
            }
            var state = _instaApi.GetStateDataAsStream();
            using (var fileStream = File.Create(stateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }
        }

        public async Task UploadPhotoAsync(Post post)
        {
            Image image = post.Images.First();

            if (image != null)
            {
                var mediaImage = new InstaImage
                {
                    Height = image.Height,
                    Width = image.Width,
                    URI = new Uri(Path.GetFullPath(image.PathResize), UriKind.Absolute).LocalPath
                };
                var result = await _instaApi.UploadPhotoAsync(mediaImage, post.Subtitle);

                post.IsPosted = result.Succeeded;
            }
            else
            {
                throw new Exception("O post não possui imagem associada!");
            }
        }

        public async Task UploadPhotosAlbumAsync(Post post)
        { 
            if (post.Images != null && post.Images.Count > 0)
            {
                List<InstaImage> listInstaImage = new List<InstaImage>();

                foreach (Image image in post.Images.ToList())
                {
                    var mediaImage = new InstaImage
                    {
                        Height = image.Height,
                        Width = image.Width,
                        URI = new Uri(Path.GetFullPath(image.PathResize), UriKind.Absolute).LocalPath
                    };
                    listInstaImage.Add(mediaImage);
                }
                var result = await _instaApi.UploadPhotosAlbumAsync(listInstaImage.ToArray(), post.Subtitle);
                 
                post.IsPosted = result.Succeeded;
            }
            else
            {
                throw new Exception("O post não possui imagem associada!");
            }
        }
    }
}