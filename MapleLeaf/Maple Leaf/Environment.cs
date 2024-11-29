namespace MapleLeaf;

public class Environment
{
    internal readonly Environment? parent;
    private readonly Dictionary<string, (string Type, object Value)> variables = new();


    internal Environment()
    {
        parent = null;
    }
    internal Environment(Environment parent)
    {
        this.parent = parent;
    }
    internal void DefineVariable(string name, string type, object value)
    {

        variables[name] = (type, value);
    }

    internal object GetVariable(Token name, bool checkParent = true)
    {
        if (variables.TryGetValue(name.lexeme, out var variable))
        {
            return variable.Value; // Only return the value part of the tuple
        }

        if(parent!=null && checkParent) return parent.GetVariable(name);
        throw new Interpreter.RuntimeError(name, "Undefined variable to get '" + name.lexeme + "'");
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
            if (parent != null)
            {
                parent.AssignVariable(name, value);
                return;
            }
            throw new Interpreter.RuntimeError(name, "Undefined variable to assign '" + name.lexeme + "'");
        }
    }

    internal string GetVariableType(Token name)
    {
        if (variables.TryGetValue(name.lexeme, out var variable))
        {
            return variable.Type;
        }
        if(parent!=null) return parent.GetVariableType(name);

        throw new Interpreter.RuntimeError(name, "Undefined variable to get '" + name.lexeme + "'");
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
