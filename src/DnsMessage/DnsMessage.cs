namespace dns_server.DnsMessage;

public class DnsMessage
{
    public Header Header = new();
    public Question Question = new();
    public Answer Answer = new();
    public Authority Authority = new();
    public Additional Additional = new();
    
    public byte[] Encode()
    {
        Header.Id = 1234;
        Header.QueryResponse = true;
        Header.OpCode = 0;
        Header.AuthoritativeAnswer = false;
        Header.TruncatedMessage = false;
        Header.RecursionDesired = false;
        Header.RecursionAvailable = false;
        Header.Reserved = 0;
        Header.ResponseCode = 0;
        Header.QuestionCount = 0;
        Header.AnswerCount = 0;
        Header.AuthorityCount = 0;
        Header.AdditionalCount = 0;
        
        return Header.Encode();
    }
}