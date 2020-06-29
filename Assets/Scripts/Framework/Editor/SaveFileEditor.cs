using Enso;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class SaveFileEditor : MonoBehaviour
    {
        [MenuItem("Enso/Delete Save")]
        public static void DeleteSave()
        {
            SaveSystem.DeleteSave();
            
            print("Delete Save");
        }
    }
}
