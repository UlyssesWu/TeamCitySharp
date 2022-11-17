using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TeamCitySharp
{
    internal class Helper
    {
    }

    /// <summary>
    /// Parse Teamcity modificationTime
    /// </summary>
    public class TeamCityDateTimeConverter : IsoDateTimeConverter
    {
        public TeamCityDateTimeConverter()
        {
            DateTimeFormat = "yyyyMMdd'T'HHmmsszzz";
        }

        public TeamCityDateTimeConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
