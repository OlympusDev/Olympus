using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Olympus.Showroom
{
    /// <summary>
    /// Data Structure for holding songs and going from song to song, this does not handle playing the songs
    /// </summary>
    /// <
    /// typeparam name="PlayData"> This parameter so works for Unity's audioClips, or anyother sound handling Object other libraries may have.</typeparam>
    public class PlayList<PlayData>
    {
        //Note to self change all these song/songformat names to something more general.
        List<PlayNode<PlayData>> playList;
       // Dictionary<PlayNode<PlayData>> allSongs;
        //For going back to original sequence if decide not to have sorted anymore.
        List<PlayNode<PlayData>> originalSequence;
        int currentIndex;

        #region Constructors and Destructor
        public PlayList()
        {
            playList = new List<PlayNode<PlayData>>();
            currentIndex = 0;
        }


        public PlayList(PlayNode<PlayData>[] listOfSongs)
        {
            playList = new List<PlayNode<PlayData>>();
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
                PlayNode<PlayData> songRemoving = playList[i];

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


        public void add(PlayNode<PlayData> song)
        {
            if (originalSequence != null)
                originalSequence.Add(song);

            playList.Add(song);
        }

        public void remove(PlayNode<PlayData> song)
        {
            if (originalSequence != null)
                originalSequence.Remove(song);

            playList.Remove(song);

            //Sets current index to be within new size of list, just incase removed what was currentlyplaying was at end of list, then next will be first in playlist
            //otherwise will stay at same index.
            currentIndex %= playList.Count;

        }

        /// <summary>
        /// Removes whatever song is in that place in the list
        /// </summary>
        /// <param name="placeInList"></param>
        public void remove(int placeInList)
        {
            if (placeInList < 0 || placeInList >= playList.Count)
                throw new System.IndexOutOfRangeException("Index out of range");

            playList.RemoveAt(placeInList);
        }

        #endregion

        #region Methods for random access of items in playList



        /*No longer needed because random access via song is now done via the dictionary of all songs. And PlayList isn't meant to be random accessed like this
         * the other indexer via integer acts as skipper
        public PlayNode<PlayData> this[PlayNode<PlayData> i]
        {
            get
            {
                return playList.Find((PlayNode<PlayData> comparing) => { return i.GetHashCode() == comparing.GetHashCode(); });
            }
        }*/

        public PlayNode<PlayData> first()
        {
            return this[0];
        }


        public PlayNode<PlayData> last()
        {
            return this[playList.Count - 1];
        }

        //For if want to skip n songs forward 
        public PlayNode<PlayData> this[int i]
        {
            get
            {
                if (i < 0 || i >= playList.Count)
                    throw new System.IndexOutOfRangeException("Index out of range");

                return playList[i];
            }
        }
        
       

        public PlayNode<PlayData> find(PlayNode<PlayData> toFind)
        {
            //Calls other find method
            return find(toFind.name, toFind.source);
        }

        public bool contains(PlayNode<PlayData> checkingFor)
        {
            return playList.Contains(checkingFor);
        }

        public PlayNode<PlayData> find(string songName,string artist)
        {
            return playList.Find((PlayNode<PlayData> comparing) => { return songName == comparing.name && artist == comparing.source; });
        }
        #endregion

        #region Changing sequence of play list
        public void sortMostPlayed()
        {
            //Only need to allocate memroy for originalSequeunce and copy contents if switching sequence
            if (originalSequence == null) originalSequence = new List<PlayNode<PlayData>>(playList);

            ///Still need to implement
        }

        public void sortLeastPlayed()
        {
            if (originalSequence == null) originalSequence = new List<PlayNode<PlayData>>(playList);
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
        public PlayNode<PlayData> next()
        {

            currentIndex = (currentIndex == size - 1) ? 0 : currentIndex + 1;
            PlayNode<PlayData> currentSong = playList[currentIndex];
            currentSong.playCounter++;
            return currentSong;
        }

        public PlayNode<PlayData> prev()
        {
            //And decrease by 2 because going into next to it will end up in current - 1
            currentIndex = (currentIndex == 0) ? size - 2 : currentIndex - 2;
            return this.next();
        }


        /// <summary>
        /// Get: Returns the current node to be played.
        /// Set: Takes new node to play and assigns to be new current.
        /// </summary>
        public PlayNode<PlayData> current
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
                for (; playList[newIndex].GetHashCode() != value.GetHashCode(); newIndex = (newIndex != size - 1) ? newIndex + 1 : 0)
                {
                    //Actually instead of throwing, maybe just maintain current state?
                    if (newIndex == currentIndex) return;
                }

                //Current index is chosen song wanted to play.
                currentIndex = newIndex;

            }
        }
        #endregion
    }


}