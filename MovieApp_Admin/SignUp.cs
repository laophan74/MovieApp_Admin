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
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form1 mn = new Form1();
            mn.Show();
            Hide();
            mn.Closed += (s, args) => Close();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox1.Text) ||
                string.IsNullOrEmpty(guna2TextBox2.Text) ||
                string.IsNullOrEmpty(guna2TextBox3.Text) ||
                string.IsNullOrEmpty(guna2TextBox4.Text))
            {
                MessageBox.Show("Xin điền thông tin!");
            }
            else
            {
                if (guna2TextBox3.Text != guna2TextBox4.Text)
                {
                    MessageBox.Show("Mật khẩu không trùng khớp");
                }
                else
                {

                    if (await AccountManager.Instance().SignUp(guna2TextBox2.Text, guna2TextBox3.Text, guna2TextBox1.Text))
                    {
                        MessageBox.Show("Tạo tài khoản thành công!");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Không đăng ký được!!!");
                    }
                }
            }
        }
    }
}
