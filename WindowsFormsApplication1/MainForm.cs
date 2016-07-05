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
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Data.DataSetExtensions;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

namespace TemperatureFetcher
{
   
    public partial class MainForm : Form
    {
        CancellationTokenSource cts;
        CancellationToken ct;
        private string filename;
        private double minIntervalTemp, meanIntervalTemp, maxIntervalTemp;
        private ExcelFile excelFile;
        public DataTable dataTable;
        public DefaultTemperatureValues defaultTemps;
        List<string> typeValues;
        private bool fileLoaded;
        private bool processDone;
        private Dictionary<string, int> headersColsIndex = new Dictionary<string, int>();
        private List<string> acceptedHeaders;
        private static double temperatureErrorValue = 9999;
        //private static string APIKEY = "42f64b8f3050c00c3f6edb156a0fd";
        //private static string APIKEY = "1dfc3f80a6ae4a81979a65cd4eeae";
        //private static string WUndergroundAPIkey = "3af58966e3c40f0d";

        public APISource apiSource;
        //private uint WorldWeatherRequestsCounter;
        //private uint WUndergroundRequestsCounter;
        private static string dataBaseFolderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Temperature Fetcher Database";
        private DateTime timeStampWarmup;
        private bool[] rowAlreadyProcessed;
        private static uint NYearsToAverage = 10;
        private static uint warmupDuration = 60; // in seconds
        private int headersRowIndex;
        private DataGridViewCellStyle numericCellStyle;

        public MainForm()
        {
            InitializeComponent();
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            this.AutoSize = true;
            this.AutoScroll = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            apiSource = new APISource("3af58966e3c40f0d", 60, 10);
            fileLoaded = false;
            processDone = false;

            defaultTemps = new DefaultTemperatureValues();
            defaultTemps.Airplane = 17;
            defaultTemps.Refrigerate = 5;

            typeValues = new List<string>() {"Stockage T ambiante", "Stockage réfrigéré", "Transport routier", "Transport réfrigéré", "Transport aérien", "Transport maritime"};
            acceptedHeaders = new List<string>() {"Type", "Ville départ", "Ville arrivée", "Pays départ", "Pays arrivée", "Départ", "Arrivée"};
            // Create dataTable fields containing all required data
            dataTable = new DataTable();
            dataTable.Columns.Add("Index", typeof(int));
            //dataTable.Columns.Add("Mois", typeof(int));
            dataTable.Columns.Add("Type", typeof(string));
            //dataTable.Columns.Add("Latitude", typeof(double));
            //dataTable.Columns.Add("Longitude", typeof(double));
            dataTable.Columns.Add("Pays départ", typeof(string));
            dataTable.Columns.Add("Ville départ", typeof(string));
            dataTable.Columns.Add("Départ", typeof(DateTime));
            dataTable.Columns.Add("Pays arrivée", typeof(string));
            dataTable.Columns.Add("Ville arrivée", typeof(string));
            dataTable.Columns.Add("Arrivée", typeof(DateTime));
            dataTable.Columns.Add("Durée", typeof(string));
            //dataTable.Columns.Add("Température", typeof(double));
            dataTable.Columns.Add("T Moy. Min. départ", typeof(double));
            dataTable.Columns.Add("T Moy. Max. départ", typeof(double));
            dataTable.Columns.Add("T Moy. Min. arrivée", typeof(double));
            dataTable.Columns.Add("T Moy. Max. arrivée", typeof(double));

            numericCellStyle = new DataGridViewCellStyle();
            numericCellStyle.Format = "F2";

            timeStampWarmup = DateTime.Now;
           
        }

        [Obsolete("displayTemperaturesOnListView is deprecated", true)]
        private void displayTemperaturesOnListView()
        { // DEPRECATED
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                ListViewItem listitem = dataViewer.Items[i];
                DataRow row = dataTable.Rows[i];
               // Console.WriteLine(row["Temperature"]);
                //listitem.SubItems[listView1.Columns.IndexOf(temperatureColumn)].Text = String.Format("{0:0.00}", row["Temperature"]);
                listitem.SubItems[dataViewer.Columns.IndexOf(tMinDepColumn)].Text = String.Format("{0:0.00}", row["TemperatureMin"]);
                listitem.SubItems[dataViewer.Columns.IndexOf(tMaxDepColumn)].Text = String.Format("{0:0.00}", row["TemperatureMax"]);
            }
        }//*/
        
        private void displayOnDataGridView() {
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["T Moy. Min. Départ"].DefaultCellStyle = numericCellStyle;
            dataGridView1.Columns["T Moy. Max. Départ"].DefaultCellStyle = numericCellStyle;
            dataGridView1.Columns["T Moy. Min. Arrivée"].DefaultCellStyle = numericCellStyle;
            dataGridView1.Columns["T Moy. Max. Arrivée"].DefaultCellStyle = numericCellStyle;
        }

        [Obsolete("displayOnListView is deprecated, please use displayOnDataGridView instead.", true)]
        private void displayOnListView() {
            for (int i = 0; i < dataTable.Rows.Count; i++) {
                DataRow row = dataTable.Rows[i];
                ListViewItem listitem = new ListViewItem(row["Index"].ToString());
                for (int j = 0; j < dataViewer.Columns.Count; j++) {
                    listitem.SubItems.Add(""); // add empty elements first and set their content hereafter
                }
                listitem.SubItems[dataViewer.Columns.IndexOf(typeColumn)].Text = row["Type"].ToString();
                //listitem.SubItems[dataViewer.Columns.IndexOf(longitudeColumn)].Text = row["Longitude"].ToString();
                //listitem.SubItems[dataViewer.Columns.IndexOf(latitudeColumn)].Text = row["Latitude"].ToString();
                listitem.SubItems[dataViewer.Columns.IndexOf(cityColumn)].Text = row["Ville"].ToString();
                listitem.SubItems[dataViewer.Columns.IndexOf(countryColumn)].Text = row["Pays"].ToString();
                listitem.SubItems[dataViewer.Columns.IndexOf(departColumn)].Text = row["Départ"].ToString();
                listitem.SubItems[dataViewer.Columns.IndexOf(arrivColumn)].Text = row["Arrivée"].ToString();
                listitem.SubItems[dataViewer.Columns.IndexOf(dureeColumn)].Text = row["Durée"].ToString();
                //listitem.SubItems.Add("");
                dataViewer.Items.Add(listitem);
            }
            dataViewer.Columns[2].Width = -2; // autoresize to header width (-1 : autoresize to largest element
        }

        [Obsolete("displayPlacesOnListView is deprecated", true)]
        private void displayPlacesOnListView() {
            for (int i = 0; i < dataTable.Rows.Count; i++) {
                ListViewItem listitem = dataViewer.Items[i];
                DataRow row = dataTable.Rows[i];
                listitem.SubItems[dataViewer.Columns.IndexOf(countryColumn)].Text = String.Format("{0:0.00}", row["Pays"]);
                listitem.SubItems[dataViewer.Columns.IndexOf(cityColumn)].Text = String.Format("{0:0.00}", row["Ville"]);
            }
        }

        private void openFileButton_Click(object sender, EventArgs e) {            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel files (*.xls; *.xslx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {              
                filename = openFileDialog1.FileName;
                
                // Reset dataTable content when reading new file
                dataTable.Rows.Clear();
                dataViewer.Items.Clear();
                
                // Populate dataTable from excel file
				//readExcel(filename);
                readExcelMSInterop(filename);

                // Display DataTable on ListView
                //displayOnListView();
                displayOnDataGridView();        
            }
            fileLoaded = true;
        }

  
        private async void processButton_Click(object sender, EventArgs e) {
           /* if (string.IsNullOrWhiteSpace(minTempIntervalTextBox.Text) || string.IsNullOrWhiteSpace(maxTempIntervalTextBox.Text)){
                MessageBox.Show("You must set both min and max interval temperatures", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } //*/ // this is not anymore mandatory if we fetch both min and max temperatures
            if (!fileLoaded) {
                MessageBox.Show("You must first load a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            processButton.Enabled = false;
     
            cts = new CancellationTokenSource();
            ct = cts.Token;
            cancelFetchButton.Visible = true;
            TimeSpan duration = new TimeSpan();
            try {
                duration = DateTime.Now - timeStampWarmup;
                waitingLabel.Text = "";
                waitingLabel.Visible = true;
                while (duration.TotalSeconds < warmupDuration){ 
                    // depending on the server API plan, wait in order not to overshoot the maximum allowed web requests per warmupDuration
                    await Task.Delay(1000);
                    ct.ThrowIfCancellationRequested();
                    waitingLabel.Text = String.Format("Warmup. Waiting {0} seconds", (int)(60-duration.TotalSeconds));
                    //Console.WriteLine(waitingLabel.Text);
                    duration = DateTime.Now - timeStampWarmup;
                }
                waitingLabel.Text = "";
                waitingLabel.Visible = false;
                await processWork();
            } catch (OperationCanceledException) {
               // MessageBox.Show("Cancelled");   // test 
            } finally
            {
                fetchProgressBar.Visible = false;
                fetchProgressBar.Value = 0;
                waitingLabel.Text = "";
                waitingLabel.Visible = false;
                cts = null;
                processButton.Enabled = true;
                processDone = true;
                cancelFetchButton.Visible = false;
                processedRowLabel.Visible = false;
                if (duration.TotalSeconds >= warmupDuration) {
                    // warmup finished, reset warmup for next process (if warmup cancelled, this won't occur so that we don't begin anew)
                    timeStampWarmup = DateTime.Now;
                }
            }
           
            /*if (!dataTable.AsEnumerable().Any(row => row["TemperatureMin"] == DBNull.Value)){
                bool contains = dataTable.AsEnumerable().Any(row => temperatureErrorValue == (double)row["TemperatureMin"]);
                if (contains) {
                //MessageBox.Show("Warning : at least one town/country couldn't be fetch (indicated with T=9999). Please verify the names of town and country and reload the file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(String.Format("Warning : at least one step couldn't be fetch in the website (indicated with T={0}). Please verify the values of Latitude, Longitude or time and reload the file.", temperatureErrorValue), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } //*/
        }

        private async Task processWork() {         
            fetchProgressBar.Visible = true;
            fetchProgressBar.Maximum = (int) (NYearsToAverage * 2 * dataTable.Rows.Count); // total number of fetch
            await Task.Delay(150); // important : stabilizes thread flow
            for (int rowi = 0; rowi < dataTable.Rows.Count; rowi++) {
                processedRowLabel.Visible = true;
                processedRowLabel.Text = String.Format("Processing row {0} / {1}", rowi+1, dataTable.Rows.Count);
                ct.ThrowIfCancellationRequested();
                //fetchProgressBar.Value = (int)(100 * (float)rowi / Math.Max(dataTable.Rows.Count-1, 1));
                if (rowAlreadyProcessed[rowi]) {
                    continue;
                }
                DataRow row = dataTable.Rows[rowi];
                try {
                    Tuple<TemperatureRequestInfo, TemperatureRequestInfo> infos = await fetchTemperatures(rowi, row);
                    row["T Moy. Min. départ"] = infos.Item1.tMin;
                    row["T Moy. Max. départ"] = infos.Item1.tMax;
                    row["T Moy. Min. arrivée"] = infos.Item2.tMin;
                    row["T Moy. Max. arrivée"] = infos.Item2.tMax;
                } catch (System.Net.WebException) {
                    MessageBox.Show(String.Format("Row {0} : Error while performing the web request. Are you connected to Internet ?", row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                } catch (System.InvalidOperationException) {
                     MessageBox.Show(String.Format("Row {0} : Error while fetching the temperature.", row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     break;
                } catch (System.NullReferenceException) {
                    MessageBox.Show(String.Format("Row {0} : Error with the internet request, the cause can be a bad key, a lack of remaining requests to the service, or a request not found.{1} Check the places data.", row["Index"], Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                } catch (KeyNotFoundException ex) {
                    //MessageBox.Show(String.Format("Row {0} : Location could not be found.", row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MessageBox.Show(String.Format("Row {0} : " + ex.Message, row["Index"]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                } 
                await Task.Delay(150); // stabilizes
            }
            
        }

        private void cancelFetchButton_Click(object sender, EventArgs e) {
            if (cts != null) {
                cts.Cancel();
                //MessageBox.Show("Operation will be canceled at the end of the current row.", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            //int val = Prompt.ShowDialog("Test", "123");
           // Options dlg1 = new Options();
           // dlg1.ShowDialog();
        }
       
    }

    public class APISource {
        public APISource(string key, uint _period, uint reqs) {
            maxRequestsPerPeriod = reqs;
            period = _period;
            APIKey = key;
            requestsCounter = 0;
        }
        public uint maxRequestsPerPeriod;
        public uint period; // in seconds
        public string APIKey;
        public uint requestsCounter;
    };

    public struct DefaultTemperatureValues {
        public double Airplane;
        public double Refrigerate;
    }

}
