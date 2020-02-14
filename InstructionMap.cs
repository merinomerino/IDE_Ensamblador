using System.Collections.Generic;
using System.Linq;
using InterpeterExtensions;
using Util;

public class InstructionMap
{
    // id -> Rule
    private InvertibleMap<Instruction, Rule> invertibleMap;
    public InstructionMap(List<Instruction> instructions)
    {
        invertibleMap = new InvertibleMap<Instruction, Rule>();
        foreach (var ins in instructions)
        {
            Add(ins);
        }
    }
    public void Add(Instruction instruction)
    {
        invertibleMap[instruction] = instruction.Rule;
    }
    public IList<Instruction> InverseLookup(Rule rule)
    {
        return invertibleMap.InverseLookup(rule);
    }
    public Instruction FindMatch(Rule rule, IList<IExprValue> basicParamsValue, IList<IExprValue> asmParamsValue)
    {

        var matches = invertibleMap.InverseLookup(rule);
        //var sorted = matches.OrderByDescending(ins => (ins.B))
        /*
        foreach(var match in matches)
        {
            if (basicParamsValue.Count != match.BasicParams.Count || 
                asmParamsValue.Count != match.AsmParams.Count)
            {
                continue;
            }
            for (int i = 0; i < match.BasicParams.Count; i++)
            {
                var basicParam = match.BasicParams[i];
                var basicSatisfy = basicParam.Parameter.constraint.SatisfyConstraint(basicParamsValue[i]);
                if (!basicSatisfy)
                    continue;
            }
            for (int i = 0; i < match.AsmParams.Count; i++)
            {
                var asmParam = match.AsmParams[i];
                var asmSatisfy = asmParam.Parameter.constraint.SatisfyConstraint(asmParamsValue[i]);
                if (!asmSatisfy)
                    continue;
            }
            return match;
        }
        return null;
        */
        foreach (var match in matches)
        {
            var basicSatisfy =
                match.BasicParams
                .Zip(basicParamsValue,
                    (param, value) => (param: param, value: value))
                .All(tuple => tuple.param.Parameter.constraint.SatisfyConstraint(tuple.value));

            var asmSatisfy =
                match.AsmParams
                .Zip(asmParamsValue,
                    (param, value) => (param: param, value: value))
                .All(tuple => tuple.param.Parameter.constraint.SatisfyConstraint(tuple.value));

            if (asmSatisfy && basicSatisfy)
                return match;
        }
        return null;
    }
    public InstructionInstance GenerateInstance(Rule rule, IList<IExprValue> basicParamsValue, IList<IExprValue> asmParamsValue)
    {
        var instruction = FindMatch(rule, basicParamsValue, asmParamsValue);
        var instance = instruction != null ? new InstructionInstance(instruction, asmParamsValue, basicParamsValue) : null;
        return instance;
    }
}