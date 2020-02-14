using System;
using System.Collections.Generic;

namespace InterpeterExtensions
{
    public static class ConstraintExtensions
    {
        public static bool SatisfyConstraint(this IConstraintType constraint, IExprValue value)
        {
            return
            value.Match(
                constStringValue: strVal =>
                   constraint.Match(
                       constant: strConst => strVal == strConst,
                       sequence: _ => false,
                       range: _ => false
                   ),
                integerValue: intVal =>
                   constraint.Match(
                       constant: _ => false,
                       sequence: sequence => InSequence(sequence, intVal),
                       range: range => InRange(range, intVal)
                   )
            );
        }
        private static bool InSequence(Sequence sequence, int value)
        {
            return InSequenceHelper(sequence.start, sequence.end, sequence.step, value);
        }
        private static bool InRange(Range range, int value)
        {
            return InSequenceHelper(range.start, range.end, 1, value);
        }
        private static bool InSequenceHelper(int start, int end, int step, int value)
        {
            int nVal = value - start;
            int nEnd = end - start;
            int nStart = 0;
            return nStart <= nVal &&
                   nVal <= nEnd &&
                   (nVal % step) == 0;
        }
    }
}
