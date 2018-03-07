using System;
using System.Collections.Generic;
using System.Threading;
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
                while (!Console.KeyAvailable)
                {
                    // Search for Spotify Player Window Class Name
                    Dictionary<IntPtr, string> windowsByClassName = 
                        spotifyPlayerWrapper.GetSpotifyPlayerWindowByClassName();

                    // If it is found, do following
                    if (windowsByClassName.Count != 0)
                    {
                    }
                    // If it is not found, log a message to screen
                    else
                    {
                        Console.WriteLine("Error: windowsByClassName.Count == 0");
                    }
                    // Do this control every ten seconds
                    Thread.Sleep(10000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
