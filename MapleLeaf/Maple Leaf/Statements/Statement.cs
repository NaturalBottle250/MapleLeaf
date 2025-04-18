﻿namespace MapleLeaf.Statements;

public abstract class Statement
{
    internal abstract R Accept<R>(IVisitor<R> visitor);
    internal interface IVisitor<R>
    {
        R VisitExpressionStatement(ExpressionStmt expression);
        R VisitPrintStatement(PrintStatement printStatement);
        
        R VisitVariableStatement(VariableStatement variableStatement);
        
        R VisitBlockStatement(Block blockStatement);
        
        R VisitIfStatement(If ifStatement);
        
        R VisitWhileStatement(While whileStatement);
        
        R VisitFunctionStatement(Function function);
    }
    
    
    
}