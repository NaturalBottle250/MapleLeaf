namespace MapleLeaf;

public class GroupingExpression : Expression
{
    readonly Expression expression;

    public GroupingExpression(Expression expression)
    {
        this.expression = expression;
    }

}