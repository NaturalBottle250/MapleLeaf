namespace MapleLeaf.Statements;

public class If : Statement
{
    internal readonly Expression condition;
    internal readonly Statement branch;
    internal readonly Statement? elseBranch;

    internal If(Expression condition, Statement branch, Statement elseBranch)
    {
        this.condition = condition;
        this.branch = branch;
        this.elseBranch = elseBranch;
    }

    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitIfStatement(this);
    }
}