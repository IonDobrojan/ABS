using System.Collections.Generic;

namespace AXSMarineDataLoader
{
    public class Links
    {
        public string next { get; set; }
    }

    public class Result
    {
        public string vsl_name { get; set; }
        public int? vsl_dwt { get; set; }
        public int? vsl_cubic { get; set; }
        public int? vsl_teu { get; set; }
        public int? imo_alpha_id { get; set; }
        public int? service_id { get; set; }
        public string service_name { get; set; }
        public string service_status { get; set; }
        public string destination { get; set; }
        public string polygon_name { get; set; }
        public int? polygon_id { get; set; }
        public string polygon_type { get; set; }
        public double? entry_draft { get; set; }
        public double? entry_speed { get; set; }
        public int? entry_heading { get; set; }
        public string entry_date { get; set; }
        public double? entry_lat { get; set; }
        public double? entry_long { get; set; }
        public double? out_draft { get; set; }
        public double? out_speed { get; set; }
        public int? out_heading { get; set; }
        public string out_date { get; set; }
        public double? out_lat { get; set; }
        public double? out_long { get; set; }
        public string destination_raw { get; set; }
        public string eta { get; set; }
        public bool is_open_event { get; set; }
    }

    public class LinerAisEventsRoot
    {
        public List<Result> results { get; set; }
        public Links links { get; set; }
    }
}
