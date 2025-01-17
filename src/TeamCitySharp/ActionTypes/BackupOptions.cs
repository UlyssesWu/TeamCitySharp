﻿namespace TeamCitySharp.ActionTypes
{
    public class BackupOptions
    {
        public string FileName { get; set; }

        public bool IncludeDatabase { get; set; }

        public bool IncludeConfigurations { get; set; }

        public bool IncludeBuildLogs { get; set; }

        public bool IncludePersonalChanges { get; set; }
    }
}