using System;
using System.Collections.Generic;
using System.Text;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;


namespace CSP_futoshiki_skyscrapper.Utils
{
    class DataLoader
    {
        private delegate void loaderMethodDelegate();

        private loaderMethodDelegate loaderMethodFunction;

        public DataLoader()
        {
            if(GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
            {
                loaderMethodFunction = LoadDataForFutoshiki;
                CheckIfCorrectFileSelected("futo");
            }
            else if(GAME_TYPE == GAME_TYPE_ENUM.SKYSCRAPPER)
            {
                loaderMethodFunction = LoadDataForSkyscrapper;
                CheckIfCorrectFileSelected("sky");
                
            }

            loaderMethodFunction();
        }

        private void CheckIfCorrectFileSelected(string prefix)
        {
            if (!FILE_NAME.Contains(prefix))
            {
                throw new WrongFileException($"selected file is not {prefix} file");
            }
        }

        private void LoadDataForSkyscrapper()
        {

        }

        private void LoadDataForFutoshiki()
        {

        }

    }
}
