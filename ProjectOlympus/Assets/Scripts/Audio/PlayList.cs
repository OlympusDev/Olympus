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
    //Note to self change all these song/songformat names to something more general.
    List<SongInfo<SongFormat>> playList;

    //For going back to original sequence if decide not to have sorted anymore.
    List<SongInfo<SongFormat>> originalSequence;
    int currentIndex;

    #region Constructors and Destructor
    public PlayList()
    {
        playList = new List<SongInfo<SongFormat>>();
        currentIndex = 0;
    }


    public PlayList(SongInfo<SongFormat>[] listOfSongs)
    {
        playList = new List<SongInfo<SongFormat>>();
        for (int i = 0; i < listOfSongs.Length; i++)
        {
            playList.Add(listOfSongs[i]);
        }

        //Current because when not accessing any random song, it acts like queue
        currentIndex = 0;

    }

     ~PlayList()
    {
        for (int i = 0; i < size; i++)
        {
            SongInfo<SongFormat> songRemoving = playList[i];

            //Restarting playCOunter because it's amount of times played on this playList, not total forever, wait but copying SongInfos also copies playCounter
            //Meaning is it's absolute playCounter not relative, up to design of playList/ use of it cause if played alot, then maybe people think to add to their own
            //PlayList? For now it's relative to each one though, since not saving anything anyway it won't really be true amount of times played
            songRemoving.playCounter = 0;
        }

    }

    #endregion
    public int size
    {
        get { return playList.Count; }
    }

    #region Editing Contents of PlayList

  
    public void add(SongInfo<SongFormat> song)
    {
        if (originalSequence != null)
            originalSequence.Add(song);

        playList.Add(song);
    }

    public void remove(SongInfo<SongFormat> song)
    {
        if (originalSequence != null)
            originalSequence.Remove(song);

        playList.Remove(song);

        //Sets current index to be within new size of list, just incase removed what was currentlyplaying and was at end of list
        currentIndex %= size;

    }

    /// <summary>
    /// Removes whatever song is in that place in the list
    /// </summary>
    /// <param name="placeInList"></param>
    public void remove(int placeInList)
    {
        playList.RemoveAt(placeInList);
    }

    #endregion 

    #region Methods for random access of items in playList
    public SongInfo<SongFormat> this[SongInfo<SongFormat> i]
    {
        get
        {
            return playList.Find((SongInfo<SongFormat> comparing) => { return i.GetHashCode() == comparing.GetHashCode(); });       
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
        return playList.Find((SongInfo<SongFormat> comparing) => { return toFind.song.GetHashCode() == comparing.song.GetHashCode(); });
    }

    #endregion

    #region Changing sequence of play list
    public void sortMostPlayed()
    {
        //Only need to allocate memroy for originalSequeunce and copy contents if switching sequence
        if (originalSequence == null) originalSequence = new List<SongInfo<SongFormat>>(playList);

        ///Still need to implement
    }

    public void sortLeastPlayed()
    {
        if (originalSequence == null) originalSequence = new List<SongInfo<SongFormat>>(playList);
        ///Still need to implement
    }

    public void shuffle()
    {
        //Still need to implement

    }

    #endregion

    #region Functions for linearly traversing through playList
    /// <summary>
    /// This updates current song to next song in playlist.
    /// prev() calls this method to avoid duplicating code would need to repeat for both
    /// </summary>
    /// <returns> The song next to past current, which is new current. </returns>
    public SongInfo<SongFormat> next()
    {
        
        currentIndex = (currentIndex == size - 1) ? 0 : currentIndex + 1;
        SongInfo<SongFormat> currentSong = playList[currentIndex];
        currentSong.playCounter++;
        return currentSong;
    }

    public SongInfo<SongFormat> prev()
    {
            //And decrease by 2 because going into next to it will end up in current - 1
            currentIndex = (currentIndex == 0) ? size - 2 : currentIndex - 2;
            return this.next();
    }


    /// <summary>
    /// Get: Returns the current song to be played.
    /// Set: Takes new song to play and assigns to be new current.
    /// </summary>
    public SongInfo<SongFormat> current
    {
        get
        {
            return playList[currentIndex];
        }
        set
        {
            int newIndex = (currentIndex + 1 == playList.Count) ? 0 : currentIndex + 1;
            
            //Traverses list to find matching one and assigns current index to index of song being requested to play, this goes circularly through playlist
            //so has chance to be infinite, need to keep track of traversals or if back to currentIndex, then that means not found
            for (; playList[newIndex].GetHashCode() != value.GetHashCode(); newIndex = (newIndex != size - 1)? newIndex + 1: 0)
            {
                //Actually instead of throwing, maybe just maintain current state?
                if (newIndex == currentIndex) throw (new System.Exception("That song does not exist in the playlist."));
            }
            
            //Current index is chosen song wanted to play.
            currentIndex = newIndex;

        }
    }
    #endregion
}


