using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VideoTransfer.Helpers
{
    public delegate void ProgressChangeDelegate(double persentage, ref bool cancel);
    public delegate void Completedelegate();

    public class MultipleFileCopier
    {
        private readonly List<Copy> _copies;
        private long _totalLength, _totalBytes;
        private int _copyInProgress;

        public MultipleFileCopier(List<Copy> copies)
        {
            _copies = copies;

            OnProgressChanged += delegate { };
            OnComplete += delegate { };
        }

        public async void Copy()
        {
            _copyInProgress = _copies.Count;
            foreach (var copy in _copies)
            {
                using (FileStream source = new FileStream(copy.SourcePath, FileMode.Open, FileAccess.Read))
                {
                    _totalLength += source.Length;
                }
            }
            foreach (var copy in _copies)
            {
                await Task.Run(() =>
                {
                    Copy(copy.SourcePath, copy.DestinationPath);
                });
            }
        }

        private void Copy(string sourcePath, string destinationPath)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer


            var copyNumber = 1;
            var initialDestPath = destinationPath;
            while (File.Exists(destinationPath))
            {
                var ext = Path.GetExtension(initialDestPath);
                destinationPath = initialDestPath.Replace(ext, "") + $"({copyNumber++})" + ext; 
            }


            using (FileStream source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                using (FileStream dest = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write))
                {
                    int currentBlockSize;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        _totalBytes += currentBlockSize;
                        double persentage = _totalBytes * 100.0 / _totalLength;

                        dest.Write(buffer, 0, currentBlockSize);

                        var cancelFlag = false;
                        OnProgressChanged?.Invoke(persentage, ref cancelFlag);

                        if (cancelFlag)
                        {
                            // Delete dest file here
                            break;
                        }
                    }
                }
            }
            CopyCompleted();
        }

        private void CopyCompleted()
        {
            if (--_copyInProgress == 0)
                OnComplete?.Invoke();
        }


        public event ProgressChangeDelegate OnProgressChanged;
        public event Completedelegate OnComplete;
    }

    public class Copy
    {
        public Copy(string sourcePath, string destinationPath)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
        }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
    }
}
