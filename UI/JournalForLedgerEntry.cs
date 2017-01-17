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
    public partial class JournalForLedgerEntry : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        ConnectionString cs=new ConnectionString();
        public int  fiscalLE6Year;
        public JournalForLedgerEntry()
        {
            InitializeComponent();
        }

        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(LedgerEntry.LedgerEntryId),RTRIM(TransactionRecord.TransactionDate),RTRIM(Ledger.LedgerName),RTRIM(LedgerEntry.FundRequisitionNo),RTRIM(LedgerEntry.VoucherNo),RTRIM(LedgerEntry.Particulars),RTRIM(LedgerEntry.Debit),RTRIM(LedgerEntry.Credit) FROM   ((BalanceFiscal INNER JOIN LedgerEntry ON BalanceFiscal.LId=LedgerEntry.LId) INNER JOIN Ledger ON BalanceFiscal.LedgerId=Ledger.LedgerId) INNER JOIN TransactionRecord ON LedgerEntry.TransactionId=TransactionRecord.TransactionId where  BalanceFiscal.FiscalId='"+fiscalLE6Year+"'  order by LedgerEntry.LedgerEntryId desc",con);
               // cmd = new SqlCommand("SELECT RTRIM(LedgerEntry.LedgerEntryId),RTRIM(TransactionRecord.TransactionDate),RTRIM(Ledger.LedgerName),RTRIM(LedgerEntry.FundRequisitionNo),RTRIM(LedgerEntry.VoucherNo),RTRIM(LedgerEntry.Particulars),RTRIM(LedgerEntry.Debit),RTRIM(LedgerEntry.Credit) from Ledger,LedgerEntry,TransactionRecord,BalanceFiscal where  BalanceFiscal.LId=LedgerEntry.LId and TransactionRecord.TransactionId=LedgerEntry.TransactionId and BalanceFiscal.FiscalId='" + fiscalLE6Year + "'  order by LedgerEntry.LedgerEntryId desc", con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6], rdr[7]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FillLedgerNameCombo()
        {
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(Ledger.LedgerName) from Ledger order by Ledger.LedgerId desc";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmbSLedgerName.Items.Add(rdr[0]);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void JournalForLedgerEntry_Load(object sender, EventArgs e)
        {
            fiscalLE6Year = MainUI.fiscalMYear;
            GetData();
            FillLedgerNameCombo();
            cmbSLedgerName.Focus();
        }

        private void newEntryButton_Click(object sender, EventArgs e)
        {
        
                             this.Hide();
            PreliStepsOfLedgerEntry frm=new PreliStepsOfLedgerEntry();
                             frm.Show();


        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                this.Hide();
                LedgerEntryUpdate frm = new LedgerEntryUpdate();
                frm.Show();
                frm.txtEntryId.Text = dr.Cells[0].Value.ToString();
                frm.txtTransactiondate.Text = dr.Cells[1].Value.ToString();
                frm.txtLedgerName.Text = dr.Cells[2].Value.ToString();
                frm.txtRequisitionNo.Text = dr.Cells[3].Value.ToString();
                frm.txtVoucherNo.Text = dr.Cells[4].Value.ToString();
                frm.txtParticulars.Text = dr.Cells[5].Value.ToString();
                frm.txtReceive.Text = dr.Cells[6].Value.ToString();
                frm.txtExpence.Text = dr.Cells[7].Value.ToString();
                frm.labelk.Text = frm.labelkl.Text;
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainUI frm=new MainUI();
            frm.Show();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
            if (dataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                dataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            Brush b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        

        private void cmbSLedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(LedgerEntry.LedgerEntryId),RTRIM(TransactionRecord.TransactionDate),RTRIM(Ledger.LedgerName),RTRIM(LedgerEntry.FundRequisitionNo),RTRIM(LedgerEntry.VoucherNo),RTRIM(LedgerEntry.Particulars),RTRIM(LedgerEntry.Debit),RTRIM(LedgerEntry.Credit) FROM   ((BalanceFiscal INNER JOIN LedgerEntry ON BalanceFiscal.LId=LedgerEntry.LId) INNER JOIN Ledger ON BalanceFiscal.LedgerId=Ledger.LedgerId) INNER JOIN TransactionRecord ON LedgerEntry.TransactionId=TransactionRecord.TransactionId where  BalanceFiscal.FiscalId='" + fiscalLE6Year + "' and Ledger.LedgerName like '" + cmbSLedgerName.Text + "%'   order by LedgerEntry.LedgerEntryId desc", con);
                // cmd = new SqlCommand("SELECT RTRIM(Ledger.LedgerId),RTRIM(Ledger.DateCreated),RTRIM(Ledger.LedgerName),RTRIM(AGRel.AccountType),RTRIM(BalanceFiscal.Balance),RTRIM(Ledger.PreviousLedgerId) from Ledger,BalanceFiscal,AGRel where Ledger.AGRelId=AGRel.AGRelId and  Ledger.LedgerId=BalanceFiscal.LedgerId and Ledger.LedgerName like '" + txtSLedgerName.Text + "%' and  BalanceFiscal.FiscalId='" + fiscalLYear + "' order by Ledger.LedgerName", con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6], rdr[7]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
