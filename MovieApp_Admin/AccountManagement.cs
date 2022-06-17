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
using myRes = MovieApp_Admin.Properties.Resources;
using System.Net;

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
        }
        async void GetAllDocument(string nameofCollection)
        {
            Query Users = db.Collection(nameofCollection);
            QuerySnapshot snap = await Users.GetSnapshotAsync();

            foreach (DocumentSnapshot docsnap in snap.Documents)
            {
                InfoUser d = docsnap.ConvertTo<InfoUser>();
                Image image = myRes._default;
                if (docsnap.Exists)
                {
                    /*Bitmap bit = MovieApp_Admin.Properties.Resources.businessman;
                    if (d.imageURL != "")
                    {
                        using (WebClient web = new WebClient())
                        {
                            Stream stream = web.OpenRead(d.imageURL);
                            bit = new Bitmap(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }*/
                    guna2DataGridView1.Rows.Add(
                        d.name,
                        d.isAdmin.ToString(),
                        d.email,
                        docsnap.Id);
                }
            }
        }

        private void guna2DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int numrow = guna2DataGridView1.Rows.Count;
            label_count.Text = numrow.ToString();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(guna2DataGridView1.CurrentRow.Cells[3].Value.ToString());
            if (MessageBox.Show("Bạn có muốn xóa!!!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DocumentReference docref = db.Collection("Users").Document(guna2DataGridView1.CurrentRow.Cells[3].Value.ToString());
                docref.DeleteAsync();
                MessageBox.Show("Đã xóa!");
            }
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.Refresh();
            GetAllDocument("Users");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            String searchstr = searchtxt.Text;
            Query searchQ = null;

            if (searchstr != "")
            {
                guna2DataGridView1.Rows.Clear();
                searchQ = db.Collection("Users").WhereGreaterThanOrEqualTo("name", searchstr)
                        .WhereLessThanOrEqualTo("name", searchstr + "\uf8ff");

                QuerySnapshot searchSS = await searchQ.GetSnapshotAsync();
                foreach (var item in searchSS)
                {
                    DocumentReference docRef = db.Collection("Users").Document(item.Id);
                    DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                    InfoUser d = snapshot.ConvertTo<InfoUser>();
                    if (snapshot.Exists)
                    {
                        guna2DataGridView1.Rows.Add(
                            d.name,
                            d.isAdmin.ToString(),
                            d.email,
                            snapshot.Id);
                    }
                }
            }
        }

        private void guna2DataGridView1_RowsAdded_1(object sender, DataGridViewRowsAddedEventArgs e)
        {
            label_count.Text = guna2DataGridView1.Rows.Count.ToString();
        }
    }
}
