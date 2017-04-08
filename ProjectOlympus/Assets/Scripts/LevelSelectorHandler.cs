using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Olympus.Showroom.UI
{
    public class LevelSelectorHandler : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private Button artButton;
        [SerializeField]
        private Button musicButton;
        [SerializeField]
        private Button assetsButton;


        private void Awake()
        {
            InitializeListeners();
        }

        private void OnEnable()
        {
            InitializeListeners();
        }

        void Update()
        {

        }

        private void ArtButton()
        {
            GameMaster.Instance.gameState = GameMaster.GameState.ArtRoom;
        }

        private void MusicButton()
        {
            GameMaster.Instance.gameState = GameMaster.GameState.MusicRoom;
        }

        private void AssetsButton()
        {
            GameMaster.Instance.gameState = GameMaster.GameState.Armory;
        }

        private void InitializeListeners()
        {
            artButton.onClick.AddListener(() => ArtButton());
            musicButton.onClick.AddListener(() => MusicButton());
            assetsButton.onClick.AddListener(() => AssetsButton());
        }
    }
}
