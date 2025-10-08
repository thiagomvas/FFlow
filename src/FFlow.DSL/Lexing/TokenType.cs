namespace FFlow.DSL.Lexing;
public enum TokenType
{
    Pipeline,       // "pipeline" keyword
    Identifier,     
    String,         
    Number,         
    Boolean,        
    Comma,          // ,
    Equals,         // =
    LeftParen,      // (
    RightParen,     // )
    Colon,          // :
    GreaterThan,    // >
    EndOfLine,      // \n
    EndOfFile
}
