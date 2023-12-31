﻿/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  22/10/2022         EPPlus Software AB           EPPlus v6
 *************************************************************************************************/
using OfficeOpenXml.FormulaParsing.Excel.Functions.Metadata;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Statistical
{
    [FunctionMetadata(
        Category = ExcelFunctionCategory.Statistical,
        EPPlusVersion = "6.0",
        Description = "Calculate, or predict, a future value by using existing values. The future value is a y-value for a given x-value.")]
    internal class Forecast : ExcelFunction
    {
        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
        {
            ValidateArguments(arguments, 3);
            var x = ArgToDecimal(arguments, 0);
            var arg1 = arguments.ElementAt(1);
            var arg2 = arguments.ElementAt(2);
            var arrayY = ArgsToDoubleEnumerable(false, false, new FunctionArgument[] { arg1 }, context).Select(a => a.Value).ToArray();
            var arrayX = ArgsToDoubleEnumerable(false, false, new FunctionArgument[] { arg2 }, context).Select(b => b.Value).ToArray();
            if (arrayY.Count() != arrayX.Count()) return CreateResult(eErrorType.NA);
            if (!arrayY.Any()) return CreateResult(eErrorType.NA);
            var result = ForecastImpl(x, arrayY, arrayX);
            return CreateResult(result, DataType.Decimal);
        }

        internal static double ForecastImpl(double x, double[] arrayY, double[] arrayX)
        {
            var avgY = arrayY.Average();
            var avgX = arrayX.Average();
            var nItems = arrayY.Length;
            var upperEquationPart = 0d;
            var lowerEquationPart = 0d;
            for (var ix = 0; ix < nItems; ix++)
            {
                upperEquationPart += (arrayX[ix] - avgX) * (arrayY[ix] - avgY);
                lowerEquationPart += System.Math.Pow(arrayX[ix] - avgX, 2);
            }
            var b = upperEquationPart / lowerEquationPart;
            var a = avgY - b * avgX;
            return a + b * x;
        }
    }
}
