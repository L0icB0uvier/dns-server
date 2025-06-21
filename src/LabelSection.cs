using System.Text;

namespace dns_server;

public class LabelSection
{
    private string _domainName;

    public string DomainName => _domainName;

    public LabelSection(string domainName)
    {
        this._domainName = domainName;
    }

    public LabelSection()
    {
        
    }

    public byte[] Encode()
    {
        var labels = _domainName.Split('.');
        List<byte> labelSectionBytes = new List<byte>();
        foreach (var label in labels)
        {
            labelSectionBytes.Add((byte)label.Length);
            labelSectionBytes.AddRange(System.Text.Encoding.UTF8.GetBytes(label));
        }
        
        labelSectionBytes.Add(0x00);
        return labelSectionBytes.ToArray();
    }

    public int Decode(byte[] bytes)
    {
        Console.WriteLine($"Label Section Bytes Received: {bytes.Length}");
        List<string> labels = new List<string>();
        int index = 0;
        while (bytes[index] != 0x00)
        {
            int length = bytes[index];
            index++;
            var contentBytes = bytes.AsSpan(index, length).ToArray();
            //Array.Reverse(contentBytes);
            string label = Encoding.UTF8.GetString(contentBytes);
            Console.WriteLine($"Label Length: {length} :: Label Content: {label}");
            labels.Add(label);
            index += length;
        }
        
        _domainName = string.Join(".", labels);
        return index + 1;
    }
}