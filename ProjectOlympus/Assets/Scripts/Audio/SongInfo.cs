using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Made this a struct because when copy contents to diff list, I need it to be copied by value so that their placeInLists aren't changed incorrectly.
//Will just explicitly preform shallow copy, need it to be passed by ref sometimes, since buttons will have Song classes attached to them, I don't want this as MonoBehaviour
//becaues it will lock it into Unity. So another class will be called SongInfo, which will have all public fields for me to change what it passes into playList to play
//correct song. Right now, it will literally have nothing other than placeInlist field, so bit of circular dependancy, it wil depend on AudioManager to correct
//the placeInList var in each Song GameObject, and then Audio manager depends on Song GameObject to send back previously assigned placeInList to keep random access O(1)
public class SongInfo<SongFormat>
{ 
    //The actual audio data
    SongFormat data;

    //Keeps track amount of times this song was played, kept in track incase of wanting to have sort mostplayed/least played functionality
    private int timesPlayed;

    public SongInfo(string name,SongFormat val) 
    {
        this.name = name;
        this.data = val;
        timesPlayed = 0;    
    }

    public SongInfo(SongInfo<SongFormat> rhs)
    {
        name = rhs.name;
        data = rhs.data;
        timesPlayed = 0;
    //    timesPlayed = rhs.timesPlayed;
    }

    public string name
    {
        private set;
        get;
    }

    public SongFormat song
    {
        get
        {
            //Anytime access songs means playing, but then again can't assume this cause can get song just for more info on it?
            //Returns song to play in whatever player consumer happens to have.
            return data;
        }
    }

    public int playCounter
    {
        get
        {
            return timesPlayed;
        }
        set
        {
            timesPlayed = value;
        }
    }

    //Defined for sorting based on times played
    public static bool operator>(SongInfo<SongFormat> lhs, SongInfo<SongFormat> rhs)
    {
        return lhs.timesPlayed > rhs.timesPlayed;
    }

    public static bool operator<(SongInfo<SongFormat> lhs, SongInfo<SongFormat>  rhs)
    {
        return lhs.timesPlayed < rhs.timesPlayed;
    }

}
