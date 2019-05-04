using UnityEditor;
using UnityEditor.SceneManagement;

namespace DuckOfDoom.SightReading
{
    public class SceneSelectionMenu
    {
        [MenuItem("Custom/Open Main scene %#&b")]
        private static void OpenStart()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
        }

        [MenuItem("Custom/Open SightReading scene %#&s")]
        private static void OpenMenu()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SightReading.unity", OpenSceneMode.Single);
        }

        [MenuItem("Custom/Open Visualization scene %#&v")]
        private static void OpenSkinShop()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Visualization.unity", OpenSceneMode.Single);
        }
    }
}
