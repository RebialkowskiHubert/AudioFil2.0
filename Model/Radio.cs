using System;
using System.Threading.Tasks;

namespace AudioFil
{
    public class Radio : BaseSource
    {
        private bool running;
        public bool Running
        {
            get => running;
            set
            {
                running = value;
                if (!running && RunningTask != null)
                    RunningTask.Wait();
            }
        }

        private string metadata;
        public string Metadata
        {
            get => metadata;
            set
            {
                OnMetadataChanged?.Invoke(this, new MetadataEventArgs(metadata, value));
                metadata = value;
            }
        }

        public Task RunningTask;

        public event EventHandler<MetadataEventArgs> OnMetadataChanged;

        public Radio(int id, string name, Uri url)
        {
            Id = id;
            Name = name;
            Url = url;
        }

    }
}
