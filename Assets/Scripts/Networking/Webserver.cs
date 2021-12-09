using System;
using System.Collections.Generic;
using System.IO;
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;  
using UnityEngine;
  

public class Webserver : MonoBehaviour
{
    private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private const int bufSize = 8 * 1024;
    private State state = new State();
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv = null;

    public class State
    {
        public byte[] buffer = new byte[bufSize];
    }
    public void Start()
    {

        Debug.Log("http://" + NetworkHandler.GetLocalIPAddress().ToString()+ ":2000/android.apk");
        try
        {                
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(NetworkHandler.GetLocalIPAddress().ToString()), 2000));
            _socket.Listen(10);
            _socket.BeginAccept(AcceptCallback, _socket);                
        }
        catch(Exception ex)
        {
            throw new Exception("listening error" + ex);
        }
    }

    public void AcceptCallback(IAsyncResult ar)
    {
        try
        {
            Socket acceptedSocket = _socket.EndAccept(ar);               
            ClientController.AddClient(acceptedSocket);
            _socket.BeginAccept(AcceptCallback, _socket);
        }
        catch (Exception ex)
        {
            throw new Exception("Base Accept error"+ ex);
        }
    }

    public class ReceivePacket
    {
        private byte[] _buffer;
        private Socket _receiveSocket;
        private int _clientId;

        public ReceivePacket(Socket receiveSocket, int id)
        {
           _receiveSocket = receiveSocket;
           _clientId = id;
        }
        public void StartReceiving()
    {
        try
        {
            _buffer = new byte[4];
            _receiveSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
        }
        catch {}
    }

    private void ReceiveCallback(IAsyncResult AR)
    {
        try
        {
            // if bytes are less than 1 takes place when a client disconnect from the server.
            // So we run the Disconnect function on the current client

            int len = _receiveSocket.EndReceive(AR);

            if (len > 1)
            {
                // Convert the first 4 bytes (int 32) that we received and convert it to an Int32 (this is the size for the coming data).
                _buffer = new byte[1024];  
                // Next receive this data into the buffer with size that we did receive before
                _receiveSocket.Receive(_buffer, _buffer.Length, SocketFlags.None); 
                // When we received everything its onto you to convert it into the data that you've send.
                // For example string, int etc... in this example I only use the implementation for sending and receiving a string.

                // Convert the bytes to string and output it in a message box
                string data = Encoding.Default.GetString(_buffer);

                var fullPacket = new List<byte>();

                Byte[] bytes = File.ReadAllBytes("/var/www/html/android.apk");
                string header = "";

                header += "HTTP/1.1 200 OK\r\n";
                header += "Accept-Ranges: bytes\r\n";
                header += "Content-Length: " + bytes.Length + "\r\n";
                header += "Content-Type: application/vnd.android.package-archive\r\n";
                header += "\r\n";
                

                fullPacket.AddRange(Encoding.Default.GetBytes(header));
                fullPacket.AddRange(bytes);

                _receiveSocket.Send(fullPacket.ToArray());
                // Now we have to start all over again with waiting for a data to come from the socket.
                StartReceiving();
            }
            else
            {
                Disconnect();
            }
        }
        catch
        {
            // if exeption is throw check if socket is connected because than you can startreive again else Dissconect
            if (!_receiveSocket.Connected)
            {
                Disconnect();
            }
            else
            {
                StartReceiving();
            }
        }
    }

    private void Disconnect()
    {
        // Close connection
        _receiveSocket.Disconnect(true);
        // Next line only apply for the server side receive
        ClientController.RemoveClient(_clientId);
        // Next line only apply on the Client Side receive
        //Here you want to run the method TryToConnect()
    }
    }

    class Client
    {
        public Socket _socket { get; set; }
        public ReceivePacket Receive { get; set; }
        public int Id { get; set; }

        public Client(Socket socket, int id)
        {
            Receive = new ReceivePacket(socket, id);
            Receive.StartReceiving();
            _socket = socket;
            Id = id;
        }
    }

     static class ClientController
     {
          public static List<Client> Clients = new List<Client>();

          public static void AddClient(Socket socket)
          {
              Clients.Add(new Client(socket,Clients.Count));
          }

          public static void RemoveClient(int id)
          {
              Clients.RemoveAt(Clients.FindIndex(x => x.Id == id));
          }
      }
}

