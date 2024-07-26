using System.Collections.Generic;

using VoraciousEBookReader.EPUB.Interfaces;

namespace VoraciousEBookReader.EPUB.Searching
{
    public class SearchOperator : ISearch
    {
        public SearchOperator(Operator op, List<ISearch> operands)
        {
            if (op == Operator.Invalid) op = Operator.And; // set to be the default search type.
            Op = op;
            foreach (var item in operands)
            {
                Operands.Add(item);
            }
        }

        public enum Operator { Invalid, And, Or }
        public Operator Op { get; set; } = Operator.And;
        public bool IsNegated { get; set; } = false;
        public void SetIsNegated(bool isNegated)
        {
            IsNegated = isNegated;
        }
        public static Operator ConvertToOperator(char ch)
        {
            switch (ch)
            {
                case '&': return Operator.And;
                case '|': return Operator.Or;
                default: return Operator.Invalid;
            }
        }
        public List<ISearch> Operands { get; } = new List<ISearch>();
        public bool Matches(IGetSearchArea searchObject)
        {
            switch (Op)
            {
                default:
                case Operator.Invalid: // should not happen when parsing is correct.
                    return false;
                case Operator.And:
                    if (Operands.Count == 0) return true; // should not every happen gotta return something...
                    foreach (var item in Operands)
                    {
                        var value = item.Matches(searchObject);
                        if (!value) return IsNegated ? true : false;
                    }
                    return IsNegated ? false : true;
                case Operator.Or:
                    if (Operands.Count == 0) return true; // should not every happen gotta return something...
                    foreach (var item in Operands)
                    {
                        var value = item.Matches(searchObject);
                        if (value) return IsNegated ? false : true;
                    }
                    return IsNegated ? true : false;
            }
        }

        public bool MatchesFlat(string search) // is the giant gnarly per-book string in the index file
        {
            if (IsNegated) return true;
            switch (Op)
            {
                default:
                case Operator.Invalid: // should not happen when parsing is correct.
                    return false;
                case Operator.And:
                    if (Operands.Count == 0) return true; // should not every happen gotta return something...
                    foreach (var item in Operands)
                    {
                        var value = item.MatchesFlat(search);
                        if (!value) return false;
                    }
                    return true;
                case Operator.Or:
                    if (Operands.Count == 0) return true; // should not every happen gotta return something...
                    foreach (var item in Operands)
                    {
                        var value = item.MatchesFlat(search);
                        if (value) return true;
                    }
                    return false;
            }
        }

        public override string ToString()
        {
            if (Operands.Count == 0) return "[[null]]";

            var retval = Operands[0] is SearchAtom ? Operands[0].ToString() : $"( {Operands[0].ToString()} )";
            for (int i = 1; i < Operands.Count; i++)
            {
                switch (Op)
                {
                    case Operator.And: retval += " & "; break;
                    case Operator.Or: retval += " | "; break;
                }
                var opstr = Operands[i] is SearchAtom ? Operands[i].ToString() : $"( {Operands[i].ToString()} )";
                retval += opstr;
            }
            if (IsNegated) retval = $"!( {retval} )";
            return retval;
        }
    }
}
