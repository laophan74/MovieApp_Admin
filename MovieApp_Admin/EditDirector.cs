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
    public partial class EditDirector : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlDirector = "";
        private InfoDirector flm;
        DocumentReference docref;
        public EditDirector()
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
                if (urlDirector != "")
                {
                    DocumentReference docRef = db.Collection("Directors").Document(Director.document);
                    string addID = docRef.Id;
                    string upDirector = addID + ".jpg";
                    string directorava = "";

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

                    var InfoDirector = new InfoDirector
                    {
                        name = name.Text,
                        avatar = directorava
                    };
                    await docRef.SetAsync(InfoDirector);
                    MessageBox.Show("Sửa thành công!");
                    this.Close();
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>
                    {
                        { "name", name.Text },
                    };
                    DocumentReference docRef = db.Collection("Directors").Document(Director.document);
                    await docRef.UpdateAsync(update);
                    MessageBox.Show("Sửa thành công!");
                    this.Close();
                }
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
        private async void GetAllData()
        {
            docref = db.Collection("Directors").Document(Director.document);
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                flm = snap.ConvertTo<InfoDirector>();
                

                if (flm.avatar != "")
                {
                    using (WebClient web1 = new WebClient())
                    {
                        Stream myIMG1 = web1.OpenRead(flm.avatar);
                        Bitmap bit1 = new Bitmap(myIMG1);
                        if (bit1 != null) pictureBox1.Image = bit1;
                        myIMG1.Flush();
                        myIMG1.Close();
                    }
                }
                name.Text = flm.name;
            }
        }

        private void EditDirector_Load(object sender, EventArgs e)
        {
            GetAllData();
        }
    }
}
