using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olympus.Showroom
{
    public class GameMaster : MonoBehaviour
    {
        static public GameMaster Instance { get { return _instance; } }
        static private GameMaster _instance;

        public enum GameState
        {
            none,
            LevelSelect,
            ArtRoom,
            MusicRoom,
            Armory
        }

        private GameState currentState;
        private GameState lastState;
        public GameState savedState = GameState.none; // deprecated
        public GameState gameState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning("Game Master is already in play, deleting new.");
                Destroy(this.gameObject);
            }
            else
            { _instance = this; }
        }

        // Update is called once per frame
        void Update()
        {
            if (lastState != currentState)
            {
                lastState = currentState;
            }
        }
    }
}
