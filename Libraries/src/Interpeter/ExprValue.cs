using System;

namespace Libraries
{
    public interface IExprValue
    {
        T Match<T>(
            Func<string, T> constStringValue,
            Func<int, T> integerValue
        );
    }
    public class ConstStringValue : IExprValue
    {
        private readonly string constString;
        public string ConstString { get { return constString; } }
        public ConstStringValue(string constString)
        {
            this.constString = constString;
        }
        public T Match<T>(
            Func<string, T> constStringValue,
            Func<int, T> integerValue
        )
        {
            return constStringValue(constString);
        }

        public override string ToString()
        {
            return constString;
        }
    }
    public class IntegerValue : IExprValue
    {
        private readonly int integer;
        public int Integer { get { return integer; } }
        public IntegerValue(int integer)
        {
            this.integer = integer;
        }
        public T Match<T>(
            Func<string, T> constStringValue,
            Func<int, T> integerValue
        )
        {
            return integerValue(integer);
        }
        public override string ToString()
        {
            return integer.ToString();
        }
    }
}