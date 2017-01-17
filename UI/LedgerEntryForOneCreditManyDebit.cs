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
    public partial class LedgerEntryForOneCreditManyDebit : Form
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        ConnectionString cs=new ConnectionString();
        public int  iTransactionId = 0, lEntryId, cEntryId, k,  genericOTypeId, creditLedgerEntryId, debitContraEntryId;
        public string contraLedgerName, conTraLedgerId, cmb11LedgerName, firstLedgerId, ledgerId2, userId, secondLedgerId, fullName, lGenericType, creditAGRelId1, debitAGRelId2,aGRelId;
        public decimal takeSum = 0, takeSub = 0, takeRemove = 0, creditBalance1 = 0, lCBalance1 = 0, lDBalance2 = 0, debitBalance2 = 0;
        public string OAgrelId, accountOTypeD, accountOType, dLId2;
        public int fiscalLE3Year,cLId,dLId;
        public static DateTime startDateOneCManyD, endDateOneCManyD;
        public LedgerEntryForOneCreditManyDebit()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void Reset()
        {
            cmbC1DM1LedgerName.SelectedIndex = -1;
            txtC1DM1Entrydate.Value = this.txtC1DM1Entrydate.MaxDate;
            txtC1DM1RequisitionNo.Text = "";
            txtC1DM1VoucherNo.Text = "";
            txtC1DM1Particulars.Text = "";
            txtC1DM1CreditBalance.TextChanged -= txtC1DM1CreditBalance_TextChanged;
            txtC1DM1CreditBalance.Text = "";
            txtC1DM1CreditBalance.TextChanged += txtC1DM1CreditBalance_TextChanged;
            cmbC1DM2LedgerName.SelectedIndex = -1;
            txtc1DM2FundRequisition.Text = "";
            txtC1DM2VoucherNo.Text = "";
            txtC1DM2Particulars.Text = "";
            txtC1DM2DebitBalance.Text = "";
            group1.Enabled = true;
            listView1.Items.Clear();
            takeSum = 0;
            takeSub = 0;
            takeRemove = 0;
            creditBalance1 = 0;
            lCBalance1 = 0;
            lDBalance2 = 0;
            debitBalance2 = 0;
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
                    cmbC1DM1LedgerName.Items.Add(rdr[0]);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LedgerEntryForOneCreditManyDebit_Load(object sender, EventArgs e)
        {
            txtC1DM1TransactionType.Text = "Credit";
            txtC1DM2TransactionType.Text = "Debit";
            cmbC1DM1LedgerName.Focus();
            userId = frmLogin.uId.ToString();
            fiscalLE3Year = MainUI.fiscalMYear;
            group1.Enabled = true;
            groupBox2.Enabled = false;
            Ledger1CmbFill();


            startDateOneCManyD = MainUI.startDateM;
            endDateOneCManyD = MainUI.endDateM;
           
            txtC1DM1Entrydate.MinDate = startDateOneCManyD;
            txtC1DM1Entrydate.MaxDate = endDateOneCManyD;
            if (DateTime.UtcNow.ToLocalTime() > endDateOneCManyD)
            {
                txtC1DM1Entrydate.Value = txtC1DM1Entrydate.MaxDate;

            }
            else
            {
                txtC1DM1Entrydate.Value = DateTime.Now;
            }
            

        }
        private void GetLId11()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select BalanceFiscal.LId from BalanceFiscal where BalanceFiscal.LedgerId='" + firstLedgerId + "' and  BalanceFiscal.FiscalId='" + fiscalLE3Year + "'";
            cmd = new SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                cLId = (rdr.GetInt32(0));
            }
        }
        private void cmbC1DM1LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtC1DM1Entrydate.Focus();
            try
            {
                con = new SqlConnection(cs.DBConn);

                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "SELECT  RTRIM(Ledger.LedgerId),RTRIM(Ledger.LedgerName),RTRIM(Ledger.AGRelId) from Ledger WHERE Ledger.LedgerName = '" + cmbC1DM1LedgerName.Text + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    firstLedgerId = (rdr.GetString(0));
                    cmb11LedgerName = (rdr.GetString(1));
                    creditAGRelId1 = (rdr.GetString(2));

                }

                if ((rdr != null))
                {
                    rdr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                GetLId11();
                cmbC1DM1LedgerName.Text = cmbC1DM1LedgerName.Text.Trim();
                cmbC1DM2LedgerName.Items.Clear();
                cmbC1DM2LedgerName.Text = "";
                cmbC1DM2LedgerName.Enabled = true;
                cmbC1DM2LedgerName.Focus();

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(Ledger.LedgerName) from Ledger  Where Ledger.LedgerName!= '" + cmb11LedgerName + "' order by Ledger.LedgerId desc";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmbC1DM2LedgerName.Items.Add(rdr[0]);
                }
                con.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetLId22()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select BalanceFiscal.LId from BalanceFiscal where BalanceFiscal.LedgerId='" + ledgerId2 + "' and  BalanceFiscal.FiscalId='" + fiscalLE3Year + "'";
            cmd = new SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                dLId = (rdr.GetInt32(0));
            }
        }
        private void cmbC1DM2LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtc1DM2FundRequisition.Focus();
            try
            {
                group1.Enabled = false;
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string query = "Select  RTRIM(Ledger.LedgerId),RTRIM(Ledger.AGRelId)  from Ledger where Ledger.LedgerName='" + cmbC1DM2LedgerName.Text +"' ";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ledgerId2 = (rdr.GetString(0));
                    debitAGRelId2 = (rdr.GetString(1));
                }
                con.Close();
                GetLId22();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (cmbC1DM2LedgerName.Text == "")
            {
                MessageBox.Show("You must select a LedgerName.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbC1DM2LedgerName.Focus();
                return;
            }

            if (txtC1DM2Particulars.Text == "")
            {
                MessageBox.Show("You must enter Particulars", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtC1DM2Particulars.Focus();
                return;
            }

            if (txtC1DM2DebitBalance.Text == "")
            {
                MessageBox.Show("Please enter  debit amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtC1DM2DebitBalance.Focus();
                return;
            }

            try
            {
                decimal val1 = 0;
                decimal.TryParse(txtC1DM1CreditBalance.Text, out val1);

                takeSub = takeSum;
                takeSum = takeSum + Convert.ToDecimal(txtC1DM2DebitBalance.Text);
                if (val1 < takeSum)
                {
                    MessageBox.Show("Your input amount exceed the limit", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    takeSum = takeSub;
                    txtC1DM2DebitBalance.Text = "";
                    txtC1DM2DebitBalance.Focus();
                    return;
                }
                else
                {
                    if (listView1.Items.Count == 0)
                    {
                        ListViewItem lst = new ListViewItem();
                        lst.SubItems.Add(cmbC1DM2LedgerName.Text);
                        secondLedgerId = Convert.ToString(ledgerId2);
                        lst.SubItems.Add(secondLedgerId);
                        lst.SubItems.Add(txtc1DM2FundRequisition.Text);
                        lst.SubItems.Add(txtC1DM2VoucherNo.Text);
                        lst.SubItems.Add(txtC1DM2Particulars.Text);
                        lst.SubItems.Add(txtC1DM2DebitBalance.Text);
                        dLId2 = Convert.ToString(dLId);
                        lst.SubItems.Add(dLId2);
                        aGRelId = Convert.ToString(debitAGRelId2);
                        lst.SubItems.Add(aGRelId);

                        listView1.Items.Add(lst);
                        cmbC1DM2LedgerName.SelectedIndex = -1;
                        txtc1DM2FundRequisition.Text = "";
                        txtC1DM2VoucherNo.Text = "";
                        txtC1DM2Particulars.Text = "";
                        txtC1DM2DebitBalance.Text = "";

                        return;
                    }

                    ListViewItem lst1 = new ListViewItem();
                    lst1.SubItems.Add(cmbC1DM2LedgerName.Text);
                    secondLedgerId = Convert.ToString(ledgerId2);
                    lst1.SubItems.Add(secondLedgerId);
                    lst1.SubItems.Add(txtc1DM2FundRequisition.Text);
                    lst1.SubItems.Add(txtC1DM2VoucherNo.Text);
                    lst1.SubItems.Add(txtC1DM2Particulars.Text);
                    lst1.SubItems.Add(txtC1DM2DebitBalance.Text);
                    dLId2 = Convert.ToString(dLId);
                    lst1.SubItems.Add(dLId2);
                    aGRelId = Convert.ToString(debitAGRelId2);
                    lst1.SubItems.Add(aGRelId);

                    listView1.Items.Add(lst1);
                    cmbC1DM2LedgerName.SelectedIndex = -1;
                    txtc1DM2FundRequisition.Text = "";
                    txtC1DM2VoucherNo.Text = "";
                    txtC1DM2Particulars.Text = "";
                    txtC1DM2DebitBalance.Text = "";
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            takeRemove = Convert.ToDecimal(listView1.SelectedItems[0].SubItems[6].Text);
            takeSum = takeSum - takeRemove;

            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].Selected)
                {
                    listView1.Items[i].Remove();
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            Reset();
            groupBox2.Enabled = false;
        }

        private void txtC1DM1CreditBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtC1DM1CreditBalance_TextChanged(object sender, EventArgs e)
        {
            
            if (cmbC1DM1LedgerName.Text == "")
            {
                MessageBox.Show("Please select Ledger Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbC1DM1LedgerName.Focus();
                return;
            }
            if (txtC1DM1Particulars.Text == "")
            {
                MessageBox.Show("Please enter Particulars", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtC1DM1Particulars.Focus();
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
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string cb = "insert into TransactionRecord(TransactionDate,EntryDateTime,InputBy) VALUES (@d1,@d2,@d3)";
            cmd = new SqlCommand(cb);
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@d1", Convert.ToDateTime(txtC1DM1Entrydate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
            cmd.Parameters.AddWithValue("@d2", DateTime.UtcNow.ToLocalTime());
            cmd.Parameters.AddWithValue("@d3", fullName);
            cmd.ExecuteReader();
            con.Close();

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
        private void SaveCreditLedgerBalance()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" + firstLedgerId + "' and BalanceFiscal.LId='" + cLId + "'";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    creditBalance1 = (rdr.GetDecimal(0));
                }
                con.Close();

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string query = "select AccountType from AGRel where AGRel.AGRelId='" + creditAGRelId1 + "'";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    accountOTypeD = (rdr.GetString(0));

                }
                con.Close();

                if (accountOTypeD == "Asset" || accountOTypeD == "Expense")
                {
                    decimal a = decimal.Parse(txtC1DM1CreditBalance.Text);
                    lCBalance1 = creditBalance1 + a;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance1 + " where BalanceFiscal.LedgerId ='" + firstLedgerId + "' and BalanceFiscal.LId='" + cLId + "' ";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                }
                if (accountOTypeD == "Liability" || accountOTypeD == "Equity" || accountOTypeD == "Revenue")
                {
                    decimal b = decimal.Parse(txtC1DM1CreditBalance.Text);
                    lCBalance1 = creditBalance1 - b;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance1 + " where BalanceFiscal.LedgerId ='" + firstLedgerId + "' and BalanceFiscal.LId='" + cLId + "' ";
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
        private void SaveCreditContraEntry()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("d1", cmbC1DM1LedgerName.Text);
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
        private void GetCreditContraEntryId()
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
        private void GetDebitLedgerEntryId()
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
        private void GetDebitEntryId()
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
        private void SaveDebitContraLCLRelation()
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
                SaveDebitContraLCLRelation();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != Convert.ToInt32(txtManyD.Text))
            {
               MessageBox.Show("Number Of Debit Entry does not match....Please Check before Submit","error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            GetUserName();
            SaveNewTransaction();
            CreateTransactionId();

            try
            {
                if (takeSum > Convert.ToDecimal(txtC1DM1CreditBalance.Text) || takeSum < Convert.ToDecimal(txtC1DM1CreditBalance.Text))
                {
                    MessageBox.Show("Your Transaction Parameters are invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (takeSum == Convert.ToDecimal(txtC1DM1CreditBalance.Text))
                {
                    if (txtC1DM1TransactionType.Text == "Credit")
                    {
                        SaveCreditLedgerBalance();
                        con = new SqlConnection(cs.DBConn);
                        con.Open();
                        string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Credit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                        cmd = new SqlCommand(cb);
                        cmd.Connection = con;
                      
                        cmd.Parameters.AddWithValue("@d1", txtC1DM1RequisitionNo.Text);
                        cmd.Parameters.AddWithValue("@d2", txtC1DM1VoucherNo.Text);
                        cmd.Parameters.AddWithValue("@d3", txtC1DM1Particulars.Text);
                        cmd.Parameters.AddWithValue("@d4", decimal.Parse(txtC1DM1CreditBalance.Text));
                        cmd.Parameters.AddWithValue("@d5", lCBalance1);
                        cmd.Parameters.AddWithValue("@d6", iTransactionId);
                        cmd.Parameters.AddWithValue("@d7", cLId);
                        cmd.ExecuteReader();
                        con.Close();

                        LEntryId();
                        SaveCreditContraEntry();
                        GetCreditContraEntryId();
                    }


                    for (int i = 0; i <= listView1.Items.Count - 1; i++)
                    {
                        if (txtC1DM2TransactionType.Text == "Debit")
                        {
                            try
                            {
                                con = new SqlConnection(cs.DBConn);
                                con.Open();
                                string ct = "select Balance from BalanceFiscal where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and  BalanceFiscal.LId='" + listView1.Items[i].SubItems[7].Text + "' ";
                                cmd = new SqlCommand(ct);
                                cmd.Connection = con;
                                rdr = cmd.ExecuteReader();
                                if (rdr.Read())
                                {
                                    debitBalance2 = (rdr.GetDecimal(0));
                                   

                                }
                                con.Close();

                                con = new SqlConnection(cs.DBConn);
                                con.Open();
                                string q1 = "Select RTRIM(AGRel.AccountType) from AGRel where AGRel.AGRelId='" + listView1.Items[i].SubItems[8].Text + "'";
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
                                    decimal x = decimal.Parse(listView1.Items[i].SubItems[6].Text);
                                    lDBalance2 = debitBalance2 - x;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance2 + " where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId ='" + listView1.Items[i].SubItems[7].Text + "'";
                                    cmd = new SqlCommand(cb2);
                                    cmd.Connection = con;
                                    cmd.ExecuteReader();
                                    con.Close();

                                }
                                // if (genericOTypeId == 2)
                                if (accountOType == "Liability" || accountOType == "Equity" || accountOType == "Revenue")
                                {
                                    decimal y = decimal.Parse(listView1.Items[i].SubItems[6].Text);
                                    lDBalance2 = debitBalance2 + y;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance2 + " where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId ='" + listView1.Items[i].SubItems[7].Text + "'";
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

                            //Con.Close
                            con = new SqlConnection(cs.DBConn);
                            string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Debit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                            cmd = new SqlCommand(cb);
                            cmd.Connection = con; 
                            cmd.Parameters.AddWithValue("d1", listView1.Items[i].SubItems[3].Text);
                            cmd.Parameters.AddWithValue("d2", listView1.Items[i].SubItems[4].Text);
                            cmd.Parameters.AddWithValue("d3", listView1.Items[i].SubItems[5].Text);
                            cmd.Parameters.AddWithValue("d4", decimal.Parse(listView1.Items[i].SubItems[6].Text));
                            cmd.Parameters.AddWithValue("d5", lDBalance2);
                            cmd.Parameters.AddWithValue("d6", iTransactionId);
                            cmd.Parameters.AddWithValue("d7", listView1.Items[i].SubItems[7].Text);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            GetDebitLedgerEntryId();


                            con = new SqlConnection(cs.DBConn);
                            string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("d1", listView1.Items[i].SubItems[1].Text);
                            cmd.Parameters.AddWithValue("d2", listView1.Items[i].SubItems[2].Text);
                            con.Open();
                            cmd.ExecuteReader();
                            con.Close();


                            GetDebitEntryId();
                            SaveLCLRelation();

                        }

                    }

                    MessageBox.Show("Transaction Completed Successfully", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                  
                                            this.Hide();
                    PreliStepsOfLedgerEntry frmk=new PreliStepsOfLedgerEntry();
                                           frmk.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtC1DM2DebitBalance_TextChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;
            decimal val2 = 0;
            decimal.TryParse(txtC1DM1CreditBalance.Text, out val1);
            decimal.TryParse(txtC1DM2DebitBalance.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("This Amount must be less than or equal to Ledger '" + txtC1DM1TransactionType.Text + "' amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtC1DM2DebitBalance.Text = "";
                txtC1DM2DebitBalance.Focus();
                return;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            JournalForLedgerEntry frm = new JournalForLedgerEntry();
               frm.Show();
        }

        private void txtC1DM1RequisitionNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtC1DM1VoucherNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtc1DM2FundRequisition_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtC1DM2VoucherNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            group1.Enabled = true;
        }

        private void closeButton_Click_1(object sender, EventArgs e)
        {
                        this.Hide();
   PreliStepsOfLedgerEntry frm=new PreliStepsOfLedgerEntry();
                          frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtC1DM1RequisitionNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtC1DM1VoucherNo.Focus();
                e.Handled = true;
            }
        }

        private void txtC1DM1VoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtC1DM1Particulars.Focus();
                e.Handled = true;
            }
        }

        private void txtC1DM1Particulars_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtC1DM1CreditBalance.Focus();
                e.Handled = true;
            }
        }

        private void txtC1DM1CreditBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbC1DM2LedgerName.Focus();
                e.Handled = true;
            }
        }

        private void txtc1DM2FundRequisition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtC1DM2VoucherNo.Focus();
                e.Handled = true;
            }
        }

        private void txtC1DM2VoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtC1DM2Particulars.Focus();
                e.Handled = true;
            }
        }

        private void txtC1DM2Particulars_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtC1DM2DebitBalance.Focus();
                e.Handled = true;
            }
        }

        private void txtC1DM2DebitBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                submitButton_Click(this, new EventArgs());
            }
        }
        }
    }

