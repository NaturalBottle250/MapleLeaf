namespace MapleLeaf;

public interface MLCallable
{
    int GetArity();
    object Call(Interpreter interpreter, List<object> args);
}