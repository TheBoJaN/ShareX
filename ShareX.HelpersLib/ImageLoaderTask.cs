using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace ShareX.HelpersLib
{
    class ImageLoaderTask
    {
        private string filePath;
        private Action<Image> callback;

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }

        public ImageLoaderTask(string filePath, Action<Image> callback)
        {
            this.filePath = filePath;
            this.callback = callback;
        }

        public void Load()
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && Helpers.IsImageFile(filePath) && File.Exists(filePath))
                {
                    callback.Invoke(ImageHelpers.LoadImage(filePath));
                }
                else if (Uri.IsWellFormedUriString(filePath, UriKind.Absolute))
                {
                    WebClient client = new WebClient();

                    byte[] imageData = client.DownloadData(filePath);
                    using (MemoryStream stream = new MemoryStream(imageData))
                        callback.Invoke(Image.FromStream(stream));
                }
                else
                {
                    callback.Invoke(null);
                }
            }
            catch
            {
                callback.Invoke(null);
            }
        }
    }
}
