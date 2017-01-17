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
    public partial class NewEntryForLedger : Form
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public int  iTransactionId = 0, lEntryId, cEntryId, k, genericOTypeId,creditLedgerEntryId,debitContraEntryId;
        public string contraLedgerName, conTraLedgerId, cmb11LedgerName, ledgerId1, ledgerId2, userId, secondLedgerId, fullName, lGenericType, debitAGRelId1, creditAGRelId2,creditAGRelId,cLID2;
        public decimal takeSum=0, takeSub=0,takeRemove=0,debitBalance=0,lDBalance=0,lCBalance=0,creditBalance=0;
        public string OAgrelId, accountOTypeD, accountOType;
        public int fiscalLE2Year;
        public int dLId, cLId;
        public DateTime startDateOneDManyC, endDateOneDManyC;
        public NewEntryForLedger()
        {
            InitializeComponent();
        }
        
        
        public void Ledger2FillCombo()
        {
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(LedgerName) from Ledger where Ledger.LedgerName!='"+cmb1LedgerName.Text+"' order by LedgerId desc";
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
                    cmb1LedgerName.Items.Add(rdr[0]);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void NewEntryForLedger_Load(object sender, EventArgs e)
        {
            txt1TransactionType.Text = "Debit";
            txt2TransactionType.Text = "Credit";
            userId = frmLogin.uId.ToString();
            fiscalLE2Year = MainUI.fiscalMYear;
            groupBox2.Enabled = false;
            Ledger1CmbFill();


            startDateOneDManyC = MainUI.startDateM;
            endDateOneDManyC = MainUI.endDateM;
            //startDateInd1 = Convert.ToDateTime(startDateInd);
            //endDateInd1 = Convert.ToDateTime(endDateInd);
            txt1Entrydate.MinDate = startDateOneDManyC;
            txt1Entrydate.MaxDate = endDateOneDManyC;
            if (DateTime.UtcNow.ToLocalTime() > endDateOneDManyC)
            {
                txt1Entrydate.Value = txt1Entrydate.MaxDate;

            }
            else
            {
                txt1Entrydate.Value = DateTime.Now;
            }

            
            
        }

        public void GetLedgerId()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);

                con.Open();
                cmd = con.CreateCommand();

                cmd.CommandText = "SELECT RTRIM(Ledger.LedgerId)  from  Ledger.LedgerName = '" + cmb1LedgerName.Text + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ledgerId1 = (rdr.GetString(0));
                   
                   
                }
                if ((rdr != null))
                {
                    rdr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
               

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            JournalForLedgerEntry frm = new JournalForLedgerEntry();
            frm.Show();
        }

        private void Reset()
        { 
            cmb1LedgerName.SelectedIndex= -1;
            txt1Entrydate.Value = this.txt1Entrydate.MaxDate;
            txt1RequisitionNo.Text = "";
            txt1VoucherNo.Text = "";
            txt1Particulars.Text = "";
            txt1Amount.TextChanged -= txt1Amount_TextChanged;
            txt1Amount.Text = "";
            txt1Amount.TextChanged += txt1Amount_TextChanged;
            cmb2LedgerName.SelectedIndex = -1;
            txt2FundRequisition.Text = "";
            txt2VoucherNo.Text = "";
            txt2Particulars.Text = "";
            txt2Amount.Text = "";
            group1.Enabled = true;
            listView1.Items.Clear();
            takeSum = 0; 
            takeSub=0;
            takeRemove=0;
            debitBalance = 0;
            lDBalance = 0;
            lCBalance = 0;
            creditBalance=0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {  
        }
        private void cmbLedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        private void txtReceive_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtExpence_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void dynamicButton_Click(object sender, EventArgs e)
        {
            AddNewTextBox();
        }

        private int A = 1;

        public System.Windows.Forms.TextBox AddNewTextBox()
        {
            System.Windows.Forms.TextBox txt1 = new System.Windows.Forms.TextBox();
            this.Controls.Add(txt1);
            txt1.Top = A*28;
            txt1.Left = 150;
            txt1.Text = "ContraLedgerName" + this.A.ToString();
            A = A + 1;
            return txt1;

        }

        private void cmbContraLedger_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbContraParticulars_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void adToChartButton_Click(object sender, EventArgs e)
        {         
        }
        private void button2_Click(object sender, EventArgs e)
        {

            Reset();
            groupBox2.Enabled = false;


        }

        private void cmb1LedgerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cmb1TransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmb1TransactionType.Text == "Debit")
            //{
            //    txt2TransactionType.Text = "Credit";
            //}
            //if (cmb1TransactionType.Text == "Credit")
            //{
            //    txt2TransactionType.Text = "Debit";
            //}
        }
        private void GetLId1()
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
        private void cmb1LedgerName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            txt1Entrydate.Focus();
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

        private void txt2FundRequisition_TextChanged(object sender, EventArgs e)
        {       
       }

        private void addButton_Click(object sender, EventArgs e)
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

            if (txt2Amount.Text == "")
            {
                MessageBox.Show("Please enter  credit amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt2Amount.Focus();
                return;
            }

            try
            {
                decimal val1 = 0;
                decimal.TryParse(txt1Amount.Text, out val1);
               
                takeSub = takeSum;
                takeSum = takeSum + Convert.ToDecimal(txt2Amount.Text);
                if (val1 < takeSum)
                {
                    MessageBox.Show("Your input amount exceed the limit", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    takeSum = takeSub;
                    txt2Amount.Text = "";
                    txt2Amount.Focus();
                    return;
                }
                else
                {
                    if (listView1.Items.Count == 0)
                    {
                        ListViewItem lst = new ListViewItem();
                        lst.SubItems.Add(cmb2LedgerName.Text);
                        secondLedgerId = Convert.ToString(ledgerId2);
                        lst.SubItems.Add(secondLedgerId);
                        lst.SubItems.Add(txt2FundRequisition.Text);
                        lst.SubItems.Add(txt2VoucherNo.Text);
                        lst.SubItems.Add(txt2Particulars.Text);
                        lst.SubItems.Add(txt2Amount.Text);
                        cLID2 = Convert.ToString(cLId);
                        lst.SubItems.Add(cLID2);
                        creditAGRelId = Convert.ToString(creditAGRelId2);
                        lst.SubItems.Add(creditAGRelId);
                        listView1.Items.Add(lst);

                        cmb2LedgerName.SelectedIndex = -1;
                        txt2FundRequisition.Text = "";
                        txt2VoucherNo.Text = "";
                        txt2Particulars.Text = "";
                        txt2Amount.Text = "";

                        return;
                    }

                    ListViewItem lst1 = new ListViewItem();
                    lst1.SubItems.Add(cmb2LedgerName.Text);
                    secondLedgerId = Convert.ToString(ledgerId2);
                    lst1.SubItems.Add(secondLedgerId);
                    lst1.SubItems.Add(txt2FundRequisition.Text);
                    lst1.SubItems.Add(txt2VoucherNo.Text);
                    lst1.SubItems.Add(txt2Particulars.Text);
                    lst1.SubItems.Add(txt2Amount.Text);
                    cLID2 = Convert.ToString(cLId);
                    lst1.SubItems.Add(cLID2);
                    creditAGRelId = Convert.ToString(creditAGRelId2);
                    lst1.SubItems.Add(creditAGRelId);
                    listView1.Items.Add(lst1);

                    cmb2LedgerName.SelectedIndex = -1;
                    txt2FundRequisition.Text = "";
                    txt2VoucherNo.Text = "";
                    txt2Particulars.Text = "";
                    txt2Amount.Text = "";
                    return;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt2Amount_TextChanged(object sender, EventArgs e)
        {
     
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

        private void GetUserName()
        {
            con=new SqlConnection(cs.DBConn);
            con.Open();
            string query = "Select Name From Registration where UserId='"+userId+"'";
            cmd=new SqlCommand(query,con);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                fullName = (rdr.GetString(0));
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
                cmd.Parameters.AddWithValue("d1", cmb1LedgerName.Text);
                cmd.Parameters.AddWithValue("d2", ledgerId1);
                con.Open();
                cmd.ExecuteReader();
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
        private void SaveNewTransaction()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            string cb = "insert into TransactionRecord(TransactionDate,EntryDateTime,InputBy) VALUES (@d1,@d2,@d3)";
            cmd = new SqlCommand(cb);
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@d1", Convert.ToDateTime(txt1Entrydate.Value, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat));
            cmd.Parameters.AddWithValue("@d2",DateTime.UtcNow.ToLocalTime());
            cmd.Parameters.AddWithValue("@d3", fullName);
            cmd.ExecuteReader();
            con.Close();

        }

        private void SaveDebitLedgerBalance()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" + ledgerId1 + "' and BalanceFiscal.LId='" + dLId + "'";
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
                string query = "select AccountType from AGRel where AGRel.AGRelId='" + debitAGRelId1 + "'";
                cmd = new SqlCommand(query, con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    accountOTypeD = (rdr.GetString(0));

                }
                con.Close();

                if (accountOTypeD == "Asset" || accountOTypeD == "Expense")
                {
                    decimal a = decimal.Parse(txt1Amount.Text);
                    lDBalance = debitBalance + a;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance + " where BalanceFiscal.LedgerId ='" + ledgerId1 + "' and BalanceFiscal.LId='" + dLId + "' ";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                }
                if (accountOTypeD == "Liability" || accountOTypeD == "Equity" || accountOTypeD == "Revenue")
                {
                    decimal b = decimal.Parse(txt1Amount.Text);
                    lDBalance = debitBalance - b;
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update BalanceFiscal set Balance=" + lDBalance + " where BalanceFiscal.LedgerId ='" + ledgerId1 + "' and BalanceFiscal.LId='" + dLId + "' ";
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
            if (listView1.Items.Count != Convert.ToInt32(txtCEntryNo.Text))
            {
                MessageBox.Show("Number Of Credit Entry does not match....Please Check before Submit", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetUserName();
            SaveNewTransaction();
            CreateTransactionId();

            try
            {
                if (takeSum > Convert.ToDecimal(txt1Amount.Text) || takeSum < Convert.ToDecimal(txt1Amount.Text))
                {
                    MessageBox.Show("Your Transaction Parameters(Debit & Credit) are not Equal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (takeSum == Convert.ToDecimal(txt1Amount.Text))
                {
                    if (txt1TransactionType.Text == "Debit")
                    {
                        SaveDebitLedgerBalance();
                        con = new SqlConnection(cs.DBConn);
                        con.Open();
                        string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Debit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                        cmd = new SqlCommand(cb);
                        cmd.Connection = con;
                        //cmd.Parameters.AddWithValue("@d1", firstLedgerId);
                        cmd.Parameters.AddWithValue("@d1", txt1RequisitionNo.Text);
                        cmd.Parameters.AddWithValue("@d2", txt1VoucherNo.Text);
                        cmd.Parameters.AddWithValue("@d3", txt1Particulars.Text);
                        cmd.Parameters.AddWithValue("@d4", decimal.Parse(txt1Amount.Text));
                        cmd.Parameters.AddWithValue("@d5", lDBalance);
                        cmd.Parameters.AddWithValue("@d6", iTransactionId);
                        cmd.Parameters.AddWithValue("@d7", dLId);
                        cmd.ExecuteReader();
                        con.Close();
                        LEntryId();
                        SaveDebitContraEntry();
                        GetDebitContraEntryId();
                    }

                   
                    for (int i = 0; i <= listView1.Items.Count - 1; i++)
                    {
                        if (txt2TransactionType.Text == "Credit")
                        {
                            try
                            {
                                con = new SqlConnection(cs.DBConn);
                                con.Open();
                                string ct = "select Balance from BalanceFiscal where  BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId='" + listView1.Items[i].SubItems[7].Text + "' ";
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
                                string q1 = "Select RTRIM(AGRel.AccountType) from AGRel where AGRel.AGRelId='" +listView1.Items[i].SubItems[8].Text + "'";
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
                                    lCBalance = creditBalance - x;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance + " where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "' and BalanceFiscal.LId ='" + listView1.Items[i].SubItems[7].Text + "'";
                                    cmd = new SqlCommand(cb2);
                                    cmd.Connection = con;
                                    cmd.ExecuteReader();
                                    con.Close();

                                }
                                // if (genericOTypeId == 2)
                                 if (accountOType == "Liability" || accountOType == "Equity" || accountOType == "Revenue")
                              
                                {
                                    decimal y = decimal.Parse(listView1.Items[i].SubItems[6].Text);
                                    lCBalance = creditBalance + y;
                                    con = new SqlConnection(cs.DBConn);
                                    con.Open();
                                    string cb2 = "Update BalanceFiscal set Balance=" + lCBalance + " where BalanceFiscal.LedgerId='" + listView1.Items[i].SubItems[2].Text + "'and BalanceFiscal.LId ='" + listView1.Items[i].SubItems[7].Text + "'";
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

                            string cb = "insert into LedgerEntry(FundRequisitionNo,VoucherNo,Particulars,Credit,Balances,TransactionId,LId) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                            cmd = new SqlCommand(cb);
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("d1", listView1.Items[i].SubItems[3].Text);
                            cmd.Parameters.AddWithValue("d2", listView1.Items[i].SubItems[4].Text);
                            cmd.Parameters.AddWithValue("d3", listView1.Items[i].SubItems[5].Text);
                            cmd.Parameters.AddWithValue("d4", decimal.Parse(listView1.Items[i].SubItems[6].Text));
                            cmd.Parameters.AddWithValue("d5", lCBalance);
                            cmd.Parameters.AddWithValue("d6", iTransactionId);
                            cmd.Parameters.AddWithValue("d7", listView1.Items[i].SubItems[7].Text);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            GetCreditLedgerEntryId();


                            con = new SqlConnection(cs.DBConn);
                            string query = "insert into ContraEntry(ContraLName,ContraLId) values(@d1,@d2)";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("d1", listView1.Items[i].SubItems[1].Text);
                            cmd.Parameters.AddWithValue("d2", listView1.Items[i].SubItems[2].Text);
                            con.Open();
                            cmd.ExecuteReader();
                            con.Close();


                            GetCEntryId();
                            SaveLCLRelation();
                           
                        }
                    
                    }
                    
                    MessageBox.Show("Transaction Completed Successfully", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                    this.Hide();
                    PreliStepsOfLedgerEntry frmk = new PreliStepsOfLedgerEntry();
                    frmk.Show();
                    //groupBox2.Enabled = false;
                }
              }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
           
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

        private void txt2Amount_TextChanged_1(object sender, EventArgs e)
        {
            decimal val1 = 0;
            decimal val2 = 0;
            decimal.TryParse(txt1Amount.Text, out val1);
            decimal.TryParse(txt2Amount.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("This Amount must be less than  Ledger '"+txt1TransactionType.Text+"' amount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt2Amount.Text = "";
                txt2Amount.Focus();
                return;
            }
        }

        private void txt2FundRequisition_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
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

        private void txt1Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt2Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt1Amount_TextChanged(object sender, EventArgs e)

        {
            if (cmb1LedgerName.Text == "")
            {
                MessageBox.Show("Please select Ledger Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmb1LedgerName.Focus();
                return;
            }
            if (txt1Particulars.Text == "")
            {
                MessageBox.Show("Please enter Particulars", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt1Particulars.Focus();
                return;
            }
            groupBox2.Enabled = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void closeButton_Click_1(object sender, EventArgs e)
        {
                            this.Hide();
         PreliStepsOfLedgerEntry frm =new PreliStepsOfLedgerEntry();
                                frm.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txt1Entrydate_ValueChanged(object sender, EventArgs e)
        {
            txt1RequisitionNo.Focus();
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
                txt1Amount.Focus();
                e.Handled = true;
            }
        }

        private void txt1Amount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmb2LedgerName.Focus();
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
                txt2Amount.Focus();
                e.Handled = true;
            }
        }

        private void txt2Amount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                submitButton_Click(this, new EventArgs());
            }
        }
      }
    }

