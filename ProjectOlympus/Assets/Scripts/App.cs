using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olympus.Showroom.Preload
{
    public class App : MonoBehaviour
    {
        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Showroom");
            GameMaster.Instance.gameState = GameMaster.GameState.LevelSelect;
        }
    }
}
