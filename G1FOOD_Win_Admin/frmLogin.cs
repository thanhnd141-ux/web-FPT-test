using DataAccess.DTO;
using G1FOOD_Win_Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MyStoreWinApp
{
    public partial class frmLogin : Form
    {
        private readonly HttpClient client = null;
        private string LoginApiUrl;
        public frmLogin()
        {
            InitializeComponent();
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            LoginApiUrl = "https://localhost:44377/api/Login/";
            txt_Email.Text = "admin@gmail.com";
            txt_Password.Text = "aA@admin123";
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit the program?", "Message", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }


        public async Task<AccountDTO> login(string txt_Email, string txt_Pass)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(LoginApiUrl, new LoginDTO { AccountEmail = txt_Email, AccountPassword = txt_Pass });

            string stringData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            AccountDTO account = JsonSerializer.Deserialize<AccountDTO>(stringData, options);

            return account;

        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            // Get input data by user from textbox. 

            string username = txt_Email.Text;
            string password = txt_Password.Text;
            // checking by login function.
            if (login(username, password) != null)
            {
                frmMain ml = new frmMain();
                this.Hide();
                ml.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Wrong username or password!!!", "Login Message");
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
