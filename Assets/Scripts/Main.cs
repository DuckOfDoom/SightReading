using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuckOfDoom.SightReading
{
    public class Main : MonoBehaviour
    {
        public void Start()
        {
            SceneManager.LoadScene(Consts.VISUALIZATION_SCENE_NAME);
        }
    }
}
