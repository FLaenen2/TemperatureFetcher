using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using GemBox.Spreadsheet;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using ExcelMS = Microsoft.Office.Interop.Excel;

namespace TemperatureFetcher
{
    public partial class MainForm
    {

        private void setHeadersIndex(ExcelMS.Range range, int typeRow, int typeCol, int nCols){
            headersColsIndex["Type"] = typeCol;
            for (int column_i = 1; column_i < nCols; column_i++) {
                string header = (string)(range[typeRow, typeCol+column_i] as ExcelMS.Range).Value;
                var exists = acceptedHeaders.Any(x => string.Compare(x, header) == 0);
                if (!exists) {
                    string messageList = Environment.NewLine + string.Join("'" + Environment.NewLine + "'", acceptedHeaders);
                    MessageBox.Show(String.Format("Error while reading the headers row in excel file. Header read is '{0}', not recognized. Here are the accepter headers :", header) + messageList, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new KeyNotFoundException();
                } else {
                    headersColsIndex[header] = column_i+typeCol;
                }
            }
        }

        private void setHeadersIndex(List<Excel.Cell> listCells){
            //int index = 0;
            // associate the name of a column (header string) with the column index
            headersColsIndex["Type"] = 0; // required, just for commodity for now
            for (int column_i = 0; column_i < listCells.Count; column_i ++){
                string header = listCells[column_i].Text;
                var exists = acceptedHeaders.Any(x => string.Compare(x, header) == 0);
                if (!exists){
                    string messageList = Environment.NewLine + string.Join("'" + Environment.NewLine + "'", acceptedHeaders);
                    MessageBox.Show(String.Format("Error while reading the headers row in excel file. Header read is '{0}', not recognized. Here are the accepter headers :", header) + messageList, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new KeyNotFoundException();
                }else{
                    headersColsIndex[header] = column_i;
                }
            }
        }

        private void readExcelMSInterop(string filename) {
            ExcelMS.Application xlApp = new ExcelMS.Application();
            ExcelMS.Workbook xlWorkbook = xlApp.Workbooks.Open(@filename);
            ExcelMS._Worksheet xlWorksheet = (ExcelMS._Worksheet)xlWorkbook.Sheets[1];
            ExcelMS.Range xlRange = xlWorksheet.UsedRange;
            ExcelMS.Range typeCell = xlRange.Find("Type");

            int typeColumn = typeCell.Column;
            int headersRowIndex = typeCell.Row;
            int ncols = 7;
            setHeadersIndex(xlRange, headersRowIndex, typeColumn, ncols);

            int row_i = headersRowIndex + 1;
            DataRow newRow;
            while (true){
                newRow = dataTable.NewRow();

                newRow["Index"] = row_i - headersRowIndex;
                        
                // READ TYPE
                string type = (string)(xlRange[row_i, headersColsIndex["Type"]] as ExcelMS.Range).Value;
                if (string.IsNullOrEmpty(type)) { // finished reading lines
                    break;
                }
                var exists = typeValues.Any(x => string.Compare(x, type) == 0);
                if (!exists) {
                    string messageList = Environment.NewLine + string.Join(Environment.NewLine, typeValues);
                    MessageBox.Show(String.Format("Error while reading type for step number {0} row {1} in excel file. Type read is '{2}', not recognized. Here are accepted types : ", newRow["index"].ToString(), row_i, type) + messageList, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new KeyNotFoundException();
                } else {
                    newRow["Type"] = type;
                }

                // READ DEPARTURE TOWN
                newRow["Ville départ"] = Regex.Replace((string)(xlRange[row_i, headersColsIndex["Ville départ"]] as ExcelMS.Range).Value2, @"\s+", "-");  // replace groups of white spaces by '-'
                // READ DEPARTURE COUNTRY
                newRow["Pays départ"] = Regex.Replace((string)(xlRange[row_i, headersColsIndex["Pays départ"]] as ExcelMS.Range).Value2, @"\s+", "-");  // replace groups of white spaces by '-'

                // READ ARRIVAL TOWN
                newRow["Ville arrivée"] = Regex.Replace((string)(xlRange[row_i, headersColsIndex["Ville arrivée"]] as ExcelMS.Range).Value2, @"\s+", "-");  // replace groups of white spaces by '-'
                // READ ARRIVAL COUNTRY
                newRow["Pays arrivée"] = Regex.Replace((string)(xlRange[row_i, headersColsIndex["Pays arrivée"]] as ExcelMS.Range).Value2, @"\s+", "-");  // replace groups of white spaces by '-'

                // READ DEPARTURE TIME
                var temp1 = xlRange[row_i, headersColsIndex["Départ"]] as ExcelMS.Range;
                double temp2 = double.Parse(temp1.Formula.ToString(), CultureInfo.InvariantCulture);
               // var temp3 = temp1.Value;
               try { 
                    //newRow["Départ"] = Convert.ToDateTime((xlRange[row_i, headersColsIndex["Départ"]] as ExcelMS.Range).Text.ToString());   
                   newRow["Départ"] = DateTime.FromOADate(temp2);   
               }
               catch (System.FormatException e) {
                   MessageBox.Show(String.Format("Error while reading 'Départ' field for 'Ville départ' = {0}. Ensure it has the correct format.", newRow["Ville départ"]) + System.Environment.NewLine + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   break;
               }                
                // READ ARRIVAL TIME
                try {
                    Console.WriteLine( Convert.ToDateTime(newRow["Départ"]).Hour);
                    newRow["Arrivée"] = Convert.ToDateTime((xlRange[row_i, headersColsIndex["Arrivée"]] as ExcelMS.Range).Text.ToString());
                }
                catch (System.FormatException e) {
                    MessageBox.Show(String.Format("Error while reading 'Arrivée' field for 'Ville départ' = {0}. Ensure it has the correct format.", newRow["Ville départ"]) + System.Environment.NewLine + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                } 
                newRow["Durée"] = ((DateTime)newRow["Arrivée"] - (DateTime)newRow["Départ"]).ToString();
                dataTable.Rows.Add(newRow);
                row_i++;
            }
            rowAlreadyProcessed = Enumerable.Repeat(false, dataTable.Rows.Count).ToArray();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataTable.Columns["Départ"].Ordinal | e.ColumnIndex == dataTable.Columns["Arrivée"].Ordinal){
                // recalculate durations
                foreach (DataRow row in dataTable.Rows){
                    row["Durée"] = ((DateTime)row["Arrivée"] - (DateTime)row["Départ"]).ToString();
                }
            } 
        }

        [Obsolete("readExcelGemBox is deprecated", true)]
        private void readExcelGemBox(string filename)
        { // read Excel File using GemBox Software : http://www.gemboxsoftware.com/spreadsheet/overview
            // WARNING : deprecated. Implemented readExcel for more flexible data validation
            dataTable = new DataTable();
            // Select the first worksheet from the file.
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            try
            {
                excelFile = ExcelFile.Load(filename);
            }
            catch (Exception)
            {
                MessageBox.Show("Limit of 150 rows reached");
            }

            ExcelWorksheet ws = excelFile.Worksheets[0];

            ExtractToDataTableOptions options = new ExtractToDataTableOptions(0, 0, 149);
            options.ExtractDataOptions = ExtractDataOptions.StopAtFirstEmptyRow;
            options.ExcelCellToDataTableCellConverting += (sendere, ee) =>
            {
                if (!ee.IsDataTableValueValid)
                {
                    // GemBox.Spreadsheet doesn't automatically convert numbers to strings in ExtractToDataTable() method because of culture issues; 
                    // someone would expect the number 12.4 as "12.4" and someone else as "12,4".
                    ee.DataTableValue = ee.ExcelCell.Value == null ? null : ee.ExcelCell.Value.ToString();
                    ee.Action = ExtractDataEventAction.Continue;
                }
            };//*/
            ws.ExtractToDataTable(dataTable, options);
            foreach (DataRow row in dataTable.Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                for (int i = 1; i < dataTable.Columns.Count; i++)
                {
                    item.SubItems.Add(row[i].ToString());
                }
                dataViewer.Items.Add(item);
            }
        }

        

        private void readExcel(string filename)
        { // read Excel with help of Excel.dll : http://www.codeproject.com/Tips/801032/Csharp-How-To-Read-xlsx-Excel-File-With-Lines-Of-C
            Excel.worksheet worksheet;
            try // opening the excel file
            {
                worksheet = Excel.Workbook.Worksheets(filename).ToList()[0]; // get content of first worksheet. Seems to already filter blank first lines
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show(String.Format("Cannot open the file '{0}'. It is maybe an invalid file, or non readable (not allowed), or already in used. Ensure it has been written with Excel for Windows.", filename), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool headersFound = false;
            for (int row_i = 0; row_i < worksheet.Rows.Length; row_i++)
            {                
                Excel.Row row = worksheet.Rows.ToList()[row_i]; // Get ith row of read rows
                DataRow newRow;
                try
                {      
                    List <Excel.Cell> listCells = row.Cells.ToList();
                    Excel.Cell cell = listCells[0]; // read first column of current row

                    if (!headersFound){ // search first for the line containing the headers (i.e. which first column is "Type")
                        string content = cell.Text;
                        //Console.WriteLine(content); // test
                        if (content.Equals("Type")) { // found header "Type"
                            headersRowIndex = row_i; // set this line as the line containing headers
                            //MessageBox.Show(row_i.ToString()); // test
                            setHeadersIndex(listCells);
                            headersFound = true;
                            continue; // skip to next line, which we assume is the beginning of the data content
                        } else {
                            continue; // just skip to next line
                        }
                    } else {
                        newRow = dataTable.NewRow();
                        newRow["Index"] = row_i - headersRowIndex;
                        // READ TYPE
                        if (cell != null) {
                            if (string.IsNullOrEmpty(cell.Text)) { // happens if empty content in excel file
                                break;
                            }
                            var exists = typeValues.Any(x => string.Compare(x, cell.Text) == 0);
                            if (!exists) {
                                string messageList = Environment.NewLine + string.Join(Environment.NewLine, typeValues);
                                MessageBox.Show(String.Format("Error while reading type for step number {0} row in excel file. Type read is '{1}', not recognized. Here are accepted types : ", newRow["index"].ToString(), cell.Text) + messageList, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new KeyNotFoundException();
                            } else {
                                newRow["Type"] = cell.Text;
                            }
                        }

                        // READ LATITUDE
                        /*cell = listCells[1];
                        if (cell != null) { newRow["Latitude"] = Convert.ToDouble(cell.Text); }
                        // READ LONGITUDE
                        cell = listCells[2];
                        if (cell != null) { newRow["Longitude"] = Convert.ToDouble(cell.Text); }//*/

                        // READ DEPARTURE TOWN
                        cell = listCells[headersColsIndex["Ville départ"]];
                        if (cell != null) { newRow["Ville départ"] = cell.Text; }
                        // READ DEPARTURE COUNTRY
                        cell = listCells[headersColsIndex["Pays départ"]];
                        if (cell != null) { newRow["Pays départ"] = Regex.Replace(cell.Text, @"\s+", "-"); }  // replace groups of white spaces by '-'
                        //*/

                        // READ ARRIVAL TOWN
                        cell = listCells[headersColsIndex["Ville arrivée"]];
                        if (cell != null) { newRow["Ville arrivée"] = cell.Text; }
                        // READ ARRIVAL COUNTRY
                        cell = listCells[headersColsIndex["Pays arrivée"]];
                        if (cell != null) { newRow["Pays arrivée"] = Regex.Replace(cell.Text, @"\s+", "-"); }  // replace groups of white spaces by '-'
                        //*/

                        // READ DEPARTURE TIME
                        cell = listCells[headersColsIndex["Départ"]];
                        DateTime begin;
                        if (cell == null && (row_i - headersRowIndex) >= 2) {
                            begin = (DateTime)dataTable.Rows[(row_i - headersRowIndex) - 2]["Arrivée"];
                            newRow["Départ"] = begin;
                            //   newRow["Mois"] = (int)begin.Month;
                        } else if (String.IsNullOrWhiteSpace(cell.Text) && row_i >= 2) {
                            begin = (DateTime)dataTable.Rows[(row_i - headersRowIndex) - 2]["Arrivée"];
                            newRow["Départ"] = begin;
                        } else {
                            if (!DateTime.TryParseExact(cell.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out begin)) {
                                if (!DateTime.TryParseExact(cell.Text, "dd/MM/yy HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out begin)) {
                                    MessageBox.Show(String.Format("Error when reading 'Départ' field at row {0}. Check that the date format is dd/MM/yyyy HH:mm or dd/MM/yy HH:mm", row_i), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }
                            }
                            if ((row_i - headersRowIndex) >= 2 && DateTime.Compare(begin, (DateTime)dataTable.Rows[(row_i - headersRowIndex) - 2]["Départ"]) < 0) {
                                MessageBox.Show(String.Format("Start time at step {0} is earlier than the previous one at line {1}", (row_i - headersRowIndex), (row_i - headersRowIndex) - 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            newRow["Départ"] = begin;
                            //   newRow["Mois"] = (int)begin.Month;
                        }

                        // READ ARRIVAL TIME
                        cell = listCells[headersColsIndex["Arrivée"]];
                        DateTime end;
                        if (cell != null) {
                            bool tst = DateTime.TryParseExact(cell.Text, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
                            if (!tst) {
                                tst = DateTime.TryParseExact(cell.Text, "dd/MM/yy HH:mm", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
                                if (!tst) {
                                    MessageBox.Show(String.Format("Error when reading 'Arrivée' field at row {0}. Check that the date format is dd/MM/yyyy HH:MM or dd/MM/yy HH:mm", row_i), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }
                            }
                            newRow["Arrivée"] = end;
                            if ((row_i - headersRowIndex) >= 2 && DateTime.Compare(end, (DateTime)dataTable.Rows[(row_i - headersRowIndex) - 2]["Arrivée"]) < 0) {
                                MessageBox.Show(String.Format("End time at line {0} is earlier than the previous one at line {1}", row_i, row_i - 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        dataTable.Rows.Add(newRow);
                        newRow["Durée"] = ((DateTime)newRow["Arrivée"] - (DateTime)newRow["Départ"]);
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    MessageBox.Show(String.Format("Error reading cells in Excel file row number {0}. Check that all cells are fulfilled.", row_i - 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                catch (KeyNotFoundException)
                {
                    break;
                }
            }  // END of for loop on lines
            rowAlreadyProcessed = Enumerable.Repeat(false, dataTable.Rows.Count).ToArray();
            //  MessageBox.Show(String.Format("Read {0} lines", rowi - 1), "Finished reading", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

       
    }
}
