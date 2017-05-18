using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Olympus.Showroom
{
    public class PlayNode<PlayData>
    {
        //The actual audio data
        PlayData data;

        //Keeps track amount of times this song was played, kept in track incase of wanting to have sort mostplayed/least played functionality
        private int timesPlayed;

        public PlayNode(string source,string name, PlayData val)
        {
            this.source = source;
            this.name = name;
            this.data = val;
            timesPlayed = 0;
        }

        public PlayNode(PlayNode<PlayData> rhs)
        {
            source = rhs.source;
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

        public string source
        {
            private set;
            get;
        }

        public PlayData song
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
        public static bool operator >(PlayNode<PlayData> lhs, PlayNode<PlayData> rhs)
        {
            return lhs.timesPlayed > rhs.timesPlayed;
        }

        public static bool operator <(PlayNode<PlayData> lhs, PlayNode<PlayData> rhs)
        {
            return lhs.timesPlayed < rhs.timesPlayed;
        }

    }
}