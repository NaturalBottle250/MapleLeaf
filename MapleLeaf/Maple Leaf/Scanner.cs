namespace MapleLeaf;

public class Scanner
{

    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
    {
        { "and", TokenType.AND },
        { "break", TokenType.BREAK },
        { "class", TokenType.CLASS },
        { "continue", TokenType.CONTINUE },
        { "else", TokenType.ELSE },
        { "false", TokenType.FALSE },
        { "float", TokenType.FLOAT },
        { "for", TokenType.FOR },
        { "fun", TokenType.FUN },
        { "if", TokenType.IF },
        { "int", TokenType.INT },
        { "null", TokenType.NULL },
        { "or", TokenType.OR },
        { "print", TokenType.PRINT },
        { "return", TokenType.RETURN },
        { "string", TokenType.STRING},
        { "super", TokenType.SUPER },
        { "this", TokenType.THIS },
        { "true", TokenType.TRUE },
        { "var", TokenType.VAR },
        { "void", TokenType.VOID },
        { "while", TokenType.WHILE }
    };

    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    private int start;
    private int current;
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
            case ':': AddToken(TokenType.COLON); break;
            case '(': AddToken(TokenType.LPAREN); break;
            case ')': AddToken(TokenType.RPAREN); break;
            case '{': AddToken(TokenType.LBRACE); break;
            case '}': AddToken(TokenType.RBRACE); break;
            case '!': AddToken(GetMatch('=')? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(GetMatch('=')? TokenType.EQUAL_EQUAL : TokenType.ASSIGN); break;
            case '<': AddToken(GetMatch('=')? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': AddToken(GetMatch('=')? TokenType.GREATER_EQUAL : TokenType.GREATER); break;



            case '\n': line++; break;
            case '"': AddString(); break;
                
            default:
                if (char.IsDigit(character))
                {
                    AddNumber();
                }
                else if (char.IsLetter(character))
                {
                    AddIdentifier();
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

    private void AddToken(TokenType tokenType, object? literal, int offset = 0)
    {
        String text = source.Substring(start+offset, current-start-offset);
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
        AddToken(TokenType.STRING_VALUE, (source.Substring(start+1, current-start-1)),1);
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
            AddToken(TokenType.FLOAT_VALUE, float.Parse(value));
            return;
        }
        AddToken(TokenType.INT_VALUE, int.Parse(value));

    }

    private void AddIdentifier()
    {
        while (char.IsLetterOrDigit(GetCurrentChar())) GetNextChar();
        string value = source.Substring(start, current-start);
        
        TokenType tokenType;

        if (keywords.ContainsKey(value))
            tokenType = keywords[value];
        else
            tokenType = TokenType.IDENTIFIER;

        AddToken(tokenType);
        
    }
    
    private bool FileEnded()
    {
        return current >= source.Length;
    }
}