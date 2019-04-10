using System;
using CSP_futoshiki_skyscrapper.Utils;
using static System.Console;
using CSP_futoshiki_skyscrapper.CSP;
using NAudio.Wave;

namespace CSP_futoshiki_skyscrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            DataLoader dataLoader = new DataLoader();

            if (Utilities.ALGORITHM_TYPE == Utilities.ALGORITHM_TYPE_ENUM.BACKTRACKING)
            {
                CSPBacktracking cSPBacktracking = new CSPBacktracking();

            }
            else if (Utilities.ALGORITHM_TYPE == Utilities.ALGORITHM_TYPE_ENUM.FORWARD_CHECKING)
            {
                CSPForwardChecking cSPForwardChecking = new CSPForwardChecking();

            }

            WriteLine("finished!");
            PlayFinishSound();
            //ReadLine();
        }

        private static void PlayFinishSound()
        {
            WaveStream mainOutputStream = new WaveFileReader(Utilities.SOUND_FILE_NAME);
            WaveChannel32 volumeStream = new WaveChannel32(mainOutputStream);

            WaveOutEvent player = new WaveOutEvent();

            player.Init(volumeStream);

            player.Play();

        }

    }
}
