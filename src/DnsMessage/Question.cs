using System.Buffers.Binary;

namespace dns_server.DnsMessage;

public class Question
{
    public LabelSection Name;
    public ushort Type = 1;
    public ushort Class = 1;

    public Question(string domainName, ushort type, ushort @class)
    {
        Name = new LabelSection(domainName);
        Type = type;
        Class = @class;
    }

    public byte[] Encode()
    {
        List<byte> questionBytes = new List<byte>();
        
        questionBytes.AddRange(Name.Encode());
        ByteManipulationHelper.AddUShortToList(Type, questionBytes);
        ByteManipulationHelper.AddUShortToList(Class, questionBytes);
        
        Console.WriteLine($"Question sent bytes: {BitConverter.ToString(questionBytes.ToArray())}");
        return questionBytes.ToArray();
    }


}