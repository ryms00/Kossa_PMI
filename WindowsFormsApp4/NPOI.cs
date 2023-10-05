using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//공통 NPOI
using NPOI;
using NPOI.SS.UserModel;
//표준 xls 버젼 excel시트
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
//확장 xlsx 버젼 excel 시트
using NPOI.XSSF;
using NPOI.XSSF.UserModel;


namespace WindowsFormsApp4
{
    class NPOI
    {
        public NPOI()
        {
            //var version = "xls";
            ////var version = "xlsx";
            //// Workbook 생성.
            //var workbook = CreateWorkbook(version);

            //// Workbook안에 시트 생성.
            //var sheet = workbook.CreateSheet("Test Sheet");
            //// Sheet에서 셀 취득
            //var cell = GetCell(sheet, 0, 0);
            //// 셀에 데이터 작성
            //cell.SetCellValue("TEST Result");

            //cell = GetCell(sheet, 0, 1);
            //cell.SetCellValue(100);

            //cell = GetCell(sheet, 0, 2);
            //cell.SetCellValue(DateTime.Now);


            //// 셀에 데이터 포멧 지정
            //var style = workbook.CreateCellStyle();

            //// 날짜 포멧
            //style.DataFormat = HSSFDataFormat.GetBuiltinFormat("m/d/yy");
            //// 정렬 포멧
            //style.Alignment = HorizontalAlignment.Center;
            //style.VerticalAlignment = VerticalAlignment.Top;
            //// 셀 색지정
            //style.FillBackgroundColor = IndexedColors.Gold.Index;
            //// 폰트 설정
            //var font = workbook.CreateFont();
            //font.Color = IndexedColors.Red.Index;
            //cell.CellStyle = style;

            ////셀 너비 자동 지정
            //sheet.AutoSizeColumn(0);
            //sheet.AutoSizeColumn(1);
            //sheet.AutoSizeColumn(2);

            //cell = GetCell(sheet, 1, 0);
            //cell.SetCellValue(1);

            //cell = GetCell(sheet, 1, 1);
            //cell.SetCellValue(2);

            //cell = GetCell(sheet, 1, 2);
            ////함수식
            //cell.SetCellFormula("SUM(A2:B2)");

            //// WriteExcel(workbook, @"d:\work\test." + version);


        }
        public IWorkbook CreateWorkbook(string version)
        {
            //표준 xls 버젼
            if ("xls".Equals(version))
            {
                return new HSSFWorkbook();
            }
            //확장 xlsx 버젼
            else if ("xlsx".Equals(version))
            {
                return new XSSFWorkbook();
            }
            throw new NotSupportedException();
        }

        public IRow GetRow(ISheet sheet, int rownum)
        {
            var row = sheet.GetRow(rownum);
            if (row == null)
            {
                row = sheet.CreateRow(rownum);
            }
            return row;
        }
        public ICell GetCell(IRow row, int cellnum)
        {
            var cell = row.GetCell(cellnum);
            if (cell == null)
            {
                cell = row.CreateCell(cellnum);
            }
            return cell;
        }

        public ICell GetCell(ISheet sheet, int rownum, int cellnum)
        {
            var row = GetRow(sheet, rownum);
            return GetCell(row, cellnum);
        }
        public void WriteExcel(IWorkbook workbook, string filepath)
        {
            using (var file = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(file);
            }
        }

        public void GetExecl(string path, string extension)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                ISheet sheet = null;
                IWorkbook workbook = null;

                if (".xls".Equals(extension))
                    workbook = new HSSFWorkbook(fileStream);

                else if (".xlsx".Equals(extension))
                    workbook = new XSSFWorkbook(fileStream);

                else
                    throw new NotSupportedException();


                sheet = workbook.GetSheetAt(0);

                int columnIndex = 0; 

                for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row != null)
                    {
                        ICell cell = row.GetCell(columnIndex);
                        string cellValue = cell?.ToString(); 

                        if (cell.CellType == CellType.Numeric)
                        {
                            double numericValue = cell.NumericCellValue;
                        }
                        Console.WriteLine($"Row {rowIndex + 1}, Column {columnIndex + 1}: {cellValue}");
                    }
                }
            }
        }
    }
}
