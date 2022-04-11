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
    public partial class EditFilm : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlPos;
        private InfoFilm flm;
        public EditFilm()
        {
            InitializeComponent();
        }

        private void EditFilm_Load(object sender, EventArgs e)
        {
            GetAllData();
        }

        async void GetAllData()
        {

            DocumentReference docref = db.Collection("Films").Document(Film.showid);
            DocumentSnapshot snap = await docref.GetSnapshotAsync();

            if (snap.Exists)
            {
                flm = snap.ConvertTo<InfoFilm>();

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
                richTextBox_Genre.Text = flm.genre;
                richTextBox_Numrate.Text = flm.numRate.ToString();
                richTextBox_Time.Text = flm.time.ToString();
                richTextBox_Totalpoint.Text = flm.totalPoint.ToString();
                richTextBox_Year.Text = flm.year.ToString();
            }
        }

        private async void guna2Button5_Click(object sender, EventArgs e)
        {
            var streamUp = File.Open(urlPos, FileMode.Open);
            var task = new FirebaseStorage(
                "filmreview-de9c4.appspot.com",
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                    ThrowOnCancel = true
                }).Child("FilmPoster").Child(Film.showid).PutAsync(streamUp);
            string poster = await task;


            DocumentReference docref = db.Collection("Films").Document(Film.showid);
            var InfoFilm = new InfoFilm
            {
                name = textBox_Name.Text,

                descript = richTextBox_Descript.Text,

                poster = poster,

                category = { },//array,

                year = Convert.ToInt32(richTextBox_Year.Text),

                trailer = "",

                genre = richTextBox_Genre.Text,
                totalPoint = Convert.ToInt32(richTextBox_Totalpoint.Text),

                numRate = Convert.ToInt32(richTextBox_Numrate.Text),
                time = Convert.ToInt32(richTextBox_Time.Text),

                eps = Convert.ToInt32(richTextBox_Eps.Text)
            };

            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                await docref.SetAsync(InfoFilm);
            }
            MessageBox.Show("Thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
    }
}
