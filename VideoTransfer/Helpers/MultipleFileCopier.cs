﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VideoTransfer.Helpers
{
    public delegate void ProgressChangeDelegate(double persentage, ref bool cancel);
    public delegate void Completedelegate();

    public class MultipleFileCopier
    {
        #region Private fields

        private readonly List<Copy> _copies;
        private readonly bool _deleteVideos;
        private long _totalLength, _totalBytes;
        private int _copyInProgress; 

        #endregion

        #region Constructors

        public MultipleFileCopier(List<Copy> copies, bool deleteVideos)
        {
            _copies = copies;
            _deleteVideos = deleteVideos;
            OnProgressChanged += delegate { };
            OnComplete += delegate { };
        }
        
        #endregion

        #region Public methods

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
                }).ContinueWith(r =>
                {
                    if (r.Exception != null || !r.IsCompleted || !_deleteVideos) return;
                    File.Delete(copy.SourcePath);
                    foreach (var file in Directory.GetFiles(Path.GetDirectoryName(copy.SourcePath), Path.GetFileName(copy.SourcePath).Replace("MP4", "*")))
                    {
                        File.Delete(file);
                    }
                }); 
            }
        }

        #endregion

        #region Private methods

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

        #endregion

        #region Events

        public event ProgressChangeDelegate OnProgressChanged;
        public event Completedelegate OnComplete;

        #endregion
    }

    public class Copy
    {
        #region Constructors

        public Copy(string sourcePath, string destinationPath)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
        }

        #endregion

        #region Properties

        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        #endregion
    }
}