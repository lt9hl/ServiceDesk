using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace ServiceDesk.ApplicationData
{
    public class DataSecurity
    {
        protected static string password = "";
        
        public static string selectPasswordAction(string inputPassword)
        {
            password = inputPassword;
            if((App.Current as App).actionWithPassword == passwordActionsEnum.encrypt)
            {
                return EncryptString();
            }
            else
            {
                return DecryptString();
            }
        }
        protected static string EncryptString()
        {
            string encryptedString = "";
            int passwordOperator = password.Length;
            for (int i = 0; i < password.Length; i++)
            {
                encryptedString += Convert.ToString((int)(char)password[i] * password.Length);
                if (i != password.Length - 1)
                {
                    encryptedString += ".";
                }
            }

            return encryptedString;
        }
        public static string DecryptString()
        {
            int passwordOperator = password.Count(x => x == '.') + 1;
            var list = password.Split('.');
            var decryptedString = "";

            for (int i = 0; i < list.Length; i++)
            {
                decryptedString += (char)(Convert.ToInt32(list[i]) / passwordOperator);
            }

            return decryptedString;
        }
    }
}
