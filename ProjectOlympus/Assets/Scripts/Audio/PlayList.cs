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
    List<SongInfo<SongFormat>> playList;
   
    //For going back to original sequence if decide not to have sorted anymore.
    List<SongInfo<SongFormat>> originalSequence;
    SongInfo<SongFormat> currentSong;


    public PlayList()
    {
        playList = new List<SongInfo<SongFormat>>();
       // originalSequence = playList;
       // currentSong = songs[0]; 
    }


    public PlayList(string[] names, SongFormat[] listOfSongs)
    {
        playList = new List<SongInfo<SongFormat>>();
        for (int i = 0; i < listOfSongs.Length; i++)
        {
           playList.Add(new SongInfo<SongFormat>(names[i],listOfSongs[i],i));
            //Diff copies not referencing same node, because index of songs can change if sorted, where as elements in original sequence are forever same indicies.
           // originalSequence.Add(new SongNode<SongFormat>(listOfSongs[i],i));*/
        }
        current = playList[0];
        
    } 
    public int size
    {
        get { return playList.Count; }
    }

    #region Editing Contents of play list, so far none of these are being used because getting all sounds at start.

    public void add(string songName,SongFormat song)
    {
        //Appending to last so 
        SongInfo<SongFormat> node = new SongInfo<SongFormat>(songName, song, playList.Count);

        if (originalSequence != null)
            originalSequence.Add(node);

        playList.Add(node);

    }

    public void remove(string songName)
    {
        //Basically makes it so no two songs in list can have same song name, rather this'll break if they do. I haven't disallowed yet
        playList.RemoveAt(playList.FindIndex((SongInfo<SongFormat> comparing) => { return comparing.name == songName; }));
    }

    public void remove(SongInfo<SongFormat> song)
    {
        playList.RemoveAt(song.placeInList);
    }
    #endregion //only add is implemented and not currently used/tested because getting all songs upon constructing.

    #region Methods for random access of items in playList
    //Returns node, because will use this to also update the names, if for whatever reason keeps SongNode that inserted here kept 
    public SongInfo<SongFormat> this[SongInfo<SongFormat> i]
    {
        get
        {
            //currentSong = songs[i.index];
            return playList[i.placeInList];
        }
    }

    public SongInfo<SongFormat> this[int i]
    {
        get
        {
            return playList[i];
        }
    }

    public SongInfo<SongFormat> find(SongInfo<SongFormat> toFind)
    {
        return playList.Find((SongInfo<SongFormat> comparing) => { return toFind.name == comparing.name; });
    }

    #endregion

    #region Changing sequence of play list
    public void sortMostPlayed()//Constructs max heap based on timesplayed of each songNode and assigns it to songs
    {
        //Only need to allocate memroy for originalSequeunce and copy contents if switching sequence
        if (originalSequence == null) originalSequence = new List<SongInfo<SongFormat>>(playList);

        ///Still need to implement
    }

    public void sortLeastPlayed()//Constructs min heap based on timesPlayed of each songNode and assigns it to songs
    {
        if (originalSequence == null) originalSequence = new List<SongInfo<SongFormat>>(playList);
        ///Still need to implement
    }

    public void shuffle()
    {
        //Still need to implement, this just testing if saving placeInList method works
        int cap = Random.Range(playList.Count, playList.Count * Random.Range(1, 5));
        for (int i = 0; i < playList.Count; i++)
        {
            SongInfo<SongFormat> holder = playList[i];
            //Gets a random number between 0 and some multiple of current size moded by current size
            int newIndex = Random.Range(i, playList.Count * Random.Range(2, 100)) % playList.Count + i;
            playList[i] = playList[newIndex];
            playList[newIndex] = holder;
        }
    }

    #endregion

    #region Functions for linearly traversing through playList
    public SongFormat next()
    {
        //By updating based on specific node indices, instead of based on index of current list, then this function will work regardless
        //if songs is min heap, max heap, or original sequence.
        currentSong = playList[(currentSong.placeInList + 1 != playList.Count) ? currentSong.placeInList + 1 : 0];

        ++currentSong.playCounter;
        return currentSong.song;
    }

    public SongFormat prev()
    {

        //Okay it works, BUT BREAKS if press too fast.    
        //currentSong = playList[(currentSong.placeInList == 0) ? playList.Count - 2 : currentSong.placeInList - 2];
        //return next();
        //Old method DOES not break if press too fast, fuck it I'll look into later, and just diplicate the playCounter, IT WORKS I'M DONE... for now.

        currentSong = playList[(currentSong.placeInList == 0) ? playList.Count - 1 : currentSong.placeInList - 1];
        ++currentSong.playCounter;
        return currentSong.song;
    }
    public SongInfo<SongFormat> current
    {
        get { return currentSong; }
        set { currentSong = playList[value.placeInList]; }
    }

    #endregion
}


