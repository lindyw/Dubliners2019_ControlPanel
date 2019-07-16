public static class NetOP 
{
	public const int None = 0;
	public const int story = 1;

}
[System.Serializable]
public abstract class NetMsg
{
	public byte OP { set; get; } // OperationCode
	//public string story { set; get; } 

	public NetMsg()
	{
		OP = NetOP.None;
	}
}
