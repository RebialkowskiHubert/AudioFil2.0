using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows;

namespace AudioFil.Helpers
{
    public class XMLHandling
    {
        private XmlDocument doc;
        private readonly string pathRadio = "Playlista.xml";


        public ObservableCollection<BaseSource> LoadRadios()
        {
            ObservableCollection<BaseSource> radios;
            doc = new XmlDocument();
            try
            {
                if (!File.Exists(pathRadio))
                    CreateRadioPlaylistFile();

                if (!File.Exists(GetPlaylistPath()))
                    CreateSongsPlaylistFile();

                doc.Load(pathRadio);
                int stations = doc.GetElementsByTagName("Stacja").Count;
                radios = new ObservableCollection<BaseSource>();

                int i;
                for (i = 0; i < stations; i++)
                {
                    XmlNode station = doc.GetElementsByTagName("Stacja").Item(i);
                    var id = int.Parse(station.FirstChild.InnerText);
                    var nazwa = station.LastChild.PreviousSibling.InnerText;
                    var url = new Uri(station.LastChild.InnerText);
                    radios.Add(new Radio(id, nazwa, url));
                }

                return radios;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<BaseSource> LoadSongs()
        {
            ObservableCollection<BaseSource> songs = null;
            doc = new XmlDocument();
            try
            {
                if (!File.Exists(pathRadio))
                    CreateRadioPlaylistFile();

                string playlistPath = GetPlaylistPath();

                if (!File.Exists(playlistPath))
                    CreateSongsPlaylistFile();

                doc.Load(playlistPath);
                int songsCounter = doc.GetElementsByTagName("media").Count;
                songs = new ObservableCollection<BaseSource>();

                int i;
                for (i = 0; i < songsCounter; i++)
                {
                    XmlNode song = doc.GetElementsByTagName("media").Item(i);
                    string nazwa = i.ToString();
                    Uri url = new Uri(song.Attributes["src"].Value);
                    songs.Add(new Song(nazwa, url));
                }

                return songs;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void AddRadio(Radio r)
        {
            doc = new XmlDocument();
            try
            {
                doc.Load(pathRadio);
                int count = (doc.GetElementsByTagName("Stacja").Count) + 1;

                XmlNode root = doc.SelectSingleNode("AudioFil");
                XmlNode station = doc.CreateNode(XmlNodeType.Element, "Stacja", null);
                XmlNode id = doc.CreateNode(XmlNodeType.Element, "Id", null);
                id.InnerText = count.ToString();
                XmlNode name = doc.CreateNode(XmlNodeType.Element, "Nazwa", null);
                name.InnerText = r.Name;
                XmlNode url = doc.CreateNode(XmlNodeType.Element, "Url", null);
                url.InnerText = r.Url.ToString();

                station.AppendChild(id);
                station.AppendChild(name);
                station.AppendChild(url);
                root.AppendChild(station);

                doc.Save(pathRadio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRadio(Radio old, Radio newr)
        {
            doc = new XmlDocument();

            try
            {
                doc.Load(pathRadio);

                foreach(XmlNode node in doc.GetElementsByTagName("Stacja"))
                {
                    if (node.FirstChild.InnerText.Equals(old.Id.ToString()))
                    {
                        node.FirstChild.NextSibling.InnerText = newr.Name;
                        node.LastChild.InnerText = newr.Url.ToString();
                        break;
                    }
                }

                doc.Save(pathRadio);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteRadio(Radio r)
        {
            try
            {
                XDocument xDoc = XDocument.Load(pathRadio);
                xDoc.Root.Elements("Stacja").Elements("Id").Where(stat => stat.Value == r.Id.ToString()).Select(stat => stat.Parent).Remove();
                xDoc.Save(pathRadio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddSong(string songPath)
        {
            XDocument xdoc = XDocument.Load(GetPlaylistPath());

            xdoc.Root.Element("body").Element("seq").AddFirst
            (
                new XElement("media", new XAttribute("src", songPath))
            );

            xdoc.Save(GetPlaylistPath());
        }

        public string GetSongPath()
        {
            doc = new XmlDocument();
            try
            {
                doc.Load(pathRadio);

                XmlNode root = doc.SelectSingleNode("AudioFil");

                return root.SelectSingleNode("MusicPath").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetPlaylistPath()
        {
            doc = new XmlDocument();
            try
            {
                doc.Load(pathRadio);

                XmlNode root = doc.SelectSingleNode("AudioFil");

                return root.SelectSingleNode("PlaylistPath").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CreateRadioPlaylistFile()
        {
            using StreamWriter sw = new StreamWriter(pathRadio);
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sw.WriteLine("<AudioFil>");
            sw.WriteLine("</AudioFil>");
        }

        private void CreateSongsPlaylistFile()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\" + "Listy odtwarzania");
            using StreamWriter sw = new StreamWriter(GetPlaylistPath());
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<?wpl version=\"1.0\"?>\n<smil>\n<head>\n<meta name=\"Generator\" content=\"Microsoft Windows Media Player -- 12.0.17134.48\"/>\n<meta name=\"ItemCount\" content=\"146\"/>\n<meta name=\"IsFavorite\"/>\n<meta name=\"ContentPartnerListID\"/>\n<meta name=\"ContentPartnerNameType\"/>\n<meta name=\"ContentPartnerName\"/>\n<meta name=\"Subtitle\"/>\n<author/>\n<title>MUZA</title>\n</head>\n<body>\n<seq>\n</seq>\n</body>\n</smil>");
        }
    }
}
