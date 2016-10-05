using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KStank.stanks_inventory {
    [CustomEditor(typeof(Inventory))]
    public class E_Inventory : Editor {
        Inventory inventory;

        bool[] itemActive = new bool[0];

        Item itemToAdd;
        bool addingItem = false;
        int itemToAddId = 1;

        public override void OnInspectorGUI() {
            GUI.changed = false;

            Item.LoadItemPool();
            inventory = target as Inventory;

            EditorGUILayout.BeginVertical();

            EditorUtil.AddSpace(2);

            if(GUILayout.Button("Add Item", EditorStyles.toolbarButton))
                addingItem = !addingItem;

            if(addingItem) {
                EditorGUILayout.BeginVertical();
                
                itemToAddId = EditorGUILayout.IntField(new GUIContent("ID: "), itemToAddId);

                if(GUILayout.Button(new GUIContent("Look Up Item"))) {
                    itemToAdd = Item.LookUpItem(itemToAddId);

                    if(itemToAdd == null) {
                        Debug.Log("Item not found!");
                    } else {
                        //Yes, this is redundent. But it prevents a bug, so its needed
                        Item invItem = inventory.Find(itemToAdd.ID);

                        if(invItem != null)
                            itemToAdd = invItem;
                        
                        AddItemWindow.Init(inventory, itemToAdd);
                    }
                }

                EditorGUILayout.EndVertical();
            }

            EditorUtil.AddSpace(2);

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
        static Item item = null;

        public static void Init(Inventory inventory, Item item) {
            window = GetWindow(typeof(AddItemWindow)) as AddItemWindow;
            window.titleContent.text = "Add this item?";
            window.position = windowRect;
            window.Show();

            AddItemWindow.inventory = inventory;
            AddItemWindow.item = item;
        }

        void OnGUI() {
            GUI.changed = false;

            if(item == null)
                return;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(new GUIContent("ID: " + item.ID));
            EditorGUILayout.LabelField(new GUIContent("Name: " + item.Name));
            EditorGUILayout.LabelField(new GUIContent("Icon Name: " + item.IconName));
            EditorGUILayout.LabelField(new GUIContent("Max Stack Size: " + item.MaxStackSize));

            EditorUtil.AddSpace(1);

            EditorGUILayout.LabelField(new GUIContent("Are you sure you want to add this item?"));
            EditorGUILayout.LabelField(new GUIContent("If you already have it, it won't be added!"));

            EditorUtil.AddSpace(1);

            if(GUILayout.Button("Add")) {
                inventory.Add(item);

                window.Close();
            }

            EditorGUILayout.EndVertical();
        }
    }
}
