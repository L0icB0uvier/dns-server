namespace dns_server.DnsMessage;

public class Header
{
    public ushort Id { get; set; }
    public bool QueryResponse { get; set; }
    public byte OpCode { get; set; }
    public bool AuthoritativeAnswer { get; set; }
    public bool TruncatedMessage { get; set; }
    public bool RecursionDesired { get; set; }
    public bool RecursionAvailable { get; set; }
    public byte Reserved { get; set; }
    public byte ResponseCode { get; set; }
    public ushort QuestionCount { get; set; }
    public ushort AnswerCount { get; set; }
    public ushort AuthorityCount { get; set; }
    public ushort AdditionalCount { get; set; }


    public byte[] Encode()
    {
        byte[] headerBytes = new byte[12];
        
        var idBytes = BitConverter.GetBytes(Id);
        var rest = BitConverter.GetBytes(1 << 7);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(idBytes);
        } 
        idBytes.CopyTo(headerBytes, 0);
        rest.CopyTo(headerBytes, 2);

        return headerBytes;
    }
}