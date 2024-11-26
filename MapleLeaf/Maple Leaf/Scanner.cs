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

        while (!FileEnded())
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
            case ' ':
                case '\t':
                    case '\r':
                        break;

            case '/':
                if (GetMatch('/'))
                {
                    while (GetCurrentChar() != '\n' && !FileEnded())
                        GetNextChar();
                }
                else AddToken(TokenType.SLASH);
                break;

            case '+': AddToken(TokenType.PLUS); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '*': AddToken(TokenType.STAR); break;
            case '%': AddToken(TokenType.MOD); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '(': AddToken(TokenType.LPAREN); break;
            case ')': AddToken(TokenType.RPAREN); break;
            case '{': AddToken(TokenType.LBRACE); break;
            case '}': AddToken(TokenType.RBRACE); break;
            case '\n': line++; break;
            case '"': AddString(); break;
                
            default:
                if (char.IsDigit(character))
                {
                    AddNumber();
                }
                else
                {
                    MapleLeaf.Error(line,$"Unexpected Token {(int)character}");

                }
                break;
        }

    }

    private char GetNextChar()
    {
        char character = source[current++];

        return character;
    }

    private char GetCurrentChar(int offset = 0)
    {
        if (FileEnded() || current+offset >= source.Length) return '\0';
        return source[current+offset];
    }


    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, object? literal)
    {
        String text = source.Substring(start, current-start);
        tokens.Add(new Token(tokenType, text, literal!, line));
    }

    private bool GetMatch(char character)
    {
        if (FileEnded()) return false;
        
        if(source[current] != character) return false;

        current++;
        return true;

    }


    private void AddString()
    {
        int currentLine = line;
        while (GetCurrentChar() != '"' && !FileEnded())
        {
            if (GetCurrentChar() == '\n') line++;
            GetNextChar();
        }

        if (FileEnded())
        {
            MapleLeaf.Error(currentLine,$"Unterminated string");
            return;
        }
        AddToken(TokenType.STRING, (source.Substring(start+1, current-start-1)));
        GetNextChar();

    }

    private void AddNumber()
    {
        while(char.IsDigit(GetCurrentChar()))
            GetNextChar();

        bool isFloat = false;
        if (GetCurrentChar() == '.' && char.IsDigit(GetCurrentChar(1)))
        {
            isFloat = true;
            GetNextChar();
            
            while(char.IsDigit(GetCurrentChar()))
                GetNextChar();
        }
        
        string value = source.Substring(start, current-start);
        if (isFloat)
        {
            AddToken(TokenType.FLOAT, value);
            return;
        }
        AddToken(TokenType.INT, value);

    }
    private bool FileEnded()
    {
        return current >= source.Length;
    }
}