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

    public Question()
    {
        Name = new LabelSection();
        Type = 1;
        Class = 1;       
    }

    public byte[] Encode()
    {
        List<byte> questionBytes = new List<byte>();
        
        questionBytes.AddRange(Name.Encode());
        ByteManipulationHelper.AddUShortToList(Type, questionBytes);
        ByteManipulationHelper.AddUShortToList(Class, questionBytes);
        
        //Console.WriteLine($"Question sent bytes: {BitConverter.ToString(questionBytes.ToArray())}");
        return questionBytes.ToArray();
    }

    public int Decode(byte[] bytes)
    {
        var nameBytesUsed = Name.Decode(bytes);
        Type = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(nameBytesUsed, 2));
        Class = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(nameBytesUsed + 2, 2));
        return nameBytesUsed + 4;
    }
}