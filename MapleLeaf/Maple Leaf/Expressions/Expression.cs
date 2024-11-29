namespace MapleLeaf;

public abstract class Expression
{
    
     internal abstract T Accept<T>(IVisitor<T> visitor);
    internal interface IVisitor<T>
    {
        T VisitLiteral(LiteralExpression expression);
        T VisitUnary(UnaryExpression expression);
        T VisitBinary(BinaryExpression expression);
        T VisitGrouping(GroupingExpression expression);
        
        T VisitVariable(VariableExpression expression);
        
        T VisitAssignment(AssignExpression expression);
        
        T VisitLogical(LogicalExpression expression);
        
        T VisitCall(CallExpression expression);
        
        
    }
    
}