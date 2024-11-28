namespace MapleLeaf;

public class LiteralExpression : Expression
{
    public readonly object? value;

    public LiteralExpression(object? value)
    {
        this.value = value;
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitLiteral(this);
    }

    public override string ToString()
    {
        return value?.ToString() ?? "null";
    }
}