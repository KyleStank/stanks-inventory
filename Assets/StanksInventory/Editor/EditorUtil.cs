using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KStank.stanks_inventory {
    public static class EditorUtil {
        public static bool ItemPoolAddingItem {
            get { return EditorPrefs.GetBool("StanksInv_AddingItem"); }
            set { EditorPrefs.SetBool("StanksInv_AddingItem", value); }
        }

        //Adds spaces to the window
        public static void AddSpace(int spaces) {
            for(int i = 0; i < spaces; i++)
                EditorGUILayout.Space();
        }

        //Indents to a level
        public static void Indent(int level = 1) {
            EditorGUI.indentLevel = level;
        }
    }
}
