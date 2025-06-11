using System.Buffers.Binary;

namespace dns_server.DnsMessage;

public class Header
{
    public ushort Id { get; set; }
    public bool QueryResponse { get; set; }
    public byte OpCode { get; set; }
    public bool AuthoritativeAnswer { get; set; }
    public bool TruncatedMessage { get; set; }
    public bool RecursionDesired { get; set; }
    public bool RecursionAvailable { get; set; }
    public byte Reserved { get; set; }
    public byte ResponseCode { get; set; }
    public ushort QuestionCount { get; set; }
    public ushort AnswerCount { get; set; }
    public ushort AuthorityCount { get; set; }
    public ushort AdditionalCount { get; set; }

    public Header(
        ushort id,
        bool queryResponse = true,
        byte opCode = 0,
        bool authoritativeAnswer = false,
        bool truncatedMessage = false,
        bool recursionDesired = false,
        bool recursionAvailable = false,
        byte reserved = 0,
        byte responseCode = 0,
        ushort questionCount = 0,
        ushort answerCount = 0,
        ushort authorityCount = 0
    )
    {
        Id = id;
        QueryResponse = queryResponse;
        OpCode = opCode;
        AuthoritativeAnswer = authoritativeAnswer;
        TruncatedMessage = truncatedMessage;
        RecursionDesired = recursionDesired;
        RecursionAvailable = recursionAvailable;
        Reserved = responseCode;
        ResponseCode = responseCode;
        QuestionCount = questionCount;
        AnswerCount = answerCount;
        AuthorityCount = authorityCount;
        AdditionalCount = answerCount;
    }
    
    public byte[] Encode()
    {
        byte[] headerBytes = new byte[12];
        
        var idBytes = BitConverter.GetBytes(Id);
        var rest = BitConverter.GetBytes(1 << 7);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(idBytes);
        } 
        idBytes.CopyTo(headerBytes, 0);
        rest.CopyTo(headerBytes, 2);
        byte[] questionCountBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(questionCountBytes, QuestionCount);
        questionCountBytes.CopyTo(headerBytes, 4);
        byte[] answerCountBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(answerCountBytes, AnswerCount);
        answerCountBytes.CopyTo(headerBytes, 6);
        
        return headerBytes;
    }
}