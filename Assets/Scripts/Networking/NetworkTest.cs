using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    public UDPSocket testsocket_1 = new UDPSocket();
    public UDPSocket testsocket_2 = new UDPSocket();
    public UDPSocket testsocket_3 = new UDPSocket();

    public System.Net.IPAddress send_to = NetworkHandler.GetLocalIPAddress();
    public int port = 12345;
    void Start()
    {
        //testsocket_1.Client(send_to.ToString(), port);
        testsocket_1.Client(NetworkHandler.GetLocalIPAddress().ToString(), port);
        testsocket_2.Server(send_to.ToString(), port);
    }

    void Update()
    {
        //4dc0408d9f304dbe80e536d86e04754a
        testsocket_1.Send(1,OrientationMessage.generate().serialize());
    
        if(Input.GetKeyDown(KeyCode.A))
            testsocket_1.Send(3,ButtonMessage.generate(KeyCode.A.ToString()).serialize());


        if(Input.GetKeyDown(KeyCode.B))
        {
            testsocket_1.Send(3,ButtonMessage.generate(KeyCode.B.ToString(),false).serialize());
        }
        if(Input.GetKeyUp(KeyCode.B))
        {
            testsocket_1.Send(3,ButtonMessage.generate(KeyCode.B.ToString(),false,false).serialize());
        }
       
    }
}
