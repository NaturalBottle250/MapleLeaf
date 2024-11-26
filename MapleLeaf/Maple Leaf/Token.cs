namespace MapleLeaf;

public class Token
{
     internal readonly TokenType tokenType;
     internal readonly string lexeme;
     internal readonly object? literal;
     internal readonly int lineNumber;

     public Token(TokenType tokenType, string lexeme,  object? literal, int lineNumber)
     {
          this.tokenType = tokenType;
          this.lexeme = lexeme;
          this.literal = literal;
          this.lineNumber = lineNumber;
     }

     public override string ToString()
     {
          return "Token: " + tokenType + ": " + lexeme + " (" + lineNumber + "): " + literal;
     }
}