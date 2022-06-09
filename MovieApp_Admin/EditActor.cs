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
    public partial class EditActor : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlActor = "";
        private InfoActor flm;
        DocumentReference docref;
        public EditActor()
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
                if (urlActor != "")
                {
                    DocumentReference docRef = db.Collection("Actors").Document(Actor.document);
                    string addID = docRef.Id;
                    string upActor = addID + ".jpg";
                    string Actorava = "";

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

                    var InfoActor = new InfoActor
                    {
                        name = name.Text,
                        age = Convert.ToInt32(age.Text),
                        avatar = Actorava
                    };
                    await docRef.SetAsync(InfoActor);
                    MessageBox.Show("Sửa thành công!");
                    this.Close();
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>
                    {
                        { "name", name.Text },
                        { "age",Convert.ToInt32( age.Text) }
                    };
                    DocumentReference docRef = db.Collection("Actors").Document(Actor.document);
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
                urlActor = open.FileName;
                var stream = File.OpenRead(open.FileName);
                Bitmap map = new Bitmap(stream);
                pictureBox1.Image = map;
                stream.Flush();
                stream.Close();
            }
        }
        private async void GetAllData()
        {
            docref = db.Collection("Actors").Document(Actor.document);
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                flm = snap.ConvertTo<InfoActor>();
                

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
                age.Text = flm.age.ToString();
            }
        }

        private void EditDirector_Load(object sender, EventArgs e)
        {
            GetAllData();
        }
    }
}
