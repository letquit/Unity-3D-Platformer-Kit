#if UNITY_EDITOR
using UnityEditor;
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
    }
}
#endif
