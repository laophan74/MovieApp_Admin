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
    public partial class AddActor : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlActor = "";
        public AddActor()
        {
            InitializeComponent();
        }

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(name.Text) ||
                pictureBox1.Image == null)
            {
                MessageBox.Show("Vui lòng nhập lại thông tin!");

            }
            else
            {
                var InfoActor = new InfoActor
                {
                    name = name.Text,
                };

                DocumentReference docRef = await db.Collection("Actors").AddAsync(InfoActor);
                string addID = docRef.Id;
                string upActor = addID + ".jpg";
                string Actorava = "";
                //Actorava
                if (urlActor != "")
                {
                    var streamPos1 = File.Open(urlActor, FileMode.Open);
                    var task = new FirebaseStorage(
                        "filmreview-de9c4.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(myProp.Default.token_txt),
                            ThrowOnCancel = true
                        }).Child("Actors").Child(upActor).PutAsync(streamPos1);
                    Actorava = await task;
                    streamPos1.Close();
                }
                Dictionary<string, object> update = new Dictionary<string, object>
                {
                    { "avatar", Actorava }
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
                urlActor = open.FileName;
                var stream = File.OpenRead(open.FileName);
                Bitmap map = new Bitmap(stream);
                pictureBox1.Image = map;
                stream.Flush();
                stream.Close();
            }
        }
    }
}
