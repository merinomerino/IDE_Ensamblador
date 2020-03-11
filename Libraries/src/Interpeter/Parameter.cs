using System;

namespace Libraries
{
    public class Parameter
    {
        public string name { get; }
        public IConstraintType constraint { get; }
        public Parameter(string name, IConstraintType constraint)
        {
            this.name = name;
            this.constraint = constraint;
        }
    }
    public class AsmParameter : IParameterType
    {
        private readonly Parameter parameter;
        public Parameter Parameter { get { return parameter; } }
        public AsmParameter(Parameter parameter)
        {
            this.parameter = parameter;
        }
        public T Match<T>(
            Func<Parameter, T> asmParameter,
            Func<Parameter, T> basicParameter
        )
        {
            return asmParameter(parameter);
        }
    }
    public class BasicParameter : IParameterType
    {
        private readonly Parameter parameter;
        public Parameter Parameter { get { return parameter; } }
        public BasicParameter(Parameter parameter)
        {
            this.parameter = parameter;
        }
        public T Match<T>(
            Func<Parameter, T> asmParameter,
            Func<Parameter, T> basicParameter
        )
        {
            return basicParameter(parameter);
        }
    }
    public interface IParameterType
    {
        T Match<T>(
          Func<Parameter, T> asmParameter,
          Func<Parameter, T> basicParameter
      );
    }
}