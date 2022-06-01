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
            SignIn mn = new SignIn();
            mn.Show();
            Hide();
            mn.Closed += (s, args) => Close();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailTextBox.Text) ||
                string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(passTextBox.Text) ||
                string.IsNullOrEmpty(guna2TextBox4.Text))
            {
                MessageBox.Show("Xin điền thông tin!");
            }
            else
            {
                if (passTextBox.Text != guna2TextBox4.Text)
                {
                    MessageBox.Show("Mật khẩu không trùng khớp");
                }
                else
                {

                    if (await AccountManager.Instance().SignUp(nameTextBox.Text, passTextBox.Text, emailTextBox.Text))
                    {
                        MessageBox.Show("Tạo tài khoản thành công!");
                        this.Hide();
                        SignIn mn = new SignIn();
                        mn.ShowDialog();
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
