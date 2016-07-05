using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using GemBox.Spreadsheet;
using Excel;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace TemperatureFetcher
{
    public class MyWorker
    {
        Form1 _form;
        public MyWorker(Form1 form)
        {
            _form = form;
            //Console.WriteLine("myworker created");
        }

        public void processWork()
        {
            for (int rowi = 0; rowi < _form.dataTable.Rows.Count; rowi++)
            {
                _form.SetProgressBarValue((int)(100 * (float)(rowi + 1) / _form.dataTable.Rows.Count));

                DataRow row = _form.dataTable.Rows[rowi];
                try
                {
                    row["Temperature"] = _form.fetchTemperatures(rowi, row);
                }
                catch (System.Net.WebException)
                {
                    MessageBox.Show(String.Format("Row {0} : Error while performing the web request. Are you connected to Internet ?", row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                /* catch (System.InvalidOperationException)
                 {
                     MessageBox.Show(String.Format("Row {0} : Error while fetching the temperature.", row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     break;
                 }//*/
                catch (System.NullReferenceException)
                {
                    MessageBox.Show(String.Format("Row {0} : Error with the internet request, the cause can be a bad key.", row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }

            }
            // Check if any fetch failed
            // bool contains = dataTable.AsEnumerable().Any(row => temperatureErrorValue == row.Field<double>("Temperature"));
            //  DataRow[] filteredRows = dataTable.Select("Temperature LIKE '%" + bad.ToString() + "%'");
            //if (contains){
            //    MessageBox.Show("Warning : at least one town/country couldn't be fetch (indicated with T=9999). Please verify the names of town and country and reload the file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

        }
    }
}
