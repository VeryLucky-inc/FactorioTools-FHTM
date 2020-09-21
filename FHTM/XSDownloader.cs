using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downloader
{
    public class FileDownload
    {
        public delegate void MethodDownloading(FileDownload fd);
        public event MethodDownloading DownloadingDone;
        public event MethodDownloading DownloadingStarted;
        public event MethodDownloading IsDownloadingEv;
        private volatile Boolean IsDownloading;
        private readonly int SizeOfC = 5120;
        private Lazy<Int64> FileSize;
        private String DownloadURL;
        private String DestinationPath;
        public String Title { get; set; }
        public IProgress<double> DownloadingProgress;
        public Int64 BytesWritten { get; private set; }
        public Int64 ContentLength => FileSize.Value;
        public Boolean Done => ContentLength == BytesWritten;
        public FileDownload(String DownloadURL, String DestinationFolderPath)
        {
            if (string.IsNullOrEmpty(DownloadURL))
            {
                throw new ArgumentNullException("Не указана ссылка");
            }
            if (string.IsNullOrEmpty(DestinationFolderPath))
            {
                throw new ArgumentNullException("Не указан путь");
            }
            this.IsDownloading = true;
            this.DownloadURL = DownloadURL;
            this.DestinationPath = GetFilePath(DestinationFolderPath);
            this.FileSize = new Lazy<long>(GetFileSize);
            this.DownloadingProgress = null;
            if (!File.Exists(DestinationPath))
            {
                BytesWritten = 0;
            }
            else
            {
                try
                {
                    BytesWritten = new FileInfo(DestinationPath).Length;
                }
                catch
                {
                    BytesWritten = 0;
                }
            }
        }
        private Int64 GetFileSize()
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(DownloadURL);
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            using (HttpWebResponse Response = (HttpWebResponse)Request.GetResponse())
            {
                if (Response.ContentLength == 0)
                {
                    throw new ArgumentNullException("Файл не может весить менее 1 байта");
                }
                return Response.ContentLength;
            }
        }
        private String GetFilePath(String DestinationPath)
        {
            if (!Directory.Exists(DestinationPath))
            {
                Directory.CreateDirectory(DestinationPath);
            }
            String FileName = Path.GetFileName(DownloadURL);
            if (String.IsNullOrEmpty(FileName))
            {
                throw new ArgumentNullException("Файл не может не иметь имени");
            }
            return Path.Combine(DestinationPath, FileName);
        }
        private async void Start(Int64 ByteAlreadyExists)
        {
            DownloadingStarted?.Invoke(this);
            if (!IsDownloading)
            {
                throw new InvalidOperationException();
            }
            if (Done)
            {
                DownloadingDone?.Invoke(this);
                return;
            }
            HttpWebRequest DownloadRequest = (HttpWebRequest)HttpWebRequest.Create(DownloadURL);
            DownloadRequest.Method = "GET";
            DownloadRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36";
            DownloadRequest.AddRange(ByteAlreadyExists);
            using (var DownloadResponse = await DownloadRequest.GetResponseAsync())
            {
                using (Stream DownloadResponseStream = DownloadResponse.GetResponseStream())
                {
                    using (FileStream SaveFileStream = new FileStream(DestinationPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        while (IsDownloading)
                        {
                            Byte[] DownloadBuffer = new Byte[SizeOfC];
                            Int32 BytesRead = await DownloadResponseStream.ReadAsync(DownloadBuffer, 0, DownloadBuffer.Length).ConfigureAwait(false);
                            if (BytesRead == 0) break;
                            await SaveFileStream.WriteAsync(DownloadBuffer, 0, BytesRead);
                            BytesWritten += BytesRead;
                            
                            IsDownloadingEv?.Invoke(this);
                            DownloadingProgress?.Report((double)BytesWritten / ContentLength);
                        }
                        await SaveFileStream.FlushAsync();
                        SaveFileStream.Close();
                        if (ContentLength == BytesWritten)
                        {
                            DownloadingDone?.Invoke(this);
                        }
                    }
                }
            }
        }
        public void Start()
        {
            IsDownloading = true;
            Start(BytesWritten);
        }
        public void Pause()
        {
            IsDownloading = false;
        }
    }


}
