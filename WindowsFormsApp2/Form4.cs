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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public Socket socket;
        public void fornewthread()
        {
            while (true)
            {
                while (socket.Connected)
                {

                    byte[] buffer = new byte[1024];
                    socket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    Thread.Sleep(100);
                    

                }

                Application.Exit();

            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.0.0.1", 815);
            Thread mythread1 = new Thread(fornewthread);
            mythread1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "Регистрация:" + " " + textBox1.Text + " " + textBox2.Text + " " + textBox3.Text + " ";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            socket.SendTo(buffer, socket.RemoteEndPoint);
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 a = new Form5();
            a.Show();
            this.Close();
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
