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
    Description = "Returns the geometric mean of an array or range of positive data.")]
    internal class Rsq : ExcelFunction
    {
        public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
        {
            ValidateArguments(arguments, 2);
            var arg1 = arguments.ElementAt(0);
            var arg2 = arguments.ElementAt(1);
            var knownXs = ArgsToDoubleEnumerable(false, false, new FunctionArgument[] { arg1 }, context).ToArray();
            var knownYs = ArgsToDoubleEnumerable(false, false, new FunctionArgument[] { arg2 }, context).ToArray();
            var result = System.Math.Pow(Pearson.PearsonImpl(knownXs.Select(x => x.Value).ToArray(), knownYs.Select(x => x.Value).ToArray()), 2);
            return CreateResult(result, DataType.Decimal);
        }
    }
}
