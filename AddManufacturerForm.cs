using COMP2614_Assign06.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP2614_Assign06
{
    public partial class AddManufacturerForm : Form
    {
        public string ManufacturerName { get; set; }
        public int MfgDiscount { get; set; }
        private ManufacturerRepo manufacturerRepo;

        public AddManufacturerForm()
        {
            InitializeComponent();
        }


        private void AddManufacturerForm_Load(object sender, EventArgs e)
        {
            manufacturerRepo = new ManufacturerRepo();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bool discountValid = Utils.Validator.ValidateInteger(textBoxDiscount.Text);
            if (!discountValid)
            {
                MessageBox.Show("Invalid discount. A whole number is required",
                    "Manufacturer Add Confirmation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBoxMfgName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter a name for the manufacturer",
                    "Manufacturer Add Confirmation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int discount;
            int.TryParse(textBoxDiscount.Text, out discount);
            try
            {
                manufacturerRepo.AddManufacturer(new Manufacturer
                {
                    mfg = textBoxMfgName.Text.Trim(),
                    mfgDiscount = discount
                });
                DialogResult result = MessageBox.Show("Success", "Manufacturer Add Confirmation",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Dispose();
            } catch
            {
                MessageBox.Show("Error adding new manufacturer.",
                    "Manufacturer Add Confirmation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
