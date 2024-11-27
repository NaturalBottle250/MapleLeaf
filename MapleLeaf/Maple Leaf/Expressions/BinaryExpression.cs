using System.Xml.Xsl;

namespace MapleLeaf;

public class BinaryExpression : Expression
{
    public readonly Expression left, right;
    public readonly Token operatorToken;

    public BinaryExpression(Expression left, Token operatorToken,Expression right)
    {
        this.left = left;
        this.operatorToken = operatorToken;
        this.right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBinary(this);
    }
}