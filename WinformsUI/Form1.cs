using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WinformsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //If valid
            if (txtUserName.Text == "Quinn")
            {
                //  UserName/password return User
     
                //  hide Controls
                lblUserName.Hide();
                txtUserName.Hide();
                lblPassword.Hide();
                txtPassword.Hide();
                lblError.Hide();
                btnLogIn.Hide();

                //  show controls
                lblWelcome.Show();

                //  set values
                lblWelcome.Text = "Welcome Quinn";
            }
            else
            {
                //  Show error
                lblError.Show();
                txtPassword.Text = string.Empty;
            }
        }
    }
}
