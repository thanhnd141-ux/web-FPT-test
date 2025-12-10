using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G1FOOD_Win_Admin
{
    public partial class frmProduct : Form
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl;
        public frmProduct()
        {
            InitializeComponent();
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:44377/api/Product/";
        }

        private async void frmProduct_Load(object sender, EventArgs e)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<ProductDTO> products = JsonSerializer.Deserialize<List<ProductDTO>>(stringData, options);

            dgv_Product.Columns.Add("ProductId", "Product ID");
            dgv_Product.Columns.Add("ProductName", "Product Name");
            dgv_Product.Columns.Add("ProductDescription", "Product Description");
            dgv_Product.Columns.Add("ProductPrice", "Product Price");
            dgv_Product.Columns.Add("ProductSalePercent", "Sale Percent");
            dgv_Product.Columns.Add("ProductStatus", "Product Status");
            var imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Product Image";
            imageColumn.Name = "ProductImage";
            dgv_Product.Columns.Add(imageColumn);
            dgv_Product.Columns.Add("CategoryId", "Category ID");

            foreach (ProductDTO product in products)
            {
                int rowIndex = dgv_Product.Rows.Add(); 

                dgv_Product.Rows[rowIndex].Cells["ProductId"].Value = product.ProductId;
                dgv_Product.Rows[rowIndex].Cells["ProductName"].Value = product.ProductName;
                dgv_Product.Rows[rowIndex].Cells["ProductDescription"].Value = product.ProductDescription;
                dgv_Product.Rows[rowIndex].Cells["ProductPrice"].Value = product.ProductPrice;
                dgv_Product.Rows[rowIndex].Cells["ProductSalePercent"].Value = product.ProductSalePercent;
                dgv_Product.Rows[rowIndex].Cells["ProductStatus"].Value = product.ProductStatus;
                dgv_Product.Rows[rowIndex].Cells["ProductImage"].Value = LoadImageFromUrl(product.ProductImage);
                dgv_Product.Rows[rowIndex].Cells["CategoryId"].Value = product.CategoryId;
            }

        }

        private Image LoadImageFromUrl(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    return Image.FromStream(stream);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu không tải được hình ảnh
                return null;
            }
        }

        private void dgv_Product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
