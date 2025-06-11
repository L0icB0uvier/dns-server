namespace dns_server;

public class LabelSection(string domainName)
{
    private readonly string DomainName = domainName;

    public byte[] Encode()
    {
        var labels = DomainName.Split('.');
        List<byte> labelSectionBytes = new List<byte>();
        foreach (var label in labels)
        {
            labelSectionBytes.Add((byte)label.Length);
            labelSectionBytes.AddRange(System.Text.Encoding.UTF8.GetBytes(label));
        }
        
        labelSectionBytes.Add(0x00);
        return labelSectionBytes.ToArray();
    }
}