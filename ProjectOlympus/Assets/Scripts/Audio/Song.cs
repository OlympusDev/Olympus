using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//It needs to have text for song name, remaining length, and times played.
//It will have children buttons for specifically playing/pausing/stopping, but the object itself is also clickable for playing/pausing
//Each indivudla Song(Prob rename something else) will handle the GUI for themselves then UI manager handle actual playlist
[RequireComponent (typeof(Button))]
public class Song : MonoBehaviour, ISong<AudioClip>
{

    #region Everything needed to play the song
    public MusicPlayer audioManager;
    public SongInfo<AudioClip> songInfo { get; set; }
    #endregion

    #region Specific buttons that are children of this GameObject.
    public Button stopButton;
    public Button playButton;
    public Button mute;
    #endregion

    #region All the UI elements attached to this GameObject
    private Button self;
    #endregion


    // Use this for initialization
    void Start ()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<MusicPlayer>();
        self = GetComponent<Button>();
        //ToDo: After decide what objects ahve each button, add listener a lambda that calls corresponding function in AudioManager
    }


    public void updateGUISongInfo()
    {
        //Probably not needed, depends on how we do the GUI for music player, so I'ma just leave this as is
     //   guiSongInfo.text = "Name: " + songInfo.name + "Length: " + songInfo.length.ToString() + "Played: " + songInfo.playCounter.ToString(); 
    }

	// Update is called once per frame
	void Update ()
    {
	}
}
