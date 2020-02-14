using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Rule
{
    public int id { get; }
    public IList<ITokenType> format { get; }
    public Rule(int id, IList<ITokenType> format)
    {
        this.id = id;
        this.format = format;
    }
    public override string ToString()
    {
        var result = string.Join(" ", format.Select(t => t.ToString()));
        return result;
    }
}
//------------------------------------------------------------
// ----------------------------------------------------
public class Instruction
{
    public int Id { get; }
    public string Name { get; }
    public IList<AsmParameter> AsmParams { get; }
    public IList<BasicParameter> BasicParams { get; }
    public Rule Rule { get; }
    public Instruction(string Name, int Id, IList<AsmParameter> AsmParams, IList<BasicParameter> BasicParams, Rule Rule)
    {
        this.Name = Name;
        this.Id = Id;
        this.AsmParams = AsmParams;
        this.BasicParams = BasicParams;
        this.Rule = Rule;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj is Instruction)
        {
            Instruction ins = obj as Instruction;
            return Id == ins.Id;
        }
        return false;
    }
}
public class InstructionInstance
{
    public Instruction Instruction { get; }
    public IList<IExprValue> AsmValues { get; }
    public IList<IExprValue> BasicValues { get; }
    public InstructionInstance(Instruction Instruction, IList<IExprValue> AsmValues, IList<IExprValue> BasicValues)
    {
        //TODO: verificar que se cumplan las restricciones de los parametros
        this.Instruction = Instruction;
        this.AsmValues = AsmValues;
        this.BasicValues = BasicValues;
    }
    public override string ToString()
    {
        var listStr = AsmValues.Select(x => x.ToString());
        return string.Join(" ", listStr);
    }

}
