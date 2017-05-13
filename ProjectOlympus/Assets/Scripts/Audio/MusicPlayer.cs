using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This handles all audio in Game. Will include references to AudioSources for music, sound effects etc. As well as the front end for GUI to call.
/// Note: As of now only implementing for musicPlayer, possibly moving to MusicPlayer class that this AudioManager has, but for now keeping as is.
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    #region Fields relating to music Player/ Playing background music.
    //All songs in our resources
    AudioClip[] allSongs;
    //Songs queued up to play
    PlayList<AudioClip> songPlayList;
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
	// Use this for initialization
	void Start ()
    {
        initMusicPlayer();
    }

    #region MusicPlayer back end
    void initMusicPlayer()
    {

        //There will be objects in scene with text, when they interact with those and music player then will play
        //Gets all the audio clips in resources
        allSongs = Resources.LoadAll<AudioClip>("Sounds/Music");
        if (allSongs == null)
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
        songPlayList = new PlayList<AudioClip>();
    }

    void processMusicPlayer()
    {
        if (currentState != MusicPlayerStates.OFF)
        {
            //Means it's finished the song, or queued up a different song 
            if (currentState != MusicPlayerStates.PAUSED && !musicPlayer.isPlaying)
            {
                if (currentState != MusicPlayerStates.LOOPING)
                {
                    musicPlayer.clip = songPlayList.next().song;
                }
                musicPlayer.Play();
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

    //Thought about keeping as Pause and Unpause but it's basically a switch on and off so not worth 2 seperate methods 
    public void SwitchSongState()
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
    public void playSong(string song)
    {
        try
        {
            /* //If not in playlist then this thrown to catch below
             removeSong(song);
             //If not thrown need to re add the song, actually but then this fucks the placement so no good
             enqueueSong(song);*/
            var songData = songPlayList.find(song);
            if (songData == null) throw new System.ArgumentException("That song is not in our data");
            songPlayList.current = songPlayList.find(song);
        }
        catch(System.Exception err)
        {
            Debug.Log(err.Data.ToString());
            return;
        }
        //So that in update next in list will be the one passed into this method.
        songPlayList.prev();

        playNextSong();        

    }

    public void removeSong(string song)
    {
        var songData = songPlayList.find(song);
        if (songData == null) throw new System.ArgumentException("That song is not in our data");
            songPlayList.remove(songData);
    }

    public void enqueueSong(string song)
    {
        SongInfo<AudioClip> toAdd;
        foreach (AudioClip clip in allSongs)
            if (clip.name == song)
            {
                toAdd = new SongInfo<AudioClip>(clip.name,clip);
                break;
            }
    }

    #endregion
    public void loop()
    {
        if (currentState != MusicPlayerStates.LOOPING)
            currentState = MusicPlayerStates.LOOPING;
        else
            currentState = MusicPlayerStates.PLAYING;
    }

    public string[] viewAllSongs()
    {
        string[] songNames = new string[allSongs.Length];
        for (int i = 0; i < allSongs.Length; i++)
        {
            songNames[i] = allSongs[i].name;
        }
        return songNames;
    }
    //Not Used.
    public void sortPlayList(string priority)
    {
        //ToDo.
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {

        processMusicPlayer();
    }

}

