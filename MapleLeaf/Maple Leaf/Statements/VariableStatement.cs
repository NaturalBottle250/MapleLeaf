namespace MapleLeaf.Statements;

public class VariableStatement : Statement
{
    internal readonly Token name;
    internal readonly Token type;
    internal readonly Expression? initializer;

    internal VariableStatement(Token name, Token type, Expression initializer)
    {
        this.name = name;
        this.type = type;
        this.initializer = initializer;
    }
    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitVariableStatement(this);
    }
}