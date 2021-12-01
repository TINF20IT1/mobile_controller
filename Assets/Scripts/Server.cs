using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text; 
using System.Threading;  


public class StateObject
{
    // Size of receive buffer.  
    public const int BufferSize = 1024;

    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];

    // Received data string.
    public StringBuilder sb = new StringBuilder();

    // Client socket.
    public Socket workSocket = null;
}  
public class Server
{
    private bool started;
    private int port;
    private Socket listener;

    private  ManualResetEvent allDone = new ManualResetEvent(false);
    public Server(int port_)
    {
        port = port_;
        started = false;
    }

    public void StartServer()
    {
        Thread thread = new Thread(Listen);
        thread.Start();
        started = true;

    }

    private void Listen()
    {

        // Establish the local endpoint
        // for the socket. Dns.GetHostName
        // returns the name of the host
        // running the application.
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
    
        // Creation TCP/IP Socket using
        // Socket Class Constructor
        Socket listener = new Socket(ipAddr.AddressFamily,
                    SocketType.Dgram, ProtocolType.Udp);
    
        try 
        {
            
            // Using Bind() method we associate a
            // network address to the Server Socket
            // All client that will connect to this
            // Server Socket must know this network
            // Address
            listener.Bind(localEndPoint);
    
            // Using Listen() method we create
            // the Client list that will want
            // to connect to Server
            listener.Listen(10);
    
            while (true) 
            {
                
                Debug.Log("Waiting connection ... ");
    
                // Suspend while waiting for
                // incoming connection Using
                // Accept() method the server
                // will accept connection of client
                Socket clientSocket = listener.Accept();

                Debug.Log("Accepted request ... ");

    
                // Data buffer
                byte[] bytes = new Byte[1024];
                string data = null;
    
                while (true) 
                {
    
                    int numByte = clientSocket.Receive(bytes);
                    
                    data += Encoding.ASCII.GetString(bytes, 0, numByte);

                    Debug.Log(data);

                    

                    if (data.IndexOf("<EOF>") > -1)
                        break;
                }
    
                Debug.Log(String.Format("Text received -> {0} ", data));
                byte[] message = Encoding.ASCII.GetBytes("Test Server");
    
                // Send a message to Client
                // using Send() method
                clientSocket.Send(message);
    
                // Close client Socket using the
                // Close() method. After closing,
                // we can use the closed Socket
                // for a new Client Connection
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
        
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }
}
