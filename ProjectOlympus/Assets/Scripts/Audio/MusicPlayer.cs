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
        //Assigned in editor so that can put in diff MusicPlayers per scene for whatever reason.
        // musicPlayer = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        //Incase file paths are different

        AudioClip[] clips = (Resources.LoadAll<AudioClip>("Sounds\\Music") == null) ? Resources.LoadAll<AudioClip>("Sounds/Music")
            : Resources.LoadAll<AudioClip>("Sounds\\Music");

        songPlayList = new PlayList<AudioClip>();

        for (int i = 0; i < clips.Length; i++)
        {
            //Will create as many song objects as there are songs found in folder, I don't want to move Prefabs to Resources cause may cause conflicts, so currently
            //just inside Resources and does work.
            GameObject song = Instantiate(Resources.Load("Song")) as GameObject;
            songPlayList.add(new SongInfo<AudioClip>(clips[i].name, clips[i]));

            song.GetComponent<ISong<AudioClip>>().songInfo = songPlayList[i];
            song.transform.parent = musicPlayer.transform;
            song.transform.localPosition = Vector3.forward * 3;
        }
        turnOnMusicPlayer();
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

    public void playSong(ISong<AudioClip> songHolder)
    {
        songPlayList.current = songHolder.songInfo;

        //So that in update next in list will be the one passed into this method.
        songPlayList.prev();

        playNextSong();        

    }

    public void dequeueSong(ISong<AudioClip> songHolder)
    {
        playNextSong();

        songPlayList.remove(songHolder.songInfo);
    }

    public void enqueueSong(ISong<AudioClip> songHolder)
    {
        songPlayList.add(songHolder.songInfo);
    }

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
        switch (priority)
        {
            case "least":
                songPlayList.sortLeastPlayed();
                break;
            case "most":
                songPlayList.sortMostPlayed();
                break;
            case "random":
                songPlayList.shuffle();
                break;
        }

    }
    #endregion

    // Update is called once per frame
    void Update ()
    {

        processMusicPlayer();
    }

}
