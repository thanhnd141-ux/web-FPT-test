using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G1FOOD_Win_Admin
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProduct frmProduct = new frmProduct();

            frmProduct.TopLevel = false;
            frmProduct.Parent = panel1;

            frmProduct.Visible = true;
        }

        private void orderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOrder frmOrder = new frmOrder();

            frmOrder.TopLevel = false;
            frmOrder.Parent = panel1;

            frmOrder.Visible = true;
        }
    }
}
