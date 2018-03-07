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
                        Console.WriteLine("Spotify Window is found");

                        // Search sub-window names in the class name
                        foreach (KeyValuePair<IntPtr, string> window in windowsByClassName)
                        {
                            // If windows contains "-" then it plays song
                            if (window.Value.Contains("-") || window.Value.Equals("Spotify"))
                            {
                                log.Info("Spotify is playing song");

                                // If volume is muted, then unmute it
                                if (AudioManager.GetMasterVolumeMute())
                                {
                                    Console.WriteLine("Muted! Unmuting...");
                                    log.Info("Muted! Unmuting...");
                                    AudioManager.SetMasterVolumeMute(false);
                                    AudioManager.SetMasterVolume(50);
                                }
                            }
                            // If windows does not contain "-", then it plays add.
                            else
                            {
                                Console.WriteLine("Spotify is playing add");
                                log.Info("Spotify is playing add");
                                AudioManager.SetMasterVolumeMute(true);
                            }
                        }
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
