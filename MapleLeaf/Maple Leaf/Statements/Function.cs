namespace MapleLeaf.Statements;

public class Function : Statement
{
    internal readonly Token name, returnType;
    internal readonly List<Token> parameters, parameterTypes;
    internal readonly List<Statement> body;

    public Function(Token name, Token returnType, List<Token> parameters, List<Token> parameterTypes, List<Statement> body)
    {
        this.name = name;
        this.returnType = returnType;
        this.parameters = parameters;
        //Console.WriteLine("Function Constructor, has " + parameters.Count + " parameters");
        this.parameterTypes = parameterTypes;
        this.body = body;
    }

    internal override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitFunctionStatement(this);
    }
}