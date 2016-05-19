using Quobject.SocketIoClientDotNet.Client;
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
    public partial class MainForm : Form
    {
        Socket socket;
        IAuthenticationService authenticationService;
        public MainForm()
        {
            

            InitializeComponent();
            authenticationService = new AuthenticationService();
            socket = IO.Socket("http://localhost:8079");
            socket.On("coordinate_changed", On_CoordinateChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            if (authenticationService.Authenticate(textBox2.Text, textBox3.Text))
            {
                textBox4.Text = authenticationService.AuthenticationToken;
                textBox1.Text = authenticationService.Claim.UserName;
            }
        }

        private void On_CoordinateChanged(object data)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(RefreshGridData));
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateUserForm cuserform = new CreateUserForm();
            cuserform.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshGridData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserService userservice = new UserService(authenticationService);
            if(authenticationService.IsAuthenticated)
            {
                UpdateUserCoordinateForm upUserCoordinate = new UpdateUserCoordinateForm(authenticationService.Claim.UserName, userservice);
                upUserCoordinate.ShowDialog();
            }
            else
            {
                MessageBox.Show("User Not Authenticated");
            }
          
        }

        public void RefreshGridData()
        {
            
            UserService userservice = new UserService(authenticationService);
            var userCoordinateResponse = userservice.ViewUserCoordinates();

            if(userCoordinateResponse.IsSuccess)
            {
                dataGridView1.DataSource = userCoordinateResponse.UserCoordinates;
            }
            else
            {
                MessageBox.Show(userCoordinateResponse.Message);
            }
            
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
