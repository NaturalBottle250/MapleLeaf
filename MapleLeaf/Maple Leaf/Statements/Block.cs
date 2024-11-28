namespace MapleLeaf.Statements;

public class Block : Statement
{
    internal readonly List<Statement> statements;

    internal Block(List<Statement> statements)
    {
        this.statements = statements;
    }
    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitBlockStatement(this);
    }
}