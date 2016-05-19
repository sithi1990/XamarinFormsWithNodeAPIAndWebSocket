using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VAppApiServiceAccess;
using VAppApiServiceAccess.Models;

namespace TestWebSocketWindowsApp
{
    public partial class CreateUserForm : Form
    {

        public CreateUserForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserService userservice = new UserService();
            User user = new User() { FirstName = textBox1.Text, LastName = textBox2.Text, UserName = textBox3.Text, Password = textBox4.Text };
            var result = userservice.CreateUser(user);
            if(result.IsSuccess)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }
    }
}
