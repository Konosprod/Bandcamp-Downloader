using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using TagLib;

namespace Bandcamp_Downloader
{
    public partial class listMusic : Form
    {
        String dir;
        List<string> urls;
        List<string> names;
        bool check = false;
        String al;
        String aut;
        public listMusic(List<String> urls, List<String>names, String path, String album, String author)
        {
            InitializeComponent();

            foreach (string n in names)
            {
                checkedListBox1.Items.Add(n, true);
            }

            dir = path;
            this.urls = urls;
            this.names = names;
            al = album;
            aut = author;
        }

        private void downloadMusic(String url, String name, int num)
        {
            WebClient wc = new WebClient();
            string pattern = @"http://[a-z0-9]*.bandcamp.com/download/track\?enc=[a-z0-9-]*&fsig=[a-z0-9]*&id=[a-z0-9]*&stream=[0-9]*&ts=[a-z0-9.]*";
            Regex r = new Regex(pattern);
            String source = "";
            source = wc.DownloadString(url);
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex rn = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            name = rn.Replace(name, "_");
            MessageBox.Show(r.Match(source).ToString());
            wc.DownloadFile(r.Match(source).ToString(), dir + name + ".mp3");
            TagLib.File tagFile = TagLib.File.Create(dir + name + ".mp3");
            tagFile.Tag.Album = al;
            tagFile.Tag.Track = (uint)num;
            tagFile.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (int i in checkedListBox1.CheckedIndices)
            {
                downloadMusic(urls[i], names[i], i+1);
            }
            MessageBox.Show("Téléchargement terminé !");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (check)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                else
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }

            if (check)
            {
                check = false;
            }
            else
            {
                check = true;
            }
        }
    }
}
