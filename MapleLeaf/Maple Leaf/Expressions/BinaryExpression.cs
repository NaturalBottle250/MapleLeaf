using System.Xml.Xsl;

namespace MapleLeaf;

public class BinaryExpression : Expression
{
    private readonly Expression left, right;
    readonly Token operatorToken;

    public BinaryExpression(Expression left, Token operatorToken,Expression right)
    {
        this.left = left;
        this.operatorToken = operatorToken;
        this.right = right;
    }

}