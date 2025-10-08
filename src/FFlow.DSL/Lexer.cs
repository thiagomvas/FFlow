using FFlow.DSL.Lexing;

namespace FFlow.DSL;

public class Lexer
{
    private int _currentIndex;
    private readonly string _input;

    private int _line = 0;
    private int _col = 0;
    private bool _inIdentifier = false;
    
    public Lexer(string input)
    {
        _input = input;
    }
    
    public IEnumerable<Token> Tokenize()
    {
        while (!IsAtEnd())
        {
            var c = Peek();

            switch (c)
            {
                case ' ':
                case '\r':
                case '\t':
                    Advance(); // skip whitespace
                    break;
                case '\n':
                    Advance();
                    _line++;
                    yield return MakeToken(TokenType.EndOfLine, "\\n");
                    break;
                case '(':
                    Advance();
                    yield return MakeToken(TokenType.LeftParen, "(");
                    break;
                case ')':
                    Advance();
                    yield return MakeToken(TokenType.RightParen, ")");
                    break;
                case ',':
                    Advance();
                    yield return MakeToken(TokenType.Comma, ",");
                    break;
                case '=':
                    Advance();
                    yield return MakeToken(TokenType.Equals, "=");
                    break;
                case '>':
                    Advance();
                    yield return MakeToken(TokenType.GreaterThan, ">");
                    break;
                case ':':
                    Advance();
                    yield return MakeToken(TokenType.Colon, ":");
                    break;
                case '"':
                    yield return String();
                    break;  
                default:
                    if (char.IsLetter(c) || c == '_')
                        yield return IdentifierOrKeyword();
                    else if (char.IsDigit(c))
                        yield return Number();
                    else
                        throw new Exception($"Unexpected character '{c}' at line {_line}");
                    break;
            }
        }

        yield return MakeToken(TokenType.EndOfFile, "");
    }

    
    private bool IsAtEnd() => _currentIndex >= _input.Length;
    private char Peek() => _input[_currentIndex];
    private char Advance() => _input[_currentIndex++];
    private Token MakeToken(TokenType type, string value) => new(type, value, _line, _col);
    
    private Token AdvanceToken()
    {
        var c = Advance();
        _col++;
        return MakeToken(TokenType.Identifier, c.ToString());
    }

    private Token String()
    {
        Advance(); 
        var start = _currentIndex;
        while (!IsAtEnd() && Peek() != '"')
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd()) throw new Exception("Unterminated string literal.");

        var value = _input[start.._currentIndex];
        Advance(); // skip closing "
        return MakeToken(TokenType.String, value);
    }

    private Token Number()
    {
        var start = _currentIndex;
        while (!IsAtEnd() && (char.IsDigit(Peek()) || Peek() == '.')) Advance();
        var value = _input[start.._currentIndex];
        return MakeToken(TokenType.Number, value);
    }

    private Token IdentifierOrKeyword()
    {
        var start = _currentIndex;
        while (!IsAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_' || Peek() == '.')) Advance();

        var value = _input[start.._currentIndex];
        if (value == "pipeline") return MakeToken(TokenType.Pipeline, value);
        if (value == "true" || value == "false") return MakeToken(TokenType.Boolean, value);

        return MakeToken(TokenType.Identifier, value);
    }

}