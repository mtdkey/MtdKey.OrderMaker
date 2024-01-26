using System.Globalization;
using System.Linq;
using System;

namespace MtdKey.OrderMaker.Core.Scripts.StoreIds
{
    public class FieldsFilter : FilterHandler
    {
        public override string ReplaceFilter(string script, FilterSQLparams filter)
        {
      
            if (filter.DocFieldModels.Count > 0)
            {
                var fieldIds = string.Empty;
                filter.DocFieldModels.ForEach(field =>
                {
                    if (fieldIds != string.Empty) fieldIds += ",";
                    fieldIds += $"'{field.Id}'";
                });

                script = script.Replace("/*and FieldId in*/", $" and FieldId in ({fieldIds})");
            }

            filter.FilterFields.ForEach(field =>
            {
                FilterDateHandler(ref script, field);
                FilterTextHandler(ref script, field);
                FilterMemoHandler(ref script, field);
                FilterIntHandler(ref script, field);
                FilterDecimalHandler(ref script, field);
            });


            return script;
        }


        private static void FilterDateHandler(ref string script, FilterFieldModel model)
        {
            bool IsDate = model.Type == FieldType.Date || model.Type == FieldType.DateTime;
            if (IsDate is not true) return;

            string[] dates = model.Value.Split("***").ToArray();
            if (!DateTime.TryParseExact(dates[0], model.ValueExt, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateStart) ||
               !DateTime.TryParseExact(dates[1], model.ValueExt, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateFinish))
                return;
            var term = script.Contains("and (Result") ? "or" : "and";
            script = script.Replace("/*and Result*/",
                $" {term} (Result between '{dateStart:yyyy-MM-dd 00:00:00}' " +
                $"and '{dateFinish:yyyy-MM-dd 23:59:59}') /*and Result*/ ");
        }

        private static void FilterTextHandler(ref string script, FilterFieldModel model)
        {
            bool IsText = model.Type == FieldType.Text || model.Type == FieldType.Link;
            if (IsText is not true) return;
            TermHandler(ref script, model);

        }

        private static void FilterMemoHandler(ref string script, FilterFieldModel model)
        {
            bool IsMemo = model.Type == FieldType.Memo;
            if (IsMemo is not true) return;
            TermHandler(ref script, model);
        }

        private static void FilterIntHandler(ref string script, FilterFieldModel model)
        {
            bool IsInt = model.Type == FieldType.Int || model.Type == FieldType.Checkbox;
            if (IsInt is not true) return;
            TermHandler(ref script, model);
        }

        private static void FilterDecimalHandler(ref string script, FilterFieldModel model)
        {
            bool IsDecimal = model.Type == FieldType.Decimal;
            if (IsDecimal is not true) return;
            TermHandler(ref script, model);
        }

        private static void TermHandler(ref string script, FilterFieldModel model)
        {
            var value = model.Value;
            var term = script.Contains("and (Result") ? "or" : "and";

            string result = $" {term} (Result like '%{value}%' and FieldId = '{model.FieldId}') /*and Result*/";
            switch (model.Term)
            {
                case 1:
                    {
                        result = $" {term} (Result = '{value}' and FieldId = '{model.FieldId}') /*and Result*/";
                        break;
                    }
                case 2:
                    {
                        result = $" {term} (Result < '{value}' and FieldId = '{model.FieldId}') /*and Result*/";
                        break;
                    }
                case 3:
                    {
                        result = $" {term} (Result > '{value}' and FieldId = '{model.FieldId}') /*and Result*/";
                        break;
                    }
                case 5:
                    {
                        result = $" {term} (Result <> '{value}' and FieldId = '{model.FieldId}') /*and Result*/";
                        break;
                    }
            }

            script = script.Replace("/*and Result*/", result);
        }
    }
}
