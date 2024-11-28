namespace MapleLeaf;

public class VariableExpression : Expression
{
    internal readonly Token name;
    
    internal VariableExpression(Token name) => this.name = name;
    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitVariable(this);
    }
}