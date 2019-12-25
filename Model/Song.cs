using System;
using System.IO;

namespace AudioFil
{
    public class Song : BaseSource
    {
        public string Artist { get; set; }
        public DateTime Time { get; set; }

        public string Title
        {
            get => Path.GetFileNameWithoutExtension(Url != null ? Url.ToString() : "");
        }

        public Song(string name, Uri url)
        {
            Name = name;
            Url = url;
        }
    }
}
