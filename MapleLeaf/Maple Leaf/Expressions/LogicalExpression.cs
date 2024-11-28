namespace MapleLeaf;

public class LogicalExpression : Expression
{
    internal readonly Expression left, right;
    internal readonly Token operatorToken;

    public LogicalExpression(Expression left, Token operatorToken, Expression right)
    {
        this.left = left;
        this.right = right;
        this.operatorToken = operatorToken;
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitLogical(this);
    }
}