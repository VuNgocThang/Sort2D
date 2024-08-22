using UnityEngine.SceneManagement;

// by nt.Dev93
namespace ntDev
{
    public static class ManagerScene
    {
        public const string SceneLoadScene = "SceneLoadScene";
        public const string SceneSplash = "SceneSplash";
        public const string SceneGame = "SceneGame";
        public const string SceneGameTown = "SceneGameTown";
        public const string SceneGameScifiCity = "SceneGameScifiCity";
        public const string SceneGameScifiWorld = "SceneGameScifiWorld";

        public static string nextScene = SceneGame;
        public static void LoadScene(string name)
        {
            nextScene = name;
            ManagerGame.Clear();

            ManagerLoading.ShowLoadScene(() => SceneManager.LoadScene(SceneLoadScene), 1);
        }
    }
}