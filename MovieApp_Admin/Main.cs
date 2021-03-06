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
using System.Collections;
using System.Net;
using myRes = MovieApp_Admin.Properties.Resources;
namespace MovieApp_Admin
{
    public partial class Main : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();

        public Main()
        {
            InitializeComponent();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Panel_Container.Controls.Clear();
            AccountManagement account = new AccountManagement();
            account.TopLevel = false;
            account.Dock = DockStyle.Fill;  
            this.Panel_Container.Controls.Add(account);
            account.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Panel_Container.Controls.Clear();
            Film account = new Film();
            account.TopLevel = false;
            account.Dock = DockStyle.Fill;
            this.Panel_Container.Controls.Add(account);
            account.Show();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            DocumentReference U = db.Collection("Users").Document(CLient.uid);
            DocumentSnapshot snap = await U.GetSnapshotAsync();
                InfoUser user = snap.ConvertTo<InfoUser>();
                Image image = myRes._default;
                if (snap.Exists)
                {
                    /*if (user.avatar != "")
                    {
                        using (WebClient web = new WebClient())
                        {
                            Stream stream = web.OpenRead(user.avatar);
                            Bitmap bit = new Bitmap(stream);
                            if (bit != null) image = bit;
                            stream.Flush();
                            stream.Close();
                        }
                    }*/
                    label_username.Text = user.name;
                }
            
        }

        private void avatarptb_Click(object sender, EventArgs e)
        {
            UserProfile userProfile    = new UserProfile();
            userProfile.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            this.Panel_Container.Controls.Clear();
            Director account = new Director();
            account.TopLevel = false;
            account.Dock = DockStyle.Fill;
            this.Panel_Container.Controls.Add(account);
            account.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Panel_Container.Controls.Clear();
            AccountManagement account = new AccountManagement();
            account.TopLevel = false;
            account.Dock = DockStyle.Fill;
            this.Panel_Container.Controls.Add(account);
            account.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Panel_Container.Controls.Clear();
            Actor account = new Actor();
            account.TopLevel = false;
            account.Dock = DockStyle.Fill;
            this.Panel_Container.Controls.Add(account);
            account.Show();

        }
    }
}
