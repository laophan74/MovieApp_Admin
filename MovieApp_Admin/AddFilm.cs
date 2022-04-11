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
            if (string.IsNullOrEmpty(richTextBox_Category.Text) ||
                string.IsNullOrEmpty(richTextBox_Descript.Text) ||
                string.IsNullOrEmpty(richTextBox_Eps.Text) ||
                string.IsNullOrEmpty(richTextBox_Genre.Text) ||
                string.IsNullOrEmpty(richTextBox_Numrate.Text) ||
                string.IsNullOrEmpty(richTextBox_Time.Text) ||
                string.IsNullOrEmpty(richTextBox_Totalpoint.Text) ||
                string.IsNullOrEmpty(richTextBox_Year.Text)
                )
            {
                MessageBox.Show("Vui lòng nhập lại thông tin!");
            }
            else
            {
                Random r = new Random();
                var streamUp = File.Open(urlPos, FileMode.Open);
                var task = new FirebaseStorage(
                    "filmreview-de9c4.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                        ThrowOnCancel = true
                    }).Child("FilmPoster").Child(r.Next(100000000,999999999).ToString()).PutAsync(streamUp);
                string poster = await task;
                ArrayList array = new ArrayList();

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
                DocumentReference docRef = await db.Collection("Films").AddAsync(InfoFilm);
                MessageBox.Show("Thêm thành công!");

                textBox_Name.Text = "";
                richTextBox_Category.Text = "";
                richTextBox_Descript.Text = "";
                richTextBox_Eps.Text = "";
                richTextBox_Genre.Text = "";
                richTextBox_Numrate.Text = "";
                richTextBox_Time.Text = "";
                richTextBox_Totalpoint.Text = "";
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
    }
}
