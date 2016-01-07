using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Config
{
    public class FormatStringExpression
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">{Name} - {Created} ({Created})</param>
        public FormatStringExpression(string expression)
        {
            PropertyNames = new List<string>();

            string pattern = @"(?<=\{)(.*?)(?=\})";
            foreach (Match match in Regex.Matches(expression, pattern))
            {
                if (!PropertyNames.Contains(match.Value))
                {
                    PropertyNames.Add(match.Value);
                }
            }

            FormatString = Regex.Replace(expression, pattern, this.ReplaceNameByIndex);
        }
        /// <summary>
        /// {"Name","Created"}
        /// </summary>
        public List<string> PropertyNames { get; private set; }

        /// <summary>
        /// {0} - {1} ({1})
        /// </summary>
        public string FormatString { get; private set; }

        private string ReplaceNameByIndex(Match m)
        {
            return PropertyNames.IndexOf(m.Value).ToString();
        }

    }
}