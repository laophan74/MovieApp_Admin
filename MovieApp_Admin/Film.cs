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
    public partial class Film : Form
    {
        public static string showid;
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        private string urlPos;
        private InfoFilm flm;
        
        public Film()
        {
            InitializeComponent();
        }

        private void Film_Load(object sender, EventArgs e)
        {
            GetAllDocument("Films");
        }

        async void GetAllDocument(string nameofCollection)
        {            
            Query U = db.Collection(nameofCollection);
            QuerySnapshot snap = await U.GetSnapshotAsync();
            

            foreach (DocumentSnapshot docsnap in snap.Documents)
            {
                InfoFilm film = docsnap.ConvertTo<InfoFilm>();
                if (docsnap.Exists)
                {
                    guna2DataGridView1.Rows.Add(
                        Image.FromFile(@"D:\abc.jpeg") ,
                        film.name, 
                        film.category,
                        film.descript,
                        docsnap.Id
                        );

                }
            }
        }
        private void guna2DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int numrow = guna2DataGridView1.Rows.Count;
            label1.Text = numrow.ToString();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            showid = guna2DataGridView1.CurrentRow.Cells[4].Value.ToString();
            

            EditFilm addFilm = new EditFilm();
            addFilm.ShowDialog();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddFilm.list.Clear();
            AddFilm add = new AddFilm();
            add.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa!!!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DocumentReference docref = db.Collection("Films").Document(guna2DataGridView1.CurrentRow.Cells[4].Value.ToString());
                docref.DeleteAsync();
                MessageBox.Show("Đã xóa!");
            }
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.Refresh();
            GetAllDocument("Films");

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
