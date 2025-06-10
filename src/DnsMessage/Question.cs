using System.Buffers.Binary;

namespace dns_server.DnsMessage;

public class Question
{
    public string Name = "codecrafters.io";
    public ushort Type = 1;
    public ushort Class = 1;

    public Question(string Name, ushort Type, ushort Class)
    {
        this.Name = Name;
        this.Type = Type;
        this.Class = Class;
    }

    public byte[] Encode()
    {
        var labels = Name.Split('.');
        
        var byteCount = CalculateQuestionBytesCount(labels);

        Console.WriteLine($"Byte Count: {byteCount}");
        byte[] questionBytes = new byte[byteCount + 4];
        
        int index = 0;
        foreach (var label in labels)
        {
            byte labelLength = (byte)label.Length;
            questionBytes[index] = labelLength;
            index++;
            
            System.Text.Encoding.UTF8.GetBytes(label).CopyTo(questionBytes, index);
            index += label.Length;
        }
        
        byte terminationByte = 0x00;
        questionBytes[index] = terminationByte;
        index++;
        
        byte[] typeBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(typeBytes, Type);
        typeBytes.CopyTo(questionBytes, index);
        index += 2;
        
        byte[] classBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(classBytes, Class);
        classBytes.CopyTo(questionBytes, index);
        Console.WriteLine(BitConverter.ToString(questionBytes));
        return questionBytes;
    }

    private static int CalculateQuestionBytesCount(string[] labels)
    {
        int byteCount = 0;
        foreach (var label in labels)
        {
            byteCount++;
            byteCount += label.Length;
        }

        byteCount++;
        return byteCount;
    }
}