using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Client : MonoBehaviour {

	private byte reliableChannel;
	private int connectionId;
	private int hostId;

	private const int MAX_CLIENT = 1;
	private const int PORT = 26000;
	private const string SERVER_IP = "134.226.45.68"; //"127.0.0.1"; // or "localhost"
	private const int BYTE_SIZE = 1024; //8192 booleans

	private byte error;
	private bool isStarted = false;

    private string msg = "";

	#region Monobehaviour
	// Use this for initialization
	private void Start () {
		// Client object never die
		DontDestroyOnLoad (gameObject);
		Init();
	}
	#endregion

	public void Init()
	{
		// network transport system component initialization
		NetworkTransport.Init ();
		ConnectionConfig cc = new ConnectionConfig ();
		// reliable, each msg is guaranteed to be delivered, but not guaranteed to be in order
		cc.AddChannel (QosType.Reliable);

		HostTopology topo = new HostTopology (cc, MAX_CLIENT);

		//Client only code - Standalone Client
		hostId = NetworkTransport.AddHost(topo, 0); // sending zero as support (not for peer to peer, only for server)
		connectionId = NetworkTransport.Connect(hostId, SERVER_IP,PORT,0, out error);
		Debug.Log (string.Format ("Attempting to connect on {0}...", SERVER_IP));

		isStarted = true;
	}

	// Update is called once per frame
	void Update () {
		UpdateMessagePump ();
	}

	void ShutDown()
	{
		isStarted = false;
		NetworkTransport.Shutdown ();
	}

	public void UpdateMessagePump()
	{
		if (!isStarted) 
			return;

		int recHostId;
		int connectionId; // which user is sending me this
		int channelId; // which lane is he sending that message

		byte[] recBuffer = new byte[BYTE_SIZE];
		int dataSize; 
		byte error;

		NetworkEventType type = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, recBuffer, BYTE_SIZE, out dataSize, out error);
		switch (type)
		{
		case NetworkEventType.Nothing:
			break;

		case NetworkEventType.ConnectEvent:
			Debug.Log("You have connected to the server.");
			break;

		case NetworkEventType.DisconnectEvent:
			Debug.Log("You have been disconnected.");
			break;
		case NetworkEventType.DataEvent:
			BinaryFormatter formatter = new BinaryFormatter ();
			MemoryStream ms = new MemoryStream (recBuffer);
			NetMsg msg = (NetMsg)formatter.Deserialize (ms);
			OnData (connectionId, channelId, recHostId, msg);
//			Debug.Log ("Data"); // we don't need to send data back to the client
			break;
		}
	}

	#region OnData
	private void OnData(int cnnId, int channelId, int recHostId, NetMsg msg)
	{
		switch (msg.OP) 
		{
		case NetOP.None:
			break;
//		case NetOP.story:
//			SelectStory (cnnId, channelId, recHostId, (NetMsg_SelectStory)msg);
//			break;
		}
	}
	#endregion

	private void SelectStory(int cnnId, int channelId, int recHostId, NetMsg_SelectStory story)
	{
		Debug.Log (string.Format ("{0}", story.number));
	}
		
	#region Send
	public void SendServer(NetMsg msg)
	{
		// Hold data
		byte[] buffer = new byte[BYTE_SIZE];

		// Crush data into a byte[] array
		BinaryFormatter formatter = new BinaryFormatter();
		MemoryStream ms = new MemoryStream (buffer);
		formatter.Serialize (ms, msg);

		//buffer[0] = 255; // testing with sending a byte

		NetworkTransport.Send (hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);

	}
	#endregion

    public void SetMsg()
    {
        msg = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        if (msg == "X") { SendMsg(); }
    }
	public void SendMsg()
	{
		NetMsg_SelectStory ss = new NetMsg_SelectStory ();
        ss.number = msg;
		SendServer (ss);
	}
}
