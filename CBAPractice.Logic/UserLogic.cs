using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Logic
{
    public class UserLogic
    {
        public string CreatePassword()
        {
            string password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
            return password;
        }

        public string EncryptPassword(string pass)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
            byte[] result = md5.Hash;
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }
            return str.ToString();
        }

        public void SendMail(string userEmail, string userPassword)
        {
            string strFromEmail = "dipe4real@gmail.com";
            string strToEmail = userEmail;
            string strSubject = "Your First Time Password has been sent";

            MailMessage MailMsg = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");

            MailMsg.From = new MailAddress(strFromEmail);
            MailMsg.Subject = strSubject;

            MailMsg.To.Add(strToEmail);

            MailMsg.Body = "Your New Password is " + userPassword +".";
            MailMsg.Body += "  You Must Change Your Password After First Login by Going to the 'Change Password' Page under the 'ManageUsers' Section. ";


            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("dipe4real@gmail.com", "tiwatope");
            smtp.EnableSsl = true;
            smtp.Send(MailMsg);
        }
    }
}
