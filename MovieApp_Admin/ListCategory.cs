using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApp_Admin
{
    public partial class ListCategory : Form
    {
        public static string cate;

        public ListCategory()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddFilm addFilm = new AddFilm();
            foreach (object category in listBox1.SelectedItems)
            {
                int dem = 0;
                for (int i = 0; i < AddFilm.list.Count; i++)
                {
                    if (category.ToString() == AddFilm.list[i])
                        dem = dem + 1;

                }
                if(dem == 0)
                {
                   AddFilm.list.Add(category.ToString());
                }
            }
            MessageBox.Show("Thành công!");
            
            /*for (int i = 0; i < AddFilm.list.Count; i++)
            {
               cate = AddFilm.list[i] + ", ";
            }*/
            this.Hide();
        }
    }
}
