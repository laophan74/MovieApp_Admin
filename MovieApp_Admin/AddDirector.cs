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
    public partial class AddDirector : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlDirector = "";
        public AddDirector()
        {
            InitializeComponent();
        }

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(name.Text) ||
                string.IsNullOrEmpty(age.Text) ||
                pictureBox1.Image == null)
            {
                MessageBox.Show("Vui lòng nhập lại thông tin!");

            }
            else
            {
                var InfoDirector = new InfoDirector
                {
                    name = name.Text,
                    age = Convert.ToInt32(age.Text),
                };

                DocumentReference docRef = await db.Collection("Directors").AddAsync(InfoDirector);
                string addID = docRef.Id;
                string upDirector = addID + ".jpg";
                string directorava = "";
                //directorava
                if (urlDirector != "")
                {
                    var streamPos1 = File.Open(urlDirector, FileMode.Open);
                    var task = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("Directors").Child(upDirector).PutAsync(streamPos1);
                    directorava = await task;
                    streamPos1.Close();
                }
                Dictionary<string, object> update = new Dictionary<string, object>
                {
                    { "avatar", directorava }
                };
                await docRef.UpdateAsync(update);
                MessageBox.Show("Thêm thành công!");
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
                pictureBox1.Image = map;
                stream.Flush();
                stream.Close();
            }
        }
    }
}
