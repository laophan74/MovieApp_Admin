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
    public partial class AccountManagement : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        public AccountManagement()
        {
            InitializeComponent();
        }

        private void AccountManagement_Load(object sender, EventArgs e)
        {
            GetAllDocument("Users");
            int numrow = guna2DataGridView1.Rows.Count;
            label1.Text = numrow.ToString();
        }
        async void GetAllDocument(string nameofCollection)
        {
            Query Users = db.Collection(nameofCollection);
            QuerySnapshot snap = await Users.GetSnapshotAsync();

            foreach (DocumentSnapshot docsnap in snap.Documents)
            {
                InfoUser user = docsnap.ConvertTo<InfoUser>();
                if (docsnap.Exists)
                {
                    guna2DataGridView1.Rows.Add(user.name, user.isAdmin.ToString());
                }
            }
        }

        private void guna2DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int numrow = guna2DataGridView1.Rows.Count;
            label1.Text = numrow.ToString();
        }
    }
}
