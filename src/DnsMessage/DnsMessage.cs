namespace dns_server.DnsMessage;

public class DnsMessage
{
    private Header _header;
    private List<Question> _questions;
    private List<Answer> _answers;
    private Authority _authority;
    private Additional _additional;
    
    public Header Header => _header;
    public List<Question> Questions => _questions;
    
    public List<Answer> Answers => _answers;
    
    public Authority Authority => _authority;
    public Additional Additional => _additional;

    public DnsMessage()
    {
        _header = null;
        _questions = new List<Question>();
        _answers = new List<Answer>();
        _authority = null;
        _additional = null;
    }

    public DnsMessage(Header header, List<Question> questions, List<Answer> answers, Authority authority,
        Additional additional)
    {
        _header = header;
        _questions = questions;
        _answers = answers;
        _authority = authority;
        _additional = additional;
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

    public void Decode(byte[] receivedData)
    {
        _header = new Header();
        _header.Decode(receivedData.AsSpan(0, 12).ToArray());
        
        int index = 12;
        for (var i = 0; i < _header.QuestionCount; i++)
        {
            var remainingBytes = receivedData.Skip(index).ToArray();
            var question = new Question();
            var bytesRead = question.Decode(remainingBytes);
            _questions.Add(question);
            index += bytesRead;
        }
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