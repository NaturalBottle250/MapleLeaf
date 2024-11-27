namespace MapleLeaf;

public abstract class Expression
{
    
    public abstract T Accept<T>(IVisitor<T> visitor);
    public interface IVisitor<T>
    {
        T VisitLiteral(LiteralExpression expression);
        T VisitUnary(UnaryExpression expression);
        T VisitBinary(BinaryExpression expression);
        T VisitGrouping(GroupingExpression expression);
        
        
    }
    
}