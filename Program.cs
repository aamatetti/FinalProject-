using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MusicPlaylistAnalyzer
{
    public class Song
    {
        public string Name, Artist, Album, Genre;
        public int Size, Time, Year, Plays;

        public Song(string name, string artist, string album, string genre, int size, int time, int year, int plays)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Genre = genre;
            Size = size;
            Time = time;
            Year = year;
            Plays = plays;
        }

        public string toString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", Name, Artist, Album, Genre, Size, Time, Year, Plays);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length !=2)
            {
                Console.WriteLine("Please follow the correct input parameters: MusicPlaylistAnalyzer <music_playlist_file_path> <report_file_path>");
                Environment.Exit(0);
            }

            List<Song> songList = new List<Song>();

            try
            {
                using (StreamReader streamRead = new StreamReader(args[0]))
                {
                    var lineNum = 0;
                    var rowNum = 8;
                    streamRead.ReadLine();

                    while(streamRead.EndOfStream)
                    {
                        string line = streamRead.ReadLine();
                        lineNum++;
                        string[] strings = line.Split('\t');

                        if (strings.Length > 8)
                        {
                            Console.WriteLine($"Row {lineNum} contains {strings.Length} values. It should only contain {rowNum}.");
                            Environment.Exit(0);
                        }

                        else if (strings.Length < 8)
                        {
                            Console.WriteLine($"Row {lineNum} contains {strings.Length} values. It should contain {rowNum}.");
                            Environment.Exit(0);
                        }

                        else 
                        {
                            Song songData = new Song((strings[0]), (strings[1]), (strings[2]), (strings[3]), Int32.Parse(strings[4]), Int32.Parse(strings[5]), Int32.Parse(strings[6]), Int32.Parse(strings[7]));
                            songList.Add(songData);
                        }
                    }

                    streamRead.Close();

                }

             }
           
            catch (Exception)
            {
                Console.WriteLine("Playlist cannot be opened.");
            }

            try
            {
                using (StreamWriter streamWrite = new StreamWriter(args[1]))
                {
                    Song[] songs = songList.ToArray();
                    streamWrite.WriteLine("Music Playlist Report\n");

                    var over199 = from Song in songs where Song.Plays >= 200 select Song;
                    streamWrite.WriteLine("Songs that receive 200 or more plays: ");
                    foreach (Song song in over199)
                    {
                        streamWrite.WriteLine(song.toString());
                    }

                    var alternativeGenre = from Song in songs where Song.Genre = "Alternative" select Song;
                    int i = 0;
                    foreach (Song song in alternativeGenre)
                    {
                        i++;
                    }
                    streamWrite.WriteLine("Number of alternative songs: {i}\n");

                    var HipHopRapGenre = from Song in songs where Song.Genre = "HipHop/Rap" select Song;
                    i = 0;
                    foreach (Song song in HipHopRapGenre)
                    {
                        i++;
                    }
                    streamWrite.WriteLine("Number of HipHop/Rap songs: {i}\n");

                    var fromtheFishBowl = from Song in songs where Song.Album = "Welcome to the Fishbowl" select Song;
                    streamWrite.WriteLine("Songs for Welcome to the Fishbowl Album: ");
                    foreach (Song song in fromtheFishBowl)
                    {
                        streamWrite.WriteLine(song.toString());
                    }

                    var before1970 = from Song in songs where Song.Year < 1970 select Song;
                    streamWrite.WriteLine("\nSongs from before 1970: ");
                    foreach (Song song in before1970)
                    {
                        streamWrite.WriteLine(song.toString());
                    }

                    var longerThan85 = from Song in songs where Song.Name.Length > 85 select Song;
                    streamWrite.WriteLine("\nSong names longer than 85 characters: ");
                    foreach (Song song in longerThan85)
                    {
                        streamWrite.WriteLine(song.toString());
                    }

                    var longestSongs = from Song in songs orderby Song.Time descending select Song;
                    var longestSong = longestSongs.First();
                    streamWrite.WriteLine("\nThe longest song is: ");
                    streamWrite.WriteLine(longestSong.toString());

                    streamWrite.Close();
                   
                }

                Console.WriteLine("\n Your Music Playlist Report has been created!");

            }

            catch (Exception)
            {
                Console.WriteLine("\n Error: Report file cannot be opened");
            }

            Console.ReadLine();
        }
    }
}
