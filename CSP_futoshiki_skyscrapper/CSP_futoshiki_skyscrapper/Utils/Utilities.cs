using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsvHelper;
using CSP_futoshiki_skyscrapper.CSP;

namespace CSP_futoshiki_skyscrapper.Utils
{
    class Utilities
    {
        public enum GAME_TYPE_ENUM { FUTOSHIKI, SKYSCRAPPER};
        public enum HEURISTIC_TYPE_ENUM { MOST_LIMITING, GREEDY, SMALL_DOMAIN_AND_MANY_CONSTRAINTS}
        public enum ALGORITHM_TYPE_ENUM { BACKTRACKING, FORWARD_CHECKING}

        public const string SOUND_FILE_NAME = @"sound.wav";
        public const string CSV_SAVE_LOCATION_STATISTICS = @"C:\Users\marar\Desktop\CSPResults\Statistics\";
        public const string CSV_SAVE_LOCATION_SOLUTION = @"C:\Users\marar\Desktop\CSPResults\Solutions\";


        //public const string FILE_NAME = @"Data_bad\test_sky_4_1.txt";
        //public const GAME_TYPE_ENUM GAME_TYPE = GAME_TYPE_ENUM.SKYSCRAPPER;

        public const string FILE_NAME = @"Data_bad\test_futo_7_2.txt";
        public const GAME_TYPE_ENUM GAME_TYPE = GAME_TYPE_ENUM.FUTOSHIKI;

        public const HEURISTIC_TYPE_ENUM HEURISTIC_TYPE = HEURISTIC_TYPE_ENUM.SMALL_DOMAIN_AND_MANY_CONSTRAINTS;
        public const ALGORITHM_TYPE_ENUM ALGORITHM_TYPE = ALGORITHM_TYPE_ENUM.BACKTRACKING;

        public const string DATA_SEPARATOR = ";";
        public const string RELATIONS_FILE_SEPARATOR = "REL:";
        public const string CSV_FILE_EXTENSION = ".csv";
        public const string TXT_FILE_EXTENSION = ".txt";

        public static void SaveStatisticsToFile(string method, List<CsvStatistics> statisticsList)
        {
            string problemName = FILE_NAME.Substring(8, FILE_NAME.Length - 4-8);
            string heuristicName = HEURISTIC_TYPE.ToString();
            using (var writer = new StreamWriter($"{CSV_SAVE_LOCATION_STATISTICS}{problemName}-{method}-{heuristicName}-{DateTime.Now.ToFileTime()}{CSV_FILE_EXTENSION}", true))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteComment("STATYSTYKI");
                csv.NextRecord();
                csv.WriteHeader<CsvStatistics>();
                csv.NextRecord();
                csv.WriteRecords(statisticsList);
            }
        }

        public static void SaveAllSolutionsToTxtFile(string method, List<ICSPSolvable> solutionsList)
        {
            string problemName = FILE_NAME.Substring(9, FILE_NAME.Length - 4 - 8);
            string heuristicName = HEURISTIC_TYPE.ToString();

            for (int i = 0; i < solutionsList.Count; i++)
            {
                using (StreamWriter sr = File.CreateText($"{CSV_SAVE_LOCATION_SOLUTION}{i + 1}.-{problemName}-{method}-{heuristicName}-{DateTime.Now.ToFileTime()}{TXT_FILE_EXTENSION}"))
                {
                    sr.Write(solutionsList[i].ToString());
                };
               
            }

        }
    }
}
