/*----------------------------------------------------------------
// ��Ȩ��http://XingFuStar.cnblogs.com
//
// �ļ����� OpeareExcel
// �ļ����������� ��C#������Excel,
//
// ���ߣ�XingFuStar
// ���ڣ�2007��8��10��
//
// ��ǰ�汾��V1.0.2
//
// �޸����ڣ�2007��8��13��
// �޸����ݣ����Ӵ򿪱���ȹ���
// �޸����ڣ�2007��9��12��
// �޸����ݣ��޸Ĺر�Excelʱ�ṩ����ѡ��
//----------------------------------------------------------------*/


using System;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace Test
{
    class ExcelEdit
    {
        string myFileName;
        Excel.Application myExcel;
        Excel.Workbook myWorkBook;

        /// <summary>
        /// ���캯����������Excel������
        /// </summary>
        public ExcelEdit()
        {
            //�벻Ҫɾ��������Ϣ
            //��Ȩ��http://XingFuStar.cnblogs.com
        }

        /// <summary>
        /// ����Excel������
        /// </summary>
        public void CreateExcel()
        {
            myExcel = new Excel.Application();
            myWorkBook = myExcel.Application.Workbooks.Add(true);
        }

        /// <summary>
        /// ��ʾExcel
        /// </summary>
        public void ShowExcel()
        {
            myExcel.Visible = true;
        }

        /// <summary>
        /// ������д��Excel
        /// </summary>
        /// <param name="data">Ҫд��Ķ�ά��������</param>
        /// <param name="startRow">Excel�е���ʼ��</param>
        /// <param name="startColumn">Excel�е���ʼ��</param>
        public void WriteData(string[,] data, int startRow, int startColumn)
        {
            int rowNumber = data.GetLength(0);
            int columnNumber = data.GetLength(1);

            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    //��Excel�У����ĳ��Ԫ���Ե����š�'����ͷ����ʾ�õ�Ԫ��Ϊ���ı�����ˣ�������ÿ����Ԫ��ǰ��ӵ����š� 
                    myExcel.Cells[startRow + i, startColumn + j] = "'" + data[i, j];
                }
            }
        }

        /// <summary>
        /// ������д��Excel
        /// </summary>
        /// <param name="data">Ҫд����ַ���</param>
        /// <param name="starRow">д�����</param>
        /// <param name="startColumn">д�����</param>
        public void WriteData(string data, int row, int column)
        {
            myExcel.Cells[row, column] = data;
        }

        /// <summary>
        /// ������д��Excel
        /// </summary>
        /// <param name="data">Ҫд������ݱ�</param>
        /// <param name="startRow">Excel�е���ʼ��</param>
        /// <param name="startColumn">Excel�е���ʼ��</param>
        public void WriteData(System.Data.DataTable data, int startRow, int startColumn)
        {
            for (int i = 0; i <= data.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= data.Columns.Count - 1; j++)
                {
                    //��Excel�У����ĳ��Ԫ���Ե����š�'����ͷ����ʾ�õ�Ԫ��Ϊ���ı�����ˣ�������ÿ����Ԫ��ǰ��ӵ����š� 
                    myExcel.Cells[startRow + i, startColumn + j] = "'" + data.Rows[i][j].ToString();
                }
            }
        }

        /// <summary>
        /// ��ȡָ����Ԫ������
        /// </summary>
        /// <param name="row">�����</param>
        /// <param name="column">�����</param>
        /// <returns>�ø������</returns>
        public string ReadData(int row, int column)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[row, column], myExcel.Cells[row, column]);
            return range.Text.ToString();
        }

        /// <summary>
        /// ��Excel�в���ͼƬ
        /// </summary>
        /// <param name="pictureName">ͼƬ�ľ���·�����ļ���</param>
        public void InsertPictures(string pictureName)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            //��������ֱ�ʾλ�ã�λ��Ĭ��
            worksheet.Shapes.AddPicture(pictureName, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        }

        /// <summary>
        /// ��Excel�в���ͼƬ
        /// </summary>
        /// <param name="pictureName">ͼƬ�ľ���·�����ļ���</param>
        /// <param name="left">��߾�</param>
        /// <param name="top">�ұ߾�</param>
        /// <param name="width">��</param>
        /// <param name="heigth">��</param>
        public void InsertPictures(string pictureName, int left, int top, int width, int heigth)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            worksheet.Shapes.AddPicture(pictureName, MsoTriState.msoFalse, MsoTriState.msoTrue, top, left, heigth, width);
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="sheetNum">��������ţ������ң���1��ʼ</param>
        /// <param name="newSheetName">�µĹ�������</param>
        public void ReNameSheet(int sheetNum, string newSheetName)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.Worksheets[sheetNum];
            worksheet.Name = newSheetName;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="oldSheetName">ԭ�й�������</param>
        /// <param name="newSheetName">�µĹ�������</param>
        public void ReNameSheet(string oldSheetName, string newSheetName)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.Worksheets[oldSheetName];
            worksheet.Name = newSheetName;
        }

        /// <summary>
        /// �½�������
        /// </summary>
        /// <param name="sheetName">��������</param>
        public void CreateWorkSheet(string sheetName)
        {
            Excel.Worksheet newWorksheet = (Excel.Worksheet)myWorkBook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            newWorksheet.Name = sheetName;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sheetName">��������</param>
        public void ActivateSheet(string sheetName)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.Worksheets[sheetName];
            worksheet.Activate();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sheetNum">���������</param>
        public void ActivateSheet(int sheetNum)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.Worksheets[sheetNum];
            worksheet.Activate();
        }

        /// <summary>
        /// ɾ��һ��������
        /// </summary>
        /// <param name="SheetName">ɾ���Ĺ�������</param>
        public void DeleteSheet(int sheetNum)
        {
            ((Excel.Worksheet)myWorkBook.Worksheets[sheetNum]).Delete();
        }

        /// <summary>
        /// ɾ��һ��������
        /// </summary>
        /// <param name="SheetName">ɾ���Ĺ��������</param>
        public void DeleteSheet(string sheetName)
        {
            ((Excel.Worksheet)myWorkBook.Worksheets[sheetName]).Delete();
        }

        /// <summary>
        /// �ϲ���Ԫ��
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        public void CellsUnite(int startRow, int startColumn, int endRow, int endColumn)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.MergeCells = true;
        }

        /// <summary>
        /// ��Ԫ�����ֶ��뷽ʽ
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="hAlign">ˮƽ����</param>
        /// <param name="vAlign">��ֱ����</param>
        public void CellsAlignment(int startRow, int startColumn, int endRow, int endColumn, ExcelHAlign hAlign, ExcelVAlign vAlign)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.HorizontalAlignment = hAlign;
            range.VerticalAlignment = vAlign;
        }

        /// <summary>
        /// ����ָ����Ԫ��ı߿�
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        public void CellsDrawFrame(int startRow, int startColumn, int endRow, int endColumn)
        {
            CellsDrawFrame(startRow, startColumn, endRow, endColumn,
                true, true, true, true, true, true, false, false,
                LineStyle.����ֱ��, BorderWeight.ϸ, ColorIndex.�Զ�);
        }

        /// <summary>
        /// ����ָ����Ԫ��ı߿�
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="isDrawTop">�Ƿ������</param>
        /// <param name="isDrawBottom">�Ƿ������</param>
        /// <param name="isDrawLeft">�Ƿ������</param>
        /// <param name="isDrawRight">�Ƿ������</param>
        /// <param name="isDrawHInside">�Ƿ�ˮƽ�ڿ�</param>
        /// <param name="isDrawVInside">�Ƿ񻭴�ֱ�ڿ�</param>
        /// <param name="isDrawDown">�Ƿ�б������</param>
        /// <param name="isDrawUp">�Ƿ�б������</param>
        /// <param name="lineStyle">������</param>
        /// <param name="borderWeight">�ߴ�ϸ</param>
        /// <param name="color">����ɫ</param>
        public void CellsDrawFrame(int startRow, int startColumn, int endRow, int endColumn,
            bool isDrawTop, bool isDrawBottom, bool isDrawLeft, bool isDrawRight,
            bool isDrawHInside, bool isDrawVInside, bool isDrawDiagonalDown, bool isDrawDiagonalUp,
            LineStyle lineStyle, BorderWeight borderWeight, ColorIndex color)
        {
            //��ȡ���߿�ĵ�Ԫ��
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);

            //������б߿�
            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = LineStyle.��;
            range.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = LineStyle.��;

            //�����ǰ��������߿� 
            if (isDrawTop)
            {
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlEdgeTop].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlEdgeTop].ColorIndex = color;
            }

            if (isDrawBottom)
            {
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlEdgeBottom].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlEdgeBottom].ColorIndex = color;
            }

            if (isDrawLeft)
            {
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlEdgeLeft].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlEdgeLeft].ColorIndex = color;
            }

            if (isDrawRight)
            {
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlEdgeRight].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlEdgeRight].ColorIndex = color;
            }

            if (isDrawVInside)
            {
                range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlInsideVertical].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlInsideVertical].ColorIndex = color;
            }

            if (isDrawHInside)
            {
                range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlInsideHorizontal].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlInsideHorizontal].ColorIndex = color;
            }

            if (isDrawDiagonalDown)
            {
                range.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlDiagonalDown].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlDiagonalDown].ColorIndex = color;
            }

            if (isDrawDiagonalUp)
            {
                range.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = lineStyle;
                range.Borders[XlBordersIndex.xlDiagonalUp].Weight = borderWeight;
                range.Borders[XlBordersIndex.xlDiagonalUp].ColorIndex = color;
            }
        }

        /// <summary>
        /// ��Ԫ�񱳾�ɫ����䷽ʽ
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="color">��ɫ����</param>
        public void CellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.Interior.ColorIndex = color;
            range.Interior.Pattern = Pattern.Solid;
        }

        /// <summary>
        /// ��Ԫ�񱳾�ɫ����䷽ʽ
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="color">��ɫ����</param>
        /// <param name="pattern">��䷽ʽ</param>
        public void CellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color, Pattern pattern)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.Interior.ColorIndex = color;
            range.Interior.Pattern = pattern;
        }

        /// <summary>
        /// �����и�
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="height">�и�</param>
        public void SetRowHeight(int startRow, int endRow, int height)
        {
            //��ȡ��ǰ����ʹ�õĹ�����
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Rows[startRow.ToString() + ":" + endRow.ToString(), System.Type.Missing];
            range.RowHeight = height;
        }

        /// <summary>
        /// �Զ������и�
        /// </summary>
        /// <param name="columnNum">�к�</param>
        public void RowAutoFit(int rowNum)
        {
            //��ȡ��ǰ����ʹ�õĹ�����
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Rows[rowNum.ToString() + ":" + rowNum.ToString(), System.Type.Missing];
            range.EntireColumn.AutoFit();

        }

        /// <summary>
        /// �����п�
        /// </summary>
        /// <param name="startColumn">��ʼ��(�ж�Ӧ����ĸ)</param>
        /// <param name="endColumn">������(�ж�Ӧ����ĸ)</param>
        /// <param name="width"></param>
        public void SetColumnWidth(string startColumn, string endColumn, int width)
        {
            //��ȡ��ǰ����ʹ�õĹ�����
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[startColumn + ":" + endColumn, System.Type.Missing];
            range.ColumnWidth = width;
        }

        /// <summary>
        /// �����п�
        /// </summary>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endColumn">������</param>
        /// <param name="width"></param>
        public void SetColumnWidth(int startColumn, int endColumn, int width)
        {
            string strStartColumn = GetColumnName(startColumn);
            string strEndColumn = GetColumnName(endColumn);
            //��ȡ��ǰ����ʹ�õĹ�����
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[strStartColumn + ":" + strEndColumn, System.Type.Missing];
            range.ColumnWidth = width;
        }

        /// <summary>
        /// �Զ������п�
        /// </summary>
        /// <param name="columnNum">�к�</param>
        public void ColumnAutoFit(string column)
        {
            //��ȡ��ǰ����ʹ�õĹ�����
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[column + ":" + column, System.Type.Missing];
            range.EntireColumn.AutoFit();

        }

        /// <summary>
        /// �Զ������п�
        /// </summary>
        /// <param name="columnNum">�к�</param>
        public void ColumnAutoFit(int columnNum)
        {
            string strcolumnNum = GetColumnName(columnNum);
            //��ȡ��ǰ����ʹ�õĹ�����
            Excel.Worksheet worksheet = (Excel.Worksheet)myExcel.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[strcolumnNum + ":" + strcolumnNum, System.Type.Missing];
            range.EntireColumn.AutoFit();

        }

        /// <summary>
        /// ������ɫ
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="color">��ɫ����</param>
        public void FontColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.Font.ColorIndex = color;
        }

        /// <summary>
        /// ������ʽ(�Ӵ�,б��,�»���)
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="isBold">�Ƿ�Ӵ�</param>
        /// <param name="isItalic">�Ƿ�б��</param>
        /// <param name="underline">�»�������</param>
        public void FontStyle(int startRow, int startColumn, int endRow, int endColumn, bool isBold, bool isItalic, UnderlineStyle underline)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.Font.Bold = isBold;
            range.Font.Underline = underline;
            range.Font.Italic = isItalic;
        }

        /// <summary>
        /// ��Ԫ�����弰��С
        /// </summary>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startColumn">��ʼ��</param>
        /// <param name="endRow">������</param>
        /// <param name="endColumn">������</param>
        /// <param name="fontName">��������</param>
        /// <param name="fontSize">�����С</param>
        public void FontNameSize(int startRow, int startColumn, int endRow, int endColumn, string fontName, int fontSize)
        {
            Excel.Range range = myExcel.get_Range(myExcel.Cells[startRow, startColumn], myExcel.Cells[endRow, endColumn]);
            range.Font.Name = fontName;
            range.Font.Size = fontSize;
        }

        /// <summary>
        /// ��һ�����ڵ�Excel�ļ�
        /// </summary>
        /// <param name="fileName">Excel����·�����ļ���</param>
        public void Open(string fileName)
        {
            myExcel = new Excel.Application();
            myWorkBook = myExcel.Workbooks.Add(fileName);
            myFileName = fileName;
        }

        /// <summary>
        /// ����Excel
        /// </summary>
        /// <returns>����ɹ�����True</returns>
        public bool Save()
        {
            if (myFileName == "")
            {
                return false;
            }
            else
            {
                try
                {
                    myWorkBook.Save();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Excel�ĵ����Ϊ
        /// </summary>
        /// <param name="fileName">��������·�����ļ���</param>
        /// <returns>����ɹ�����True</returns>
        public bool SaveAs(string fileName)
        {
            try
            {
                myWorkBook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
        }

        /// <summary>
        /// �ر�Excel
        /// </summary>
        public void Close()
        {
            myWorkBook.Close(Type.Missing, Type.Missing, Type.Missing);
            myExcel.Quit();
            myWorkBook = null;
            myExcel = null;
            GC.Collect();
        }

        /// <summary>
        /// �ر�Excel
        /// </summary>
        /// <param name="isSave">�Ƿ񱣴�</param>
        public void Close(bool isSave)
        {
            myWorkBook.Close(isSave, Type.Missing, Type.Missing);
            myExcel.Quit();
            myWorkBook = null;
            myExcel = null;
            GC.Collect();
        }

        /// <summary>
        /// �ر�Excel
        /// </summary>
        /// <param name="isSave">�Ƿ񱣴�</param>
        /// <param name="fileName">�洢�ļ���</param>
        public void Close(bool isSave, string fileName)
        {
            myWorkBook.Close(isSave, fileName, Type.Missing);
            myExcel.Quit();
            myWorkBook = null;
            myExcel = null;
            GC.Collect();
        }

        #region ˽�г�Ա
        private string GetColumnName(int number)
        {
            int h, l;
            h = number / 26;
            l = number % 26;
            if (l == 0)
            {
                h -= 1;
                l = 26;
            }
            string s = GetLetter(h) + GetLetter(l);
            return s;
        }

        private string GetLetter(int number)
        {
            switch (number)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 7:
                    return "G";
                case 8:
                    return "H";
                case 9:
                    return "I";
                case 10:
                    return "J";
                case 11:
                    return "K";
                case 12:
                    return "L";
                case 13:
                    return "M";
                case 14:
                    return "N";
                case 15:
                    return "O";
                case 16:
                    return "P";
                case 17:
                    return "Q";
                case 18:
                    return "R";
                case 19:
                    return "S";
                case 20:
                    return "T";
                case 21:
                    return "U";
                case 22:
                    return "V";
                case 23:
                    return "W";
                case 24:
                    return "X";
                case 25:
                    return "Y";
                case 26:
                    return "Z";
                default:
                    return "";
            }
        }
        #endregion


    }

    /// <summary>
    /// ˮƽ���뷽ʽ
    /// </summary>
    public enum ExcelHAlign
    {
        ���� = 1,
        ����,
        ����,
        ����,
        ���,
        ���˶���,
        ���о���,
        ��ɢ����
    }

    /// <summary>
    /// ��ֱ���뷽ʽ
    /// </summary>
    public enum ExcelVAlign
    {
        ���� = 1,
        ����,
        ����,
        ���˶���,
        ��ɢ����
    }

    /// <summary>
    /// �ߴ�
    /// </summary>
    public enum BorderWeight
    {
        ��ϸ = 1,
        ϸ = 2,
        �� = -4138,
        ���� = 4
    }

    /// <summary>
    /// ����ʽ
    /// </summary>
    public enum LineStyle
    {
        ����ֱ�� = 1,
        ���� = -4115,
        �ߵ���� = 4,
        ���߼����� = 5,
        �� = -4118,
        ˫�� = -4119,
        �� = -4142,
        ������б�� = 13
    }

    /// <summary>
    /// �»��߷�ʽ
    /// </summary>
    public enum UnderlineStyle
    {
        ���»��� = -4142,
        ˫�� = -4119,
        ˫�߳���ȫ�� = 5,
        ���� = 2,
        ���߳���ȫ�� = 4
    }

    /// <summary>
    /// ��Ԫ����䷽ʽ
    /// </summary>
    public enum Pattern
    {
        Automatic = -4105,
        Checker = 9,
        CrissCross = 16,
        Down = -4121,
        Gray16 = 17,
        Gray25 = -4124,
        Gray50 = -4125,
        Gray75 = -4126,
        Gray8 = 18,
        Grid = 15,
        Horizontal = -4128,
        LightDown = 13,
        LightHorizontal = 11,
        LightUp = 14,
        LightVertical = 12,
        None = -4142,
        SemiGray75 = 10,
        Solid = 1,
        Up = -4162,
        Vertical = -4166
    }

    /// <summary>
    /// ������ɫ����,�Ծ�Excel����ɫ��
    /// </summary>
    public enum ColorIndex
    {
        ��ɫ = -4142,
        �Զ� = -4105,
        ��ɫ = 1,
        ��ɫ = 53,
        ��� = 52,
        ���� = 51,
        ���� = 49,
        ���� = 11,
        ���� = 55,
        ��ɫ80 = 56,
        ��� = 9,
        ��ɫ = 46,
        ��� = 12,
        ��ɫ = 10,
        ��ɫ = 14,
        ��ɫ = 5,
        ���� = 47,
        ��ɫ50 = 16,
        ��ɫ = 3,
        ǳ��ɫ = 45,
        ���ɫ = 43,
        ���� = 50,
        ˮ��ɫ = 42,
        ǳ�� = 41,
        ������ = 13,
        ��ɫ40 = 48,
        �ۺ� = 7,
        ��ɫ = 44,
        ��ɫ = 6,
        ���� = 4,
        ���� = 8,
        ���� = 33,
        ÷�� = 54,
        ��ɫ25 = 15,
        õ��� = 38,
        ��ɫ = 40,
        ǳ�� = 36,
        ǳ�� = 35,
        ǳ���� = 34,
        ���� = 37,
        ���� = 39,
        ��ɫ = 2
    }
}