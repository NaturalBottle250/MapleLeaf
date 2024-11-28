
namespace MapleLeaf.Statements;

public class ExpressionStmt : Statement
{
    internal readonly Expression expression;


    internal ExpressionStmt(Expression expression)
    {
        this.expression = expression;
    }
    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitExpressionStatement(this);
    }
}