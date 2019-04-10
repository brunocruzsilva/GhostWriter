using ImageMagick;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoWriter.Robots;
using VideoWriter.Struct;

namespace VideoWriter
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        static System.Drawing.Size GetThumbnailSize(System.Drawing.Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 40;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new System.Drawing.Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        private void buttonCreateVideo_Click(object sender, EventArgs e)
        {
        }

        private async void buttonCreatePost_Click(object sender, EventArgs e)
        {
            bool successPost = await CreatePost();
        }

        private async Task<bool> CreatePost()
        {
            StateBot stateBot = new StateBot();
            TextBot textBot = new TextBot();
            ImageBot imageBot = new ImageBot();
            InstagramBot instaBot = new InstagramBot();

            Post post = await stateBot.Load();

            post.Keyword = textBoxNameActor.Text;

            await textBot.FetchTvCreditsFromTMDB(post);
            await textBot.FetchBiographyFromWikipedia(post);
            await textBot.SanitizeContent(post);

            await stateBot.Save(post);

            await imageBot.FetchImagesFromGoogle(post);
            await imageBot.DownloadImages(post);
            await imageBot.ScaleImages(post);

            await stateBot.Save(post);

            await instaBot.LoginAsync();

            if (post.Images.Count <= 1)
                await instaBot.UploadPhotoAsync(post);
            else
                await instaBot.UploadPhotosAlbumAsync(post);

            await imageBot.DeleteFileImages(post);

            return true;
        }
    }
}
