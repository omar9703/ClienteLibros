using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace PracticaServerLibros
{
    public partial class Form2 : Form
    {
        List<Books> data;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("localhost", 8071);
            Byte[] senddata = Encoding.ASCII.GetBytes("Select * from Books3 where disponible = 'si'");
            udpClient.Send(senddata, senddata.Length);
            udpClient.Close();
            UdpClient udpClient2 = new UdpClient(8061);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receiveBytes = udpClient2.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            Console.WriteLine(returnData);
            udpClient2.Close();

            data = JsonConvert.DeserializeObject<List<Books>>(returnData);
            Console.WriteLine(data);
            checkedListBox1.Items.Clear();
            foreach (var t in data)
            {
                
                checkedListBox1.Items.Add(t.nombre);
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Books> y = new List<Books>();
            foreach(var s in checkedListBox1.CheckedItems)
            {
                foreach(var t in data)
                {
                    if(t.nombre == s.ToString())
                    {
                        Console.WriteLine("h");
                        y.Add(t);
                    }
                }
            }
            if (y.Count >0) {
                var results = JsonConvert.SerializeObject(y);
                UdpClient udpClient = new UdpClient();
                udpClient.Connect("localhost", 8071);
                Byte[] senddata = Encoding.ASCII.GetBytes(results);
                udpClient.Send(senddata, senddata.Length);
                udpClient.Close();
            }
            Close();
        }
    }
}
