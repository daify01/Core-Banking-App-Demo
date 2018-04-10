using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.ManageCustomerAcct
{
    public partial class ViewLoanAccounts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<LoanAccount> TheLoanAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveAll();
                IList<EOD> eods =
                           Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                foreach (LoanAccount loanaccount in TheLoanAccount)
                {
                    if (eods[0].FinancialDate - loanaccount.TransactionDate > loanaccount.LoanDueDate - loanaccount.LoanStartDate)
                    {
                        loanaccount.LoanStatus = LoanStatus.Overdue;
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().UpdateData(loanaccount);
                    }

                    if (loanaccount.Balance == 0)
                    {
                        loanaccount.LoanStatus = LoanStatus.FullyPaid;
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().UpdateData(loanaccount);
                    }
                }


                IList<LoanAccount> loanAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveByOverdueStatus();

                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("AccountNumber", System.Type.GetType("System.String"));
                dt.Columns.Add("AccountName", System.Type.GetType("System.String"));
                dt.Columns.Add("AccountType", System.Type.GetType("System.String"));
                dt.Columns.Add("LoanAmount", System.Type.GetType("System.String"));
                dt.Columns.Add("LoanDuration", System.Type.GetType("System.Double"));
                dt.Columns.Add("LoanInterest", System.Type.GetType("System.String"));
                dt.Columns.Add("Balance", System.Type.GetType("System.Double"));
                dt.Columns.Add("LoanDueDate", System.Type.GetType("System.String"));

                foreach (LoanAccount s in loanAccount)
                {
                    
                        dr = dt.NewRow();
                        dr["AccountNumber"] = s.AccountNumber;
                        dr["AccountName"] = s.AccountName;
                    dr["AccountType"] = Enum.GetName(typeof (AccountType), s.LinkedAccount.AccountType);
                        dr["LoanAmount"] = s.LoanAmount.ToString();
                        dr["LoanDuration"] = s.LoanDuration.ToString();
                        dr["LoanInterest"] = s.LoanInterest.ToString();
                        dr["Balance"] = s.Balance.ToString();
                        dr["LoanDueDate"] = s.LoanDueDate.Date.ToString();
                        dt.Rows.Add(dr);
                    
                }

                dt.AcceptChanges();
                LoanOverdue.DataSource = dt;
                LoanOverdue.DataBind();
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {

        }

        protected void LoanOverdue_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            IList<LoanAccount> loanAccount =
                   Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveByOverdueStatus();

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("AccountNumber", System.Type.GetType("System.String"));
            dt.Columns.Add("AccountName", System.Type.GetType("System.String"));
            dt.Columns.Add("AccountType", System.Type.GetType("System.String"));
            dt.Columns.Add("LoanAmount", System.Type.GetType("System.String"));
            dt.Columns.Add("LoanDuration", System.Type.GetType("System.Double"));
            dt.Columns.Add("LoanInterest", System.Type.GetType("System.String"));
            dt.Columns.Add("Balance", System.Type.GetType("System.Double"));
            dt.Columns.Add("LoanDueDate", System.Type.GetType("System.String"));

            foreach (LoanAccount s in loanAccount)
            {

                dr = dt.NewRow();
                dr["AccountNumber"] = s.AccountNumber;
                dr["AccountName"] = s.AccountName;
                dr["AccountType"] = Enum.GetName(typeof(AccountType), s.LinkedAccount.AccountType);
                dr["LoanAmount"] = s.LoanAmount.ToString();
                dr["LoanDuration"] = s.LoanDuration.ToString();
                dr["LoanInterest"] = s.LoanInterest.ToString();
                dr["Balance"] = s.Balance.ToString();
                dr["LoanDueDate"] = s.LoanDueDate.Date.ToString();
                dt.Rows.Add(dr);

            }

            dt.AcceptChanges();
            LoanOverdue.DataSource = dt;
            LoanOverdue.DataBind();

            LoanOverdue.PageIndex = e.NewPageIndex;
            LoanOverdue.DataBind();

            //bindGrid(); 
            //SubmitAppraisalGrid.PageIndex = e.NewPageIndex;
            //SubmitAppraisalGrid.DataBind();
        }
    }
}