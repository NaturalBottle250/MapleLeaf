namespace MapleLeaf;

public class UnaryExpression : Expression
{
    public readonly Token operatorToken;
    public readonly Expression operand;


    public UnaryExpression(Token operatorToken, Expression operand)
    {
        this.operatorToken = operatorToken;
        this.operand = operand;
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitUnary(this);
    }
}