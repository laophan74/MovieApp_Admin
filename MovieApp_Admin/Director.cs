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
    public partial class Director : Form
    {
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        public static string document;

        public Director()
        {
            InitializeComponent();
        }

        private void Film_Load(object sender, EventArgs e)
        {
            GetAllDocument("Directors");
        }

        async void GetAllDocument(string nameofCollection)
        {
            Query Users = db.Collection(nameofCollection);
            QuerySnapshot snap = await Users.GetSnapshotAsync();

            foreach (DocumentSnapshot docsnap in snap.Documents)
            {
                InfoDirector d = docsnap.ConvertTo<InfoDirector>();
                if (docsnap.Exists)
                {
                    Image image = myRes._default;
                    if (d.avatar != "")
                    {
                        using (WebClient web = new WebClient())
                        {
                            Stream stream = web.OpenRead(d.avatar);
                            Bitmap bit = new Bitmap(stream);
                            if (bit != null) image = bit;
                            stream.Flush();
                            stream.Close();
                        }
                    }
                    guna2DataGridView1.Rows.Add(
                        image, 
                        d.name, 
                        d.age.ToString(),
                        docsnap.Id);
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            document = guna2DataGridView1.CurrentRow.Cells[3].Value.ToString();
            EditDirector edit = new EditDirector();
            edit.Show();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddDirector no = new AddDirector();
            no.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa!!!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DocumentReference docref = db.Collection("Directors").Document(guna2DataGridView1.CurrentRow.Cells[3].Value.ToString());
                docref.DeleteAsync();
                MessageBox.Show("Đã xóa!");
            }
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.Refresh();
            GetAllDocument("Directors");

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            label_count.Text = guna2DataGridView1.Rows.Count.ToString();

        }
    }
}
