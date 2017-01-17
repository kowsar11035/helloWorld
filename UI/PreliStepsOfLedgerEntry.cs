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
    
    public partial class PreliStepsOfLedgerEntry : Form
    {
       
      
       
        ConnectionString cs=new ConnectionString();
        public int  fiscalLE9Year;
        public PreliStepsOfLedgerEntry()
        {
            InitializeComponent();
        }

        private void cmbEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEntryType.Text == "Individual Posting")
            {
                labelBatch.Visible = false;
                cmbBatch.Visible = false;
                noOfDebitEntryLabel.Visible = false;
                txtNumOfDebitEntry.Visible = false;
                NoOfCreditEntityLabel.Visible = false;
                txtNumOfCreditEntry.Visible = false;

            }
            if (cmbEntryType.Text == "Batch Posting")
            {
                cmbBatch.Focus();
                labelBatch.Visible = true;
                cmbBatch.Visible = true;
                cmbBatch.SelectedIndex = -1;
                noOfDebitEntryLabel.Visible = true;
                txtNumOfDebitEntry.Visible = true;
                txtNumOfDebitEntry.Clear();
                NoOfCreditEntityLabel.Visible = true;
                txtNumOfCreditEntry.Visible = true;
                txtNumOfCreditEntry.Clear();
            }
        }

        private void PreliStepsOfLedgerEntry_Load(object sender, EventArgs e)
        {
            fiscalLE9Year = MainUI.fiscalMYear;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {

            if (cmbEntryType.Text == "")
            {
                MessageBox.Show("Please Select Entry Type", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbEntryType.Focus();
                return;
            }

            if (cmbEntryType.Text == "Individual Posting")
            {
                
                                  this.Hide();
                LedgerEntryForIndividualPosting frm=new LedgerEntryForIndividualPosting();
                                   frm.Show();
                return;

            }

            if (cmbBatch.Text == "")
            {
                MessageBox.Show("Please Select Batch", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbBatch.Focus();
                return;
            }

            if (cmbEntryType.Text == "Batch Posting" && cmbBatch.Text == "One Debit Many Credit")
            {
                if (txtNumOfCreditEntry.Text == "")
                {
                    MessageBox.Show("Please enter your credit entry Number", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                labelBatch.Visible = true;
                cmbBatch.Visible = true;
                this.Hide();
                NewEntryForLedger frm = new NewEntryForLedger();
                frm.txtDEntryNo.Text = txtNumOfDebitEntry.Text;
                frm.txtCEntryNo.Text = txtNumOfCreditEntry.Text;
                frm.Show();

            }
            if (cmbEntryType.Text == "Batch Posting" && cmbBatch.Text == "One Credit Many Debit")
            {
                if (txtNumOfDebitEntry.Text == "")
                {
                    MessageBox.Show("Please enter your debit entry Number", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                labelBatch.Visible = true;
                cmbBatch.Visible = true;
                this.Hide();
                LedgerEntryForOneCreditManyDebit frm = new LedgerEntryForOneCreditManyDebit();
                frm.txtOneC.Text = txtNumOfCreditEntry.Text;
                frm.txtManyD.Text = txtNumOfDebitEntry.Text;
                frm.Show();

            }
            if (cmbEntryType.Text == "Batch Posting" && cmbBatch.Text == "Multiple Debit Multiple Credit")
            {
                if (txtNumOfDebitEntry.Text == "")
                {
                    MessageBox.Show("Please enter your debit entry Number", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtNumOfCreditEntry.Text == "")
                {
                    MessageBox.Show("Please enter your credit entry Number", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Hide();
             MultipleBatchPosting  frm=new MultipleBatchPosting();
             frm.txtCEntryNo.Text = txtNumOfCreditEntry.Text;
             frm.txtDEntryNo.Text = txtNumOfDebitEntry.Text;
                frm.Show();
            }

        }

        private void backButton_Click(object sender, EventArgs e)
        {
                           this.Hide();
       JournalForLedgerEntry frm=new JournalForLedgerEntry();
                         frm.Show();
        }

        private void cmbBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBatch.Text == "One Debit Many Credit")
            {
                txtNumOfDebitEntry.Text = "1";
                txtNumOfDebitEntry.ReadOnly = true;
                txtNumOfCreditEntry.Clear();
                txtNumOfCreditEntry.ReadOnly = false;
                txtNumOfCreditEntry.Focus();
            }
            if (cmbBatch.Text == "One Credit Many Debit")
            {
                txtNumOfCreditEntry.Text = "1";
                txtNumOfCreditEntry.ReadOnly = true;
                txtNumOfDebitEntry.Clear();
                txtNumOfDebitEntry.ReadOnly = false;
                txtNumOfDebitEntry.Focus();
            }
            if (cmbBatch.Text == "Multiple Debit Multiple Credit")
            {
                txtNumOfCreditEntry.Clear();
                txtNumOfCreditEntry.ReadOnly = false;
                txtNumOfDebitEntry.Clear();
                txtNumOfDebitEntry.ReadOnly = false;
                txtNumOfDebitEntry.Focus();

                

            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
                           this.Hide();
            JournalForLedgerEntry frm=new JournalForLedgerEntry();
                             frm.Show();
        }

        private void txtNumOfDebitEntry_KeyDown(object sender, KeyEventArgs e)
        {
            

        }

       
        }
    }

