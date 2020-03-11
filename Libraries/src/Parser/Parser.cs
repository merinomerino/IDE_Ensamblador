using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Libraries
{
    public class Parser
    {
        public Dictionary<int, Rule> Rules { get; }
        public InstructionMap Map { get; }
        public List<Instruction> Instructions { get; }
        public Parser(string rulesFilePath = @"files\rules.json"
                     , string instructionsFilePath = @"files\instructions.json")
        {
            var rules = new List<Rule>();
            using (StreamReader r = new StreamReader(rulesFilePath))
            {
                JArray file = JArray.Parse(r.ReadToEnd());
                foreach (JObject rule in file.Children<JObject>())
                {
                    var format = ParseFormat(rule["format"].Children());
                    var flags = rule["flags"].ToObject<List<char>>();
                    rules.Add(
                        new Rule
                        (
                            id: rule.Value<int>("id"),
                            format: format,
                            info: new RuleInfo
                            (
                                description: rule.Value<string>("description"),
                                group: rule.Value<int>("group"),
                                subgroup: rule.Value<int>("subgroup"),
                                operation: rule.Value<string>("operation"),
                                flags: flags
                            )
                       ));
                }
                Rules = rules.ToDictionary(rule => rule.Id);
            }

            using (StreamReader r = new StreamReader(instructionsFilePath))
            {
                var instructions = new List<Instruction>();
                JArray file = JArray.Parse(r.ReadToEnd());
                foreach (JObject ins in file.Children<JObject>())
                {
                    var asmParams = ins["asmParams"].Children().Select(
                        p => new AsmParameter(CreateParam((JObject)p))).ToList();
                    var basicParams = ins["basicParams"].Children().Select(
                        p => new BasicParameter(CreateParam((JObject)p))).ToList();
                    var ruleId = ins.Value<int>("rule");
                    instructions.Add(
                        new Instruction
                        (
                            Name: ins.Value<string>("name"),
                            Id: ins.Value<int>("id"),
                            AsmParams: asmParams,
                            BasicParams: basicParams,
                            Rule: Rules[ruleId]
                        ));
                }
                Instructions = instructions;
            }
            Map = new InstructionMap(Instructions);
        }
        private static List<ITokenType> ParseFormat(JEnumerable<JToken> format)
        {
            var list = new List<ITokenType>();
            foreach (JToken token in format)
            {
                if (token.Type == JTokenType.String)
                    list.Add(new FixedString((string)token));
                else if (token.Type == JTokenType.Object && token["placeHolder"] != null)
                    list.Add(CreatePlaceHolder((JObject)token["placeHolder"]));
            }
            return list;
        }
        private static ITokenType CreatePlaceHolder(JObject token)
        {
            ITokenType placeHolder;
            JObject tableID = (JObject)token["tableID"];
            if (token.Value<string>("type") == "basic")
            {
                placeHolder = new BasicPlaceHolder(
                    new PlaceHolder
                    (
                        index: token.Value<int>("index"),
                        name: token.Value<string>("name"),
                        tableId: new TableID
                                (
                                    tableID.Value<int>("index"),
                                    tableID.Value<char>("section")
                                )
                    ));
                return placeHolder;
            }
            else if (token.Value<string>("type") == "asm")
            {
                placeHolder = new AsmPlaceHolder(
                    new PlaceHolder
                    (
                        index: token.Value<int>("index"),
                        name: token.Value<string>("name"),
                        tableId: new TableID
                                (
                                    tableID.Value<int>("index"),
                                    tableID.Value<char>("section")
                                )
                    ));
                return placeHolder;
            }
            else
            {
                throw new FormatException("Expected asm or basic as type value");
            }
        }
        private static Parameter CreateParam(JObject param)
        {
            IConstraintType constraint;
            JObject constraintObj = (JObject)param["constraint"];
            if (constraintObj.Value<string>("type") == "range")
            {
                constraint = new RangeConstraint(
                    new Range
                    (
                        constraintObj.Value<int>("start"),
                        constraintObj.Value<int>("end")
                    )
                );
            }
            else if (constraintObj.Value<string>("type") == "sequence")
            {
                constraint = new SequenceConstraint(
                    new Sequence
                    (
                        constraintObj.Value<int>("start"),
                        constraintObj.Value<int>("end"),
                        constraintObj.Value<int>("step")
                    )
                );
            }
            else if (constraintObj.Value<string>("type") == "constant")
            {
                constraint = new ConstantConstraint(constraintObj.Value<string>("constant"));
            }
            else
            {
                throw new FormatException("expected constant, sequence or range as type value");
            }
            return new Parameter(param.Value<string>("name"), constraint);
        }
    }
}