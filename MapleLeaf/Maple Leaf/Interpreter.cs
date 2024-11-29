using MapleLeaf.Statements;

namespace MapleLeaf;

public class Interpreter: Expression.IVisitor<object>, Statement.IVisitor<object>
{
    private static readonly Dictionary<string, object> DefaultValues = new()
    {
        { "int", 0 },
        { "float", 0.0f },
        { "string", "" },
        { "bool", false }
        
    };
    
    
    private Environment environment = new Environment();
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
            return;
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
                //return (float)left > (float)right;
            return (float.Parse(Stringify(left)) > float.Parse(Stringify(right)));
            case TokenType.LESS:
                return (float.Parse(Stringify(left)) < float.Parse(Stringify(right)));
            case TokenType.GREATER_EQUAL:
                return (float.Parse(Stringify(left)) >= float.Parse(Stringify(right)));
            case TokenType.LESS_EQUAL:
                return (float.Parse(Stringify(left)) <= float.Parse(Stringify(right)));
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
        return environment.GetVariable(expression.name);
    }

    public object VisitAssignment(AssignExpression expression)
    {
        object value = Evaluate(expression.value);
        
        string expected = environment.GetVariableType(expression.name);

        if (!IsMatchingType(expected, value))
        {
            throw new RuntimeError(expression.name, $"Type Assignment mismatch: expected \"{expected}\", got \"{GetTypeName(value)}\".");
        }
        environment.AssignVariable(expression.name,value);

        return value;
    }

    public object VisitLogical(LogicalExpression expression)
    {
        object left = Evaluate(expression.left);

        if (expression.operatorToken.tokenType == TokenType.OR)
        {
            if (IsTF(left)) return left;
        }
        else
        {
            if (!IsTF(left)) return left;
        }
        
        return Evaluate(expression.right);
    }

    private string GetTypeName(object value)
    {
        if (value is int) return "int";
        if (value is float) return "float";
        if (value is string) return "string";
        if (value is bool) return "bool";
        return value.GetType().Name;
    }

    private bool IsMatchingType(string expected, object value)
    {
        return GetTypeName(value).ToLower().Equals(expected.ToLower());
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
        //Console.WriteLine("HI");
        //Console.WriteLine(value.GetType());
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
        else
            value = DefaultValues.TryGetValue(variableStatement.type.lexeme.ToLower(),
                                              out var defaultValue) ? defaultValue : null;

        bool varExists;
        try
        {
            environment.GetVariable(variableStatement.name);
            varExists = true;
        }
        catch (RuntimeError e)
        {
            varExists = false;
        }
        if(varExists)
            throw new RuntimeError(variableStatement.name, "Variable '" + variableStatement.name.lexeme + "' is already defined in this scope.");
        environment.DefineVariable(variableStatement.name.lexeme, variableStatement.type.lexeme, value);
        return null;
    }

    public object VisitBlockStatement(Block blockStatement)
    {
        ExecuteBlock(blockStatement.statements, new Environment(environment));
        
        return null;
    }

    public object VisitIfStatement(If ifStatement)
    {
        if(IsTF(Evaluate(ifStatement.condition)))
            Execute(ifStatement.branch);
        else if(ifStatement.elseBranch != null)
            Execute(ifStatement.elseBranch);
        
        return null;
    }

    public object VisitWhileStatement(While whileStatement)
    {
        while (IsTF(Evaluate(whileStatement.condition)))
        {
            Execute(whileStatement.body);
        }
        return null;
    }

    private void ExecuteBlock(List<Statement> statements, Environment env)
    {
        Environment previous = this.environment;

        try
        {
            this.environment = env;

            foreach (Statement statement in statements)
            {
                Execute(statement);
            }
        }
        finally
        {
            this.environment = previous;
        }
    }
}