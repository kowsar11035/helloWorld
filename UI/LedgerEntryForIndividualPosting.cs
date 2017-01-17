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
using AccountsManagementSystem.LogInUI;

namespace AccountsManagementSystem.UI
{
    public partial class LedgerEntryForIndividualPosting : Form
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public int  iTransactionId = 0, lEntryId, cEntryId, k,  genericOTypeId, creditLedgerEntryId, debitContraEntryId;
        public string contraLedgerName, conTraLedgerId, cmb11LedgerName, firstLedgerId,ledgerId2, userId, secondLedgerId, fullName, lGenericType, aGRelId1, aGRelId2;
        public decimal debitAmount = 0, creditAmount = 0, takeRemove = 0, debitBalance = 0, lDBalance = 0, lCBalance = 0, creditBalance = 0;
        public string OAgrelId, accountOTypeD, accountOType,startDateInd,endDateInd;
        public int fiscalLE1Year, lID1, lID2;
        public DateTime startDateInd1, endDateInd1;
        public LedgerEntryForIndividualPosting()
        {
            InitializeComponent();
        }
        public void Ledger1CmbFill()
        {
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(LedgerName) from Ledger order by LedgerId desc";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmbInd1LedgerName.Items.Add(rdr[0]);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            cmbInd1LedgerName.SelectedIndex = -1;
            //txtInd1Entrydate.Value = this.txtInd1Entrydate.MaxDate;
            if (DateTime.UtcNow.ToLocalTime() > endDateInd1)
            {
                txtInd1Entrydate.Value = txtInd1Entrydate.MaxDate;

            }
            else
            {
                txtInd1Entrydate.Value = DateTime.Now;
            }
            txtInd1RequisitionNo.Text = "";
            txtInd1VoucherNo.Text = "";
            txtInd1Particulars.Text = "";
            txtIndDebitBalance.TextChanged -= txtDebitBalance_TextChanged;
            txtIndDebitBalance.Text = "";
            txtIndDebitBalance.TextChanged += txtDebitBalance_TextChanged;
            cmbInd2LedgerName.SelectedIndex = -1;
            txtInd2FundRequisition.Text = "";
            txtInd2VoucherNo.Text = "";
            txtInd2Particulars.Text = "";
            txtIndCrdeitBalance.Text = "";
            group1.Enabled = true;
           
            takeRemove = 0;
            debitBalance = 0;
            lDBalance = 0;
            lCBalance = 0;
            creditBalance = 0;
        }
        private void LedgerEntryForIndividualPosting_Load(object sender, EventArgs e)
        {
            txtInd1TransactionType.Text = "Debit";
            txtInd2TransactionType.Text = "Credit";
            userId = frmLogin.uId.ToString();
            fiscalLE1Year = MainUI.fiscalMYear;

            startDateInd1 = MainUI.startDateM;
            endDateInd1 = MainUI.endDateM;
           
            txtInd1Entrydate.MinDate = startDateInd1;
            txtInd1Entrydate.MaxDate = endDateInd1;
            if (DateTime.UtcNow.ToLocalTime() > endDateInd1)
            {
                txtInd1Entrydate.Value = txtInd1Entrydate.MaxDate;

            }
            else
            {
                txtInd1Entrydate.Value = DateTime.Now;
            }

            
            groupBox2.Enabled = false;
            Ledger1CmbFill();
            cmbInd1LedgerName.Focus();
        }
        private void GetLId2()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select BalanceFiscal.LId from BalanceFiscal where BalanceFiscal.LedgerId='" + ledgerId2 + "' and  BalanceFiscal.FiscalId='" + fiscalLE1Year + "'";
            cmd = new SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                lID2 = (rdr.GetInt32(0));
            }
        }
        private void cmbInd2LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInd2FundRequisition.Focus();
            try
            {
                group1.Enabled = false;
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string query = "Select  RTRIM(Ledger.LedgerId),RTRIM(Ledger.AGRelId)  from Ledger where Ledger.LedgerName='" + cmbInd2LedgerName.Text + "' ";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ledgerId2 = (rdr.GetString(0));
                    aGRelId2 = (rdr.GetString(1));
                }
                con.Close();
                GetLId2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void txtDebitBalance_TextChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;
            decimal val2 = 0;
            decimal.TryParse(txtIndDebitBalance.Text, out val1);
            decimal.TryParse(txtIndCrdeitBalance.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("This Credit Amount must be equal to Debit amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIndCrdeitBalance.Text = "";
                txtIndCrdeitBalance.Focus();
                return;
            }
            groupBox2.Enabled = true;
        }
        private void GetUserName()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select Name From Registration where UserId='" + userId + "'";
            cmd = new SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                fullName = (rdr.GetString(0));
            }
        }
        private void SaveNewTransaction()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb = "insert into TransactionRecord(TransactionDate,EntryDateTime,InputBy) VALUES (@d1,@d2,@d3)";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1",Convert.ToDateTime(txtInd1Entrydate.Value,System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
                cmd.Parameters.AddWithValue("@d2", DateTime.UtcNow.ToLocalTime());
                cmd.Parameters.AddWithValue("@d3", fullName);
                cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void CreateTransactionId()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();

            string cty4 = "SELECT MAX(TransactionRecord.TransactionId) FROM TransactionRecord";
            cmd = new SqlCommand(cty4);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                k = (rdr.GetInt32(0));

                iTransactionId = k;

            }

        }

        private void SaveDebitLedgerBalance()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" + firstLedgerId + "' and BalanceFiscal.LId='" + lID1 + "'";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    debitBalance = (rdr.GetDecimal(0));
                }
                con.Close();

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string query = "select AccountType from AGRel where AGRel.AGRelId='" + aGRelId1 + "'";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    accountOTypeD = (rdr.GetString(0));

                }
                con.Close();

                if (accountOTypeD == "Asset" || accountOTypeD == "Expense")
                {
                    decimal a = decimal.Parse(txtIndDebitBalance.Text);
                    lDBalance = debitBalance + a;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance + " where BalanceFiscal.LedgerId ='" + firstLedgerId + "' and BalanceFiscal.LId='" + lID1 + "' ";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                }
                if (accountOTypeD == "Liability" || accountOTypeD == "Equity" || accountOTypeD == "Revenue")
                {
                    decimal b = decimal.Parse(txtIndDebitBalance.Text);
                    lDBalance = debitBalance - b;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance + " where BalanceFiscal.LedgerId ='" + firstLedgerId + "' and BalanceFiscal.LId='" + lID1 + "' ";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LEntryId()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string cty4 = "SELECT MAX(LedgerEntry.LedgerEntryId) FROM LedgerEntry";
            cmd = new SqlCommand(cty4);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                lEntryId = (rdr.GetInt32(0));

            }
            con.Close();
        }
        private void SaveDebitContraEntry()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("d1", cmbInd1LedgerName.Text);
                cmd.Parameters.AddWithValue("d2", firstLedgerId);
                con.Open();
                cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetDebitContraEntryId()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string cty4 = "SELECT MAX(ContraEntry.CEntryId) FROM ContraEntry";
            cmd = new SqlCommand(cty4);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                debitContraEntryId = (rdr.GetInt32(0));

            }
            con.Close();

        }
        private void GetCreditLedgerEntryId()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string cty4 = "SELECT MAX(LedgerEntry.LedgerEntryId) FROM LedgerEntry";
            cmd = new SqlCommand(cty4);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                creditLedgerEntryId = (rdr.GetInt32(0));

            }
            con.Close();
        }
        private void GetCEntryId()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string cty4 = "SELECT MAX(ContraEntry.CEntryId) FROM ContraEntry";
            cmd = new SqlCommand(cty4);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                cEntryId = (rdr.GetInt32(0));

            }
            con.Close();

        }
        private void SaveContraLCLRelation()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                string q1 = "insert into LECLERelation(TransactionId,LedgerEntryId,CEntryId) values(@d1,@d2,@d3)";
                cmd = new SqlCommand(q1, con);
                cmd.Parameters.AddWithValue("d1", iTransactionId);
                cmd.Parameters.AddWithValue("d2", creditLedgerEntryId);
                cmd.Parameters.AddWithValue("d3", debitContraEntryId);
                con.Open();
                cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveLCLRelation()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                string query = "insert into LECLERelation(TransactionId,LedgerEntryId,CEntryId) values(@d1,@d2,@d3)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("d1", iTransactionId);
                cmd.Parameters.AddWithValue("d2", lEntryId);
                cmd.Parameters.AddWithValue("d3", cEntryId);
                con.Open();
                cmd.ExecuteReader();
                con.Close();
                SaveContraLCLRelation();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveCreditEntry()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);

                string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Credit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                //cmd.Parameters.AddWithValue("d1", ledgerId);
                cmd.Parameters.AddWithValue("d1", txtInd2FundRequisition.Text);
                cmd.Parameters.AddWithValue("d2", txtInd2VoucherNo.Text);
                cmd.Parameters.AddWithValue("d3", txtInd2Particulars.Text);
                cmd.Parameters.AddWithValue("d4", decimal.Parse(txtIndDebitBalance.Text));
                cmd.Parameters.AddWithValue("d5", lCBalance);
                cmd.Parameters.AddWithValue("d6", iTransactionId);
                cmd.Parameters.AddWithValue("d7", lID2);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SaveContraEntry()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("d1", cmbInd2LedgerName.Text);
                cmd.Parameters.AddWithValue("d2", ledgerId2);
                con.Open();
                cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SaveDebitEntry()
        {
            try
            {
                if (txtInd1TransactionType.Text == "Debit")
                {
                    SaveDebitLedgerBalance();
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Debit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                    cmd = new SqlCommand(cb);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@d1", txtInd1RequisitionNo.Text);
                    cmd.Parameters.AddWithValue("@d2", txtInd1VoucherNo.Text);
                    cmd.Parameters.AddWithValue("@d3", txtInd1Particulars.Text);
                    cmd.Parameters.AddWithValue("@d4", decimal.Parse(txtIndDebitBalance.Text));
                    cmd.Parameters.AddWithValue("@d5", lDBalance);
                    cmd.Parameters.AddWithValue("@d6", iTransactionId);
                    cmd.Parameters.AddWithValue("@d7", lID1);
                    cmd.ExecuteReader();
                    con.Close();
                    LEntryId();
                    SaveDebitContraEntry();
                    GetDebitContraEntryId();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void UpdateLedgerCreditBalance()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" + ledgerId2+ "' and BalanceFiscal.LId='"+lID2+"'";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    creditBalance = (rdr.GetDecimal(0));
                    

                }
                con.Close();
                //////
                
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string q1 = "Select RTRIM(AGRel.AccountType) from AGRel where AGRel.AGRelId='" + aGRelId2 +"'";
                cmd = new SqlCommand(q1, con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    accountOType = (rdr.GetString(0));

                }

                con.Close();
                //if (genericOTypeId == 1)
                if (accountOType == "Asset" || accountOType == "Expense")
                {
                    decimal x = decimal.Parse(txtIndCrdeitBalance.Text);
                    lCBalance = creditBalance - x;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance + " where BalanceFiscal.LedgerId ='" + ledgerId2+ "' and BalanceFiscal.LId='" + lID2 + "' ";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                }
                // if (genericOTypeId == 2)
                if (accountOType == "Liability" || accountOType == "Equity" || accountOType == "Revenue")
                {
                    decimal y = decimal.Parse(txtIndCrdeitBalance.Text);
                    lCBalance = creditBalance + y;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance + " where BalanceFiscal.LedgerId ='" + ledgerId2 + "' and BalanceFiscal.LId='" + lID2 + "' ";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            if (cmbInd1LedgerName.Text == "")
            {
                MessageBox.Show("Please Select Credit  Ledger name", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbInd1LedgerName.Focus();
                return;
            }
            if (txtInd1Particulars.Text == "")
            {
                MessageBox.Show("Please Enter Credit Particulars", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInd1Particulars.Focus();
                return;
            }
            if (txtIndDebitBalance.Text == "")
            {
                MessageBox.Show("Please Enter Credit balance", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIndDebitBalance.Focus();
                return;
            }
            if (cmbInd2LedgerName.Text == "")
            {
                MessageBox.Show("Please Select Debit Ledger name", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbInd2LedgerName.Focus();
                return;
            }
            if (txtInd2Particulars.Text == "")
            {
                MessageBox.Show("Please Enter Debit Particulars", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInd2Particulars.Focus();
                return;
            }
            if (txtIndDebitBalance.Text == "")
            {
                MessageBox.Show("Please Enter Debit balance", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIndDebitBalance.Focus();
                return;
            }

            try
            {

                debitAmount = Convert.ToDecimal(txtIndDebitBalance.Text);
                creditAmount = Convert.ToDecimal(txtIndCrdeitBalance.Text);

                if (debitAmount == creditAmount)
                {
                    GetUserName();
                    SaveNewTransaction();
                    CreateTransactionId();
                    SaveDebitEntry();
                    UpdateLedgerCreditBalance();
                    SaveCreditEntry();
                    GetCreditLedgerEntryId();
                    SaveContraEntry();
                    GetCEntryId();
                    SaveLCLRelation();
                    MessageBox.Show("Transaction Completed Successfully", "Record", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    Reset();
                    groupBox2.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Your Transaction Parameters are invalid", "error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetLId1()
        {
            con=new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select BalanceFiscal.LId from BalanceFiscal where BalanceFiscal.LedgerId='" + firstLedgerId + "' and  BalanceFiscal.FiscalId='" + fiscalLE1Year + "'";
            cmd = new SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                lID1 = (rdr.GetInt32(0));
            }
        }
        private void cmbInd1LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);

                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "SELECT  RTRIM(Ledger.LedgerId),RTRIM(Ledger.LedgerName),RTRIM(Ledger.AGRelId) from Ledger WHERE Ledger.LedgerName = '" + cmbInd1LedgerName.Text + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    firstLedgerId = (rdr.GetString(0));
                    cmb11LedgerName = (rdr.GetString(1));
                    aGRelId1 = (rdr.GetString(2));

                }

                if ((rdr != null))
                {
                    rdr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                GetLId1();

                cmbInd1LedgerName.Text = cmbInd1LedgerName.Text.Trim();
                cmbInd2LedgerName.Items.Clear();
                cmbInd2LedgerName.Text = "";
                cmbInd2LedgerName.Enabled = true;
                cmbInd2LedgerName.Focus();

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(Ledger.LedgerName) from Ledger  Where Ledger.LedgerName!= '" + cmb11LedgerName + "' order by Ledger.LedgerId desc";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmbInd2LedgerName.Items.Add(rdr[0]);
                }
                con.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtIndDebitBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            JournalForLedgerEntry frm=new JournalForLedgerEntry();
            frm.Show();
        }

        private void txtInd1RequisitionNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtInd1VoucherNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtInd2FundRequisition_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtInd2VoucherNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtInd1Entrydate_ValueChanged(object sender, EventArgs e)
        {
            txtInd1RequisitionNo.Focus();
        }

        private void txtInd1RequisitionNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtInd1VoucherNo.Focus();
                e.Handled = true;
            }
        }

        private void txtInd1VoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtInd1Particulars.Focus();
                e.Handled = true;
            }
        }

        private void txtInd1Particulars_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtIndDebitBalance.Focus();
                e.Handled = true;
            }
        }

        private void txtIndDebitBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbInd2LedgerName.Focus();
                e.Handled = true;
            }
        }

        private void txtInd2FundRequisition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtInd2VoucherNo.Focus();
                e.Handled = true;
            }
        }

        private void txtInd2VoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Enter)
            {
                txtInd2Particulars.Focus();
                e.Handled = true;
            }
            
        }

        private void txtIndCrdeitBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                submitButton_Click(this, new EventArgs());
            }
        }

        private void txtInd2Particulars_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtIndCrdeitBalance.Focus();
                e.Handled = true;
            }
        }
    }
}
