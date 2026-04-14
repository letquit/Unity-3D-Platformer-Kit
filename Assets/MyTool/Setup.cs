#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using static System.IO.Path;
using static System.IO.Directory;
using static UnityEditor.AssetDatabase;

namespace Platformer
{
    public static class Setup
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Folders.CreateDefault("Assets", "Scripts", "Models", "Arts", "Materials", "Animations", "Settings", "ScriptableObjects");
            Refresh();
        }

        [MenuItem("Tools/Setup/Import My Favorite Assets")]
        public static void ImportMyFavoriteAssets()
        {
            Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");
        }
        
        private static class Folders
        {
            public static void CreateDefault(string root, params string[] folders)
            {
                var fullPath = Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Combine(fullPath, folder);
                    if (!Exists(path))
                    {
                        CreateDirectory(path);
                    }
                }
            }
        }

        private static class Assets
        {
            public static void ImportAsset(string asset, string subfolder,
                string folder = "C:/Users/Admin/AppData/Roaming/Unity/Asset Store-5.x")
            {
                AssetDatabase.ImportPackage(Combine(folder, subfolder, asset), false);
            } 
        }
    }
}
#endif
