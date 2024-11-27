namespace MapleLeaf;

public class LiteralExpression : Expression
{
    public readonly object? value;

    public LiteralExpression(object value)
    {
        this.value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitLiteral(this);
    }
}