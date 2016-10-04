using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KStank.stanks_inventory {
    public class OptionsWindow : EditorWindow {
        public static Rect windowRect = new Rect(500.0f, 200.0f, 300.0f, 300.0f);
        public static OptionsWindow window;

        //Strings
        string createItemBtnMsg = "Create New Item";

        Vector2 scrollPos;
        Item itemToCreate = new Item();

        [MenuItem("Window/Stanks-Inventory")]
        static void Init() {
            window = (OptionsWindow)GetWindow(typeof(OptionsWindow));
            window.titleContent.text = "Inventory Options";
            window.position = windowRect;
            window.Show();
        }

        void OnGUI() {
            GUI.changed = false;

            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField(new GUIContent("Item Pool"), EditorStyles.boldLabel);

            if(GUILayout.Button(new GUIContent(createItemBtnMsg))) {
                EditorUtil.ItemPoolAddingItem = !EditorUtil.ItemPoolAddingItem;

                //Yes, this if statement is called twice. But we want this one here because there is no need
                //to set things equal to the same thing over and over if they only need set one time.
                if(EditorUtil.ItemPoolAddingItem) {
                    itemToCreate.ID = Item.ItemPool.Count + 1;
                    createItemBtnMsg = "Hide Creation Section";
                } else {
                    createItemBtnMsg = "Create New Item";
                }
            }

            if(EditorUtil.ItemPoolAddingItem) {
                EditorUtil.Indent();

                createItemBtnMsg = "Hide Creation Section";

                itemToCreate.ID = EditorGUILayout.IntField(new GUIContent("ID: "), itemToCreate.ID);
                itemToCreate.Name = EditorGUILayout.TextField(new GUIContent("Name: "), itemToCreate.Name);
                itemToCreate.IconName = EditorGUILayout.TextField(new GUIContent("Icon Name: "), itemToCreate.IconName);
                itemToCreate.MaxStackSize = EditorGUILayout.IntField(new GUIContent("Max Stack Size: "), itemToCreate.MaxStackSize);

                if(GUILayout.Button(new GUIContent("Add Item"))) {
                    Item.ItemPool.Add(itemToCreate);
                    Item.SaveItemPool();

                    itemToCreate = new Item();
                }

                EditorUtil.Indent(0);
            }


            EditorGUILayout.LabelField(new GUIContent("Item(s):"));
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            //Load the item pool
            Item.LoadItemPool();
            foreach(Item item in Item.ItemPool) {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent(item.Name));
                if(GUILayout.Button(new GUIContent("View")))
                    ViewItem(item);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        void ViewItem(Item item) {
            ItemWindow.Init(item);
        }
    }

    class ItemWindow : EditorWindow {
        public static Rect windowRect = new Rect(500.0f, 200.0f, 400.0f, 400.0f);
        public static ItemWindow window;

        static Item item = null;
        
        public static void Init(Item item) {
            window = (ItemWindow)GetWindow(typeof(ItemWindow));
            window.titleContent.text = "Item Pool";
            window.position = windowRect;
            window.Show();

            ItemWindow.item = item;
        }

        void OnGUI() {
            GUI.changed = false;

            EditorGUILayout.BeginVertical();

            if(item == null)
                return;

            item.ID = EditorGUILayout.IntField(new GUIContent("ID: "), item.ID);
            item.Name = EditorGUILayout.TextField(new GUIContent("Name: "), item.Name);
            item.IconName = EditorGUILayout.TextField(new GUIContent("Icon Name: "), item.IconName);
            item.MaxStackSize = EditorGUILayout.IntField(new GUIContent("Max Stack Size:"), item.MaxStackSize);

            if(GUILayout.Button(new GUIContent("Save"))) {
                FindItem(item);

                Item.SaveItemPool();
            }

            if(GUILayout.Button(new GUIContent("Remove"))) {
                FindItem(item);

                Item i = Item.LookUpItem(item.ID);

                if(i == null)
                    return;

                Item.RemoveItem(i);
                Item.SaveItemPool();

                window.Close();
            }

            EditorGUILayout.EndVertical();
        }

        void FindItem(Item item) {
            for(int i = 0; i < Item.ItemPool.Count; i++) {
                if(Item.ItemPool[i].ID == item.ID)
                    Item.ItemPool[i] = item;
            }
        }
    }
}
