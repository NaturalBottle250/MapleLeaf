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

     /// <summary>
     /// Prints a Token's details in an organized fashion
     /// </summary>
     public void PrintColored()
     {
          Console.ForegroundColor = ConsoleColor.Cyan;
          Console.Write("Token: ");
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.Write( tokenType);

          Console.ForegroundColor = ConsoleColor.White;
          if(tokenType != TokenType.STRING_VALUE)
               Console.Write(" -> " + lexeme);

          Console.ForegroundColor = ConsoleColor.Gray;
          Console.Write(" (" + lineNumber + ")");

          Console.ForegroundColor = ConsoleColor.Green;
          if(literal != null)Console.Write(" : " + literal);
          Console.WriteLine();

          // Reset to default color
          Console.ResetColor();
     }

     public override string ToString()
     {
          return "Token: " + tokenType + ": " + lexeme + " (" + lineNumber + "): " + literal;
     }
}