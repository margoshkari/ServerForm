using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ServerForm
{
    public partial class Form1 : Form
    {
        static ServerData serverData = new ServerData();
        static List<string> messages = new List<string>();

        static ListBox apps = new ListBox();
        static List<Button> icons = new List<Button>();
        static Button startBtn = new Button();
        public Form1()
        {
            InitializeComponent();

            

            apps.Size = new Size(300, 400);

            startBtn.Location = new Point(apps.Location.X, apps.Size.Height);
            startBtn.Text = "Start";
            startBtn.Size = new Size(apps.Size.Width, 50);
            startBtn.Click += StartBtn_Click;

            this.Controls.Add(apps);
            this.Controls.Add(startBtn);

            try
            {
                serverData.socket.Bind(serverData.iPEndPoint);
                serverData.socket.Listen(10);

                Task.Factory.StartNew(() => Connect());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if(serverData.socketClient != null)
            {
                messages = serverData.GetMsg().Split('*').ToList();
                AddAppsToList();
                ButtonLocation();
                AddIcon();
                AddBtnEvent();
                this.Controls.AddRange(icons.ToArray());
            }
        }

        static void Connect()
        {
            while (true)
            {
                serverData.socketClient = serverData.socket.Accept();
                serverData.socketClientsList.Add(serverData.socketClient);

                serverData.socketClient.Send(Encoding.Unicode.GetBytes("Welcome!"));
            }
        }
        static void AddAppsToList()
        {
            apps.Items.AddRange(messages.ToArray());
            for (int i = 0; i < messages.Count() - 1; i++)
            {
                icons.Add(new Button());
                icons[i].Name = messages[i];
                icons[i].Text = Path.GetFileName(messages[i]);
            }
        }
        static void ButtonLocation()
        {
            int x = 300, y = 0;
            for (int i = 0; i < icons.Count(); i++)
            {
                icons[i].Location = new Point(x += 50, y);
                icons[i].Size = new Size(50, 50);
                if((i + 1) % 8 == 0)
                {
                    x = 300;
                    y += 50;
                }
            }
        }
        static void AddIcon()
        {
            Icon ic;
            for (int i = 0; i < icons.Count(); i++)
            {
                ic = Icon.ExtractAssociatedIcon(icons[i].Name);
                icons[i].BackgroundImage = ic.ToBitmap();
                icons[i].BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        static void AddBtnEvent()
        {
            icons.ForEach(item => item.Click += Item_Click);
        }

        private static void Item_Click(object sender, EventArgs e)
        {
            serverData.socketClient.Send(Encoding.Unicode.GetBytes((sender as Button).Name));
        }
    }
}
