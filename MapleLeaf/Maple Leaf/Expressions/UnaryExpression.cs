namespace MapleLeaf;

public class UnaryExpression : Expression
{
    readonly Token operatorToken;
    readonly Expression operand;


    public UnaryExpression(Token operatorToken, Expression operand)
    {
        this.operatorToken = operatorToken;
        this.operand = operand;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitUnary(this);
    }
}