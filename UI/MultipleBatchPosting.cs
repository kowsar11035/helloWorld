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
    public partial class MultipleBatchPosting : Form
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public int iTransactionId = 0, lEntryId, cEntryId, k, genericOTypeId, creditLedgerEntryId, debitContraEntryId;
        public string contraLedgerName, conTraLedgerId, cmb11LedgerName, ledgerId1, ledgerId2, userId, secondLedgerId, fullName, lGenericType, debitAGRelId1, creditAGRelId2, creditAGRelId, cLID2;
        public decimal takeSum1 = 0,takeSum2=0, takeSub1 = 0,takeSub2=0, takeRemove1 = 0,takeRemove2, debitBalance = 0, lDBalance = 0, lCBalance = 0, creditBalance = 0;
        public string OAgrelId, accountOTypeD, accountOType,dLId1, cLId2, aGRelId;
        public int fiscalLE2Year;
        public int  dLId,cLId, debitAGRelId2;
        public DateTime startDateManyDManyC, endDateManyDManyC;
        public MultipleBatchPosting()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void Ledger1CmbFill()
        {
            try
            {
                //for (int i = 0; i <= listView1.Items.Count - 1; i++)
                //{
                    //for (int j = 0; j <= listView1.Items.Count - 1; j++)
                    //{
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                   // string ct = "select RTRIM(Ledger.LedgerName) from Ledger where Ledger.AGRelId!='4' and Ledger.AGRelId!='5' and Ledger.LedgerName!='" +listView1.Items[i].SubItems[1].Text + "' order by LedgerId desc";
                    string ct ="select RTRIM(Ledger.LedgerName) from Ledger where Ledger.AGRelId!='4' and Ledger.AGRelId!='5' order by LedgerId desc";
                    cmd = new SqlCommand(ct);
                    cmd.Connection = con;
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        cmb1LedgerName.Items.Add(rdr[0]);
                    }
                    con.Close();
                    //}
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MultipleBatchPosting_Load(object sender, EventArgs e)
        {
            txt1TransactionType.Text = "Debit";
            txt2TransactionType.Text = "Credit";
            userId = frmLogin.uId.ToString();
            fiscalLE2Year = MainUI.fiscalMYear;

            startDateManyDManyC = MainUI.startDateM;
            endDateManyDManyC = MainUI.endDateM;

            txtTransactiondate.MinDate = startDateManyDManyC;
            txtTransactiondate.MaxDate = endDateManyDManyC;
            if (DateTime.UtcNow.ToLocalTime() > endDateManyDManyC)
            {
                txtTransactiondate.Value = txtTransactiondate.MaxDate;

            }
            else
            {
                txtTransactiondate.Value = DateTime.Now;
            }

            groupBox2.Enabled = false;
            Ledger1CmbFill();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (cmb1LedgerName.Text == "")
            {
                MessageBox.Show("You must select a LedgerName.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmb1LedgerName.Focus();
                return;
            }

            if (txt1Particulars.Text == "")
            {
                MessageBox.Show("You must enter Particulars", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt1Particulars.Focus();
                return;
            }

            if (txt1DebitAmount.Text == "")
            {
                MessageBox.Show("Please enter  debit amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt1DebitAmount.Focus();
                return;
            }

            try
            {
                //decimal val1 = 0;
                //decimal.TryParse(txt1DebitAmount.Text, out val1);

                //takeSub1 = takeSum1;
                takeSum1 = takeSum1 + Convert.ToDecimal(txt1DebitAmount.Text);
                //if (val1 < takeSum1)
                //{
                //    MessageBox.Show("Your input amount exceed the limit", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    takeSum1 = takeSub1;
                //    txt1DebitAmount.Text = "";
                //    txt1DebitAmount.Focus();
                //    return;
                //}
                //else
                //{
                    if (listView1.Items.Count == 0)
                    {
                        ListViewItem lst = new ListViewItem();
                        lst.SubItems.Add(cmb1LedgerName.Text);
                        secondLedgerId = Convert.ToString(ledgerId1);
                        lst.SubItems.Add(secondLedgerId);
                        lst.SubItems.Add(txt1RequisitionNo.Text);
                        lst.SubItems.Add(txt1VoucherNo.Text);
                        lst.SubItems.Add(txt1Particulars.Text);
                        lst.SubItems.Add(txt1DebitAmount.Text);
                        dLId1 = Convert.ToString(dLId);
                        lst.SubItems.Add(dLId1);
                        aGRelId = Convert.ToString(debitAGRelId1);
                        lst.SubItems.Add(aGRelId);

                        listView1.Items.Add(lst);
                        cmb1LedgerName.SelectedIndex = -1;
                        txt1RequisitionNo.Text = "";
                        txt1VoucherNo.Text = "";
                        txt1Particulars.Text = "";
                        txt1DebitAmount.Text = "";

                        return;
                    }

                    ListViewItem lst1 = new ListViewItem();
                    lst1.SubItems.Add(cmb1LedgerName.Text);
                    secondLedgerId = Convert.ToString(ledgerId1);
                    lst1.SubItems.Add(secondLedgerId);
                    lst1.SubItems.Add(txt1RequisitionNo.Text);
                    lst1.SubItems.Add(txt1VoucherNo.Text);
                    lst1.SubItems.Add(txt1Particulars.Text);
                    lst1.SubItems.Add(txt1DebitAmount.Text);
                    dLId1 = Convert.ToString(dLId);
                    lst1.SubItems.Add(dLId1);
                    aGRelId = Convert.ToString(debitAGRelId1);
                    lst1.SubItems.Add(aGRelId);

                    listView1.Items.Add(lst1);
                    cmb1LedgerName.SelectedIndex = -1;
                    txt1RequisitionNo.Text = "";
                    txt1VoucherNo.Text = "";
                    txt1Particulars.Text = "";
                    txt1DebitAmount.Text = "";
                    Ledger1CmbFill();
                    return;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void creditLedgerAddButton_Click(object sender, EventArgs e)
        {
            if (cmb2LedgerName.Text == "")
            {
                MessageBox.Show("You must select a LedgerName.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmb2LedgerName.Focus();
                return;
            }

            if (txt2Particulars.Text == "")
            {
                MessageBox.Show("You must enter Particulars", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt2Particulars.Focus();
                return;
            }

            if (txt2CreditAmount.Text == "")
            {
                MessageBox.Show("Please enter  credit amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt2CreditAmount.Focus();
                return;
            }

            try
            {
                //decimal val1 = 0;
                //decimal.TryParse(takeSum1, out val1);

                takeSub2 = takeSum2;
                takeSum2 = takeSum2 + Convert.ToDecimal(txt2CreditAmount.Text);
                if (takeSum1 < takeSum2)
                {
                    MessageBox.Show("Your input amount exceed the limit", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    takeSum2 = takeSub2;
                    txt2CreditAmount.Text = "";
                    txt2CreditAmount.Focus();
                    return;
                }
                else
                {
                    if (listView2.Items.Count == 0)
                    {
                        ListViewItem lst10= new ListViewItem();
                        lst10.SubItems.Add(cmb2LedgerName.Text);
                        secondLedgerId = Convert.ToString(ledgerId2);
                        lst10.SubItems.Add(secondLedgerId);
                        lst10.SubItems.Add(txt2FundRequisition.Text);
                        lst10.SubItems.Add(txt2VoucherNo.Text);
                        lst10.SubItems.Add(txt2Particulars.Text);
                        lst10.SubItems.Add(txt2CreditAmount.Text);
                        cLId2 = Convert.ToString(cLId);
                        lst10.SubItems.Add(cLId2);
                        aGRelId = Convert.ToString(creditAGRelId2);
                        lst10.SubItems.Add(aGRelId);

                        listView2.Items.Add(lst10);
                        cmb2LedgerName.SelectedIndex = -1;
                        txt2FundRequisition.Text = "";
                        txt2VoucherNo.Text = "";
                        txt2Particulars.Text = "";
                        txt2CreditAmount.Text = "";

                        return;
                    }

                    ListViewItem lst12 = new ListViewItem();
                    lst12.SubItems.Add(cmb2LedgerName.Text);
                    secondLedgerId = Convert.ToString(ledgerId2);
                    lst12.SubItems.Add(secondLedgerId);
                    lst12.SubItems.Add(txt2FundRequisition.Text);
                    lst12.SubItems.Add(txt2VoucherNo.Text);
                    lst12.SubItems.Add(txt2Particulars.Text);
                    lst12.SubItems.Add(txt2CreditAmount.Text);
                    cLId2 = Convert.ToString(cLId);
                    lst12.SubItems.Add(cLId2);
                    aGRelId = Convert.ToString(creditAGRelId2);
                    lst12.SubItems.Add(aGRelId);

                    listView2.Items.Add(lst12);
                    cmb2LedgerName.SelectedIndex = -1;
                    txt2FundRequisition.Text = "";
                    txt2VoucherNo.Text = "";
                    txt2Particulars.Text = "";
                    txt2CreditAmount.Text = "";
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
            takeRemove1 = Convert.ToDecimal(listView1.SelectedItems[0].SubItems[6].Text);
            takeSum1 = takeSum1 - takeRemove1;

            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].Selected)
                {
                    listView1.Items[i].Remove();
                }
            }
        }

        private void creditLedgerRemoveButton_Click(object sender, EventArgs e)
        {
            takeRemove2 = Convert.ToDecimal(listView2.SelectedItems[0].SubItems[6].Text); 
            takeSum2 = takeSum2 - takeRemove2;

            for (int i = listView2.Items.Count - 1; i >= 0; i--)
            {
                if (listView2.Items[i].Selected)
                {
                    listView2.Items[i].Remove();
                }
            }
        }
        private void GetLId1()
        {
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string query = "Select BalanceFiscal.LId from BalanceFiscal where BalanceFiscal.LedgerId='" + ledgerId1 + "' and  BalanceFiscal.FiscalId='" + fiscalLE2Year + "'";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    dLId = (rdr.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmb1LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt1RequisitionNo.Focus();
            try
            {

                con = new SqlConnection(cs.DBConn);

                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "SELECT  RTRIM(Ledger.LedgerId),RTRIM(Ledger.LedgerName),RTRIM(Ledger.AGRelId) from Ledger WHERE Ledger.LedgerName = '" + cmb1LedgerName.Text + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ledgerId1 = (rdr.GetString(0));
                    cmb11LedgerName = (rdr.GetString(1));
                    debitAGRelId1 = (rdr.GetString(2));

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
                cmb1LedgerName.Text = cmb1LedgerName.Text.Trim();
                cmb2LedgerName.Items.Clear();
                cmb2LedgerName.Text = "";
                cmb2LedgerName.Enabled = true;
                cmb2LedgerName.Focus();

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(Ledger.LedgerName) from Ledger  Where Ledger.LedgerName!= '" + cmb11LedgerName + "' order by Ledger.LedgerId desc";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmb2LedgerName.Items.Add(rdr[0]);
                }
                con.Close();


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetLId2()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select BalanceFiscal.LId from BalanceFiscal where BalanceFiscal.LedgerId='" + ledgerId2 + "' and  BalanceFiscal.FiscalId='" + fiscalLE2Year + "'";
            cmd = new SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                cLId = (rdr.GetInt32(0));
            }
        }
        private void cmb2LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt2FundRequisition.Focus();
            try
            {
                group1.Enabled = false;
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string query = "Select  RTRIM(Ledger.LedgerId),RTRIM(Ledger.AGRelId)  from Ledger where Ledger.LedgerName='" + cmb2LedgerName.Text + "' ";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ledgerId2 = (rdr.GetString(0));
                    creditAGRelId2 = (rdr.GetString(1));
                }
                con.Close();
                GetLId2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt1RequisitionNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt1VoucherNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt1DebitAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt2FundRequisition_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt2VoucherNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt2CreditAmount_KeyPress(object sender, KeyPressEventArgs e)
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
            PreliStepsOfLedgerEntry frm=new PreliStepsOfLedgerEntry();
                                 frm.Show();
        }

        private void debitCompleteButton_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == Convert.ToInt32(txtDEntryNo.Text))
            {
                group1.Enabled = false;
                groupBox2.Enabled = true;
            }
            else
            {
                MessageBox.Show("Your Number of Debit Entry is not equal to your Propossal Entry", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void txt2CreditAmount_TextChanged(object sender, EventArgs e)
        {
            //decimal val1 = 0;
            //decimal val2 = 0;
            //decimal.TryParse(txt1Amount.Text, out val1);
            //decimal.TryParse(txt2Amount.Text, out val2);
            //if (takeSum1 > takeSum2)
            //{
            //    MessageBox.Show("This Amount must be less than Debit Ledger '" + takeSum1 + "' amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    txt2CreditAmount.Text = "";
            //    txt2CreditAmount.Focus();
            //    return;
            //}
        }

        private void txt2CreditAmount_Validating(object sender, CancelEventArgs e)
        {
            if (takeSum2 > takeSum1)
            {
                MessageBox.Show("This Amount must be less than Debit Ledger '" + takeSum1 + "' amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt2CreditAmount.Text = "";
                txt2CreditAmount.Focus();
                return;
            }
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
            cmd.Parameters.AddWithValue("@d1", Convert.ToDateTime(txtTransactiondate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
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

        private void LEntryId()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }
        private void GetDebitContraEntryId()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
        private void SaveCreditContraLCLRelation()
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
                SaveCreditContraLCLRelation();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count != Convert.ToInt32(txtCEntryNo.Text))
            {
                MessageBox.Show("Your Number of Credit Entry is not equal to your Propossal Entry...Please Check before Submit", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                if (takeSum1 != takeSum2)
                {
                    MessageBox.Show("Your Transaction Parameters(Debit & Credit) are not Equal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (takeSum1 == takeSum2)
                {
                    GetUserName();
                    SaveNewTransaction();
                    CreateTransactionId();

                   //Debit Entry Start Here


                    if (txt1TransactionType.Text == "Debit")
                    {
                        for (int i = 0; i <= listView1.Items.Count - 1; i++)
                        {
                           
                                con = new SqlConnection(cs.DBConn);
                                con.Open();
                                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId='" + listView1.Items[i].SubItems[7].Text + "' ";
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
                                    lDBalance = debitBalance - x;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance + " where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId ='" + listView1.Items[i].SubItems[7].Text + "'";
                                    cmd = new SqlCommand(cb2);
                                    cmd.Connection = con;
                                    cmd.ExecuteReader();
                                    con.Close();

                                }
                                // if (genericOTypeId == 2)
                                if (accountOType == "Liability" || accountOType == "Equity" || accountOType == "Revenue")
                                {
                                    decimal y = decimal.Parse(listView1.Items[i].SubItems[6].Text);
                                    lDBalance = debitBalance + y;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance + " where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "'and BalanceFiscal.LId ='" + listView1.Items[i].SubItems[7].Text + "'";
                                    cmd = new SqlCommand(cb2);
                                    cmd.Connection = con;
                                    cmd.ExecuteReader();
                                    con.Close();

                                }

                            

                          
                            con = new SqlConnection(cs.DBConn);
                            con.Open();
                            string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Debit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                            cmd = new SqlCommand(cb);
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("d1", listView1.Items[i].SubItems[3].Text);
                            cmd.Parameters.AddWithValue("d2", listView1.Items[i].SubItems[4].Text);
                            cmd.Parameters.AddWithValue("d3", listView1.Items[i].SubItems[5].Text);
                            cmd.Parameters.AddWithValue("d4", decimal.Parse(listView1.Items[i].SubItems[6].Text));
                            cmd.Parameters.AddWithValue("d5", lDBalance);
                            cmd.Parameters.AddWithValue("d6", iTransactionId);
                            cmd.Parameters.AddWithValue("d7", listView1.Items[i].SubItems[7].Text);
                            cmd.ExecuteReader();
                            con.Close();

                            LEntryId();
                            
                            con = new SqlConnection(cs.DBConn);
                            string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("d1", listView1.Items[i].SubItems[1].Text);
                            cmd.Parameters.AddWithValue("d2", listView1.Items[i].SubItems[2].Text);
                            con.Open();
                            cmd.ExecuteReader();
                            con.Close();
                            GetDebitContraEntryId();
                        }
                    }

                    //Credit Entry Start here

                    if (txt2TransactionType.Text == "Credit")
                    {

                        for (int i = 0; i <= listView2.Items.Count - 1; i++)
                        {

                            
                                con = new SqlConnection(cs.DBConn);
                                con.Open();
                                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" +
                                            listView2.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId='" +
                                            listView2.Items[i].SubItems[7].Text + "' ";
                                cmd = new SqlCommand(ct);
                                cmd.Connection = con;
                                rdr = cmd.ExecuteReader();
                                if (rdr.Read())
                                {
                                    creditBalance = (rdr.GetDecimal(0));


                                }
                                con.Close();

                                con = new SqlConnection(cs.DBConn);
                                con.Open();
                                string q1 = "Select RTRIM(AGRel.AccountType) from AGRel where AGRel.AGRelId='" +
                                            listView2.Items[i].SubItems[8].Text + "'";
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
                                    decimal x = decimal.Parse(listView2.Items[i].SubItems[6].Text);
                                    lCBalance = creditBalance - x;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance +
                                                 " where BalanceFiscal.LedgerId='" + listView2.Items[i].SubItems[2].Text +
                                                 "' and BalanceFiscal.LId ='" + listView2.Items[i].SubItems[7].Text +
                                                 "'";
                                    cmd = new SqlCommand(cb2);
                                    cmd.Connection = con;
                                    cmd.ExecuteReader();
                                    con.Close();

                                }
                                // if (genericOTypeId == 2)
                                if (accountOType == "Liability" || accountOType == "Equity" || accountOType == "Revenue")
                                {
                                    decimal y = decimal.Parse(listView2.Items[i].SubItems[6].Text);
                                    lCBalance = creditBalance + y;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance +
                                                 " where BalanceFiscal.LedgerId='" + listView2.Items[i].SubItems[2].Text +
                                                 "'and BalanceFiscal.LId ='" + listView2.Items[i].SubItems[7].Text + "'";
                                    cmd = new SqlCommand(cb2);
                                    cmd.Connection = con;
                                    cmd.ExecuteReader();
                                    con.Close();

                                }

                            
                            //Con.Close
                            con = new SqlConnection(cs.DBConn);

                            string cb ="insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Credit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                            cmd = new SqlCommand(cb);
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("d1", listView2.Items[i].SubItems[3].Text);
                            cmd.Parameters.AddWithValue("d2", listView2.Items[i].SubItems[4].Text);
                            cmd.Parameters.AddWithValue("d3", listView2.Items[i].SubItems[5].Text);
                            cmd.Parameters.AddWithValue("d4", decimal.Parse(listView2.Items[i].SubItems[6].Text));
                            cmd.Parameters.AddWithValue("d5", lCBalance);
                            cmd.Parameters.AddWithValue("d6", iTransactionId);
                            cmd.Parameters.AddWithValue("d7", listView2.Items[i].SubItems[7].Text);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                             GetCreditLedgerEntryId();


                            con = new SqlConnection(cs.DBConn);
                            string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("d1", listView2.Items[i].SubItems[1].Text);
                            cmd.Parameters.AddWithValue("d2", listView2.Items[i].SubItems[2].Text);
                            con.Open();
                            cmd.ExecuteReader();
                            con.Close();


                             GetCEntryId();
                             SaveLCLRelation();

                        }
                    }
                }

                    MessageBox.Show("Transaction Completed Successfully", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                    this.Hide();
                    PreliStepsOfLedgerEntry frmk = new PreliStepsOfLedgerEntry();
                    frmk.Show();
                    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            cmb1LedgerName.SelectedIndex = -1;
            txtTransactiondate.Value = this.txtTransactiondate.MaxDate;
            txt1RequisitionNo.Text = "";
            txt1VoucherNo.Text = "";
            txt1Particulars.Text = "";
            txt1DebitAmount.TextChanged -= txt1DebitAmount_TextChanged;
            txt1DebitAmount.Text = "";
            txt1DebitAmount.TextChanged += txt1DebitAmount_TextChanged;
            cmb2LedgerName.SelectedIndex = -1;
            txt2FundRequisition.Text = "";
            txt2VoucherNo.Text = "";
            txt2Particulars.Text = "";
            txt2CreditAmount.Text = "";
            group1.Enabled = true;
            listView1.Items.Clear();
            takeSum1 =takeSum2= 0;
            takeSub1 =takeSub2= 0;
            takeRemove1 =takeRemove2= 0;
            debitBalance = 0;
            lDBalance = 0;
            lCBalance = 0;
            creditBalance = 0;
        }

        private void txt1DebitAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txt1RequisitionNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt1VoucherNo.Focus();
                e.Handled = true;
            }
        }

        private void txt1VoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt1Particulars.Focus();
                e.Handled = true;
            }
        }

        private void txt1Particulars_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Enter)
            {
                txt1DebitAmount.Focus();
                e.Handled = true;
            }
            
        }

        private void txt2FundRequisition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt2VoucherNo.Focus();
                e.Handled = true;
            }
        }

        private void txt2VoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt2Particulars.Focus();
                e.Handled = true;
            }
        }

        private void txt2Particulars_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt2CreditAmount.Focus();
                e.Handled = true;
            }
        }

        private void txt2CreditAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSubmit_Click(this, new EventArgs());
            }
        }
    }
}
