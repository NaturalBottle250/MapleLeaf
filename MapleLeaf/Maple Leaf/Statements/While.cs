namespace MapleLeaf.Statements;

public class While : Statement
{
    internal readonly Expression condition;
    internal readonly Statement body;


    public While(Expression condition, Statement body)
    {
        this.condition = condition;
        this.body = body;
    }

    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitWhileStatement(this);
    }
}