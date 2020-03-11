using System;

namespace Libraries
{
    public class Sequence
    {
        public int start { get; }
        public int end { get; }
        public int step { get; }
        public Sequence(int start, int end, int step)
        {
            this.start = start;
            this.end = end;
            this.step = step;
        }
    }
    public class Range
    {
        public int start { get; }
        public int end { get; }
        public Range(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
    public class ConstantConstraint : IConstraintType
    {
        public override int order { get; }
        private readonly string constant;
        public string Constant { get { return constant; } }
        public ConstantConstraint(string constant)
        {
            this.order = 3;
            this.constant = constant;
        }
        public override T Match<T>(
            Func<string, T> constant,
            Func<Sequence, T> sequence,
            Func<Range, T> range
        )
        {
            return constant(this.constant);
        }
    }
    public class SequenceConstraint : IConstraintType
    {
        public override int order { get; }
        private readonly Sequence sequence;
        public Sequence Sequence { get { return sequence; } }
        public SequenceConstraint(Sequence sequence)
        {
            this.order = 2;
            this.sequence = sequence;
        }
        public override T Match<T>(
            Func<string, T> constant,
            Func<Sequence, T> sequence,
            Func<Range, T> range
        )
        {
            return sequence(this.sequence);
        }
    }
    public class RangeConstraint : IConstraintType
    {
        public override int order { get; }
        private readonly Range range;
        public Range Range { get { return range; } }
        public RangeConstraint(Range range)
        {
            this.order = 1;
            this.range = range;
        }
        public override T Match<T>(
            Func<string, T> constant,
            Func<Sequence, T> sequence,
            Func<Range, T> range
        )
        {
            return range(this.range);
        }

    }
    public abstract class IConstraintType : IComparable<IConstraintType>
    {
        public abstract int order { get; }
        public int CompareTo(IConstraintType other)
        {
            if (other == null)
                return 1;
            else
                return order.CompareTo(other.order);
        }
        public abstract T Match<T>(
            Func<string, T> constant,
            Func<Sequence, T> sequence,
            Func<Range, T> range
        );
    }
}