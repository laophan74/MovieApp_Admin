﻿using System;
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
    public partial class Film : Form
    {
        public string fID;
        private FirestoreDb db = AccountManager.Instance().LoadDB();
        
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
                Image image = myRes._default;
                if (docsnap.Exists)
                {
                    if (film.poster != "")
                    {
                        using (WebClient web = new WebClient())
                        {
                            Stream stream = web.OpenRead(film.poster);
                            Bitmap bit = new Bitmap(stream);
                            if (bit != null) image = bit;
                            stream.Flush();
                            stream.Close();
                        }
                    }
                    guna2DataGridView1.Rows.Add(
                        image ,
                        film.name, 
                        film.category,
                        film.descript,
                        docsnap.Id);
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
            EditFilm edit = new EditFilm(fID);
            edit.Show();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {           
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

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            fID = guna2DataGridView1.CurrentRow.Cells[4].Value.ToString();
        }
    }
}
