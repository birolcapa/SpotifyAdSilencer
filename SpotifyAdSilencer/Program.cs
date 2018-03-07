using System;
using log4net;

namespace SpotifyAdSilencer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a logger
            ILog log = LogManager.GetLogger(typeof(SpotifyPlayerWrapper));

            // Create a spotify player wrapper
            SpotifyPlayerWrapper spotifyPlayerWrapper = new SpotifyPlayerWrapper();
            
            Console.WriteLine("Hold esc key to stop");
            do
            {
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
