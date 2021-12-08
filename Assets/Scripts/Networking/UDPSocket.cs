using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class UDPSocket
{
    private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private const int bufSize = 8 * 1024;
    private State state = new State();
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv = null;

    public class State
    {
        public byte[] buffer = new byte[bufSize];
    }

    public void Server(string address, int port)
    {
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        Receive();            
    }

    public void Client(string address, int port)
    {
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        _socket.Connect(IPAddress.Parse(address), port);

        Receive();            
    }

    public void Send(byte id, string data)
    {
        Send(id, Encoding.ASCII.GetBytes(data));
    }
    public void Send(byte id, byte[] data)
    {
        byte[] send = new byte[data.Length+1];
        send[0] = id;
        data.CopyTo(send,1);
        //byte[] data = Encoding.ASCII.GetBytes(' ' + text);
        _socket.BeginSend(send, 0, send.Length, SocketFlags.None, (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket.EndSend(ar);
            //Debug.Log(String.Format("SEND: {0}, {1}", bytes, text));
        }, state);
    }

    private void Receive()
    {            
        _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
        {
            //Debug.Log("TEst");

            State so = (State)ar.AsyncState;
            int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
            _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

            NetworkMessage.functions[(byte)so.buffer[0]](new ArraySegment<byte>(so.buffer,1,bytes-1).ToArray(), epFrom.ToString());

            //string data = Encoding.ASCII.GetString(so.buffer, 0, bytes);
        }, state);
    }

    public void Close()
    {
        //Debug.Log("Shutdown");
        //_socket.Shutdown(SocketShutdown.Both);
        //Debug.Log("Close");

        _socket.Close();
        //Debug.Log("DONE");

    }
}
