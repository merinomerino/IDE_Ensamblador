using System;
using System.Collections.Generic;
using System.Linq;
using Util;
using InterpeterExtensions;
using Newtonsoft.Json;

namespace pryct
{
    class Program
    {
        static void Main(string[] args)
        {
            var ruleExample = new Rule
            (
                id: 1,
                format: new ITokenType[]
                {
                    new FixedString("if"),
                    new FixedString("("),
                    new BasicPlaceHolder(new PlaceHolder(1, "Condition", new TableID(3, 'A'))),
                    new FixedString(")"),
                    new FixedString("then"),
                    new AsmPlaceHolder(new PlaceHolder(1, "Label", new TableID(2, 'A')))
                }
            );
            var instruction = new Instruction
            (
                Name: "BREQ",
                Id: 1,
                AsmParams: new List<AsmParameter>
                {
                    new AsmParameter(new Parameter("k", new RangeConstraint(new Range(-64, 63))))
                },
                BasicParams: new List<BasicParameter>
                {
                    new BasicParameter(new Parameter("", new ConstantConstraint("Equal")))
                },
                Rule: ruleExample
            );
            var instruction2 = new Instruction
            (
                Name: "BRLO",
                Id: 2,
                AsmParams: new List<AsmParameter>
                {
                    new AsmParameter(new Parameter("k", new RangeConstraint(new Range(-64, 63))))
                },
                BasicParams: new List<BasicParameter>
                {
                    new BasicParameter(new Parameter("", new ConstantConstraint("Lower")))
                },
                Rule: ruleExample
            );
            //var instruction3 = 
            var instructionMap = new InstructionMap(new List<Instruction> { instruction, instruction2 });
            var found =
                instructionMap.FindMatch(
                    ruleExample,
                    asmParamsValue: new List<IExprValue> { new IntegerValue(33) },
                    basicParamsValue: new List<IExprValue> { new ConstStringValue("Equal") }
                );
            var found2 =
                instructionMap.FindMatch(
                    ruleExample,
                    asmParamsValue: new List<IExprValue> { new IntegerValue(33) },
                    basicParamsValue: new List<IExprValue> { new ConstStringValue("Lower") }
                );
            Console.WriteLine(found != null);

            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.All;
            var foundSerialized = JsonConvert.SerializeObject(found, jss);
            Instruction foundDeserialized = JsonConvert.DeserializeObject<Instruction>(foundSerialized);
            Console.WriteLine(foundDeserialized.Name);
            
        }
    }

}
