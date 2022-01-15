using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.OleDb;
namespace WindowsFormsApp2
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        public Socket socket;
        string result = "";
        string name = "";

        public void listenServer()
        {
            while (true)
            {
                while (socket.Connected)
                {
                    byte[] forpass = new byte[1024];
                    socket.Receive(forpass);
                    string message = Encoding.UTF8.GetString(forpass);
                    string[] words = message.Split(new char[] { '$' });
                    Thread.Sleep(100);
                    if (words[0] == "Заходи")
                    {
                        result = words[1];
                        name = words[2];
                    }
                    Thread.Sleep(100);

                }
                
                Application.Exit();
            }
        }

     
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string message = "Авторизация:" + "*" + textBox1.Text + "*" + textBox2.Text + "*";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                Thread.Sleep(100);
                socket.SendTo(buffer, socket.RemoteEndPoint);
                string[] words = message.Split(new char[] { '*' });
            }
            catch { }
            Thread.Sleep(5000);



            if (result != "")
            {
                Form2 a = new Form2();
                a.ID = result;
                a.name = name;
                a.Show();
                this.Close();
            }
            else
            {
                
            }
            
            Thread.Sleep(100);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 b = new Form4();
            b.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form5_Load_1(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.0.0.1", 815);
            Thread listen = new Thread(listenServer);
            listen.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
