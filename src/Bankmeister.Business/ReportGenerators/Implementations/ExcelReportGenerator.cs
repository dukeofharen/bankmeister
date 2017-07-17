using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Bankmeister.Models;
using OfficeOpenXml;

namespace Bankmeister.Business.ReportGenerators.Implementations
{
    internal class ExcelReportGenerator : IReportGenerator
    {
        private const string DecimalFormat = "0.##";
        private const string DateFormat = "yyyy-mm-dd";
        private const string DateTimeFormat = "yyyy-mm-dd HH:MM:ss";
        // TODO Make currency configurable
        private const string CurrencyFormat = "€ ###,###,##0.00";

        public string Key => "excel";

        public string Extension => "xlsx";

        public byte[] GenerateReport(ReportModel reportModel)
        {
            using (var package = new ExcelPackage())
            {
                // TODO Strings in resource file
                WriteOverviewSheet(package, reportModel);
                WriteMutationsSheet(package, reportModel);
                WriteRecordHoldersUpSheet(package, reportModel);
                WriteRecordHoldersDownSheet(package, reportModel);
                WriteFrequenciesSheet(package, reportModel);

                return package.GetAsByteArray();
            }
        }

        private static void WriteOverviewSheet(ExcelPackage package, ReportModel model)
        {
            var sheet = package.Workbook.Worksheets.Add("Overview");

            sheet.Column(1).Width = 20;
            sheet.Column(2).Width = 20;

            ExcelRangeBase range;

            range = sheet.Cells[1, 1];
            range.Value = $"Banking overview - {model.BeginDateTime.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} / {model.EndDateTime.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}";
            range.Style.Font.Bold = true;

            range = sheet.Cells[2, 1];
            range.Value = "Start amount";

            range = sheet.Cells[2, 2];
            range.Value = model.StartAmount;
            range.Style.Numberformat.Format = CurrencyFormat;

            range = sheet.Cells[3, 1];
            range.Value = "Final amount";

            range = sheet.Cells[3, 2];
            range.Value = model.EndAmount;
            range.Style.Numberformat.Format = CurrencyFormat;

            range = sheet.Cells[4, 1];
            range.Value = "Total up";

            range = sheet.Cells[4, 2];
            range.Value = model.TotalUp;
            range.Style.Numberformat.Format = CurrencyFormat;

            range = sheet.Cells[5, 1];
            range.Value = "Total down";

            range = sheet.Cells[5, 2];
            range.Value = model.TotalDown;
            range.Style.Numberformat.Format = CurrencyFormat;

            range = sheet.Cells[6, 1];
            range.Value = "Number of mutations";

            range = sheet.Cells[6, 2];
            range.Value = model.Mutations.Count();

            range = sheet.Cells[7, 1];
            range.Value = "Report generated on";

            range = sheet.Cells[7, 2];
            range.Value = DateTime.Now;
            range.Style.Numberformat.Format = DateTimeFormat;
        }

        private static void WriteMutationsSheet(ExcelPackage package, ReportModel model)
        {
            var sheet = package.Workbook.Worksheets.Add("Mutations");

            sheet.Row(1).Style.Font.Bold = true;
            for (int i = 0; i < 7; i++)
            {
                sheet.Column(i + 1).Width = 20;
            }

            sheet.Cells[1, 1].Value = "Date";
            sheet.Cells[1, 2].Value = "Name";
            sheet.Cells[1, 3].Value = "From account";
            sheet.Cells[1, 4].Value = "To account";
            sheet.Cells[1, 5].Value = "Amount";
            sheet.Cells[1, 6].Value = "Mutation type";
            sheet.Cells[1, 7].Value = "Description";

            int counter = 2;
            foreach (var mutation in model.Mutations)
            {
                ExcelRangeBase range;

                range = sheet.Cells[counter, 1];
                range.Value = mutation.DateTime;
                range.Style.Numberformat.Format = DateFormat;

                range = sheet.Cells[counter, 2];
                range.Value = mutation.Name;

                range = sheet.Cells[counter, 3];
                range.Value = mutation.FromAccount;

                range = sheet.Cells[counter, 4];
                range.Value = mutation.ToAccount;

                range = sheet.Cells[counter, 5];
                range.Value = mutation.Amount;
                range.Style.Numberformat.Format = CurrencyFormat;
                if (mutation.Amount < 0)
                {
                    range.Style.Font.Color.SetColor(Color.Crimson);
                }

                range = sheet.Cells[counter, 6];
                range.Value = mutation.MutationType;

                range = sheet.Cells[counter, 7];
                range.Value = mutation.Description;

                counter++;
            }
        }

        private static void WriteFrequenciesSheet(ExcelPackage package, ReportModel model)
        {
            var sheet = package.Workbook.Worksheets.Add("Frequencies");

            sheet.Row(1).Style.Font.Bold = true;
            sheet.Column(1).Width = 30;
            sheet.Column(2).Width = 15;
            sheet.Column(3).Width = 15;

            ExcelRangeBase range;

            range = sheet.Cells[1, 1];
            range.Value = "Between";

            range = sheet.Cells[1, 2];
            range.Value = "Total up";

            range = sheet.Cells[1, 3];
            range.Value = "Total down";

            int counter = 2;
            foreach (var frequency in model.AmountFrequencies)
            {
                string between;
                if (frequency.ToAmount.HasValue)
                {
                    string from = frequency.FromAmount.ToString(DecimalFormat, CultureInfo.InvariantCulture);
                    string to = frequency.ToAmount.Value.ToString(DecimalFormat, CultureInfo.InvariantCulture);
                    between = $"Between € {from} and € {to}";
                }
                else
                {
                    string from = frequency.FromAmount.ToString(DecimalFormat, CultureInfo.InvariantCulture);
                    between = $"Higher than € {from}";
                }

                range = sheet.Cells[counter, 1];
                range.Value = between;

                range = sheet.Cells[counter, 2];
                range.Value = frequency.ToAmount;
                range.Style.Numberformat.Format = CurrencyFormat;

                range = sheet.Cells[counter, 3];
                range.Value = frequency.FrequencyDown;

                counter++;
            }
        }

        private static void WriteRecordHoldersUpSheet(ExcelPackage package, ReportModel model)
        {
            var sheet = package.Workbook.Worksheets.Add("Recordholders up");
            WriteRecordHoldersSheet(sheet, model.RecordHoldersUp);            
        }

        private static void WriteRecordHoldersDownSheet(ExcelPackage package, ReportModel model)
        {
            var sheet = package.Workbook.Worksheets.Add("Recordholders down");
            WriteRecordHoldersSheet(sheet, model.RecordHoldersDown);
        }

        private static void WriteRecordHoldersSheet(ExcelWorksheet sheet, IEnumerable<RecordHolderModel> recordHolders)
        {
            sheet.Row(1).Style.Font.Bold = true;
            sheet.Column(1).Width = 30;
            sheet.Column(2).Width = 15;
            sheet.Column(3).Width = 15;
            sheet.Column(4).Width = 15;

            sheet.Cells[1, 1].Value = "Name";
            sheet.Cells[1, 2].Value = "Total amount";
            sheet.Cells[1, 3].Value = "Frequency";
            sheet.Cells[1, 4].Value = "Average amount";

            int counter = 2;
            foreach (var nameAmountModel in recordHolders)
            {
                ExcelRangeBase range;

                range = sheet.Cells[counter, 1];
                range.Value = nameAmountModel.Name;

                range = sheet.Cells[counter, 2];
                range.Value = nameAmountModel.Amount;
                range.Style.Numberformat.Format = CurrencyFormat;

                range = sheet.Cells[counter, 3];
                range.Value = nameAmountModel.Frequency;

                range = sheet.Cells[counter, 4];
                range.Value = nameAmountModel.AverageAmount;
                range.Style.Numberformat.Format = CurrencyFormat;

                counter++;
            }
        }
    }
}
