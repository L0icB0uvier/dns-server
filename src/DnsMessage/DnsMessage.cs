namespace dns_server.DnsMessage;

public class DnsMessage
{
    public Header Header = new();
    public List<Question> Questions = new List<Question>()
        { new Question("codecrafters.io", 1, 1)};
    public Answer Answer = new();
    public Authority Authority = new();
    public Additional Additional = new();
    
    public byte[] Encode()
    {
        var message = Header.Encode().Concat(GetQuestionBytes()).ToArray();
        
        return message;
    }

    private byte[] GetQuestionBytes()
    {
        var result = new List<byte>();
        foreach (var question in Questions)
        {
            result.AddRange(question.Encode());
        }
        return result.ToArray();
    }
}