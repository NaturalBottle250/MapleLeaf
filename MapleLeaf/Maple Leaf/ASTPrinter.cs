using System.Text;

namespace MapleLeaf;

public class ASTPrinter : Expression.IVisitor<string>
{

    public string Print(Expression expression)
    {
        return expression.Accept(this);
    }
    public string VisitLiteral(LiteralExpression expression)
    {
        if(expression.value == null) return "null";
        return expression.value.ToString();
    }

    public string VisitUnary(UnaryExpression expression)
    {
        return Parenthesize(expression.operatorToken.lexeme, expression.operand);
    }

    public string VisitBinary(BinaryExpression expression)
    {
        return Parenthesize(expression.operatorToken.lexeme, expression.left, expression.right);
    }

    public string VisitGrouping(GroupingExpression expression)
    {
        return Parenthesize("group", expression.expression);
    }
    
    
    private string Parenthesize(string name, params Expression[] expressions)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("(").Append(name);
        foreach (Expression expression in expressions)
        {
            builder.Append(" ");
            builder.Append(expression.Accept(this));
        }
        builder.Append(")");

        return builder.ToString();
    }

}