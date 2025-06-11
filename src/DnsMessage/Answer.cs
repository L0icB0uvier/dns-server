using System.Buffers.Binary;

namespace dns_server.DnsMessage;

public class Answer
{
    public LabelSection Name;
    public ushort AnswerType;
    public ushort Class;
    public uint Ttl;
    public ushort DataLength;
    public uint IpAddress;

    public Answer(
        string domainName, 
        ushort answerType = 1, 
        ushort answerClass = 1, 
        uint ttlm = 60, 
        ushort dataLength = 4, 
        uint ipAddress = 127 << 24 | 0 << 16 | 0 << 8 | 1)
    {
        Name = new LabelSection(domainName);
        AnswerType = answerType;
        Class = answerClass;
        Ttl = ttlm;
        DataLength = dataLength;
        IpAddress = ipAddress;
    }
    
    public byte[] Encode()
    {
        List<byte> answerBytes = new List<byte>();
        
        answerBytes.AddRange(Name.Encode());
        ByteManipulationHelper.AddUShortToList(AnswerType, answerBytes);
        ByteManipulationHelper.AddUShortToList(Class, answerBytes);
        ByteManipulationHelper.AddUIntToList(Ttl, answerBytes);
        ByteManipulationHelper.AddUShortToList(DataLength, answerBytes);
        ByteManipulationHelper.AddUIntToList(IpAddress, answerBytes);
        
        return answerBytes.ToArray();
    }
}