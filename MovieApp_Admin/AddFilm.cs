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
        private string urlPos;
        private string urlTrailer;
        public static List<string> list = new List<string>();
        public AddFilm()
        {
            InitializeComponent();
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
            if (//string.IsNullOrEmpty(richTextBox_Category.Text) ||
                string.IsNullOrEmpty(richTextBox_Descript.Text) ||
                string.IsNullOrEmpty(richTextBox_Eps.Text) ||
                string.IsNullOrEmpty(richTextBox_Genre.Text) ||
                string.IsNullOrEmpty(richTextBox_Time.Text) ||
                string.IsNullOrEmpty(richTextBox_director.Text) ||
                //string.IsNullOrEmpty(richTextBox_trailer.Text) ||
                string.IsNullOrEmpty(richTextBox_Year.Text)
                )
            {
                MessageBox.Show("Vui lòng nhập lại thông tin!");
            }
            else
            {
                Random r = new Random();
                //poster
                var streamUp = File.Open(urlPos, FileMode.Open);
                var task = new FirebaseStorage(
                    "filmreview-de9c4.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                        ThrowOnCancel = true
                    }).Child("FilmPoster").Child(r.Next(100000000,999999999).ToString()).PutAsync(streamUp);
                string poster = await task;
                streamUp.Flush();
                streamUp.Close();
                //trailer
                var streamUp1 = File.Open(urlPos, FileMode.Open);
                var task1 = new FirebaseStorage(
                    "filmreview-de9c4.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                        ThrowOnCancel = true
                    }).Child("FilmTrailers").Child(r.Next(100000000, 999999999).ToString()).PutAsync(streamUp1);
                string trailer = await task1;
                streamUp1.Flush();
                streamUp1.Close();

                var InfoFilm = new InfoFilm
                {
                    name = textBox_Name.Text,

                    descript = richTextBox_Descript.Text,

                    poster = poster,

                    category = list,//array,

                    year = Convert.ToInt32(richTextBox_Year.Text),

                    trailer = trailer,

                    genre = richTextBox_Genre.Text,
                    totalPoint = 0,
                    rating = 0,

                    numRate = 0,
                    time = Convert.ToInt32(richTextBox_Time.Text),

                    eps = Convert.ToInt32(richTextBox_Eps.Text),
                    director = richTextBox_director.Text,
                };
                DocumentReference docRef = await db.Collection("Films").AddAsync(InfoFilm);
                MessageBox.Show("Thêm thành công!");

                textBox_Name.Text = "";
                //richTextBox_Category.Text = "";
                richTextBox_Descript.Text = "";
                richTextBox_Eps.Text = "";
                richTextBox_Genre.Text = "";
                richTextBox_Time.Text = "";
                richTextBox_Year.Text = "";
                pictureBox1.Image = null;
            }
        }

        private void richTextBox_Eps_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void richTextBox_Numrate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void richTextBox_Totalpoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void richTextBox_Year_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void richTextBox_Time_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ListCategory category = new ListCategory();
            category.ShowDialog();

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
                urlTrailer = open.FileName;
                /*var stream = File.OpenRead(open.FileName);
                Bitmap map = new Bitmap(stream);
                stream.Flush();
                stream.Close();*/
            }
        }

        private void AddFilm_Load(object sender, EventArgs e)
        {
        }
    }
}
