using System.IO;

namespace ULTeam.Utils
{
    public delegate void ProgressChangeDelegate(double persentage, ref bool cancel);
    public delegate void Completedelegate();

    public class CustomFileCopier
    {
        public CustomFileCopier(string source, string dest)
        {
            SourceFilePath = source;
            DestFilePath = dest;

            OnProgressChanged += delegate { };
            OnComplete += delegate { };
        }

        public void Copy()
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer

            using (FileStream source = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
            {
                long fileLength = source.Length;
                using (FileStream dest = new FileStream(DestFilePath, FileMode.CreateNew, FileAccess.Write))
                {
                    long totalBytes = 0;
                    int currentBlockSize;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double persentage = totalBytes * 100.0 / fileLength;

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

            OnComplete?.Invoke(); 
        }

        public string SourceFilePath { get; set; }
        public string DestFilePath { get; set; }

        public event ProgressChangeDelegate OnProgressChanged;
        public event Completedelegate OnComplete;
    }
}
