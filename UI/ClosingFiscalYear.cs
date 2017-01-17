using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccountsManagementSystem.DbGateway;

namespace AccountsManagementSystem.UI
{
    public partial class ClosingFiscalYear : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public static string MId;
        public string b, c, x;
        public decimal d = 0, k = 0;
        public ClosingFiscalYear()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            if (cmbTypeOfFiscalYear.Text == "")
            {
                MessageBox.Show("Please select Type Of Fiscal Year", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbTypeOfFiscalYear.Focus();
                return;
            }
            if (cmbFiscalYear.Text == "")
            {
                MessageBox.Show("Please select Fiscal Year", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbFiscalYear.Focus();
                return;
            }


            try
            {
               
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb = "Update FiscalYears set ClosingDate=@d1,Statuss=@d2 where FiscalYear='" + cmbFiscalYear.Text + "'";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", Convert.ToDateTime(System.DateTime.Today, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
                cmd.Parameters.AddWithValue("@d2", "Close");
                rdr = cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("Successfully Close the Fiscal Year", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClosingFiscalYear_Load(object sender, EventArgs e)
        {
            cmbTypeOfFiscalYear.Focus();
        }

        private void cmbTypeOfFiscalYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbFiscalYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
