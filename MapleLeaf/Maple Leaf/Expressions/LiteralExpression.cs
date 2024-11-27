namespace MapleLeaf;

public class LiteralExpression : Expression
{
    readonly object value;

    public LiteralExpression(object value)
    {
        this.value = value;
    }
}