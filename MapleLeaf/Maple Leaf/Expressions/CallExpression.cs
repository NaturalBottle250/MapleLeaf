namespace MapleLeaf;

public class CallExpression : Expression
{
    internal readonly Expression callee;
    internal readonly Token paren;
    internal readonly List<Expression> arguments;

    public CallExpression(Expression callee, Token paren, List<Expression> arguments)
    {
        this.callee = callee;
        this.paren = paren;
        this.arguments = arguments;
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitCall(this);
    }
}