namespace MapleLeaf;

public class Environment
{
    private readonly Dictionary<string, (string Type, object Value)> variables = new();

    internal void DefineVariable(string name, string type, object value)
    {
        variables[name] = (type, value);
    }

    internal object GetVariable(Token name)
    {
        if (variables.TryGetValue(name.lexeme, out var variable))
        {
            return variable.Value; // Only return the value part of the tuple
        }

        throw new Interpreter.RuntimeError(name, "Undefined variable '" + name.lexeme + "'");
    }

    internal void AssignVariable(Token name, object value)
    {
        if (variables.ContainsKey(name.lexeme))
        {
            var existing = variables[name.lexeme];
            variables[name.lexeme] = (existing.Type, value);
        }
        else
        {
            throw new Interpreter.RuntimeError(name, "Undefined variable '" + name.lexeme + "'");
        }
    }

    internal string GetVariableType(Token name)
    {
        if (variables.TryGetValue(name.lexeme, out var variable))
        {
            return variable.Type;
        }

        throw new Interpreter.RuntimeError(name, "Undefined variable '" + name.lexeme + "'");
    }
}

/*
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
*/
