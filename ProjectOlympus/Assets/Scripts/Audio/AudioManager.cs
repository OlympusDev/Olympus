using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This handles all audio in Game. Will include references to AudioSources for music, sound effects etc. As well as the front end for GUI to call.
/// Note: As of now only implementing for musicPlayer, possibly moving to MusicPlayer class that this AudioManager has, but for now keeping as is.
/// </summary>
public class AudioManager : MonoBehaviour
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
    PlayList<AudioClip> soundEffects;
	// Use this for initialization
	void Start ()
    {
        initMusicPlayer();
    }

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
        }

        //Sets up visual list of songs to have correct names and songInfo in song Componenets
        reorganizeList();

        //There will be a MusicPlayer object that will handle tunring on, but might use this also for bg music so maybe playing at Start afterall.
        //currentState = MusicPlayerStates.PLAYING;
        turnOffMusicPlayer();
    }

    #region MusicPlayer interface

    //Only when player opens musicPlayer, or however we'll handle player starting to play the music
    public void turnOnMusicPlayer()
    {

        currentState = MusicPlayerStates.PLAYING;
        
    }

    //Could do same as did for pausing and call it switch MusicPlayerState, but could vary to many things, like looping etc. so better this way as seperate methods
    //Plus it won't be same button because maybe touch to turn on and walk away a certain distance to turn off or something
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

  
    public void playNextSong()
    {
        //Since auto plays next when stops, playNextSong just stops current one, BUT does not change state so still playing
        musicPlayer.Stop();
        //musicPlayer.clip = songPlayList.next();
    }

    public void playPreviousSong()
    {
        //Goes back twice so then will play next is previous to current
        if (musicPlayer.time < musicPlayer.clip.length / 4)
            songPlayList.prev();
        //Otherwise restart current song
        songPlayList.prev();
    }

    //Actually at this point, there's no point for the playlist, CAUSE PASSING WHAT TO PLAY ANYWAY LOL FUCK THIS IS STUPID, I'M BEING STUPID, THERE MUST BE BETTER WAY THAN THIS
    //THEN.
    public void playSong(ISong<AudioClip> songHolder)
    {
        //If song passed in same as current, then just pause/unpause the song
        if (musicPlayer.clip == songPlayList[songHolder.songInfo].song && musicPlayer.clip.length > 0)
        {
            this.SwitchSongState();
        }
        else
            songPlayList.current = songHolder.songInfo;


        //So that in update next in list will be the one passed into this method.
        playPreviousSong();
    }

    public void loop()
    {
        currentState = MusicPlayerStates.LOOPING;
    }

    //Was going to do -1 for least and 1 for most, but this wil leave it open for different sorts to do on list
    //If not going to add any can swtich back.
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

        reorganizeList();
    }
    #endregion

    #region MusicPlayer GUI management

    ///I have tested these functions only on list that wasn't modified for n number of songs and worked, still need to implement changing the sequence of the list
    ///before can test it for that, but theoritically should work.


    //Only called after sorting, to maintain O(1) random access
    private void correctPlaceInList(GameObject [] array)
    {
        List<GameObject> list = new List<GameObject>(array);
        //Now What I need to do for this is find correct song this was
 
        foreach (GameObject song in list)
        {
            //Find corresponding song and updates it's place in the backendList to match placeIn GUI.
            //Copies  current songInfo of song to updated songInfo in playList, if it's held by reference than I don't have to keep copying it over and over.
            song.GetComponent<Song>().songInfo = songPlayList.find(song.GetComponent<Song>().songInfo);
        }
    }
    //This works, tested for adding songs to MusicPlayer, removing, and initial construction. No rearraing algorithms have been implemented so have not been tested for those
    //but what it's doing is same regardless so theoritically should work.
    private void reorganizeList()
    {
          GameObject[] songsInGuiList = GameObject.FindGameObjectsWithTag("Song");
          for (int i = 0; i < songsInGuiList.Length; i++)
          {
               //Updates all GameObjects text to match current order of list.
               songsInGuiList[i].GetComponent<TextMesh>().text = songPlayList[i].name;        
          }        

          correctPlaceInList(songsInGuiList);
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {
        //This needs to be reworked, cause perfectly working and fine for queue in sequence playing but on random playing this breaks caues of the next() part
        if (currentState != MusicPlayerStates.OFF)
        {
            //Means it's finished the song
            if (currentState != MusicPlayerStates.PAUSED && !musicPlayer.isPlaying)
            {
                if (currentState != MusicPlayerStates.LOOPING)
                    musicPlayer.clip = songPlayList.next().song;

                musicPlayer.Play();
            }
        }

        //For testing purposes
        if (Input.GetKeyDown(KeyCode.N))
            playNextSong();
        if (Input.GetKeyDown(KeyCode.P))
            playPreviousSong();
        if (Input.GetKeyDown(KeyCode.S))
            sortPlayList("random");
    }

}
