using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This will keep a reference to SongNode in music PlayList, main purpose of this is to maintain constant time random access  even after
/// rearranging the playlist by passing in this specific songs new placeInList which was updated here when sorted in AudioManager and PlayList.
/// </summary>
public class Song : MonoBehaviour
{
    AudioManager manager;
    //Each song willl have a buttons childrened under it, play,pause, etc as well as one over everything, this will also have fast forward/playNext/playPrev buttons but for now leaving out
    //Because design of playList may change to not have this GUI. But these buttons are on every song, for random access, and if see play on a song slot, usually see pause too
    //Note: I have not gave them these buttons yet.
    public Button playButton;
    public Button pauseButton;
    public SongInfo<AudioClip> songInfo;
    // Use this for initialization
    void Start()
    {
       
        manager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //Plays song
        playButton.onClick.AddListener(() => manager.playSong(songInfo.placeInList));
        //Pauses and Unpauses song. Edit: Spotify has it be same button and just swaps it, but will have it be able to pause and restart, to pause it's own thing and play
        //will restart song
        pauseButton.onClick.AddListener(() => manager.SwitchSongState());
    }
		
}
