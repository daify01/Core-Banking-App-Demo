using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CBAPractice.Core;
using CBAPractice.Data;

namespace FEP.WindowsService
{
    public class APIinstantiator
    {
        private string LocalhostAddress = ConfigurationSettings.AppSettings["LocalHostAddress"];
        public CustomerAccounts GetCustomerAccountsByCategory(string Accountnumber)
        {
            CustomerAccounts customerAccounts = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Customeraccounts/GetByAccountNumber/?accountnumber=" + Accountnumber).Result;
                if (response.IsSuccessStatusCode)
                {
                    customerAccounts = response.Content.ReadAsAsync<CustomerAccounts>().Result;
                    return customerAccounts;
                   
                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
           return customerAccounts;
        }

        public void UpdateCustomerAccounts(string REQUESTURI, CustomerAccounts customerAccounts)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PutAsJsonAsync(
                    REQUESTURI,
                    customerAccounts).Result;
            }
        }

        public void UpdateGlAccounts(string REQUESTURI, GlAccount glAccount)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PutAsJsonAsync(
                    REQUESTURI,
                    glAccount).Result;
            }
        }

        public void UpdatePostings(string REQUESTURI, GlPosting glPosting)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PutAsJsonAsync(
                    REQUESTURI,
                    glPosting).Result;
            }
        }

        public void InsertGlPostings(string REQUESTURI, GlPosting glPosting)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(
                    REQUESTURI,
                    glPosting).Result;
            }
        }

        public IList<CurrentConfig> GetAllCurrentConfigs()
        {
            IList<CurrentConfig> currentConfigs = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Currentconfig/GetAllCurrentConfigs").Result;
                if (response.IsSuccessStatusCode)
                {
                    currentConfigs = response.Content.ReadAsAsync<IList<CurrentConfig>>().Result;
                    return currentConfigs;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return currentConfigs;
        }

        public CurrentConfig GetByCurrentConfigBranch(int id)
        {
            CurrentConfig currentConfigs = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Currentconfig/GetByBranch/?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    currentConfigs = response.Content.ReadAsAsync<CurrentConfig>().Result;
                    return currentConfigs;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return currentConfigs;
        }


        public IList<SavingsConfig> GetAllSavingsConfigsConfigs()
        {
            IList<SavingsConfig> savingsConfigs = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Savingsconfig/GetAllSavingsConfigs").Result;
                if (response.IsSuccessStatusCode)
                {
                    savingsConfigs = response.Content.ReadAsAsync<IList<SavingsConfig>>().Result;
                    return savingsConfigs;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return savingsConfigs;
        }

        public IList<GlPosting> GetByOriginalDataElement(string originaldataElement)
        {
            IList<GlPosting> glPostings = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Glposting/GetByOriginalDataElement/?originaldataelement=" + originaldataElement).Result;
                if (response.IsSuccessStatusCode)
                {
                    glPostings = response.Content.ReadAsAsync<IList<GlPosting>>().Result;
                    return glPostings;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return glPostings;
        }

        public SavingsConfig GetBySavingsConfigBranch(int id)
        {
            SavingsConfig savingsConfig = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Savingsconfig/GetByBranch/?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    savingsConfig = response.Content.ReadAsAsync<SavingsConfig>().Result;
                    return savingsConfig;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return savingsConfig;
        }

        public GlAccount GetByGlAccountName(string accountname)
        {
            GlAccount glAccount = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Glaccount/GetByAccountname/?accountname=" + accountname).Result;
                if (response.IsSuccessStatusCode)
                {
                    glAccount = response.Content.ReadAsAsync<GlAccount>().Result;
                    return glAccount;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return glAccount;
        }

        public GlAccount GetByGlAccountCode(string accountcode)
        {
            GlAccount glAccount = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Glaccount/GetByAccountCode/?Accountcode=" + accountcode).Result;
                if (response.IsSuccessStatusCode)
                {
                    glAccount = response.Content.ReadAsAsync<GlAccount>().Result;
                    return glAccount;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return glAccount;
        }

        public IList<EOD> GetAllEods()
        {
            IList<EOD> eods = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Eod/GetAllEods").Result;
                if (response.IsSuccessStatusCode)
                {
                    eods = response.Content.ReadAsAsync<IList<EOD>>().Result;
                    return eods;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return eods;
        }

        public bool IsMyTerminal(string myterminal)
        {
            bool confirm = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Mybankterminal/GetATMTerminal/?terminalfromSWITCH=" + myterminal).Result;
                if (response.IsSuccessStatusCode)
                {
                    confirm = response.Content.ReadAsAsync<bool>().Result;
                    return confirm;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return confirm;
        }

        public bool GLAccountBalanceConfirmed(int glAccountid, double amount)
        {
            bool confirm = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Tellerpostinglogic/GetGLAccountBalanceConfirmed/?glAccountid=" + glAccountid + "&amount=" + amount).Result;
                if (response.IsSuccessStatusCode)
                {
                    confirm = response.Content.ReadAsAsync<bool>().Result;
                    return confirm;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return confirm;
        }

        public bool CustomerSavingsAccountBalanceConfirmed(int customerAccountsid, double amount)
        {
            bool confirm = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Tellerpostinglogic/GetCustomerSavingsAccountBalanceConfirmed/?customerAccountsid=" + customerAccountsid + "&amount=" + amount).Result;
                if (response.IsSuccessStatusCode)
                {
                    confirm = response.Content.ReadAsAsync<bool>().Result;
                    return confirm;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return confirm;
        }

        public bool CustomerCurrentAccountBalanceConfirmed(int customerAccountsid, double amount)
        {
            bool confirm = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Tellerpostinglogic/GetCustomerCurrentAccountBalanceConfirmed/?customerAccountsid=" + customerAccountsid + "&amount=" + amount).Result;
                if (response.IsSuccessStatusCode)
                {
                    confirm = response.Content.ReadAsAsync<bool>().Result;
                    return confirm;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return confirm;
        }

        public double CustomerAccountWithdrawal(int customerAccountsid, double amount)
        {
            double balance = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Tellerpostinglogic/GetCustomerAccountWithdrawal/?customerAccountsid=" + customerAccountsid + "&amount=" + amount).Result;
                if (response.IsSuccessStatusCode)
                {
                    balance = response.Content.ReadAsAsync<double>().Result;
                    return balance;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return balance;
        }

        public double GlAccountWithdrawal(int glAccountid, double amount)
        {
            double balance = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LocalhostAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = client.GetAsync("api/Tellerpostinglogic/GetGlAccountWithdrawal/?glAccountid=" + glAccountid + "&amount=" + amount).Result;
                if (response.IsSuccessStatusCode)
                {
                    balance = response.Content.ReadAsAsync<double>().Result;
                    return balance;

                }
                else
                {
                    string message = response.ReasonPhrase;
                }
            }
            return balance;
        }
    }
}
