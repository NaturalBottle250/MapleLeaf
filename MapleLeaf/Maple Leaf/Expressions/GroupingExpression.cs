﻿namespace MapleLeaf;

public class GroupingExpression : Expression
{
    public readonly Expression expression;

    public GroupingExpression(Expression expression)
    {
        this.expression = expression;
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitGrouping(this);
    }
}