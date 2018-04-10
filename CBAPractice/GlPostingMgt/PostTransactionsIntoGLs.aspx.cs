using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.GlPostingMgt
{
    public partial class PostTransactionsIntoGLs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            if (eod.Count == 0 || eod[0].IsClosed == false)
            {
                if (!IsPostBack)
                {
                    var glAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveAll();
                    DropDownListGlAcctToDebit.DataSource = glAccount;
                    DropDownListGlAcctToDebit.DataValueField = "Id";
                    DropDownListGlAcctToDebit.DataTextField = "GlAccountName";
                    DropDownListGlAcctToDebit.DataBind();

                    DropDownListGlAcctToCredit.DataSource = glAccount;
                    DropDownListGlAcctToCredit.DataValueField = "Id";
                    DropDownListGlAcctToCredit.DataTextField = "GlAccountName";
                    DropDownListGlAcctToCredit.DataBind();
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"]);
                        GlPosting glPosting =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                .RetrieveById(id);
                        TextBoxNameDebitNarration.Value = glPosting.DebitNarration;
                        TextBoxNameCreditNarration.Value = glPosting.CreditNarration;
                        TextBoxNameDebitOrCreditAmnt.Value = glPosting.Amount.ToString();
                        DropDownListGlAcctToDebit.SelectedValue = glPosting.GlAccountToDebit.GlAccountName;
                        DropDownListGlAcctToCredit.SelectedValue = glPosting.GlAccountToCredit.GlAccountName;
                        TextBoxId.Value = glPosting.Id.ToString();

                    }
                }
            }

            else if (eod[0].IsClosed)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Business is closed. Open Business" +
               "', function(){location = '/Start/Default.aspx';});</script>", false);
            }
            
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameDebitNarration.Value)) throw new Exception("Debit Narration field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameCreditNarration.Value)) throw new Exception("Credit Narration field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameDebitOrCreditAmnt.Value)) throw new Exception("Amount field is required");
                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    GlPosting posting = new GlPosting();
                    
                    //GlPosting glPosting = new GlPosting();
                    GlAccount glAccount =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(int.Parse(DropDownListGlAcctToCredit.SelectedValue));
                    GlAccount glAccounts =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(int.Parse(DropDownListGlAcctToDebit.SelectedValue));
                   posting.GlAccountToDebit = new GlAccount();
                   posting.GlAccountToDebit.Id = int.Parse(DropDownListGlAcctToDebit.SelectedValue);
                    var glPostings =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(posting.GlAccountToDebit.Id);
                    posting.GlAccountToCredit = new GlAccount();
                    posting.GlAccountToCredit.Id = int.Parse(DropDownListGlAcctToCredit.SelectedValue);
                    var glPosting =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(posting.GlAccountToCredit.Id);
                    posting.DebitNarration  = TextBoxNameDebitNarration.Value;
                    posting.CreditNarration = TextBoxNameCreditNarration.Value;
                    posting.Amount = double.Parse(TextBoxNameDebitOrCreditAmnt.Value);

                    IList<EOD> eod =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                    posting.TransactionDate = eod[0].FinancialDate;
                    posting.DateAdded = DateTime.Now;
                    posting.DateUpdated = DateTime.Now;
                    GlPostingLogic glPostingLogic = new GlPostingLogic();
                    //try
                    //{
                    //    glPosting.Balance = glPostingLogic.CreditGlAccount(glAccount, posting.Amount);
                    //    glPostings.Balance = glPostingLogic.DebitGlAccount(glAccounts, posting.Amount);
                    //}
                    //catch (Exception)
                    //{
                    //    if (glPosting.Balance < 0 || glPostings.Balance < 0)
                    //    {
                    //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "GL Account Balance Cannot Be Negative. Please Post Appropriately " + "', function(){});</script>", false); 
                    //    }
                    //    else if (glPosting.Balance >= 0 || glPostings.Balance >= 0)
                    //    {
                    //        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>().InsertData(posting);
                    //        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().UpdateData(glPosting);
                    //        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().UpdateData(glPostings);   
                    //    }
                    //}
                    glPosting.Balance = glPostingLogic.CreditGlAccount(glAccount, posting.Amount);
                    glPostings.Balance = glPostingLogic.DebitGlAccount(glAccounts, posting.Amount);

                    
                    if (glPosting.Balance >= 0 && glPostings.Balance >= 0)
                    {
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>().InsertData(posting);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(glPosting);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(glPostings);
                    }
                    else if (glPosting.Balance < 0 || glPostings.Balance < 0)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "GL Account Balance Cannot Be Negative. Please Post Appropriately " + "', function(){});</script>", false);
                    }

                }

                else
                {
                    //adjust code to tally with the one in 'if' clause
                    GlAccount glAccount = new GlAccount();
                    GlPosting glPosting =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    glPosting.GlAccountToDebit = new GlAccount();
                    glPosting.GlAccountToDebit.Id = int.Parse(DropDownListGlAcctToDebit.SelectedValue);
                    glPosting.GlAccountToCredit = new GlAccount();
                    glPosting.GlAccountToCredit.Id = int.Parse(DropDownListGlAcctToCredit.SelectedValue);
                    glPosting.DebitNarration = TextBoxNameDebitNarration.Value;
                    glPosting.CreditNarration = TextBoxNameCreditNarration.Value;
                    glPosting.Amount = double.Parse(TextBoxNameDebitOrCreditAmnt.Value);
                    GlPostingLogic glPostingLogic = new GlPostingLogic();
                    //glAccount.Balance = glPostingLogic.PostIntoGlAccounts(glAccount, glPosting.Amount);
                    glPosting.GlAccountToCredit.Balance = glPostingLogic.CreditGlAccount(glAccount, glPosting.Amount);
                    glPosting.GlAccountToDebit.Balance = glPostingLogic.CreditGlAccount(glAccount, glPosting.Amount);
                    glPosting.DateUpdated = DateTime.Now;

                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                        .UpdateData(glPosting);
                    //Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().UpdateData(glAccount);

                }


                TextBoxNameDebitNarration.Value = String.Empty;
                TextBoxNameCreditNarration.Value = String.Empty;
                TextBoxNameDebitOrCreditAmnt.Value = String.Empty;


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gl Posting Saved Successfully" + "', function(){location = '/GlPostingMgt/PostTransactionsIntoGLs.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                var glPostings =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(int.Parse(DropDownListGlAcctToDebit.SelectedValue));
                var glPosting =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(int.Parse(DropDownListGlAcctToCredit.SelectedValue));
                if (glPosting.Balance < 0 || glPostings.Balance < 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "GL Account Balance Cannot Be Negative. Please Post Appropriately " + "', function(){});</script>", false);
                }
                if (DropDownListGlAcctToDebit.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "GL Acct to Debit Not Selected. Please Select One " + "', function(){});</script>", false);
                }
                if (DropDownListGlAcctToCredit.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "GL Acct to Credit Not Selected. Please Select One " + "', function(){});</script>", false);
                }
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }

            }
        }
    }
}