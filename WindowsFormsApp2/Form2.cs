using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp2
{
	public partial class Form2 : Form
	{
        public Form2()
        {
            InitializeComponent();
        }
       
        public Socket socket;
        public List<dynamic> list = new List<dynamic>();
        public string ID = "";
        public string name = "";
        string hismessage = "";
       
        public void fornewthread()
        {
            while (true)
            {
                while (socket.Connected)
                {

                    
                    byte[] buffer = new byte[1024];
                    socket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    string[] message1 = message.Split(new char[] { '*' });
                    Thread.Sleep(100);
                    if (message1[0] == "Сообщения:")
                    {
                        Invoke(new Action(() => textBox2.Text += "Пользватель:" + message1[4] + Environment.NewLine + "Сообщение:" + message1[3]));
                        Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                        Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                    }
                    if (message1[0] == "История")
                    {
                        Invoke(new Action(() => textBox2.Text += "Здесь будут история сообщений"));
                    }
                }
                
                Application.Exit();
                
            }
        }

       
        
		private void Button1_Click(object sender, EventArgs e)
		{
            try
            {
                string time = DateTime.Now.ToString();
                string message = "Сообщения:" + "*" + ID + "*"+ time + "*" + textBox1.Text  + "*" + name + "*";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                Thread.Sleep(100);
                socket.SendTo(buffer, socket.RemoteEndPoint);
            }
            catch
            { }


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.0.0.1", 815);
            textBox3.Text = ID;
            textBox4.Text = name;
            Thread mythread1 = new Thread(fornewthread);
            mythread1.Start();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "Авторизация:" + "*";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                Thread.Sleep(100);
                socket.SendTo(buffer, socket.RemoteEndPoint);
            }
            catch
            { }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                string message = "История" + "*" + "name" + "*" + hismessage + "*" ;
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                Thread.Sleep(100);
                socket.SendTo(buffer, socket.RemoteEndPoint);
            }
            catch
            { }
        }
    }
}
