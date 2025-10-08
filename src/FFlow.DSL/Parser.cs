using FFlow.DSL.Lexing;

namespace FFlow.DSL;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _currentIndex;
    
    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens.ToList();
    }
    
    public PipelineNode ParsePipeline()
    {
        Consume(TokenType.Pipeline, "Expected pipeline declaration");
        var nameToken = Consume(TokenType.String, "Expected pipeline name");
        Consume(TokenType.Colon, "Expected ':' after pipeline name");
        var pipeline = new PipelineNode(nameToken.Value, []);
        return pipeline;
    }
    
    private Token Consume(TokenType type, string errorMessage)
    {
        if (Check(type)) return Advance();
        throw new Exception(errorMessage);
    }
    
    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }
    
    private Token Advance()
    {
        if (!IsAtEnd()) _currentIndex++;
        return Previous();
    }
    
    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EndOfFile;
    }
    
    private Token Peek()
    {
        return _tokens[_currentIndex];
    }
    
    private Token Previous()
    {
        return _tokens[_currentIndex - 1];
    }
    
    
}