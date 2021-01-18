using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PracticaServerLibros
{
    public partial class Principal : Form
    {
        public Principal()
        {
            
            InitializeComponent();

            Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }
        

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Principal_Load(object sender, EventArgs e)
        {
            
        }

        private List<Books> GetBooks()
        {
            var list = new List<Books>();
            list.Add(new Books() { nombre = "primero", fechaentrada = "20/05/2020"});
            list.Add(new Books() { nombre = "segundo", fechaentrada = "20/05/2020" });

            return list;
        }

        private void Principal_Shown(object sender, EventArgs e)
        {
            
            
        }
        public void Connect()
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("localhost", 8070);
            Byte[] senddata = Encoding.ASCII.GetBytes("Select * from Books3 where usuario = 'omar'");
            udpClient.Send(senddata, senddata.Length);
            udpClient.Close();
            UdpClient udpClient2 = new UdpClient(8000);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receiveBytes = udpClient2.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);
            Console.WriteLine(returnData);

            var libros = JsonConvert.DeserializeObject<List<Books>>(returnData);
            listView1.Items.Clear();
            foreach(var x in libros)
            {
                var row = new string[] {x.nombre, x.fechasalida};
                var lvi = new ListViewItem(row);
                lvi.Tag = x;
                listView1.Items.Add(lvi);
            }
        }
    }
}
