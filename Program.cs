using System;
using System.Net.Sockets;
using System.Threading;
class Program
{
    static void Main(string[] args)
    {

        new Thread(() =>
        {
            //\u0033\u0001123456WIADOMOSC
            Thread.CurrentThread.IsBackground = true;
            Connect("127.0.0.1", "\u0011NICK12");
        }).Start();
        Console.ReadLine();
    }
    static void Connect(String server, String message)
    {
        try
        {
            Int32 port = 13000;
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();
            Byte[] data = new Byte[256];
            data = System.Text.Encoding.ASCII.GetBytes(message);
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
            Thread.Sleep(200);
            Byte[] tmp1 = new Byte[2];
            stream.Read(tmp1, 0, 2);
            // Console.WriteLine(tmp1);
            stream.FlushAsync();
            int count = 0;
            while (count++ < 10)
            {
                data = new Byte[256];
                Thread.Sleep(50);
                string message2 = "\u0033\u0001NICK12WIADOMOSC";
                Byte[] tmp = new Byte[256];
                data = new Byte[256];
                tmp = System.Text.Encoding.ASCII.GetBytes(message2);
                data[0] = (byte)message2.Length;
                System.Buffer.BlockCopy(tmp, 0, data, 1, tmp.Length);
                stream.Write(data, 0, data[0]);
                Thread.Sleep(100);
                data = new Byte[256];
                String response = String.Empty;
                // Read the Tcp Server Response Bytes.
                Byte[] tmp2 = new Byte[1];
                while (!stream.DataAvailable) ;
                stream.Read(tmp2, 0, 1);
                if (tmp2[0] != 0) // 0 bit is ping byte
                {
                    Int32 bytes = stream.Read(data, 0, tmp2[0]);
                    stream.Read(tmp1, 0, 2);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", response + "XD");
                }
                
                stream.FlushAsync();
                Thread.Sleep(250);
            }
            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
        }
        Console.Read();
    }
}