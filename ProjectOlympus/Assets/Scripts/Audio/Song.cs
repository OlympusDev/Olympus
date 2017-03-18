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
    public SongNode<AudioClip> songInfo;
    // Use this for initialization
    void Start()
    {
       
        manager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GetComponent<Button>().onClick.AddListener(() => manager.playSong(songInfo.placeInList));
    }

		
		
}
