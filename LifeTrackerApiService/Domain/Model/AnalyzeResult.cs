using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LifeTrackerApiService.Domain.Model
{
    public class AnalyzeResult
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; }

        [JsonPropertyName("readResults")]
        public List<ReadResult> ReadResults { get; set; }
    }

    public class Appearance
    {
        [JsonPropertyName("style")]
        public Style Style { get; set; }
    }

    public class Line
    {
        [JsonPropertyName("boundingBox")]
        public List<int> BoundingBox { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("appearance")]
        public Appearance Appearance { get; set; }

        [JsonPropertyName("words")]
        public List<Word> Words { get; set; }
    }

    public class ReadResult
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("angle")]
        public double Angle { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("lines")]
        public List<Line> Lines { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonPropertyName("lastUpdatedDateTime")]
        public DateTime LastUpdatedDateTime { get; set; }

        [JsonPropertyName("analyzeResult")]
        public AnalyzeResult AnalyzeResult { get; set; }
    }

    public class Style
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }

    public class Word
    {
        [JsonPropertyName("boundingBox")]
        public List<int> BoundingBox { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }


}
