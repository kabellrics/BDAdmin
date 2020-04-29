using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class RootObject
    {
        public string error { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int number_of_page_results { get; set; }
        public int number_of_total_results { get; set; }
        public int status_code { get; set; }
        public List<Result> results { get; set; }
        public string version { get; set; }
    }
    public class Result
    {
        public Image image { get; set; }
        public string issue_number { get; set; }
        public string name { get; set; }
        public Volume volume { get; set; }
        public string resource_type { get; set; }
    }
    public class Volume
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }
    public class Image
    {
        public string icon_url { get; set; }
        public string medium_url { get; set; }
        public string screen_url { get; set; }
        public string screen_large_url { get; set; }
        public string small_url { get; set; }
        public string super_url { get; set; }
        public string thumb_url { get; set; }
        public string tiny_url { get; set; }
        public string original_url { get; set; }
        public string image_tags { get; set; }
    }
}
