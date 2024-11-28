using MapleLeaf.Statements;

namespace MapleLeaf;

public class Interpreter: Expression.IVisitor<object>, Statement.IVisitor<object>
{
    public  class RuntimeError : Exception
    {
        readonly Token token;

        internal RuntimeError(Token token, string message) : base(message)
        {
            this.token = token;
        }

        public Token Token => token;
    }

    //Flag
    public void Interpret(List<Statement> statements)
    {
        try
        {
            foreach (Statement statement in statements)
            {
                Execute(statement);
                
            }
        }
        catch (RuntimeError e)
        {
            MapleLeaf.RuntimeError(e);
            throw;
        }
    }

    private void Execute(Statement statement)
    {
        statement.Accept(this);
    }

    private string Stringify(object? result)
    {
        if(result == null) return "null";
        
        

        return result.ToString();
    }
    public object VisitLiteral(LiteralExpression expression)
    {
        return expression.value;
    }

    public object VisitUnary(UnaryExpression expression)
    {
        object operand = Evaluate(expression.operand);

        switch (expression.operatorToken.tokenType)
        {
            case TokenType.MINUS:
                return -(float)operand;
            case TokenType.BANG:
                return !(IsTF(operand));
        }


        return null;
    }
    
    private bool IsTF(object obj)
    {
        if (obj == null) return false;
        if(obj is Boolean boolean) return boolean;
        if(obj is int integer) return integer != 0;
        if(obj is float float32) return Math.Abs(float32) > 0.000001;
        if(obj is string stringValue) return !string.IsNullOrEmpty(stringValue);
        
        return true;
    }

    public object VisitBinary(BinaryExpression expression)
    {
        object left = Evaluate(expression.left), right = Evaluate(expression.right);

        switch (expression.operatorToken.tokenType)
        {
            case TokenType.MINUS:
                return Subtract(expression.operatorToken, left, right);
            case TokenType.PLUS:
                return Add(expression.operatorToken, left, right);
            case TokenType.STAR:
                return Multiply(expression.operatorToken, left, right);
            case TokenType.SLASH:
                return Divide(expression.operatorToken, left, right);
            case TokenType.GREATER:
                return (float)left > (float)right;
            case TokenType.LESS:
                return (float)left < (float)right;
            case TokenType.GREATER_EQUAL:
                return (float)left >= (float)right;
            case TokenType.LESS_EQUAL:
                return (float)left <= (float)right;
            case TokenType.BANG_EQUAL:
                return !IsEqual(left, right);
            case TokenType.EQUAL_EQUAL:
                return IsEqual(left, right);
                
                
        }

        return null;
    }

    public object VisitGrouping(GroupingExpression expression)
    {
        return Evaluate(expression.expression);
    }

    public object VisitVariable(VariableExpression expression)
    {
        throw new NotImplementedException();
    }


    private object Add(Token operatorToken, object left, object right)
    {
        if (left is int && right is int)
        {
            return (int)left + (int)right;
        }
        else if (left is float && right is float)
        {
            return (float)left + (float)right;
        }
        else if (left is int lInt && right is float rFloat)
        {
            return lInt + rFloat;
        }
        else if (left is float lFloat && right is int rInt)
        {
            return lFloat + rInt;
        }
        else if (left is string lStr && right is string rStr)
        {
            return lStr + rStr;
        }
        else if (left is string lStr1 && right != null)
        {
            return lStr1 + right.ToString();
        }
        else if (right is string rStr1 && left != null)
        {
            return left.ToString() + rStr1;
        }
        throw new RuntimeError(operatorToken, "Cannot add the two operands.");
    }

    private object Subtract(Token operatorToken,object left, object right)
    {
        if (left is int && right is int)
        {
            return (int)left - (int)right;
        }
        else if (left is float && right is float)
        {
            return (float)left - (float)right;
        }
        else if (left is int lInt && right is float rFloat)
        {
            return lInt - rFloat;
        }
        else if (left is float lFloat && right is int rInt)
        {
            return lFloat - rInt;
        }

        throw new RuntimeError(operatorToken, "Cannot subtract non-numerical operands.");
    }

    private object Divide(Token operatorToken,object left, object right)
    {
        if (right.Equals(0))
        {
            throw new RuntimeError(operatorToken, "Cannot divide by zero.");

        }

        if (left is int && right is int)
        {
            return (float)(int)left / (int)right;  // Ensure float division for precise results.
        }
        else if (left is float && right is float)
        {
            return (float)left / (float)right;
        }
        else if (left is int lInt && right is float rFloat)
        {
            return lInt / rFloat;
        }
        else if (left is float lFloat && right is int rInt)
        {
            return lFloat / rInt;
        }
        throw new RuntimeError(operatorToken, "Cannot divide non-numerical operands.");

    }

    private object Multiply(Token operatorToken,object? left, object? right)
    {
        if (left is int && right is int)
        {
            return (int)left * (int)right;
        }
        else if (left is float && right is float)
        {
            return (float)left * (float)right;
        }
        else if (left is int lInt && right is float rFloat)
        {
            return lInt * rFloat;
        }
        else if (left is float lFloat && right is int rInt)
        {
            return lFloat * rInt;
        }
        throw new RuntimeError(operatorToken, "Cannot multiply non-numerical operands.");

    }

    private bool IsEqual(object? left, object? right)
    {
        if(left == null && right == null) return true;
        if(left == null) return false;
        
        return left.Equals(right);
    }
    

    private object Evaluate(Expression expression)
    {
        return expression.Accept(this);
    }

    //Statements
    public object VisitExpressionStatement(ExpressionStmt expression)
    {
        Evaluate(expression.expression);
        
        return null;
    }

    public object VisitPrintStatement(PrintStatement printStatement)
    {
        object value = Evaluate(printStatement.expression);
        Console.WriteLine(Stringify(value));
        return null;
    }

    public object VisitVariableStatement(VariableStatement variableStatement)
    {
        object? value = null;
        if (variableStatement.initializer != null)
        {
            value = Evaluate(variableStatement.initializer);
        }

        return null;
    }
}