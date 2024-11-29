using MapleLeaf.Statements;

namespace MapleLeaf;

public class MLFunction : MLCallable
{
    internal readonly Function declaration;

    internal MLFunction(Function declaration)
    {
        this.declaration = declaration;
    }
    public int GetArity()
    {
        return declaration.parameters.Count;
    }

    public object Call(Interpreter interpreter, List<object> args)
    {
        //Console.WriteLine(declaration.parameters.Count);
        Environment environment = new Environment(interpreter.globals);
        for (int index = 0; index < declaration.parameters.Count; index++)
        {
            string paramName = declaration.parameters[index].lexeme;
            string expectedType = declaration.parameterTypes[index].lexeme;
            object paramValue = args[index];

            if (!interpreter.IsMatchingType(expectedType, paramValue))
            {
                throw new Interpreter.RuntimeError(declaration.parameters[index], $"Type mismatch for parameter '{paramName}': expected {expectedType}, got {interpreter.GetTypeName(paramValue)}.");
            }

            environment.DefineVariable(paramName, expectedType, paramValue);
        }
        
        
        interpreter.ExecuteBlock(declaration.body, environment);
        return null;
        
    }

    public override string ToString()
    {
        return "Function: " + declaration.name.lexeme;
    }
}