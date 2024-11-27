namespace MapleLeaf;

/// <summary>
/// Used to tokenize MapleLeaf Source Code
/// </summary>
public enum TokenType
{
    INT, FLOAT, BOOL, STRING, IDENTIFIER, VOID,
    INT_VALUE, FLOAT_VALUE, STRING_VALUE,
    
    ASSIGN, EQUAL_EQUAL, BANG, BANG_EQUAL,
    GREATER, LESS, GREATER_EQUAL, LESS_EQUAL,
    
    PLUS, MINUS, DOT, SLASH, STAR, COMMA, MOD, SEMICOLON, COLON, LPAREN, RPAREN, LBRACE, RBRACE,
    
    AND, OR, TRUE, FALSE, IF, ELSE, NULL, VAR, FOR, WHILE, BASE, THIS, RETURN,
    
    FUN, CLASS, SUPER, PRINT,
    
    BREAK, CONTINUE,
    
    EOF
}