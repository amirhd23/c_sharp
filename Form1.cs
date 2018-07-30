using COMP2614_Assign06.Repositories;
using COMP2614_Assign06.Utils;
using COMP2614_Assign06.ViewModel;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace COMP2614_Assign06
{
    public partial class Form1 : Form
    {
        const int PRODUCT_ID = 0;
        const int PRICE = 2;
        const int MFG_DISCOUNT = 4;
        const string MFG_DISCOUNT_HEADER = "Mfg\nDiscount";
        const string DELETE_DIALOG_TITLE = "Delete Confirmation";

        ProductVMRepo productVMRepo;
        ProductRepo productRepo;
        ManufacturerVMRepo manufacturerVMRepo;
        ManufacturerRepo manufacturerRepo;
        SupplierRepo supplierRepo;

        int deleteSelectedRow = -1;

        BindingSource bsProducts = new BindingSource();
        BindingSource bsManufacturers = new BindingSource();
        BindingSource bsSuppliers = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initialize();
            setFormStyles();
        }

        public void ProductsUpdateListener()
        {
            refreshProductsData();
        }


        private void initialize()
        {
            productVMRepo       = new ProductVMRepo();
            productRepo         = new ProductRepo();
            manufacturerVMRepo  = new ManufacturerVMRepo();
            manufacturerRepo    = new ManufacturerRepo();
            supplierRepo        = new SupplierRepo();
            //add database update listener delegate
            productRepo.DatabaseUpdated       += ProductsUpdateListener;

            dataGridViewProducts.DataSource = bsProducts;
            comboBoxManufacturer.DataSource = bsManufacturers;
            comboBoxSupplier.DataSource     = bsSuppliers;

            refreshProductsData();
            loadManufacturers();
            loadSuppliers();
        }

        private void refreshProductsData()
        {
            bsProducts.DataSource = productVMRepo.getAllProductsInformation();
        }

        private void loadManufacturers()
        {
            bsManufacturers.DataSource = manufacturerVMRepo
                .GetAllManufacturers().Select(m => m.ManufacturerName);
            
        }

        private void loadSuppliers()
        {
            bsSuppliers.DataSource = supplierRepo
                .GetDistinctSuppliers().Select(s => s.SupplierName);
        }


        private void setFormStyles()
        {
            StartPosition = FormStartPosition.CenterScreen;
            //chaning width of columns
            int baseWidth = dataGridViewProducts.Columns[0].Width;
            dataGridViewProducts.Columns[PRODUCT_ID].Width =    baseWidth / 4;
            dataGridViewProducts.Columns[PRICE].Width =         baseWidth / 2;
            dataGridViewProducts.Columns[MFG_DISCOUNT].Width =  baseWidth / 2;

            dataGridViewProducts.Columns[PRICE].DefaultCellStyle.Alignment = 
                DataGridViewContentAlignment.MiddleRight;
            dataGridViewProducts.Columns[MFG_DISCOUNT].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            dataGridViewProducts.Columns[MFG_DISCOUNT].HeaderText = MFG_DISCOUNT_HEADER;


        }

        private void textBoxDescription_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validateTextBoxDescription();
        }

        private bool validateTextBoxDescription()
        {
            //only alphabetical chars are allowed for product name
            bool descriptionValid = Validator.ValidateAlphabetic(textBoxDescription.Text);
            if (descriptionValid)
            {
                errorProvider1.SetError(textBoxDescription, string.Empty);
                return true;
            }
            else
            {
                errorProvider1.SetError(textBoxDescription, "Only alphabetical values are allowed.");
                return false;
            }
        }

        private bool validateTextBoxPrice()
        {
            //only decimal values are allowed for price
            bool success = Validator.ValidateDecimal(textBoxPrice.Text);
            if (success)
            {
                errorProvider1.SetError(textBoxPrice, string.Empty);
            } else
            {
                errorProvider1.SetError(textBoxPrice, "Decimal number required.");
            }
            return success;
        }

        private void textBoxPrice_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validateTextBoxPrice();
        }

        private void dataGridViewProducts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //only enable delete option if row selected
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                deleteSelectedRow = e.RowIndex;
                toolStripMenuItemDelete.Enabled = true;
            } else
            {//header is selected so disable 'Delete Row'
                toolStripMenuItemDelete.Enabled = false;
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            //delete the selected row
            string productId = dataGridViewProducts
                .Rows[deleteSelectedRow].Cells[PRODUCT_ID].Value?.ToString();
            int id;
            bool success = int.TryParse(productId, out id);
            if (success)
            {
                try
                {
                    productRepo.DeleteProduct(id);
                    MessageBox.Show("Product deleted.", 
                        DELETE_DIALOG_TITLE, 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch
                {
                    MessageBox.Show("This product has already been invoiced so it cannot be deleted.", 
                        DELETE_DIALOG_TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
            deleteSelectedRow = -1;//reset the index
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            bool descriptionValid   = validateTextBoxDescription();
            bool priceValid         = validateTextBoxPrice();
            string supplier         = (string)comboBoxSupplier.SelectedValue;
            string mfg              = (string)comboBoxManufacturer.SelectedValue;
            decimal price;
            decimal.TryParse(textBoxPrice.Text, out price);
            if (descriptionValid && priceValid)
            {
                Product newProduct = new Product
                {
                    name = textBoxDescription.Text,
                    price = price,
                    mfg = mfg,
                    vendor = supplier
                };
                productRepo.AddProduct(newProduct);
                textBoxDescription.Clear();
                textBoxPrice.Clear();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //launches open dialog for a csv file
        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string FILTER = "Text documents (.csv,*.csv)|*.csv;";
            OpenFileDialog dlg  = new OpenFileDialog();
            dlg.FileName        = "Document";
            dlg.DefaultExt      = ".csv";
            dlg.Filter          = FILTER;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = dlg.FileName;
                InputTracker inputTracker = FileReader.ReadCsvFile(filePath);
                productRepo.AddAll(inputTracker);
                if (inputTracker.error != string.Empty)
                {
                    MessageBox.Show(inputTracker.error, 
                        "Error Notification", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void manufacturerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddManufacturerForm addMfgDialog = new AddManufacturerForm();
            addMfgDialog.StartPosition = FormStartPosition.CenterParent;
            addMfgDialog.ShowInTaskbar = false;
            DialogResult result = addMfgDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                loadManufacturers();
            }
        }

        
    }
}
