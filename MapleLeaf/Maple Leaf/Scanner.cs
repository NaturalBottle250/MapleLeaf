namespace MapleLeaf;

public class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;


    internal Scanner(string source)
    {
        this.source = source;
    }

    internal List<Token> ScanTokens()
    {

        while (!LineEnded())
        {
            start = current;
            ScanNextToken();
        }
        
        
        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }


    private void ScanNextToken()
    {
        char character = GetNextChar();
        switch (character)
        {
            case '+': AddToken(TokenType.PLUS); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '*': AddToken(TokenType.STAR); break;
            //case '/': AddToken(TokenType.SLASH); break;
            case '%': AddToken(TokenType.MOD); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '(': AddToken(TokenType.LPAREN); break;
            case ')': AddToken(TokenType.RPAREN); break;
            case '{': AddToken(TokenType.LBRACE); break;
            case '}': AddToken(TokenType.RBRACE); break;
                
            default:
                MapleLeaf.Error(line,"Unexpected Token");
                break;
        }

    }

    private char GetNextChar()
    {
        return source[current++];
    }


    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, object? literal)
    {
        String text = source.Substring(start, current);
        tokens.Add(new Token(tokenType, text, literal!, line));
    }

    private bool GetMatch(char character)
    {
        if (LineEnded()) return false;
        
        if(source[current] != character) return false;

        current++;
        return true;

    }

    private bool LineEnded()
    {
        return current >= source.Length;
    }
}