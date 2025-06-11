namespace dns_server.DnsMessage;

public class DnsMessage
{
    public Header Header;
    public List<Question> Questions = new List<Question>()
    {
        new Question("codecrafters.io", 1, 1)
    };
    
    public List<Answer> Answers = new List<Answer>()
    {
        new Answer("codecrafters.io")
    };
    
    public Authority Authority = new();
    public Additional Additional = new();
    
    public DnsMessage(byte[] receivedData)
    {
        Header = new Header(receivedData);
    }
    
    public byte[] Encode()
    {
        var message = 
            Header.Encode()
            .Concat(GetQuestionBytes())
            .Concat(GetAnswerBytes())
            .ToArray();
        
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
    
    private byte[] GetAnswerBytes()
    {
        var result = new List<byte>();
        foreach (var answer in Answers)
        {
            result.AddRange(answer.Encode());
        }
        return result.ToArray();
    }
}