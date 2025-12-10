using DataAccess.DTO;
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

namespace G1FOOD_Win_Admin
{
    public partial class frmOrder : Form
    {
        private readonly HttpClient client = null;
        private string OrderApiUrl;
        private string OrderDetailsApiUrl;
        public frmOrder()
        {
            InitializeComponent();
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:44377/api/Order/";
            OrderDetailsApiUrl = "https://localhost:44377/api/OrderDetails/";
        }

        private async void frmOrder_Load(object sender, EventArgs e)
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            IEnumerable<OrderDTO> orders = JsonSerializer.Deserialize<IEnumerable<OrderDTO>>(stringData, options);

            dgv_Order.Columns.Add("OrderId", "Order ID");
            dgv_Order.Columns.Add("BuyerFullName", "Buyer Full Name");
            dgv_Order.Columns.Add("BuyerPhone", "Buyer Phone");
            dgv_Order.Columns.Add("BuyerAddress", "Buyer Address");
            dgv_Order.Columns.Add("OrderStatus", "Order Status");
            dgv_Order.Columns.Add("OrderDate", "Order Date");

            foreach (var order in orders)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgv_Order);
                row.Cells[0].Value = order.OrderId;
                row.Cells[1].Value = order.BuyerFullName;
                row.Cells[2].Value = order.BuyerPhone;
                row.Cells[3].Value = order.BuyerAddress;
                row.Cells[4].Value = order.OrderStatus;
                row.Cells[5].Value = order.OrderDate.ToString("dd/MM/yyyy");

                dgv_Order.Rows.Add(row);
            }

        }
    }
}
