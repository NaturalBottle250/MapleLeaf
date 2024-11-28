namespace MapleLeaf;

public class AssignExpression : Expression
{
    internal readonly Token name;
    internal readonly Expression value;


    internal AssignExpression(Token name, Expression value)
    {
        this.name = name;
        this.value = value;
    }
    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitAssignment(this);
    }
}