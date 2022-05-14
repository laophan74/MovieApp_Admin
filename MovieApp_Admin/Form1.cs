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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox1.Text) ||
                string.IsNullOrEmpty(guna2TextBox3.Text))
            {
                MessageBox.Show("Xin điền thông tin đăng nhập!");
            }
            else
            {
                if (await AccountManager.Instance().SignIn(guna2TextBox1.Text, guna2TextBox3.Text))
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    Main mn = new Main();
                    mn.Show();
                    Hide();
                    mn.Closed += (s, args) => this.Close();
                }               
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            SignUp su = new SignUp();
            su.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
