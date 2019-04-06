using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.Utils
{
    class Utilities
    {
        public enum GAME_TYPE_ENUM { FUTOSHIKI, SKYSCRAPPER};

        public const string FILE_NAME = @"Data_bad\test_futo_8_1.txt";
        //public const string FILE_NAME = @"Data_bad\test_sky_4_0.txt";
        public const GAME_TYPE_ENUM GAME_TYPE = GAME_TYPE_ENUM.FUTOSHIKI;
        //public const GAME_TYPE_ENUM GAME_TYPE = GAME_TYPE_ENUM.SKYSCRAPPER;
        
        public const string DATA_SEPARATOR = ";";
        public const string RELATIONS_FILE_SEPARATOR = "REL:";
    }
}
