using System;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace SpotifyAdSilencer
{
    static class Program
    {
        static void Main()
        {
            // Create a logger
            ILog log = LogManager.GetLogger(typeof(SpotifyPlayerWrapper));

            // Create a spotify player wrapper
            SpotifyPlayerWrapper spotifyPlayerWrapper = new SpotifyPlayerWrapper();
            Console.WriteLine("SpotifyAdSilencer is started.");
            Console.WriteLine("Hold ESC key to stop!");
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Search for Spotify Player Window Class Name
                    Dictionary<IntPtr, MicrosoftSpyValues> windowCommaClass = 
                        SpotifyPlayerWrapper.GetSpotifyPlayerWindowByProcessName();

                    // If Spotify winfows is found, do following
                    if (windowCommaClass.Count != 0)
                    {
                        // Search sub-window names in the class name
                        foreach (KeyValuePair<IntPtr, MicrosoftSpyValues> window in windowCommaClass)
                        {
                            // If windows contains "-" then it plays song
                            if (window.Value.WindowName.Contains("-"))
                            {
                                log.Debug("Spotify is playing song");

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
                        log.Debug("Spotify process is not found!");
                    }
                    // Do this control every ten seconds
                    Thread.Sleep(10000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
