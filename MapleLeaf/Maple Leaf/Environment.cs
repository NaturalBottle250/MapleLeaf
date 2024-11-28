namespace MapleLeaf;

public class Environment
{
    private readonly Dictionary<string, object> variables = new();


    internal void DefineVariable(string name, object value)
    {
        variables.Add(name, value);
    }


    internal object GetVariable(Token name)
    {
        if (variables.TryGetValue(name.lexeme, out object? value))
        {
            return value;
        }
        
        throw new Interpreter.RuntimeError(name, "Undefined variable '" + name.lexeme + "'");
    }
    
    
}