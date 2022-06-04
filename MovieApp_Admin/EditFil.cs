using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using Firebase.Storage;
using System.IO;
using myProp = MovieApp_Admin.Properties.Settings;
using System.Collections;
using System.Net;

namespace MovieApp_Admin
{
    public partial class EditFil : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string imgPath = "";
        private string vidPath = "";
        private InfoFilm flm;
        private string fID;
        DocumentReference docref;
        private int defaultH;
        public EditFil(string fID)
        {
            InitializeComponent();
            this.fID = fID;
            
        }

        private void EditFilm_Load(object sender, EventArgs e)
        {
            docref = db.Collection("Films").Document(fID);
            defaultH = lbCategory.Height;
            GetAllData();
        }

        async void GetAllData()
        {
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                flm = snap.ConvertTo<InfoFilm>();
                int i = 0;
                foreach(var item in lbCategory.Items)
                {
                    if (flm.category.Contains(item.ToString()))
                    {
                        lbCategory.SetSelected(i, true);
                    }
                    i++;
                }

                if (flm.poster != "")
                {
                    using (WebClient web = new WebClient())
                    {
                        Stream myIMG = web.OpenRead(flm.poster);
                        Bitmap bit = new Bitmap(myIMG);
                        if (bit != null) pictureBox1.Image = bit;
                        myIMG.Flush();
                        myIMG.Close();
                    }
                }
                richTextBox_Descript.Text = flm.descript;
                textBox_Name.Text = flm.name;
                richTextBox_Eps.Text = flm.eps.ToString();
                if (flm.genre == "Movies")
                {
                    comboBox1.Text = "Movies";
                }
                else
                {
                    comboBox1.Text = "TV series";
                }
                richTextBox_Director.Text = flm.director;
                richTextBox_Time.Text = flm.time.ToString();
                richTextBox_Year.Text = flm.year.ToString();
            }
        }

        private async void guna2Button5_Click(object sender, EventArgs e)
        {
            if (imgPath != "")
            {
                var streamPos = File.Open(imgPath, FileMode.Open);
                var task = new FirebaseStorage(
                    "filmreview-de9c4.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                        ThrowOnCancel = true
                    }).Child("FilmPoster").Child(fID + ".jpg").PutAsync(streamPos);
                flm.poster = await task;
                streamPos.Close();
            } 
            if(vidPath != "")
            {
                var streamTra = File.Open(vidPath, FileMode.Open);
                var task1 = new FirebaseStorage(
                    "filmreview-de9c4.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                        ThrowOnCancel = true
                    }).Child("FilmTrailers").Child(fID + ".mp4").PutAsync(streamTra);
                string trailer = await task1;
                streamTra.Close();
            }
            flm.name = textBox_Name.Text;
            flm.descript = richTextBox_Descript.Text;
            flm.eps = Convert.ToInt32(richTextBox_Eps.Text);
            flm.director = richTextBox_Director.Text;
            flm.time = Convert.ToInt32(richTextBox_Time.Text);
            flm.year = Convert.ToInt32(richTextBox_Year.Text);
            flm.genre = comboBox1.Text;
            List<string> list = new List<string>();
            foreach (var item in lbCategory.SelectedItems)
            {
                list.Add(item.ToString());
            }
            flm.category = list;
            await docref.SetAsync(flm);
            MessageBox.Show("Update thành công");
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                imgPath = open.FileName;
                var stream = File.OpenRead(open.FileName);
                Bitmap map = new Bitmap(stream);
                pictureBox1.Image = map;
                stream.Close();
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*" +
                ".mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;" +
                "*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;" +
                "*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;" +
                "*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;" +
                "*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            if (open.ShowDialog() == DialogResult.OK)
            {
                vidPath = open.FileName;
                
            }
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            lbCategory.Height = defaultH * 10;
        }

        private void lbCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbCategory.Height = defaultH;
            }
        }
    }
}
