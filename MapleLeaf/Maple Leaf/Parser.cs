using System.Diagnostics;

namespace MapleLeaf;

public class Parser
{
    
    private  class ParserError : Exception
    {
        
    }
    private int current;
    
    private readonly List<Token> tokens;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public Expression Parse()
    {
        try
        {
            return Expression();
        }
        catch (ParserError e)
        {
            return null;
        }
    }



    private Expression Expression()
    {
        return Equality();
    }

    private Expression Equality()
    {
        Expression expression = Comparison();
        while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token operatorToken = GetPrevious();
            Expression right = Comparison();
            expression = new BinaryExpression(expression, operatorToken, right);
        }
        
        return expression;
    }

    private Expression Comparison()
    {
        Expression expression = Term();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token operatorToken = GetPrevious();
            Expression right = Term();
            expression = new BinaryExpression(expression, operatorToken, right);
        }
        
        return expression;
    }

    private Expression Term()
    {
        Expression expression = Factor();

        while (Match(TokenType.MINUS, TokenType.PLUS))
        {
            Token operatorToken = GetPrevious();
            Expression right = Factor();
            expression = new BinaryExpression(expression, operatorToken, right);
        }
        
        return expression;
    }

    private Expression Factor()
    {
        Expression expression = Unary();

        while (Match(TokenType.SLASH, TokenType.STAR))
        {
            Token operatorToken = GetPrevious();
            Expression right = Unary();
            expression = new BinaryExpression(expression, operatorToken, right);
        }
        
        return expression;
    }

    private Expression Unary()
    {
        if (Match(TokenType.BANG, TokenType.MINUS))
        {
            Token operatorToken = GetPrevious();
            Expression right = Unary();
            return new UnaryExpression(operatorToken, right);
            
        }

        return Primary();
    }


    //Flag
    private Expression? Primary()
    {
        if (Match(TokenType.FALSE))
            return new LiteralExpression(false);
        if(Match(TokenType.TRUE))
            return new LiteralExpression(true);
        if(Match(TokenType.NULL))
            return new LiteralExpression(null);

        if (Match(TokenType.INT_VALUE, TokenType.FLOAT_VALUE, TokenType.STRING_VALUE))
        {
            return new LiteralExpression(GetPrevious().literal);
        }

        if (Match(TokenType.LPAREN))
        {
            Expression expression = Expression();
            Consume(TokenType.RPAREN, "Expected ')' after expression");
            return new GroupingExpression(expression);
        }

        throw Error(GetCurrent(), "Failed to parse expression or no valid expression found");
    }
    
    
    private bool Match(params TokenType[] types)
    {
        foreach (TokenType tokenType in types)
        {
            if (Check(tokenType))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private Token Consume(TokenType tokenType, string errorMessage)
    {
        if(Check(tokenType)) return Advance();
        
        throw Error(GetCurrent(), errorMessage);
    }

    private ParserError Error(Token token, string errorMessage)
    {
        MapleLeaf.Error(token, errorMessage);
        return new ParserError();
    }

    private void Synchronize()
    {
        Advance();
        while (!IsAtEnd())
        {
            if(GetPrevious().tokenType == TokenType.SEMICOLON) return;


            switch (GetCurrent().tokenType)
            {
                case TokenType.CLASS:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.WHILE:
                    case TokenType.RETURN:
                    case TokenType.PRINT:
                    return;
            }

            Advance();
        }
    }
    

    private bool Check(TokenType tokenType)
    {
        if(IsAtEnd()) return false;
        return GetCurrent().tokenType == tokenType;
    }

    private Token Advance()
    {
        if(!IsAtEnd()) current++;
        return GetPrevious();
    }

    private bool IsAtEnd()
    {
        return GetCurrent().tokenType == TokenType.EOF;
    }

    private Token GetCurrent()
    {
        return tokens[current];
    }

    private Token GetPrevious()
    {
        return tokens[current - 1];
    }
}