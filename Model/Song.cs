using System;

namespace AudioFil
{
    public class Song : BaseSource
    {
        public string Artist { get; set; }
        public DateTime Time { get; set; }


        public Song(string name, Uri url)
        {
            Name = name;
            Url = url;
        }
    }
}
