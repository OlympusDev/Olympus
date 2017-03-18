using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Made this a struct because when copy contents to diff list, I need it to be copied by value so that their placeInLists aren't changed incorrectly.
//Will just explicitly preform shallow copy, need it to be passed by ref sometimes, since buttons will have Song classes attached to them, I don't want this as MonoBehaviour
//becaues it will lock it into Unity. So another class will be called SongInfo, which will have all public fields for me to change what it passes into playList to play
//correct song. Right now, it will literally have nothing other than placeInlist field, so bit of circular dependancy, it wil depend on AudioManager to correct
//the placeInList var in each Song GameObject, and then Audio manager depends on Song GameObject to send back previously assigned placeInList to keep random access O(1)
public struct SongNode<SongFormat>
{ 
    //The actual audio data
    SongFormat data;



    //Keeps track amount of times this song was played, kept in track incase of wanting to have sort mostplayed/least played functionality
    private int timesPlayed;

    public SongNode(string name,SongFormat val, int placeInList) 
    {
        this.name = name;
        this.data = val;
        this.placeInList = placeInList;
        timesPlayed = 0;
    }

    public SongNode(SongNode<SongFormat> rhs)
    {
        name = rhs.name;
        data = rhs.data;
        placeInList = rhs.placeInList;
        timesPlayed = rhs.timesPlayed;
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
            ++timesPlayed;
            //Returns song to play in whatever player consumer happens to have.
            return data;
        }
    }


    //To maintain O(1) random access even after sorting to least played or most played. Will just update their placeInList property, so don't have to search for matching name.
    //Although does restrict this node to be used in a list. That's assuming they are passing in SongNodes into the List
    //Actually if use dictionary that's also O(1) and already made for me... kay. No since also like queue, it's not hashmap so won't work that easily, so I do ahve to do this
    //Another option is having PlayList have Dequeue for linear travel and dictionary for random access, more memory useoverall since storing same data just keyed differently
    //But will make access O(1)

        /// <summary>
        /// This will be how random access is handled. Player clicks a song, and tied to click event will be to playSpecific song in PlayList and passing in the correct Index
        /// So every Song GameObject will SongComponent that has copy of SongNode in playList.
        /// </summary>
    public int placeInList
    {
        get;
        set;
    }

    public static bool operator>(SongNode<SongFormat> lhs, SongNode<SongFormat> rhs)
    {
        return lhs.timesPlayed > rhs.timesPlayed;
    }

    public static bool operator<(SongNode<SongFormat> lhs, SongNode<SongFormat>  rhs)
    {
        return lhs.timesPlayed < rhs.timesPlayed;
    }

}
