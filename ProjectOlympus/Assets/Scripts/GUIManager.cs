using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olympus.Showroom.Managers
{
    public class GUIManager : MonoBehaviour {

        #region Variables

        [Header("Panels")]
        [SerializeField]
        private GameObject LevelSelectorPanel;
        [SerializeField]
        private GameObject ArtPanel;
        [SerializeField]
        private GameObject MusicPanel;
        [SerializeField]
        private GameObject AssetsPanel;

        private GameMaster.GameState lastState = 
            GameMaster.GameState.none;

        #endregion

        #region Functions

        #region Unity Functions

        void Start () {
		    
	    }
	
	    void Update () {
		    if (lastState != GameMaster.Instance.gameState)
            {
                lastState = GameMaster.Instance.gameState;
                switch (GameMaster.Instance.gameState)
                {
                    case GameMaster.GameState.none:
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                LevelSelectorPanel,
                                ArtPanel,
                                MusicPanel,
                                AssetsPanel
                            });

                        // >> PUT TRANSITION HERE <<

                        break;
                    case GameMaster.GameState.LevelSelect:
                        LevelSelectorPanel.SetActive(true);
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                ArtPanel,
                                MusicPanel,
                                AssetsPanel
                            });

                        // >> PUT TRANSITION HERE <<

                        break;
                    case GameMaster.GameState.ArtRoom:
                        ArtPanel.SetActive(true);
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                LevelSelectorPanel,
                                MusicPanel,
                                AssetsPanel
                            });

                        // >> PUT TRANSITION HERE <<

                        break;
                    case GameMaster.GameState.MusicRoom:
                        MusicPanel.SetActive(true);
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                LevelSelectorPanel,
                                ArtPanel,
                                AssetsPanel
                            });

                        // >> PUT TRANSITION HERE <<

                        break;
                    case GameMaster.GameState.Armory:
                        AssetsPanel.SetActive(true);
                        SetArrayToActiveState(
                            false,
                            new GameObject[] {
                                LevelSelectorPanel,
                                ArtPanel,
                                MusicPanel
                            });

                        // >> PUT TRANSITION HERE <<

                        break;
                }
            }
	    }

        #endregion

        #region Utility Functions

        public static void SetArrayToActiveState(bool state, GameObject[] objectArr)
        { // Sets a whole game object array to true or false
            for (int i = 0; i < objectArr.Length; i++)
            {
                objectArr[i].SetActive(state);
            }
        }

        #endregion

        #endregion
    }
}