using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Wpf.Helper
{
    public class Regedit
    {
        public void Write(string key, string value)
        {
            RegistryKey masterKey = Registry.LocalMachine.OpenSubKey("SOFTWARE",true);
            RegistryKey newkey = masterKey.CreateSubKey("StoneAnt");
            try
            {
                newkey.SetValue(key, value);
            }
            catch(Exception)
            {
                Console.WriteLine("Regedit Exception");
            }
            finally
            {
                masterKey.Close();
            }
        }

        public string Read(string key)
        {
            string result = "";
            RegistryKey masterKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            RegistryKey newkey = masterKey.CreateSubKey("StoneAnt");
            try
            {
                result = newkey.GetValue(key).ToString();
            }
            catch(Exception)
            {
                
            }
            finally
            {
                masterKey.Close();
            }
            return result;
        }
    }
}
