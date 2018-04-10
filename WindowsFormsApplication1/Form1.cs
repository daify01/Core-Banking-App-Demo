using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public string GeneratePassword()
        {
            string PasswordLength = "12";
            string NewPasword = "";
            string allowedChars = "";

            //allowedChars = "1,2,3,4,5,6,7,8,9,0";

            NewPasword = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
            return NewPasword;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strNewPassword = GeneratePassword();
            string strFromEmail = "dipe4real@gmail.com";
            string strToEmail = "sopeogund@gmail.com";
            string strSubject = "Your Password has been sent";

            if (!string.IsNullOrWhiteSpace(strNewPassword))
            {
                try
                {
                    MailMessage MailMsg = new MailMessage();
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                    MailMsg.From = new MailAddress(strFromEmail);
                    MailMsg.Subject = strSubject;

                    MailMsg.To.Add(strToEmail);

                    MailMsg.Body = "Your New Password is " + strNewPassword;

                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("dipe4real@gmail.com", "tiwatope");
                    smtp.EnableSsl = true;
                    smtp.Send(MailMsg);


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
