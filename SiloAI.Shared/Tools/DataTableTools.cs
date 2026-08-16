using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SiloAI.Shared;

public static class DataTableTools
{
    public static DataSet ReadExcelDataExportInDataTable(string Path)
    {
        DataTable dt = new DataTable();

        using SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(Path, false);
        WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
        IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
        string relationshipId = sheets.First().Id.Value;
        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
        Worksheet workSheet = worksheetPart.Worksheet;
        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
        IEnumerable<Row> rows = sheetData.Descendants<Row>();

        foreach (Cell cell in rows.ElementAt(0))
        {
            dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
        }

        foreach (Row row in rows) //this will also include your header row...
        {
            DataRow tempRow = dt.NewRow();
            int columnIndex = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                // Gets the column index of the cell with data
                int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                cellColumnIndex--; //zero based index
                if (columnIndex < cellColumnIndex)
                {
                    do
                    {
                        tempRow[columnIndex] = ""; //Insert blank data here;
                        columnIndex++;
                    }
                    while (columnIndex < cellColumnIndex);
                }
                tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);

                columnIndex++;
            }

            dt.Rows.Add(tempRow);
        }

        dt.Rows.RemoveAt(0); //...so i'm taking it out here.
        DataSet Ds = new DataSet();
        Ds.Tables.Add(dt);
        return Ds;
    }

    private static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
        SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
        if (cell.CellValue == null)
        {
            return "";
        }
        string value = cell.CellValue.InnerXml;
        if (cell.DataType != null && cell.DataType == CellValues.SharedString)
        {
            return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
        }
        //else if (cell.DataType != null && cell.DataType == CellValues.Date)
        //{

        //    return Convert.ToString(Convert.ToDateTime(stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText));
        //}
        else
        {
            return value;
        }

    }

    private static int? GetColumnIndexFromName(string columnName)
    {
        string name = columnName;
        int number = 0;
        int pow = 1;
        for (int i = name.Length - 1; i >= 0; i--)
        {
            number += (name[i] - 'A' + 1) * pow;
            pow *= 26;
        }

        return number;
    }

    public static string GetColumnName(string cellReference)
    {
        Regex regex = new Regex("[A-Za-z]+");
        Match match = regex.Match(cellReference);

        return match.Value;
    }

    public static List<object> DataTableToObjects(DataTable dataTable)
    {
        List<object> rtn = new();

        int colCount = dataTable.Columns.Count;

        foreach (DataRow dr in dataTable.Rows)
        {
            dynamic objExpando = new System.Dynamic.ExpandoObject();

            var obj = objExpando as IDictionary<string, object>;

            for (int i = 0; i < colCount; i++)
            {
                string key = dr.Table.Columns[i].ColumnName.ToString();

                if (dr[key] is null || dr[key] == DBNull.Value)
                {
                    obj[key] = null;
                }
                else
                {
                    obj[key] = dr[key];
                }
            }

            rtn.Add(obj);
        }

        return rtn;
    }

    public static DataTable GetDataTableUsingDisplayAttribute(IList data, Type dataType)
    {
        Dictionary<string, PropertyInfo> columnNamePairProperty = new();

        var propertyList = dataType.GetTypeInfo().GetProperties().ToList();

        DataTable table = new();

        foreach (PropertyInfo prop in propertyList)
        {
            var attrList = prop.GetCustomAttributes().ToList(); //as DisplayAttribute;

            foreach (var attr in attrList)
            {
                if (attr is DisplayAttribute)
                {
                    var name = ((DisplayAttribute)attr).Name;

                    var columnName = ResourceManager.GetString(name);

                    table.Columns.Add(columnName, typeof(string));

                    columnNamePairProperty.Add(columnName, prop);

                    break;
                }
            }
        }

        foreach (var item in data)
        {
            DataRow row = table.NewRow();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                row[i] = columnNamePairProperty[table.Columns[i].ColumnName].GetValue(item) ?? DBNull.Value;
            }

            table.Rows.Add(row);
        }

        return table;
    }

    public static MemoryStream GetExcelFromDataTable(DataTable table)
    {
        MemoryStream stream = new();

        using var workbook = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
        var workbookPart = workbook.AddWorkbookPart();
        workbook.WorkbookPart.Workbook = new();
        workbook.WorkbookPart.Workbook.Sheets = new();

        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new();
        sheetPart.Worksheet = new(sheetData);

        Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
        uint sheetId = 1;

        if (sheets.Elements<Sheet>().Count() > 0)
        {
            sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
        }

        Sheet sheet = new() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
        sheets.Append(sheet);

        Row headerRow = new();
        List<string> columns = new();

        foreach (DataColumn column in table.Columns)
        {
            columns.Add(column.ColumnName);

            Cell cell = new Cell();
            cell.DataType = CellValues.String;
            cell.CellValue = new CellValue(column.ColumnName);

            headerRow.AppendChild(cell);
        }

        sheetData.AppendChild(headerRow);
        foreach (DataRow dsrow in table.Rows)
        {
            Row newRow = new Row();

            foreach (string col in columns)
            {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(dsrow[col].ToString()); //

                newRow.AppendChild(cell);
            }

            sheetData.AppendChild(newRow);
        }

        return stream;
    }

    public static DataTable ReadExcelDataOutDataTable(string Path)
    {
        DataTable dt = new DataTable();

        using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(Path, false))
        {
            WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
            IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
            Worksheet workSheet = worksheetPart.Worksheet;
            SheetData sheetData = workSheet.GetFirstChild<SheetData>();
            IEnumerable<Row> rows = sheetData.Descendants<Row>();

            foreach (Cell cell in rows.ElementAt(0))
            {
                dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
            }
            foreach (Row row in rows) //this will also include your header row...
            {
                DataRow tempRow = dt.NewRow();
                int columnIndex = 0;
                foreach (Cell cell in row.Descendants<Cell>())
                {
                    var cellRefrence = cell.CellReference;

                    if (cellRefrence is not null)
                    {
                        // Gets the column index of the cell with data
                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                        cellColumnIndex--; //zero based index
                        if (columnIndex < cellColumnIndex)
                        {
                            do
                            {
                                tempRow[columnIndex] = ""; //Insert blank data here;
                                columnIndex++;
                            }
                            while (columnIndex < cellColumnIndex);
                        }
                        tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);

                        columnIndex++;
                    }
                }

                dt.Rows.Add(tempRow);
            }

        }
        dt.Rows.RemoveAt(0);

        return dt;
    }

    public static DataTable ReadExcelDataOutDataTable(Stream stream)
    {
        DataTable dt = new DataTable();

        using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(stream, false))
        {
            WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
            IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
            Worksheet workSheet = worksheetPart.Worksheet;
            SheetData sheetData = workSheet.GetFirstChild<SheetData>();
            IEnumerable<Row> rows = sheetData.Descendants<Row>();

            foreach (Cell cell in rows.ElementAt(0))
            {
                dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
            }
            foreach (Row row in rows) //this will also include your header row...
            {
                DataRow tempRow = dt.NewRow();
                int columnIndex = 0;
                foreach (Cell cell in row.Descendants<Cell>())
                {
                    var cellRefrence = cell.CellReference;

                    if (cellRefrence is not null)
                    {
                        // Gets the column index of the cell with data
                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                        cellColumnIndex--; //zero based index
                        if (columnIndex < cellColumnIndex)
                        {
                            do
                            {
                                tempRow[columnIndex] = ""; //Insert blank data here;
                                columnIndex++;
                            }
                            while (columnIndex < cellColumnIndex);
                        }
                        tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);

                        columnIndex++;
                    }
                }

                dt.Rows.Add(tempRow);
            }

        }
        dt.Rows.RemoveAt(0);

        return dt;
    }
}
