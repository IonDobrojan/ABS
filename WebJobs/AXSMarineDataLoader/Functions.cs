using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace AXSMarineDataLoader
{
    public class Functions
    {

        [NoAutomaticTrigger]
        public void LoadData(TraceWriter log)
        {
            try
            {
                AddLog(log, "AXSMarineDataLoader webjob starts. ", EventLogEntryType.Information);
                
                LoadLinerAISEvents();

                AddLog(log, "AXSMarineDataLoader webjob end work. ", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                AddLog(log, "AXSMarineDataLoader webjob error\nMessage "
                          + ex.Message + "\nTrace: " + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void LoadLinerAISEvents()
        {
            var token = CloudConfigurationManager.GetSetting("AISEventsToken");
            var date = (DateTime.UtcNow.AddDays(-1).Date).ToString("yyyy-MM-dd");
            var polygonTypes = new List<string> { "zone", "port", "terminal", "shipyard", "waypoint", "anchorage" };
            var allData = new List<Result>();
            foreach (var polygon in polygonTypes)
            {
                var url = $"https://webservicesv5.axsmarine.com/rest/liner/ais/events/2/?page_size=10000&polygon_types={polygon}&token={token}&out_date_from={date}";

                while (!string.IsNullOrEmpty(url))
                {
                    var json = DownloadString(url);
                    var data = JsonConvert.DeserializeObject<LinerAisEventsRoot>(json);
                    allData.AddRange(data.results);
                    url = data.links.next;
                }
            }


            byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(allData));
            SharePointMethods.UploadFileToSharePoint("ABS", "liner_ais_events.json", bytes);
        }

        private void AddLog(TraceWriter log, string logMessage, EventLogEntryType type)
        {
            switch (type)
            {
                case EventLogEntryType.Error:
                    {
                        log.Error(logMessage);
                        break;
                    }

                case EventLogEntryType.Information:
                default:
                    {
                        log.Info(logMessage);
                        break;
                    }
            }
        }

        public static string DownloadString(string address)
        {
            WebClient client = new WebClient();
            return client.DownloadString(address);
        }
    }
}
