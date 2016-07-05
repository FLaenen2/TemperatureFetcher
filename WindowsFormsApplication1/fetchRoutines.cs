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


namespace TemperatureFetcher {

    public partial class MainForm {

        /*private async Task<double> fetchWorldWeather(double latitude, double longitude, int monthIndex) {
            if (String.IsNullOrWhiteSpace(APIKEY)) {
                if (String.IsNullOrWhiteSpace(keyTextBox.Text)) {
                    MessageBox.Show("You must provide an APIKEY to perform the research", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return temperatureErrorValue;
                } else {
                    APIKEY = keyTextBox.Text;
                }
            }
            string fileName = String.Format("WorldWeather_{0:000.0}_{1:000.0}_{2}.xml", latitude, longitude, monthIndex);
            string fullfile = System.IO.Path.Combine(dataBaseFolderName, fileName);
            bool exists = System.IO.File.Exists(fullfile);
            XmlDocument myDoc = new XmlDocument();
            if (!exists) {
                string request = String.Format("http://api.worldweatheronline.com/premium/v1/weather.ashx?key={0}&q={1},{2}&format=xml", APIKEY, latitude.ToString(CultureInfo.GetCultureInfo("en-GB")), longitude.ToString(CultureInfo.GetCultureInfo("en-GB")));
                myDoc = MakeRequest(request);
                WorldWeatherRequestsCounter++;
                await Task.Delay(5000);  // test
                if (WorldWeatherRequestsCounter % 5 == 0) {
                    await Task.Delay(1000); //
                }
                saveXML(myDoc, fullfile);
            } else {
                myDoc.Load(fullfile);
            }

            XmlNodeList testerror = myDoc.SelectNodes("/data/error");
            if (testerror.Count != 0) {
                throw new System.InvalidOperationException("Query not found");
            }
            XmlNodeList monthsElements = myDoc.SelectNodes("/data/ClimateAverages/month");
            XmlNode node = monthsElements[monthIndex];
            double avgMinTemp = double.Parse(node["avgMinTemp"].InnerText, CultureInfo.InvariantCulture);
            double avgMaxTemp = double.Parse(node["absMaxTemp"].InnerText, CultureInfo.InvariantCulture);
            double mostContrainTemp = Math.Abs(avgMinTemp - meanIntervalTemp) > Math.Abs(avgMaxTemp - meanIntervalTemp) ? avgMinTemp : avgMaxTemp;
            return mostContrainTemp;
        }//*/

        /*private async Task<double> fetchWorldWeather(string town, string country, int monthIndex) {
            if (String.IsNullOrWhiteSpace(APIKEY)) {
                if (String.IsNullOrWhiteSpace(keyTextBox.Text)) {
                    MessageBox.Show("You must provide an APIKEY to perform the research", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return temperatureErrorValue;
                } else {
                    APIKEY = keyTextBox.Text;
                }
            }
            string fileName = String.Format("WorldWeather_{0}_{1}_{2}.xml", town, country, monthIndex);
            string fullfile = System.IO.Path.Combine(dataBaseFolderName, fileName);
            bool exists = System.IO.File.Exists(fullfile);
            XmlDocument myDoc = new XmlDocument();
            if (!exists) {
                myDoc = MakeRequest(String.Format("http://api.worldweatheronline.com/premium/v1/weather.ashx?key={0}&q={1},{2}&format=xml", APIKEY, town, country));
                WorldWeatherRequestsCounter++;
                if (WorldWeatherRequestsCounter % 5 == 0) {
                    await Task.Delay(1000); //
                }
                saveXML(myDoc, fullfile);
            } else {
                myDoc.Load(fullfile);
            }

            XmlNodeList testerror = myDoc.SelectNodes("/data/error");
            if (testerror.Count != 0) {
                throw new System.InvalidOperationException("Query not found");
            }
            XmlNodeList monthsElements = myDoc.SelectNodes("/data/ClimateAverages/month");
            XmlNode node = monthsElements[monthIndex];
            double avgMinTemp = double.Parse(node["avgMinTemp"].InnerText, CultureInfo.InvariantCulture);
            double avgMaxTemp = double.Parse(node["absMaxTemp"].InnerText, CultureInfo.InvariantCulture);
            double mostContrainTemp = Math.Abs(avgMinTemp - meanIntervalTemp) > Math.Abs(avgMaxTemp - meanIntervalTemp) ? avgMinTemp : avgMaxTemp;
            return mostContrainTemp;
        }//*/

        public static async Task<XmlDocument> MakeRequest(string requestUrl) {
            try {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response;
                WebResponse webResp;
                try {
                    webResp = await request.GetResponseAsync();
                    response = webResp as HttpWebResponse;
                } catch (System.Net.WebException) {
                    throw;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);
            } catch (NullReferenceException e) {
                throw e;
            }
        }

        private void saveXML(XmlDocument doc, string fullfilename) {
            System.IO.Directory.CreateDirectory(dataBaseFolderName);
            System.IO.FileStream fs = System.IO.File.Create(String.Format(fullfilename));
            doc.Save(fs);
            fs.Close();
        }

        private class TemperatureRequestInfo {
            public double tMin, tMax;
            public string country, city, latitude, longitude; // geographic info
        }

        private async Task<TemperatureRequestInfo> processfetchWUnderground(string request, string fullfile, bool last) {
            TemperatureRequestInfo myinfo = new TemperatureRequestInfo();
            bool exists = System.IO.File.Exists(fullfile);
            XmlDocument myDoc = new XmlDocument();
            if (!exists) {
                myDoc = await MakeRequest(request);
                await Task.Delay(1000);
                apiSource.requestsCounter++;
                //Console.WriteLine(String.Format("Request {0} counter : ", request) + apiSource.requestsCounter);
                if (apiSource.requestsCounter % apiSource.maxRequestsPerPeriod == 0) {   // free subscription plan only offers 10 requests per minute
                   // +1 because requestsCounter is initialized with 0
                    waitingLabel.Text = "";
                    waitingLabel.Visible = true;
                    float t = apiSource.period; 
                    while (t > 0) {
                        waitingLabel.Text = String.Format("Waiting {0} sec", t);
                        //Console.WriteLine(String.Format("Waiting {0} sec", t));
                        t -= 1;
                        ct.ThrowIfCancellationRequested();
                        await Task.Delay(1000);
                    }
                    waitingLabel.Visible = false;
                }             
            } else {
                myDoc.Load(fullfile);
            }//*/
            XmlNode node = myDoc.SelectSingleNode("/response/history/dailysummary/summary");
            if (node == null) {
                if (myDoc.SelectSingleNode("/response/error") != null) {// can be because of bad location 
                    throw new KeyNotFoundException(myDoc.SelectSingleNode("/response/error/type").InnerText + " : " + myDoc.SelectSingleNode("/response/error/description").InnerText + "\nLocation might not have been found. Request is " + request);
                } else {
                    myinfo.tMin = temperatureErrorValue;
                    myinfo.tMax = temperatureErrorValue;
                    return myinfo;
                }
            }
            if (String.IsNullOrEmpty(node.InnerText)) { // when no record is found
                myinfo.tMin = temperatureErrorValue;
                myinfo.tMax = temperatureErrorValue;
                return myinfo;
            }
            XmlNode geoNode = myDoc.SelectSingleNode("/response/location");
            myinfo.country = geoNode["country_name"].InnerText;
            myinfo.city = geoNode["city"].InnerText;
            myinfo.latitude = geoNode["lat"].InnerText;
            myinfo.longitude = geoNode["lon"].InnerText;
            myinfo.tMin = double.Parse(node["mintempm"].InnerText, CultureInfo.InvariantCulture);
            myinfo.tMax = double.Parse(node["maxtempm"].InnerText, CultureInfo.InvariantCulture);
            saveXML(myDoc, fullfile); // save result in xml if already asked to save requests. Save once we are sure there was no error (bad request...)
            return myinfo;
        }

        private async Task<TemperatureRequestInfo> fetchWUnderground(double latitude, double longitude, int year, int month, int day, bool last) {
            string fileName = String.Format("WUnderground_{0:000.0}_{1:000.0}_{2,4}{3,2}{4,2}.xml", latitude, longitude, year, month, day);
            string fullfile = System.IO.Path.Combine(dataBaseFolderName, fileName);
            string request = String.Format("http://api.wunderground.com/api/{0}/geolookup/history_{1,4}{2,2}{3,2}/q/{4},{5}.xml", apiSource.APIKey, year, month, day, latitude.ToString(CultureInfo.GetCultureInfo("en-GB")), longitude.ToString(CultureInfo.GetCultureInfo("en-GB")));
            TemperatureRequestInfo myinfo = new TemperatureRequestInfo();
            myinfo = await processfetchWUnderground(request, fullfile, last); 
            return myinfo;
        }

        private async Task<TemperatureRequestInfo> fetchWUnderground(string town, string country, int year, int month, int day, bool last) {
            string fileName = String.Format("WUnderground_{0}_{1}_{2,4}{3,0:00}{4,0:00}.xml", town, country, year, month, day);
            string fullfile = System.IO.Path.Combine(dataBaseFolderName, fileName);
            string request = String.Format("http://api.wunderground.com/api/{0}/geolookup/history_{1,4}{2,2}{3,2}/q/{4}/{5}.xml", apiSource.APIKey, year, month, day, country, town);
            TemperatureRequestInfo myinfo = new TemperatureRequestInfo();
            myinfo = await processfetchWUnderground(request, fullfile, last);
            return myinfo;
        }

        private async Task<Tuple<TemperatureRequestInfo, TemperatureRequestInfo>> fetchTemperatures(int rowi, DataRow row) {
            // main entry to fetch temperatures.
            TemperatureRequestInfo myinfoDeparture = new TemperatureRequestInfo();
            TemperatureRequestInfo myinfoArrival = new TemperatureRequestInfo();
            string type = row["Type"].ToString();
            if (type.Equals("Transport routier", StringComparison.InvariantCultureIgnoreCase) || type.Equals("Transport maritime", StringComparison.InvariantCultureIgnoreCase) ||  type.Equals("Stockage T ambiante", StringComparison.InvariantCultureIgnoreCase)) {
                await Task.Delay(150); // stabilization
                //return 10; // test
                int yeari = DateTime.Now.Year - 1; // ensure that we don't fetch future dates temperatures
                List<double> yearlyTMinDep = new List<double>();
                List<double> yearlyTMaxDep = new List<double>();
                List<double> yearlyTMinArr = new List<double>();
                List<double> yearlyTMaxArr = new List<double>();
                while (true) {
                    ct.ThrowIfCancellationRequested();
                    bool last = rowi == dataTable.Rows.Count - 1; // obsolete, but may be useful
                    myinfoDeparture = await fetchWUnderground(row["Ville départ"].ToString(), row["Pays départ"].ToString(), yeari, ((DateTime)row["Départ"]).Month, ((DateTime)row["Arrivée"]).Day, last);
                    fetchProgressBar.Value++;
                    myinfoArrival =  await fetchWUnderground(row["Ville arrivée"].ToString(), row["Pays arrivée"].ToString(), yeari, ((DateTime)row["Départ"]).Month, ((DateTime)row["Arrivée"]).Day, last);
                    fetchProgressBar.Value++;
                    Console.WriteLine(String.Format("Fetch {0} / {1}", fetchProgressBar.Value, fetchProgressBar.Maximum));
                    Console.WriteLine("Requests : " + apiSource.requestsCounter);
                    yearlyTMinDep.Add(myinfoDeparture.tMin);
                    yearlyTMaxDep.Add(myinfoDeparture.tMax);
                    yearlyTMinArr.Add(myinfoArrival.tMin);
                    yearlyTMaxArr.Add(myinfoArrival.tMax);
                    if (((DateTime)row["Départ"]).Year - yeari >= NYearsToAverage) {
                        // fetch n (= NyearsToAverage) years back in time
                        break;
                    }
                    yeari--;
                }
                myinfoDeparture.tMin = yearlyTMinDep.Average();
                myinfoDeparture.tMax = yearlyTMaxDep.Average();
                myinfoArrival.tMin = yearlyTMinArr.Average();
                myinfoArrival.tMax = yearlyTMaxArr.Average();
                return Tuple.Create(myinfoDeparture, myinfoArrival);//*/
            } else if (type.Equals("Transport aérien", StringComparison.InvariantCultureIgnoreCase) ) {
                myinfoDeparture.tMin = 17;
                myinfoDeparture.tMax = 17;
                myinfoArrival.tMin = 17;
                myinfoArrival.tMax = 17;
                return Tuple.Create(myinfoDeparture, myinfoArrival);//*/
            } else if (type.Equals("Stockage réfrigéré", StringComparison.InvariantCultureIgnoreCase) || type.Equals("Transport réfrigéré", StringComparison.InvariantCultureIgnoreCase) ) {
                // TemperatureRequestInfo myinfo;
                //TemperatureRequestInfo nextTemperature = await fetchTemperatures(rowi + 1, dataTable.Rows[rowi + 1]);
                //dataTable.Rows[rowi + 1]["Temperature"] = nextTemperature;
                /*dataTable.Rows[rowi + 1]["TemperatureMin"] = nextTemperature.temperatureMin;
                dataTable.Rows[rowi + 1]["TemperatureMax"] = nextTemperature.temperatureMax;
                dataTable.Rows[rowi + 1]["Ville"] = nextTemperature.city;
                dataTable.Rows[rowi + 1]["Pays"] = nextTemperature.country;
                rowAlreadyProcessed[rowi + 1] = true;  //*/
                myinfoDeparture.tMin = 5;
                myinfoDeparture.tMax = 5;
                myinfoArrival.tMin = 5;
                myinfoArrival.tMax = 5;
                // double previousTemperature = (double) dataTable.Rows[rowi - 1]["Temperature"];
                // double mostConstrainTemp = Math.Abs(nextTemperature - meanIntervalTemp) > Math.Abs(previousTemperature - meanIntervalTemp) ? nextTemperature : previousTemperature;                
                return Tuple.Create(myinfoDeparture, myinfoArrival);//*/
            } else {
                MessageBox.Show(String.Format("Error : did not understand Type '{0}' at line number : {1}", row["Type"].ToString(), row["Index"]));
                //TemperatureRequestInfo myinfo;
                myinfoDeparture.tMin = temperatureErrorValue;
                myinfoDeparture.tMax = temperatureErrorValue;
                myinfoArrival.tMin = temperatureErrorValue;
                myinfoArrival.tMax = temperatureErrorValue;
                return Tuple.Create(myinfoDeparture, myinfoArrival);//*/
            }
        }
    }
}
