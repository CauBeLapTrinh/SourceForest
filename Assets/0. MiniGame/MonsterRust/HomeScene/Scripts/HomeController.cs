using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minigame.MonsterRush
{
    public class HomeController : MonoBehaviour
    {

        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}
