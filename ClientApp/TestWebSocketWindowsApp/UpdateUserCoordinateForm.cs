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
    public partial class UpdateUserCoordinateForm : Form
    {
        public UpdateUserCoordinateForm()
        {
            InitializeComponent();
        }

        string userName;
        UserService userService;
        public UpdateUserCoordinateForm(string userName,UserService userService)
        {
            InitializeComponent();
            this.userName = userName;
            this.userService = userService;
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            UserCoordinate userCoordinate = new UserCoordinate();
            userCoordinate.UserName = this.userName;
            userCoordinate.Latitude = textBox2.Text;
            userCoordinate.Longitude = textBox1.Text;
            var result = userService.UpdateUserCoordinates(userCoordinate);
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
