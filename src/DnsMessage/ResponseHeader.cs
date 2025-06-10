using System.Buffers.Binary;

namespace dns_server.DnsMessage;

public class Header
{
    public ushort Id { get; set; } = 1234;
    public bool QueryResponse { get; set; } = true;
    public byte OpCode { get; set; } = 0;
    public bool AuthoritativeAnswer { get; set; } = false;
    public bool TruncatedMessage { get; set; } = false;
    public bool RecursionDesired { get; set; } = false;
    public bool RecursionAvailable { get; set; } = false;
    public byte Reserved { get; set; } = 0;
    public byte ResponseCode { get; set; } = 0;
    public ushort QuestionCount { get; set; } = 1;
    public ushort AnswerCount { get; set; } = 0;
    public ushort AuthorityCount { get; set; } = 0;
    public ushort AdditionalCount { get; set; } = 0;
    
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
        byte[] questionCountBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(questionCountBytes, QuestionCount);
        questionCountBytes.CopyTo(headerBytes, 4);
        return headerBytes;
    }
}