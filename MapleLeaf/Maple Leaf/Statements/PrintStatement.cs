namespace MapleLeaf.Statements;

public class PrintStatement : Statement
{
    internal readonly Expression expression;
    internal PrintStatement(Expression expression)
    {
        this.expression = expression;
    }


    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitPrintStatement(this);
    }
}