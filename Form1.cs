using System;
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
using System.IO;

namespace Bandcamp_Downloader
{
    public partial class Form1 : Form
    {
        String path;
        List<String> urls;
        List<String> names;

        public Form1()
        {
            InitializeComponent();
            urls = new List<string>();
            names = new List<string>();
            path = "";
        }

        private void parsePage()
        {
            WebClient wc = new WebClient();
            String source;
            String pattern = "<a href=\"[A-Za-z0-9/.:-]*\" itemprop=\"url\"><span itemprop=\"name\">.[^<]*</span></a>";
            Regex r = new Regex(pattern);
            Regex rname = new Regex("name\">.[^<]*<");
            Regex rurl = new Regex("href=\"[A-Za-z0-9/.:-]*\"");
            String burl = textBox1.Text.Remove(textBox1.Text.LastIndexOf('/'));
            burl = burl.Remove(burl.LastIndexOf('/'));

            source = wc.DownloadString(textBox1.Text);

            foreach (Match m in r.Matches(source))
            {
                String tname = rname.Match(m.ToString()).ToString();
                String turl = rurl.Match(m.ToString()).ToString();

                turl = turl.Remove(turl.Length-1);
                turl = turl.Remove(0, 6);

                tname = tname.Remove(tname.Length - 1);
                tname = tname.Remove(0, 6);
                urls.Add(burl + turl);
                names.Add(tname);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while(folderBrowserDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Vous devez choisir un dossier !");
            }
            path = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((path != "") && (textBox1.Text != ""))
            {
                parsePage();
                String album = "";
                String author = "";
                album = textBox1.Text.Substring(textBox1.Text.LastIndexOf('/') + 1);
                author = textBox1.Text.Substring(7, textBox1.Text.IndexOf('.') - 7);

                path = path + "/" + author + "/" + album + "/";
                Directory.CreateDirectory(path);

                listMusic lm = new listMusic(urls, names, path, album, author);
                lm.Show();
                
            }
            else
            {
                MessageBox.Show("Vous devez sélectionner un dossier et entrer une URL !");
            }
        }
    }
}
