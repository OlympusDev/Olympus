using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Olympus.Showroom.MusicPlayer
{
    /// <summary>
    /// This handles all audio in Game. Will include references to AudioSources for music, sound effects etc. As well as the front end for GUI to call.
    /// Note: As of now only implementing for musicPlayer, possibly moving to MusicPlayer class that this AudioManager has, but for now keeping as is.
    /// </summary>
    public class MusicPlayer : MonoBehaviour
    {
        #region Fields relating to music Player/ Playing background music.
        //All songs in our resources
        Dictionary<string,List<PlayNode<AudioClip>>> allSongs;
        //Songs queued up to play
        PlayList<AudioClip> songPlayList;
        //Current song playing in player, may  not be same as current song in playlist
        PlayNode<AudioClip> currentSongPlaying;
        public AudioSource musicPlayer;
        enum MusicPlayerStates : byte
        {
            OFF,
            PAUSED,
            PLAYING,
            FASTFORWARDING,
            REWINDING,
            LOOPING

        }
        MusicPlayerStates currentState;
        #endregion

        void Start()
        {
            initMusicPlayer();
        }

        // Update is called once per frame
        void Update()
        {

            processMusicPlayer();
        }

        #region MusicPlayer back end
        void initMusicPlayer()
        {

            //There will be objects in scene with text, when they interact with those and music player then will play
            //Gets all the audio clips in resources
            AudioClip[] songs = Resources.LoadAll<AudioClip>("Sounds/Music");
            if (songs == null)
            {
                throw new System.Exception("Unable to get any audio clips or failed to find directory");
                /* The player can add the songs them selves
                //Adds them into playlist
                for (int i = 0; i < clips.Length; i++)
                {

                    songPlayList.add(new SongInfo<AudioClip>(clips[i].name, clips[i]));
                }
                */
                //turnOnMusicPlayer();
            }

            allSongs = new Dictionary<string, List<PlayNode<AudioClip>>>();
            foreach (AudioClip song in songs)
            {
                string songName = song.name.Substring(0, song.name.IndexOf("by", 0) + 1);
                //Need to find substring "by, gets everything past "by" assuming all song data will be formatted "songname" by "artist name"
                string artistName = song.name.Substring(song.name.IndexOf("by", 0), songs.Length - song.name.IndexOf("by"));

                if (!allSongs.ContainsKey(artistName))
                    allSongs[artistName] = new List<PlayNode<AudioClip>>();

                allSongs[artistName].Add(new PlayNode<AudioClip>(artistName, songName, song));
            }
            songPlayList = new PlayList<AudioClip>();
        }

      

        void processMusicPlayer()
        {
            if (currentState != MusicPlayerStates.OFF)
            {
                //No longer playing means clip done playing
                if (!musicPlayer.isPlaying)
                { 
                    //Means it's finished the song, or queued up a different song 
                    if (currentState != MusicPlayerStates.PAUSED)
                    {
                        
                        if (currentState != MusicPlayerStates.LOOPING)
                        {
                            musicPlayer.clip = songPlayList.next().song;
                        }
                        musicPlayer.Play();
                    }
                    else if (paused() && !songPlayList.contains(currentSongPlaying))
                    {
                        //If song selected finished playing, then make current clip where left off in playlist.
                        musicPlayer.clip = songPlayList.current.song;
                        musicPlayer.Play();
                    }
                }
            }
        }


        #endregion

        #region MusicPlayer front end

        public void turnOnMusicPlayer()
        {
            currentState = MusicPlayerStates.PLAYING;
        }

        public void turnOffMusicPlayer()
        {
            currentState = MusicPlayerStates.OFF;
            musicPlayer.Stop();
        }

        public bool paused()
        {
            return currentState == MusicPlayerStates.PAUSED;
        }

        //Thought about keeping as Pause and Unpause but it's basically a switch on and off so not worth 2 seperate methods 
        public void SwitchPlayerState()
        {
            if (currentState == MusicPlayerStates.PAUSED)
                musicPlayer.UnPause();
            else
            {
                musicPlayer.Pause();
                currentState = MusicPlayerStates.PAUSED;
            }
        }


        //This is called by playPrevSong() and playSong(Song) to reuse the loop check and music stopping lines
        public void playNextSong()
        {
            //Since auto plays next when stops, playNextSong just stops current one, BUT does not change state so still playing
            if (currentState == MusicPlayerStates.LOOPING)
                currentState = MusicPlayerStates.PLAYING;

            musicPlayer.Stop();
        }

        public void playPreviousSong()
        {
            //Goes back twice so then will play next is previous to current
            if (musicPlayer.time < musicPlayer.clip.length / 4)
                songPlayList.prev();
            //Otherwise restart current song
            songPlayList.prev();

            playNextSong();
         
        }

        #region To be called by some GUI to play specific songs
        public void playSong(string song, string artist)
        {
            try
            {
                PlayNode<AudioClip> songToPlay = null;
                foreach (var songNode in allSongs[artist])
                {
                    //If found sound by the artist then we can play it
                    if (songNode.name == song)
                    {
                        songToPlay = songNode;
                        break;
                    }
                }
                if (songToPlay == null) throw new System.Exception("We have no record of that song");
                //Pauses PlayList, incase song to play not in playlist.
                SwitchPlayerState();
                musicPlayer.clip = songToPlay.song;
                
                if (songPlayList.contains(songToPlay))
                {
                    //Sets song playing as current in playlist and then unpauses
                    songPlayList.current = songToPlay;
                    //Sets current song to one before want to play cause in update plays next
                    songPlayList.prev();
                    //unpauses then stops player so that update can auto go to next song.
                    SwitchPlayerState();
                    musicPlayer.Stop();
                }

            }
            catch (System.Exception err)
            {
                Debug.Log(err.Data.ToString());
                return;
            }
        }

        public void removeSongFromPlayList(string songName, string artist)
        {
            PlayNode<AudioClip> songData = songPlayList.find(songName,artist);
            //Now throwing from front end directly means if called via GUI might not be able to catch. I'm not 100% on that, will have to test.
            if (songData == null) throw new System.ArgumentException("That song is not currently in the playlist");
            songPlayList.remove(songData);
        }

        public void addSongToPlayList(string songName, string artist)
        {
            if (!allSongs.ContainsKey(artist))
            {
                throw new System.Exception("We do not hold any songs from that artist.");
            }

            PlayNode<AudioClip> songToAdd = null;

            //Need to parse name and artist name.
            foreach (PlayNode<AudioClip> song in allSongs[artist])
            {

                if (song.name == songName)
                {

                    songToAdd = song;
                    break;
                }
            }

            if (songToAdd == null)
                throw new System.Exception("We don't have a record of that song by this artist.");

            songPlayList.add(songToAdd);
        }

        #endregion
        public void loop()
        {
            if (currentState != MusicPlayerStates.LOOPING)
                currentState = MusicPlayerStates.LOOPING;
            else
                currentState = MusicPlayerStates.PLAYING;
        }

        
        //Not Used.
        public void sortPlayList(string priority)
        {
            //ToDo.
        }
        #endregion

       

    }

}