using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KStank.stanks_inventory {
    [CustomEditor(typeof(Inventory))]
    public class E_Inventory : Editor {
        Inventory inventory;

        bool[] itemActive = new bool[0];

        public override void OnInspectorGUI() {
            GUI.changed = false;

            Item.LoadItemPool();
            inventory = target as Inventory;

            EditorGUILayout.BeginVertical();

            EditorUtil.AddSpace(2);

            if(GUILayout.Button("Add Item", EditorStyles.toolbarButton))
                AddItemWindow.Init(inventory);

            EditorUtil.AddSpace(1);

            inventory.fileName = EditorGUILayout.TextField(new GUIContent("File Name: "), inventory.fileName);

            //Make sure file extensions is NOT included
            if(inventory.fileName.Contains(".json"))
                inventory.fileName = Util.RemoveAfter(inventory.fileName, ".");

            inventory.MaxItems = EditorGUILayout.IntField(new GUIContent("Max Items in Inventory: "), inventory.MaxItems);

            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Refresh"))
                inventory.Load();

            if(GUILayout.Button("Save"))
                inventory.Save();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Inventory Contents:"), EditorStyles.boldLabel);

            for(int i = 0; i < inventory.TakenSpace; i++) {
                Item item = inventory.InventoryList[i];

                SetActiveItems();

                EditorGUILayout.BeginVertical();
                
                itemActive[i] = EditorGUILayout.Foldout(itemActive[i], new GUIContent(item.Name));

                if(itemActive[i]) {
                    EditorUtil.Indent();

                    EditorGUILayout.LabelField(new GUIContent("ID: " + item.ID));
                    item.Name = EditorGUILayout.TextField(new GUIContent("Name: "), item.Name);
                    item.IconName = EditorGUILayout.TextField(new GUIContent("Icon Name: "), item.IconName);
                    item.StackSize = EditorGUILayout.IntSlider(new GUIContent("Current Stack Size: "), item.StackSize, 0, item.MaxStackSize);
                    item.MaxStackSize = EditorGUILayout.IntField(new GUIContent("Max Stack Size: "), item.MaxStackSize);

                    if(GUILayout.Button("Remove")) {
                        inventory.Remove(item);

                        for(int b = 0; b < itemActive.Length; b++)
                            itemActive[b] = false;
                    }

                    EditorUtil.Indent(0);
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        void SetActiveItems() {
            bool[] oldActive = itemActive;
            itemActive = new bool[inventory.TakenSpace];

            for(int i = 0; i < itemActive.Length; i++)
                if(oldActive.Length > i)
                    itemActive[i] = oldActive[i];
        }
    }

    class AddItemWindow : EditorWindow {
        public static Rect windowRect = new Rect(500.0f, 200.0f, 400.0f, 250.0f);
        public static AddItemWindow window;

        static Inventory inventory = null;
        static Item item = new Item();

        static int itemId = 0;
        static string itemName = "";
        static bool isPreviewingItem = false;

        public static void Init(Inventory inventory) {
            window = GetWindow(typeof(AddItemWindow)) as AddItemWindow;
            window.titleContent.text = "Item Search";
            window.position = windowRect;
            window.Show();

            AddItemWindow.inventory = inventory;

            itemId = 0;
            itemName = "";
            isPreviewingItem = false;
        }

        void OnGUI() {
            GUI.changed = false;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(new GUIContent("Enter the ID or name of the item your looking for."));
            itemId = EditorGUILayout.IntField(new GUIContent("ID: "), itemId);
            itemName = EditorGUILayout.TextField(new GUIContent("Name: "), itemName);

            EditorUtil.AddSpace(1);

            if(GUILayout.Button("Search")) {
                Item searchItem;

                if(itemId > 0)
                    searchItem = Item.LookUpItem(itemId);
                else
                    searchItem = Item.LookUpItem(itemName);

                if(searchItem == null) {
                    Debug.Log("Item not found!");
                    return;
                }

                item = searchItem;

                //Check if item already exists in inventory, so we can stack it, if possible
                Item invItem = inventory.Find(item.ID);

                if(invItem != null)
                    item = invItem;

                isPreviewingItem = true;
            }

            if(isPreviewingItem) {
                EditorUtil.AddSpace(2);

                DisplayResults();

                if(GUILayout.Button(new GUIContent("Add Item"))) {
                    inventory.Add(item);

                    window.Close();
                }
            }

            EditorGUILayout.EndVertical();
        }

        void DisplayResults() {
            EditorGUILayout.LabelField(new GUIContent("ID: " + item.ID));
            EditorGUILayout.LabelField(new GUIContent("Name: " + item.Name));
            EditorGUILayout.LabelField(new GUIContent("Icon Name: " + item.IconName));
            EditorGUILayout.LabelField(new GUIContent("Max Stack Size: " + item.MaxStackSize));
        }
    }
}
