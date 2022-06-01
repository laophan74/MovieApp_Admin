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

namespace MovieApp_Admin
{
    public partial class AddFilm : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlPos = "";
        private string urlTrailer = "";
        private string urlDirector = "";
        private int defaultH;
        public AddFilm()
        {
            InitializeComponent();
            defaultH = lbCategory.Height;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                urlPos = open.FileName;
                var stream = File.OpenRead(open.FileName);
                Bitmap map = new Bitmap(stream);
                pictureBox1.Image = map;
                stream.Flush();
                stream.Close();
            }
        }

        private async void guna2Button5_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(descript.Text) ||
                string.IsNullOrEmpty(eps.Text) ||
                string.IsNullOrEmpty(genre.Text) ||
                string.IsNullOrEmpty(time.Text) ||
                string.IsNullOrEmpty(country.Text) ||
                string.IsNullOrEmpty(year.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (var item in lbCategory.SelectedItems)
                {
                    list.Add(item.ToString());
                }
                var InfoFilm = new InfoFilm
                {
                    name = name.Text,

                    descript = descript.Text,

                    poster = "",

                    category = list,

                    year = Convert.ToInt32(year.Text),

                    trailer = "",

                    genre = genre.SelectedItem.ToString(),
                    totalPoint = 0,
                    rating = 0,

                    numRate = 0,
                    time = Convert.ToInt32(time.Text),

                    eps = Convert.ToInt32(eps.Text),
                    director = director.Text,
                    country = country.SelectedItem.ToString(),
                };
                DocumentReference docRef = await db.Collection("Films").AddAsync(InfoFilm);
                string addID = docRef.Id;
                string upPos = addID + ".jpg";
                string upTra = addID + ".mp4";
                string poster = "", trailer = "";
                //poster
                if (urlPos != "")
                {
                    var streamPos = File.Open(urlPos, FileMode.Open);
                    var task = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("FilmPoster").Child(upPos).PutAsync(streamPos);
                    poster = await task;
                    streamPos.Close();
                }
                //trailer
                if (urlTrailer != "")
                {
                    var streamTra = File.Open(urlTrailer, FileMode.Open);
                    var task1 = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("FilmTrailers").Child(upTra).PutAsync(streamTra);
                    trailer = await task1;
                    streamTra.Close();
                }
                Dictionary<string, object> update = new Dictionary<string, object>
                {
                    { "poster", poster },
                    { "trailer", trailer }
                };
                await docRef.UpdateAsync(update);
                MessageBox.Show("Thêm Film thành công!");
                this.Close();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
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
                label_trailer.Text = open.FileName;
                /*axWindowsMediaPlayer1.URL = open.FileName;
                axWindowsMediaPlayer1.Ctlcontrols.play();*/
                urlTrailer = open.FileName;
            }
        }

        private void lbCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbCategory.Height = defaultH;
            }
        }

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(descript.Text) ||
                string.IsNullOrEmpty(eps.Text) ||
                string.IsNullOrEmpty(genre.Text) ||
                string.IsNullOrEmpty(time.Text) ||
                string.IsNullOrEmpty(director.Text) ||
                string.IsNullOrEmpty(year.Text))
            {
                MessageBox.Show("Vui lòng nhập lại thông tin!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (var item in lbCategory.SelectedItems)
                {
                    list.Add(item.ToString());
                }
                var InfoFilm = new InfoFilm
                {
                    name = name.Text,

                    descript = descript.Text,

                    poster = "",

                    category = list,

                    year = Convert.ToInt32(year.Text),

                    trailer = "",

                    genre = genre.Text,
                    totalPoint = 0,
                    rating = 0,

                    numRate = 0,
                    time = Convert.ToInt32(time.Text),

                    eps = Convert.ToInt32(eps.Text),
                    director = director.Text,
                    directorava = ""
                };
                DocumentReference docRef = await db.Collection("Films").AddAsync(InfoFilm);
                string addID = docRef.Id;
                string upPos = addID + ".jpg";
                string upDirector = addID + ".jpg";
                string upTra = addID + ".mp4";
                string poster = "", trailer = "", directorava = "";
                //poster
                if (urlPos != "")
                {
                    var streamPos = File.Open(urlPos, FileMode.Open);
                    var task = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("FilmPoster").Child(upPos).PutAsync(streamPos);
                    poster = await task;
                    streamPos.Close();
                }
                //directorava
                if (urlDirector != "")
                {
                    var streamPos = File.Open(urlDirector, FileMode.Open);
                    var task = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("Directors").Child(upDirector).PutAsync(streamPos);
                    directorava = await task;
                    streamPos.Close();
                }
                //trailer
                if (urlTrailer != "")
                {
                    var streamTra = File.Open(urlTrailer, FileMode.Open);
                    var task1 = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("FilmTrailers").Child(upTra).PutAsync(streamTra);
                    trailer = await task1;
                    streamTra.Close();
                }
                Dictionary<string, object> update = new Dictionary<string, object>
                {
                    { "poster", poster },
                    { "trailer", trailer }
                };
                await docRef.UpdateAsync(update);
                MessageBox.Show("Thêm Film thành công!");
                this.Close();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                urlDirector = open.FileName;
                var stream = File.OpenRead(open.FileName);
                Bitmap map = new Bitmap(stream);
                pictureBox2.Image = map;
                stream.Flush();
                stream.Close();
            }
        }

        private void eps_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbCategory.Height = defaultH * 5;

        }
    }
}
