using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;

namespace NT106_Demo
{
    public partial class DemoFirebase : Form
    {
        public DemoFirebase()
        {
            InitializeComponent();
            config = new FirebaseConfig()
            {
                AuthSecret = "hYR0g2duV4mn6ZxgxbteOUKStbYvu4mjHOSLB3TJ",
                BasePath = "https://nt106-demo-default-rtdb.asia-southeast1.firebasedatabase.app/"
            };
            client = new FireSharp.FirebaseClient(config);
        }
        IFirebaseConfig config;
        IFirebaseClient client;
        private async void PushData(User user)
        {
            SetResponse response = await client.SetAsync("Users/" + "U" + user.UserId, user);
            User result = response.ResultAs<User>();
            MessageBox.Show("Data Inserted");
        }
        private async void GetData()
        {
            FirebaseResponse response = await client.GetAsync("Users/1");
            User user = response.ResultAs<User>();
            MessageBox.Show(user.Name);
        }

        private void btnPush_Click(object sender, EventArgs e)
        {
            User user = new User()
            {
                UserId = textBox1.Text,
                Name = textBox2.Text,
                Email = textBox3.Text
            };
            PushData(user);
        }
        private Thread fetchDataThread;
        private void StartFetchDataThread()
        {
            fetchDataThread = new Thread(new ThreadStart(FetchDataLoop));
            fetchDataThread.IsBackground = true;
            fetchDataThread.Start();
        }
        private void FetchDataLoop()
        {
            while (true)
            {
                try
                {
                    Invoke(new Action(async () =>
                    {
                        FirebaseResponse response = await client.GetAsync("Users");
                        Dictionary<string, User> users = response.ResultAs<Dictionary<string, User>>();

                        dataGridView1.Rows.Clear();
                        foreach (var user in users)
                        {
                            dataGridView1.Rows.Add(new object[] { user.Value.UserId, user.Value.Name, user.Value.Email, "Edit", "Delete" });
                        }
                    }));
                }
                catch
                {

                }

                Thread.Sleep(2000);
            }
        }
        public async Task DeleteUserAsync(string userId)
        {
            await client.DeleteAsync($"Users/U{userId}");
        }
        private void DemoFirebase_Load(object sender, EventArgs e)
        {
            StartFetchDataThread();
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                string userId = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                await DeleteUserAsync(userId);

                MessageBox.Show("User deleted successfully");
            }
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["UserName"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void DemoFirebase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                fetchDataThread.Abort();
            }
            catch
            {

            }
        }
    }
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
