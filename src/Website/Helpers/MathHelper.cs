using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Helpers
{
    public static class MathHelper
    {
        /// <summary>
        /// Rounds to the inputted number of significant figures
        /// </summary>
        /// <param name="input"></param>
        /// <param name="significantFigures"></param>
        /// <returns></returns>
        public static double ToSignificantFigures(this double input, int significantFigures)
        {
            if (input == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(input))) + 1 - significantFigures);
            return scale * Math.Truncate(input / scale);
        }

        /// <summary>
        /// Rounds to the inputted number of significant figures
        /// </summary>
        /// <param name="input"></param>
        /// <param name="significantFigures"></param>
        /// <returns></returns>
        public static double? ToSignificantFigures(this double? input, int significantFigures)
        {
            if (input == null || input == 0)
                return 0;

            return ToSignificantFigures((double)input, significantFigures);
        }
    }
}
