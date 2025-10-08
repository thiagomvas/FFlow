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
        Consume(TokenType.EndOfLine, "Expected end of line after pipeline declaration");
        List<StepNode> steps = new List<StepNode>();
        while (!IsAtEnd())
        {
            var step = ParseStep();
            steps.Add(step);
        }
        
        
        var pipeline = new PipelineNode(nameToken.Value,  steps);
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
    
    private bool Match(TokenType type)
    {
        if (Check(type))
        {
            Advance();
            return true;
        }
        return false;
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

    private StepNode ParseStep()
    {
        var identifierToken = Consume(TokenType.Identifier, "Expected step identifier");
        Consume(TokenType.LeftParen, "Expected '(' after step name");
        var parameters = new Dictionary<string, object>();
        var unnamedArgs = new List<string>();
        var seenNamed = false;

        if (!Check(TokenType.RightParen))
        {
            do
            {
                if (Check(TokenType.String) && !seenNamed)
                {
                    // Nameless argument
                    var valueToken = Advance();
                    unnamedArgs.Add(valueToken.Value);
                }
                else
                {
                    var paramNameToken = Consume(TokenType.Identifier, "Expected parameter name");
                    Consume(TokenType.Equals, "Expected '=' after parameter name");

                    Token paramValueToken;
                    if (Check(TokenType.Identifier))
                        paramValueToken = Consume(TokenType.Identifier, "Expected parameter value");
                    else if (Check(TokenType.String))
                        paramValueToken = Consume(TokenType.String, "Expected parameter value");
                    else if (Check(TokenType.Number))
                        paramValueToken = Consume(TokenType.Number, "Expected parameter value");
                    else if (Check(TokenType.Boolean))
                        paramValueToken = Advance();
                    else
                        throw new Exception("Expected parameter value");
                    
                    parameters[paramNameToken.Value] = paramValueToken.Value;
                    seenNamed = true;
                }
            } while (Match(TokenType.Comma));
        }

        Consume(TokenType.RightParen, "Expected ')' after parameters");
        if (Check(TokenType.EndOfLine))
        {
            Advance();
        }

        var p = parameters.Concat(unnamedArgs
            .Select((arg, index) => new KeyValuePair<string, object>($"arg{index + 1}", arg)))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        return new StepNode(identifierToken.Value, p);

    }
    
    
}