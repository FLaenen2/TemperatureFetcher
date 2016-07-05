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

namespace TemperatureFetcher
{
    public partial class MainForm
    {
        private void exportButton_Click(object sender, EventArgs e) { // write data to Excel File using GemBox Spreadsheet : http://www.gemboxsoftware.com/spreadsheet/overview
            // Free version has limitation of 150 rows per worksheet, and 5 worksheet per file
            /*if (!processDone) {
                MessageBox.Show("You must first process a file !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//*/
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("Avec temperatures");
            System.Data.DataView view = new System.Data.DataView(dataTable);
            //System.Data.DataTable selected = view.ToTable("Selected", false, "Type", "Latitude", "Longitude", "Debut", "Fin", "Duree", "TemperatureMin", "TemperatureMax");
            System.Data.DataTable selected = view.ToTable("Selected", false, "Type", "Pays départ", "Ville départ", "Départ", "Pays arrivée", "Ville arrivée", "Arrivée", "Durée", "T Moy. Min. départ", "T Moy. Max. départ", "T Moy. Min. arrivée", "T Moy. Max. arrivée");
            // DataTable subTable = dataTable.AsEnumerable()
            //            .Select(x => new  { col1 = x["Type"], col2 = x["Ville"], col3 = x["Pays"], col4 = x["Debut"], col5 = x["Fin"], col6 = x["Temperature"]});
            ws.InsertDataTable(selected,
            new InsertDataTableOptions() {
                ColumnHeaders = true,
                StartRow = 1
            });

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files | *.xlsx";
            saveFileDialog.DefaultExt = "xlsx";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                try {
                    ef.Save(saveFileDialog.FileName);
                } catch (System.IO.IOException ex) {
                    MessageBox.Show(ex.Message, "Error during export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

}
