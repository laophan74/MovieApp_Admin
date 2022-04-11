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
    public partial class Main : Form
    {
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
    }
}
