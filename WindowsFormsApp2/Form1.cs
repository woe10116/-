using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;


namespace WindowsFormsApp2
{




	public partial class Form1 : Form
	{
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientsList = new List<Socket>();
        public Socket client;
        int ID = 0;
        string name = "";
        string password = "";
        public string hismessage = "";
        public string term = "";
        public void fornewthread()
        {
           
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ipAddress, 815);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptSocket();
                clientsList.Add(client);
                Thread messageAccept = new Thread(() =>
                {

                    Invoke(new Action(() => textBox2.Text += "Подключается с  " + client.RemoteEndPoint));
                    Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                    byte[] buffer = new byte[1024];
                    while (client.Connected)
                    {
                        string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + " ";
                        Thread.Sleep(100);
                        client.Receive(buffer);
                        string message = Encoding.UTF8.GetString(buffer);
                        string[] words = message.Split(new char[] { '*' });

                        string[] reg = words[0].Split(' ');
                        Console.WriteLine(reg);
                        if (reg[0] == "Регистрация:")
                        {
                            using (var cn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = Database1.accdb "))
                            {
                                cn.Open();
                                using (OleDbCommand Number = cn.CreateCommand())
                                {
                                    Number.CommandText = "INSERT INTO Авторизация ([NickName],[Login],[Password]) values" +
                                    " ('" + reg[1] + "','" + reg[2] + "','" + reg[3] + "');";
                                    int numberOfUpdatedItems = Number.ExecuteNonQuery();

                                }
                                cn.Close();
                            }

                            MessageBox.Show("Вы успешно зарегистрировались");
                        }

                        if (words[0] == "Авторизация:")

                        {

                            using (OleDbConnection cn = new OleDbConnection())
                            {
                                cn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = Database1.accdb";
                                {
                                    try
                                    {

                                        cn.Open();

                                        string strSQL = "SELECT Авторизация.Password, Авторизация.ID_p, Авторизация.Nickname FROM Авторизация WHERE Авторизация.Login ='" + words[1] + "'";
                                        OleDbCommand myCommand = new OleDbCommand(strSQL, cn);
                                        OleDbDataReader dr = myCommand.ExecuteReader();
                                        while (dr.Read())
                                        {
                                            password = dr[0].ToString();
                                            ID = Convert.ToInt32(dr[1]);
                                            name = dr[2].ToString();
                                        }
                                    }
                                    catch (OleDbException ex)
                                    {


                                        Console.WriteLine(ex.Message);
                                    }
                                    finally
                                    {

                                        cn.Close();
                                    }
                                }
                                if (password == words[2])
                                {
                                    string ok = "Заходи" + "$" + ID + "$" + name + "$";
                                    byte[] forpass = Encoding.UTF8.GetBytes(ok);
                                    client.Send(forpass);

                                }
                                else
                                {
                                    string ok = "Нет" + "$";
                                    byte[] forpass = Encoding.UTF8.GetBytes(ok);
                                    client.Send(forpass);
                                }

                            }
                        }


                        if (words[0] == "История")
                        {


                            using (OleDbConnection cn = new OleDbConnection())
                            {
                                cn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = Database1.accdb";
                                {
                                    try
                                    {

                                        cn.Open();

                                        string strSQL = "SELECT Авторизация.Nickname, MessageHistory.message, MessageHistory.term FROM Авторизация, MessageHistory WHERE Авторизация.ID_p = MessageHistory.Nickname_ID";
                                        OleDbCommand myCommand = new OleDbCommand(strSQL, cn);
                                        OleDbDataReader dr = myCommand.ExecuteReader();
                                        while (dr.Read())
                                        {
                                            name = dr[0].ToString();
                                            hismessage = dr[1].ToString();
                                            term = dr[3].ToString();
                                        }
                                    }
                                    catch (OleDbException ex)
                                    {


                                        Console.WriteLine(ex.Message);
                                    }
                                    finally
                                    {

                                        cn.Close();
                                    }
                                }
                              

                            }
                        }
                    
                

                    if (words[0] == "Сообщения:")
                        {
                            using (var cn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = Database1.accdb "))
                            {
                                cn.Open();
                                using (OleDbCommand Number = cn.CreateCommand())
                                {
                                    Number.CommandText = "INSERT INTO [MessageHistory] ([Nickname_ID],[Message],[Term]) values" +
                                        " ('" + words[1] + "','" + words[3] + "','" + words[2] + "');";
                                    int numberOfUpdatedItems = Number.ExecuteNonQuery();
                                }
                                cn.Close();
                            }
                        }
                        
                    
                        Invoke(new Action(() => textBox2.Text += client.RemoteEndPoint + " "));
                        Invoke(new Action(() => textBox2.Text += time + message));
                        Invoke(new Action(() => textBox2.Text += Environment.NewLine));
                        
                        Thread.Sleep(100);
                        foreach (Socket clint in clientsList)
                        {
                            clint.Send(buffer);
                        }
                        buffer = new byte[1024];
                        message = Encoding.UTF8.GetString(buffer);
                    }
                    clientsList.Remove(client);
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                });
                messageAccept.Start();
            }
        }
        public Form1()
		{
			InitializeComponent();
		}

        

        private void Button1_Click(object sender, EventArgs e)
		{
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            socket.Bind(new IPEndPoint(IPAddress.Any, 815));
            Thread mythread = new Thread(fornewthread);
			mythread.Start();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

}


