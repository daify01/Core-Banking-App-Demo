using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using CBAPractice.Core;
using CBAPractice.Data;
using DotNetOpenAuth.AspNet.Clients;


namespace CBAPractice
{
    public class QueryController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        public DataTableSuccessMessage Branch()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchName = HttpContext.Current.Request.Form["searchName"];
                string searchCode = HttpContext.Current.Request.Form["searchCode"];
                var branches =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>()
                        .PagedSearchByNameAndRCNo(searchName, searchCode, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    branches.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                BRANCHName = y.BranchName,
                                RCNumber = y.RcNumber,
                                ViewDetails =
                                    @"<button type='button' onclick=""viewDetails('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">View Details</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public DataTableSuccessMessage OnUsWithdrawal()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchName = HttpContext.Current.Request.Form["searchName"];
                string searchId = HttpContext.Current.Request.Form["searchName"];
                string searchLocation = HttpContext.Current.Request.Form["searchCode"];
                var terminals =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>()
                        .PagedSearchByName_IDAndLocation(searchName, searchId, searchLocation, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    terminals.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                TerminalName = y.Name,
                                TerminaLID = y.TerminalID,
                                ViewDetails =
                                    @"<button type='button' onclick=""viewDetails('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">View Details</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public DataTableSuccessMessage Users()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchfName = HttpContext.Current.Request.Form["searchFName"];
                string searchlName = HttpContext.Current.Request.Form["searchLName"];
                string searchbranch = HttpContext.Current.Request.Form["searchBranch"];
                var users =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>()
                        .PagedSearchByNameAndBranch(searchfName, searchlName, searchbranch, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    users.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                Fname = y.FirstName,
                                LName = y.LastName,
                                URole = y.UserRole.ToString(),
                                BRanch = y.Branch.BranchName,
                                ViewDetails =
                                    @"<button type='button' onclick=""viewDetails('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">View Details</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public Branch BranchDetails(int id)
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveById(id);
        }

        public OnUsWithdrawal OnUsWithdrawalsDetails(int id)
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>().RetrieveById(id);
        }

        public UserDetail UserDetails(int id)
        {
            return new UserDetail(Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrieveById(id));
        }

        //public User UserDetails(int id)
        //{
        //    UserDetail user =
        //        new UserDetail(
        //            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrieveById(id));
        //    return user;

        //}
            

        public DataTableSuccessMessage Customer()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string fname = HttpContext.Current.Request.Form["searchFName"];
                string lname = HttpContext.Current.Request.Form["searchLName"];
                string gender = HttpContext.Current.Request.Form["searchGender"];
                string searchAddress = HttpContext.Current.Request.Form["searchAddress"];
                var customers =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>()
                        .PagedSearchByNameAndAddress(fname, lname, gender, searchAddress, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    customers.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                FIRSTName = y.FirstName,
                                LASTName = y.LastName,
                                OTHERNames = y.OtherNames,
                                ViewDetails =
                                    @"<button type='button' onclick=""viewDetails('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">View Details</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public CustomerDetail CustomerDetails(int id)
        {
            return new CustomerDetail(Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>()
                .RetrieveById(id)); 

        }

        public DataTableSuccessMessage GlCategory()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string name = HttpContext.Current.Request.Form["searchName"];
                string glcategory = HttpContext.Current.Request.Form["searchGlAcctCategory"];
                var glCategory =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>()
                        .PagedSearchByNameAndGlAcctCategory(name, glcategory, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    glCategory.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                GLCATEGORYName = y.GlCategoryName,
                                MAINACCOUNTCategory = y.MainAccountCategory.ToString(),
                                ViewDetails =
                                    @"<button type='button' onclick=""viewDetails('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">View Details</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public GlCategoryDetail GlCategoryDetails(int id)
        {
            return new GlCategoryDetail(Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>().RetrieveById(id));

        }

        public DataTableSuccessMessage GlAccount()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchGlName = HttpContext.Current.Request.Form["searchGLName"];
                string searchGlCode = HttpContext.Current.Request.Form["searchGLCode"];
                string searchGlBranch = HttpContext.Current.Request.Form["searchGLBranch"];
                string searchCategory = HttpContext.Current.Request.Form["searchCategory"];
                var glAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                        .PagedSearchByGLNameAndCategory(searchGlName, searchGlCode, searchGlBranch, searchCategory, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    glAccount.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                AccountName = y.GlAccountName,
                                AccountCode = y.GlAccountCodes,
                                Category = y.GlCategory.GlCategoryName,
                                Branch = y.Branch.BranchName,
                                BAlance = y.Balance,
                                Edit =
                                    @"<button type='button' onclick=""edit('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">Edit</button>"
                            }).ToArray();

                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public GlAccount GlAccountDetails(int id)
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                .RetrieveById(id);

        }

        public DataTableSuccessMessage Tellers()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchGLAccountName = HttpContext.Current.Request.Form["searchGLAccountName"];
                string searchUser = HttpContext.Current.Request.Form["searchUser"];
                string searchlName = HttpContext.Current.Request.Form["searchLName"];
                var tellers =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                        .PagedSearchByName(searchUser, searchGLAccountName, searchlName, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    tellers.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                FIRSTName = y.User.FirstName,
                                LASTName = y.User.LastName,
                                TILLAccount = y.GlAccount.GlAccountName,
                                ViewDetails =
                                    @"<button type='button' onclick=""viewDetails('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">View Details</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public Teller TellerDetails(int id)
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                .RetrieveById(id);

        }

        public DataTableSuccessMessage CustomerAccounts()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string name = HttpContext.Current.Request.Form["searchName"];
                string searchAddress = HttpContext.Current.Request.Form["searchAddress"];
                var customers =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                        .PagedSearchByFirstNameAndLastName(name, searchAddress, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    customers.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                FIRSTName = y.FirstName,
                                LASTName = y.LastName,
                                OTHERNames = y.OtherNames,
                                EMail = y.Email,
                                GENder = y.Gender.ToString(),
                                AddAccounts =
                                    @"<button type='button' onclick=""Redirect('" + y.Id +
                                    @"');"" class=""btn btn-info btn-xs"">Add Account</button>"
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public DataTableSuccessMessage LoanAccounts()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string name = HttpContext.Current.Request.Form["searchName"];
                string lduration = HttpContext.Current.Request.Form["searchLDuration"];
                string ptype = HttpContext.Current.Request.Form["searchPType"];
                string searchAddress = HttpContext.Current.Request.Form["searchBranch"];
                var loan =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                        .PagedSearchByNameAndAcctNumber(name, lduration, ptype, searchAddress, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    loan.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                ACctName = y.AccountName,
                                ACctNo = y.AccountNumber,
                                AcctType = Enum.GetName(typeof(AccountType), y.LinkedAccount.AccountType),
                                AMnt = y.LoanAmount,
                                Duration = y.LoanDuration,
                                Interest = y.LoanInterest,
                                BAlance = y.Balance,
                                StartDate = y.LoanStartDate,
                                DueDate = y.LoanDueDate,
                                PaymentType = Enum.GetName(typeof(PaymentSchedule),y.PaymentSchedule),
                                NoOFDays = y.NumberOfDays,
                                Status = Enum.GetName(typeof(LoanStatus),y.LoanStatus)
                            }).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public CustomerAccountDetail CustomerAccountDetails(int id)
        {
            return new CustomerAccountDetail(Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveById(id));
        }

        //public DataTableSuccessMessage CustomerLoanAccount()
        //{
        //    try
        //    {
        //        int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
        //        int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
        //        int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
        //        int total;
        //        //string search = HttpContext.Current.Request.Form["search[value]"];
        //        string searchAccountNumber = HttpContext.Current.Request.Form["searchAccountNumber"];
        //        string searchAccountName = HttpContext.Current.Request.Form["searchAccountName"];
        //        var customersAccounts =
        //            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountDb>()
        //                .PagedSearchByAccountNameAndAccountNumber(searchAccountName, searchAccountNumber, start, length,
        //                    out total);
        //        //add button

        //        var result = new DataTableSuccessMessage();
        //        result.draw = draw;
        //        result.recordsFiltered = customersAccounts.Count;
        //        result.recordsTotal = total;
        //        result.data =
        //            customersAccounts.Select(
        //                y =>
        //                    new
        //                    {
        //                        ID = y.Id,
        //                        AccountNumber = y.AccountNumber,
        //                        AcccountName = y.CustomerAccountName,
        //                        AccountType = y.AccountType.ToString(),
        //                        Branch = y.Branch.Name,
        //                        Balance = y.Balance,
        //                        LinkToLoanAccount =
        //                            @"<button type='button' onclick=""linkAccount('" + y.Id +
        //                            @"');"" class=""btn btn-info btn-xs"">Link Account to Loan</button>"
        //                    }).ToArray();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DataTableSuccessMessage()
        //        {
        //            error = ex.Message
        //        };
        //    }
        //}

        public DataTableSuccessMessage CustomerAccountForView()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string name = HttpContext.Current.Request.Form["searchName"];
                string type = HttpContext.Current.Request.Form["searchType"];
                string branch = HttpContext.Current.Request.Form["searchBranch"];
                string searchAccountNumber = HttpContext.Current.Request.Form["searchAddress"];
                var customerAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                        .PagedSearchByNameAndAccountNumber(name, type, branch, searchAccountNumber, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                for (int i = 0; i < customerAccount.Count; i++)
                {
                    if (customerAccount[i].IsClosed)
                    {
                        result.data =
                            customerAccount.Select(
                                y =>
                                    new
                                    {
                                        ID = y.Id,
                                        ACCountNumber = y.AccountNumber,
                                        ACCountName = y.AccountName,
                                        ACCountType = y.AccountType.ToString(),
                                        BAlance = y.Balance,
                                        ViewDetails =
                                            @"<button type='button' onclick=""viewDetails('" + y.Id +
                                            @"');"" class=""btn btn-info btn-xs"">View Details</button>",
                                            CloseAccount = y.IsClosed ?
                                        @"<button type='button' id ='close' disabled='disabled' class =""btn btn-danger btn-xs"">Closed</button>"
                                        : @"<button type='button' onclick=""closeAccount('" + y.Id +
                                        @"');"" class=""status-" +
                                        y.Id + @" btn btn-danger btn-xs"">Close Account</button>",
                                            OpenAccount = y.IsClosed == false ?
                                   @"<button type='button' disabled='disabled' class =""btn btn-info btn-xs"">Open</button>"
                                   : @"<button type='button' onclick=""openAccount('" + y.Id +
                                   @"');"" class=""status-" +
                                   y.Id + @" btn btn-info btn-xs"">Open Account</button>"
                                        
                                            //@"<button type='button' onclick=""closeAccount('" + y.Id +
                                            //@"');"" class=""status-" +
                                            //y.Id + @" btn btn-danger btn-xs"">Closed</button>"
                                    }).ToArray();
                    }
                    // $('.status-' + id).html('Closed').addClass('btn-danger').removeClass('btn-info');
                    else
                    {
                        result.data =
                           customerAccount.Select(
                               y =>
                                   new
                                   {
                                       ID = y.Id,
                                       ACCountNumber = y.AccountNumber,
                                       ACCountName = y.AccountName,
                                       ACCountType = y.AccountType.ToString(),
                                       BAlance = y.Balance,
                                       ViewDetails =
                                           @"<button type='button' onclick=""viewDetails('" + y.Id +
                                           @"');"" class=""btn btn-info btn-xs"">View Details</button>",
                                           CloseAccount = y.IsClosed ?
                                        @"<button type='button' disabled='disabled' class =""btn btn-danger btn-xs"">Closed</button>"
                                        : @"<button type='button' onclick=""closeAccount('" + y.Id +
                                        @"');"" class=""status-" +
                                        y.Id + @" btn btn-danger btn-xs"">Close Account</button>",

                                       OpenAccount = y.IsClosed == false ?
                                   @"<button type='button' disabled='disabled' class =""btn btn-info btn-xs"">Open</button>"
                                   : @"<button type='button' onclick=""openAccount('" + y.Id +
                                   @"');"" class=""status-" +
                                   y.Id + @" btn btn-info btn-xs"">Open Account</button>"

                                       //CloseAccount =
                                       //     @"<button type='button' onclick=""closeAccount('" + y.Id +
                                       //     @"');"" class=""status-" +
                                       //     y.Id + @" btn btn-danger btn-xs"">Close Account</button>"
                                       
                                   }).ToArray();
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public bool CloseAccount(int id)
        {
            var customerAccount =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                    .RetrieveById(id);
            customerAccount.IsClosed = true;
            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                .UpdateData(customerAccount);
            return true;
        }

        public bool OpenAccount(int id)
        {
            var customerAccount =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                    .RetrieveById(id);
            customerAccount.IsClosed = false;
            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                .UpdateData(customerAccount);
            return false;
        }

        
        public DataTableSuccessMessage GlPosting()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchGlName = HttpContext.Current.Request.Form["searchGLName"];
                string searchCode = HttpContext.Current.Request.Form["searchGLCode"];
                ////string searchDate = HttpContext.Current.Request.Form["searchDate"];
                var glPosting =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                        .PagedSearchByGLNameAndCode(searchGlName, searchCode, start, length, out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    glPosting.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                ActualDate = y.DateAdded,
                                TRansactionDate = y.TransactionDate,
                                DebitAccountName = y.GlAccountToDebit.GlAccountName,
                                DebitAccountCode = y.GlAccountToDebit.GlAccountCodes,
                                CreditAccountName = y.GlAccountToCredit.GlAccountName,
                                CreditAccountCode = y.GlAccountToCredit.GlAccountCodes,
                                DEbitNarration = y.DebitNarration,
                                CReditNarration = y.CreditNarration,
                                AMount = y.Amount
                            }).ToArray();

                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        


        public DataTableSuccessMessage TellerPosting()
        {
            try
            {
                int draw = Convert.ToInt32(HttpContext.Current.Request.Form["draw"]);
                int start = Convert.ToInt32(HttpContext.Current.Request.Form["start"]);
                int length = Convert.ToInt32(HttpContext.Current.Request.Form["length"]);
                int total, querytotal;
                //string search = HttpContext.Current.Request.Form["search[value]"];
                string searchAccountName = HttpContext.Current.Request.Form["searchCustomerAccountName"];
                string searchTillAccountName = HttpContext.Current.Request.Form["searchTillAccountName"];
                string searchAccountNumber = HttpContext.Current.Request.Form["searchAccountNumber"];
                DateTime searchDate = Convert.ToDateTime(HttpContext.Current.Request.Form["searchDate"]);
                var tellerPosting =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerPostingDb>()
                        .PagedSearchByAccountNameAndAccountNumber(searchAccountName, searchTillAccountName, searchAccountNumber, searchDate, start, length,
                           out querytotal, out total);
                //add button

                var result = new DataTableSuccessMessage();
                result.draw = draw;
                result.recordsFiltered = querytotal;
                result.recordsTotal = total;
                result.data =
                    tellerPosting.Select(
                        y =>
                            new
                            {
                                ID = y.Id,
                                TRansactionDate = y.TransactionDate,
                                CustomerAccountName = y.CustomerAccounts.AccountName,
                                CustomerAccountNumber = y.CustomerAccounts.AccountNumber,
                                TillAccountName = y.GlAccount.GlAccountName,
                                PostingType = Enum.GetName(typeof(PostingType),y.PostingType),
                                NArration = y.Narration,
                                AMount = y.Amount
                            }).ToArray();

                return result;
            }
            catch (Exception ex)
            {
                return new DataTableSuccessMessage()
                {
                    error = ex.Message
                };
            }
        }

        public IList<GlAccount> TrialBalance()
        {
            var GlAccounts =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveAll();
            return GlAccounts;
        }

        public IList<GlPosting> TrialBalanceFromPostings()
        {
            DateTime date = Convert.ToDateTime(HttpContext.Current.Request.Form["date"]);
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                .RetrievePostingToDate(date);
        }

        public IList<KeyValuePair<GlAccount, double>> TrialBalanceContent()
        {
            IList<KeyValuePair<GlAccount, double>> values = new List<KeyValuePair<GlAccount, double>>();
            DateTime date = Convert.ToDateTime(HttpContext.Current.Request.Form["date"]);
            IList<GlAccount> glAccounts =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveAll();
            IList<GlPosting> glPostings =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                    .RetrievePostingToDate(date);
            
            for (int i = 0; i < glAccounts.Count; i++)
            {
                double totalAmount = 0;
                for (int j = 0; j < glPostings.Count; j++)
                {
                    if (glAccounts[i].Id == glPostings[j].GlAccountToDebit.Id)
                    {
                        if (glAccounts[i].GlAccountCodes.StartsWith("1") || glAccounts[i].GlAccountCodes.StartsWith("5"))
                        {
                            totalAmount += glPostings[j].Amount;
                            totalAmount = Math.Round(totalAmount, 2);
                        }
                        else if (glAccounts[i].GlAccountCodes.StartsWith("2") ||
                                 glAccounts[i].GlAccountCodes.StartsWith("3") ||
                                 glAccounts[i].GlAccountCodes.StartsWith("4"))
                        {
                            totalAmount -= glPostings[j].Amount;
                            totalAmount = Math.Round(totalAmount, 2);
                        }

                        
                    }
                    else if (glAccounts[i].Id == glPostings[j].GlAccountToCredit.Id)
                    {
                        if (glAccounts[i].GlAccountCodes.StartsWith("1") || glAccounts[i].GlAccountCodes.StartsWith("5"))
                        {
                            totalAmount -= glPostings[j].Amount;
                            totalAmount = Math.Round(totalAmount, 2);
                        }
                        else if (glAccounts[i].GlAccountCodes.StartsWith("2") ||
                                 glAccounts[i].GlAccountCodes.StartsWith("3") ||
                                 glAccounts[i].GlAccountCodes.StartsWith("4"))
                        {
                            totalAmount += glPostings[j].Amount;
                            totalAmount = Math.Round(totalAmount, 2);
                        }
                      
                    }
                   
                }
               
                var value = new KeyValuePair<GlAccount, double>(glAccounts[i], totalAmount);
                values.Add(value);
            }

            return values;
        }



        public IList<GlAccount> ProfitOrLoss()
        {
            var GlAccounts =
                   Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveAll();
            return GlAccounts;
        }

        string GetNodeString(SiteMapNode node)
        {
            string x = string.Empty;
            //if ((_user.UserRole == Role.Admin && node.Roles != null &&
            //     (node.Roles.Contains("*") || node.Roles.Contains("Admin"))) ||
            //    (_user.UserRole == Role.Teller && node.Roles != null &&
            //     (node.Roles.Contains("*") || node.Roles.Contains("Teller"))))
            
                x = string.Format(@"<a href='{0}'>
                <i class='fa {1}'></i> <span>{2}</span>", node.Url, node.ResourceKey, node.Title);

                if (node.HasChildNodes)
                {
                    x += string.Format(@"<i class='fa fa-angle-left pull-right'></i>");
                }

                x += string.Format(@"</a>");
            
            return x;
        }

        public string GenerateMenuLinks()
        {
            string searchMEnu = HttpContext.Current.Request.Form["searchmenu"];
            StringBuilder sideMenuText = new StringBuilder("");

            if (!String.IsNullOrWhiteSpace(searchMEnu))
            {
                foreach (SiteMapNode childNode in SiteMap.RootNode.ChildNodes)
                {
                    if (childNode.Title.ToLower().Contains(searchMEnu.ToLower()))
                    {
                        sideMenuText.Append("<li class='treeview'>");
                        sideMenuText.Append(GetNodeString(childNode));
                        sideMenuText.Append("<ul class='treeview-menu'>");
                        foreach (SiteMapNode child in childNode.ChildNodes)
                        {
                            //if ((_user.UserRole == Role.Admin && child.Roles != null &&
                            //     (child.Roles.Contains("*") || child.Roles.Contains("Admin"))) ||
                            //    (_user.UserRole == Role.Teller && child.Roles != null &&
                            //     (child.Roles.Contains("*") || child.Roles.Contains("Teller"))))

                            sideMenuText.Append("<li>");
                            sideMenuText.Append(GetNodeString(child));
                            sideMenuText.Append("</li>");

                        }
                        sideMenuText.Append("</ul>");
                        sideMenuText.Append("</li>");
                    }
                    
                }
                return sideMenuText.ToString();
            }

            else
            {
                foreach (SiteMapNode childNode in SiteMap.RootNode.ChildNodes)
                {
                    sideMenuText.Append("<li class='treeview'>");
                    sideMenuText.Append(GetNodeString(childNode));
                    sideMenuText.Append("<ul class='treeview-menu'>");
                    foreach (SiteMapNode child in childNode.ChildNodes)
                    {
                        //if ((_user.UserRole == Role.Admin && child.Roles != null &&
                        //     (child.Roles.Contains("*") || child.Roles.Contains("Admin"))) ||
                        //    (_user.UserRole == Role.Teller && child.Roles != null &&
                        //     (child.Roles.Contains("*") || child.Roles.Contains("Teller"))))

                        sideMenuText.Append("<li>");
                        sideMenuText.Append(GetNodeString(child));
                        sideMenuText.Append("</li>");

                    }
                    sideMenuText.Append("</ul>");
                    sideMenuText.Append("</li>");
                }
                return sideMenuText.ToString();
            }
            
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class DataTableSuccessMessage
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }
        public string error { get; set; }
    }

}