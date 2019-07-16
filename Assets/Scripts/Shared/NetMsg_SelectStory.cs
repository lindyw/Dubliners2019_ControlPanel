[System.Serializable]

public class NetMsg_SelectStory : NetMsg {

	public NetMsg_SelectStory()
	{
		OP = NetOP.story;
	}

	public string number { set; get; }
}
