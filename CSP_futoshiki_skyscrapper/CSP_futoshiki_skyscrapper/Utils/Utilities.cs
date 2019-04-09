using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsvHelper;

namespace CSP_futoshiki_skyscrapper.Utils
{
    class Utilities
    {
        public enum GAME_TYPE_ENUM { FUTOSHIKI, SKYSCRAPPER};
        public enum HEURISTIC_TYPE_ENUM { MOST_LIMITING, GREEDY}

        public const string SOUND_FILE_NAME = @"sound.wav";
        public const string CSV_SAVE_LOCATION_STATISTICS = @"C:\Users\marar\Desktop\CSPResults\";


        //public const string FILE_NAME = @"Data_bad\test_futo_4_1.txt";
        public const string FILE_NAME = @"Data_bad\test_sky_4_0.txt";
        //public const GAME_TYPE_ENUM GAME_TYPE = GAME_TYPE_ENUM.FUTOSHIKI;
        public const GAME_TYPE_ENUM GAME_TYPE = GAME_TYPE_ENUM.SKYSCRAPPER;

        public const HEURISTIC_TYPE_ENUM HEURISTIC_TYPE = HEURISTIC_TYPE_ENUM.MOST_LIMITING;


        public const string DATA_SEPARATOR = ";";
        public const string RELATIONS_FILE_SEPARATOR = "REL:";
        public const string CSV_FILE_EXTENSION = ".csv";

        public static void SaveStatisticsToFile(string method, List<CsvStatistics> statisticsList)
        {
            string problemName = FILE_NAME.Substring(8, FILE_NAME.Length - 4-8);
            using (var writer = new StreamWriter($"{CSV_SAVE_LOCATION_STATISTICS}{problemName}-{method}-{DateTime.Now.ToFileTime()}{CSV_FILE_EXTENSION}", true))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteComment("STATYSTYKI");
                csv.NextRecord();
                csv.WriteHeader<CsvStatistics>();
                csv.NextRecord();
                csv.WriteRecords(statisticsList);
            }
        }

    }
}
