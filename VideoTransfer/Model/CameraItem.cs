using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTransfer.Model
{
    public class CameraItem
    {
        public CameraItem(string path, bool isDirectory)
        {
            Path = path;
            IsDirectory = isDirectory;
        }
        public string Path { get; set; }
        public bool IsDirectory { get; set; }

        protected bool Equals(CameraItem other)
        {
            return string.Equals(Path, other.Path) && IsDirectory == other.IsDirectory;
        }
        public override bool Equals(object obj)
        {
            var a = obj as CameraItem;
            return a != null ? Equals(a) : base.Equals(obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Path?.GetHashCode() ?? 0) * 397) ^ IsDirectory.GetHashCode();
            }
        }
    }
}
