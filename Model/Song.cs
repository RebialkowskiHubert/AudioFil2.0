using System;

namespace AudioFil
{
    public class Song : BaseSource
    {
        public string Artist { get; set; }
        public string Path { get; set; }
        public DateTime Time { get; set; }
    }
}
