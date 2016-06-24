using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace WpfMvvmSample.Model.Classes
{
    public class Media
    {
        [XmlElement("path")]
        public string path { get; set; }
        [XmlIgnore]
        public string filename { get; set; }
        [XmlIgnore]
        public string type { get; set; }
        [XmlIgnore]
        public string album { get; set; }
        [XmlIgnore]
        public string year { get; set; }
        [XmlIgnore]
        public string genre { get; set; }
        [XmlIgnore]
        public string title { get; set; }
        [XmlIgnore]
        public string length { get; set; }
        [XmlIgnore]
        public string autor { get; set; }
        [XmlIgnore]
        public string error { get; set; }
        [XmlIgnore]
        public string all { get; set; }

        public Media()
        {
            this.path = "";
            this.filename = "";
            this.type = "inconnu";
            this.album = "inconnu";
            this.year = "inconnu";
            this.genre = "inconnu";
            this.title = "inconnu";
            this.length = "00:00:05";
            this.autor = "inconnu";
            this.error = "None";
            this.all = "inconnu";
        }
        public string toString()
        {
            string str = String.Format("path = {0}, filename = {1}, type = {2}, album = {3}, year = {4}, genre = {5}, title = {6}, length = {7}, autor = {8}",
                                        path, filename, type, album, year, genre, title, length, autor);
            return str;
        }
        public object this[string propertyName]
        {
            get
            {
                Type myType = typeof(Media);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(Media);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }

    public class MediaDirectory
    {
        public List<Media> mediaList { get; set; }
        public string name { get; set; }
        public MediaDirectory()
        {
            this.mediaList = new List<Media>();
        }
        public MediaDirectory(string path)
        {
            this.mediaList = new List<Media>();
            this.mediaList = Model.deserialize(path).mediaList;
        }

        // order = 0 for DESC order
        public List<Media> orderBy(string attr, int order)
        {
            List<Media> lm = new List<Media>();
            IEnumerable<Media> rq;

            if (order == 0)
                rq = from m in mediaList orderby m[attr] descending select m;
            else
                rq = from m in mediaList orderby m[attr] select m;
            foreach (Media m in rq)
            {
                lm.Add(m);
            }
            this.mediaList = lm;
            return this.mediaList;
        }
        public List<Media> filterBy(string attr, string query)
        {
            List<Media> lm = new List<Media>();
            IEnumerable<Media> rq;
            rq = from m in mediaList where m[attr].ToString().ToUpper().Contains(query.ToUpper()) select m;
            foreach (Media m in rq)
            {
                lm.Add(m);
            }
            return lm;
        }
        public void moveMediaToPos(Media m, int pos)
        {
            mediaList.Remove(m);
            mediaList.Insert(pos, m);
        }

    }

    class Model
    {
        public static void getFileMetaData(Media m)
        {
            int idx = m.path.LastIndexOf('\\');
            string directory = m.path.Substring(0, idx);
            string filename = m.path.Substring(idx + 1, m.path.Length - idx - 1);
            List<string> arrHeaders = new List<string>();
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder objFolder;
            objFolder = shell.NameSpace(@directory);
            m.filename = filename;
            String ext = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.'));
            if (ext.Equals(".avi") || ext.Equals(".mkv") || ext.Equals(".wmv") || ext.Equals(".mov") || ext.Equals("mp4") || ext.Equals(".mpeg") || ext.Equals(".mpg"))
            {
                m.type = "Video";
            }
            else if (ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".bmp") || ext.Equals(".gif"))
            {
                m.type = "Image";
            }
            else if (ext.Equals(".mp3"))
            {
                m.type = "Musique";
            }
            else
            {
                m.type = "Autre";
            }

            for (int i = 0; i < short.MaxValue; i++)
            {
                string header = objFolder.GetDetailsOf(null, i);
                if (String.IsNullOrEmpty(header))
                    break;
                arrHeaders.Add(header);
            }

            foreach (Shell32.FolderItem2 item in objFolder.Items())
            {
                if (objFolder.GetDetailsOf(item, 0).Equals(@filename))
                {
                    List<int> idList = new List<int> { 11, 14, 15, 16, 20, 21, 27 };
                    if (objFolder.GetDetailsOf(item, idList[0]).Length > 0)
                        m.type = objFolder.GetDetailsOf(item, idList[0]);
                    if (objFolder.GetDetailsOf(item, idList[1]).Length > 0)
                        m.album = objFolder.GetDetailsOf(item, idList[1]);
                    if (objFolder.GetDetailsOf(item, idList[2]).Length > 0)
                        m.year = objFolder.GetDetailsOf(item, idList[2]);
                    if (objFolder.GetDetailsOf(item, idList[3]).Length > 0)
                        m.genre = objFolder.GetDetailsOf(item, idList[3]);
                    if (objFolder.GetDetailsOf(item, idList[4]).Length > 0)
                        m.autor = objFolder.GetDetailsOf(item, idList[4]);
                    if (objFolder.GetDetailsOf(item, idList[5]).Length > 0)
                        m.title = objFolder.GetDetailsOf(item, idList[5]);
                    if (objFolder.GetDetailsOf(item, idList[6]).Length > 0)
                        m.length = objFolder.GetDetailsOf(item, idList[6]);
                }
            }
            if (m.title.Equals("inconnu"))
                m.title = m.filename;
            m.all = m.type + m.genre + m.autor + m.title;
        }
        static public MediaDirectory deserialize(string path)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(MediaDirectory));
                TextReader reader = new StreamReader(@path);
                object obj = deserializer.Deserialize(reader);
                MediaDirectory XmlData = (MediaDirectory)obj;
                reader.Close();
                foreach (Media m in XmlData.mediaList)
                {
                    Model.getFileMetaData(m);
                }
                return XmlData;
            }
            catch (Exception e)
            {
            }
            return new MediaDirectory();
        }

        static public Boolean serialize(MediaDirectory md, string playlistPath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MediaDirectory));
                using (TextWriter writer = new StreamWriter(@playlistPath))
                {
                    serializer.Serialize(writer, md);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
    

}
