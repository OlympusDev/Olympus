using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Data Structure for holding songs and going from song to song, this does not handle playing the songs
/// </summary>
/// <
/// typeparam name="SongFormat"> This parameter so works for Unity's audioClips, or anyother sound handling Object other libraries may have.</typeparam>
public class PlayList<SongFormat>
{
 

    List<SongNode<SongFormat>> playList;
    //For going back to original sequence if decide not to have sorted anymore.
    List<SongNode<SongFormat>> originalSequence;
    SongNode<SongFormat> currentSong;


    public PlayList()
    {
        playList = new List<SongNode<SongFormat>>();
       // originalSequence = playList;
       // currentSong = songs[0]; 
    }


    public PlayList(string[] names, SongFormat[] listOfSongs)
    {
        playList = new List<SongNode<SongFormat>>();
        for (int i = 0; i < listOfSongs.Length; i++)
        {
           playList.Add(new SongNode<SongFormat>(names[i],listOfSongs[i],i));
            //Diff copies not referencing same node, because index of songs can change if sorted, where as elements in original sequence are forever same indicies.
           // originalSequence.Add(new SongNode<SongFormat>(listOfSongs[i],i));*/
        }
        current = playList[0];
        
    } 
    public int size
    {
        get { return playList.Count; }
    }

    #region Editing Contents of play list

    public void add(string songName,SongFormat song)
    {
        //Appending to last so 
        SongNode<SongFormat> node = new SongNode<SongFormat>(songName, song, playList.Count);

        if (originalSequence != null)
            originalSequence.Add(node);

        playList.Add(node);

    }

    public void remove(string songName)
    { }

    public void remove(SongFormat song)
    {
        
    }
    public void remove(SongNode<SongFormat> song)
    {
        remove(song.name);
    }
    #endregion //only add is implemented and not currently used/tested because getting all songs upon constructing.

    #region Methods for random access of items in playList
    //Returns node, because will use this to also update the names, if for whatever reason keeps SongNode that inserted here kept 
    public SongNode<SongFormat> this[SongNode<SongFormat> i]
    {
        get
        {
            //currentSong = songs[i.index];
            return playList[i.placeInList];
        }
    }

    public SongNode<SongFormat> this[int i]
    {
        get
        {
            return playList[i];
        }
    }

    public SongNode<SongFormat> find(SongNode<SongFormat> toFind)
    {
        return playList.Find((SongNode<SongFormat> comparing) => { return toFind.name == comparing.name; });
    }

    #endregion

    #region Changing sequence of play list
    public void sortMostPlayed()//Constructs max heap based on timesplayed of each songNode and assigns it to songs
    {
        //Only need to allocate memroy for originalSequeunce and copy contents if switching sequence
        if (originalSequence == null) originalSequence = new List<SongNode<SongFormat>>(playList);

        ///Still need to implement
    }

    public void sortLeastPlayed()//Constructs min heap based on timesPlayed of each songNode and assigns it to songs
    {
        if (originalSequence == null) originalSequence = new List<SongNode<SongFormat>>(playList);
        ///Still need to implement
    }

    #endregion

    #region Functions for linearly traversing through playList
    public SongFormat next()
    {
        //By updating based on specific node indices, instead of based on index of current list, then this function will work regardless
        //if songs is min heap, max heap, or original sequence.
        currentSong = playList[(currentSong.placeInList != playList.Count - 1) ? currentSong.placeInList + 1 : 0];
        return currentSong.song;
    }

    public SongFormat prev()
    {
        currentSong =  playList[currentSong.placeInList - 2];
        //Again stupid, cause doesn't actually reduce lines, got rid of duplicated return current song, but still same lines?
        //So? Fuck it does same thing anyway, later on maybe do more in switching then this will make more sense and be more optimal to avoid duplicating
        //but for now set up to avoid it
        return next();
    }
    public SongNode<SongFormat> current
    {
        get { return currentSong; }
        set { currentSong = playList[value.placeInList]; }
    }

    #endregion
}


