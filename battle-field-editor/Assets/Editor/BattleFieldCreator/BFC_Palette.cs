using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using BattleFiledCreator;
using UnityEngine.UI;

namespace AlterEditor.BattleFieldCreator
{
    public class BFC_Palette : EditorWindow
    {
        private const int kSpace = 20;
        private const int kCowNum = 5;

        private Sprite m_BgSprite;
        private Texture m_Texture;
        private Vector2 scrollPos;

        int selected;
        
        private string m_PrefabPath = "Prefabs/Parts/Tree";
	    
        [MenuItem("Window/Editor/BattleFieldCreator/Palette")]
        static void Open()
        {
            EditorWindow.GetWindow<BFC_Palette>(BFC_Common.kPrefix + "_Palette");
        }

        public void OnEnable()
        {
        }

        void OnGUI()
        {
            int screenW = Screen.width;

            var prefabArr = new GameObject[2];
            
            GUILayout.Label( "プレハブのパス" );
            m_PrefabPath = GUILayout.TextField(m_PrefabPath);
            
            GUILayout.Label( "プレハブのインポート" );
            if (GUILayout.Button("Import"))
            {
                GameObject tree = Resources.Load<GameObject>(m_PrefabPath);
                m_Texture = tree.GetComponent<SpriteRenderer>().sprite.texture;
            }

            
            selected = GUILayout.Toolbar (selected,
                new string[]{ "aaa", "bbb", "ccc", "ddd", "eee", "fff" }, EditorStyles.toolbarButton);

            
            


            GUILayout.BeginArea(new Rect(0, 100, screenW, 500));
            GUILayout.BeginVertical();
 
            scrollPos = GUILayout.BeginScrollView(scrollPos,false,true, GUILayout.Width(screenW),GUILayout.MinHeight(200),GUILayout.MaxHeight(1000),GUILayout.ExpandHeight(true));
            
            EditorGUILayout.BeginHorizontal( GUI.skin.box );
            for (int i = 0; i < kCowNum; i++)
            {
                var options = new []{GUILayout.Width (64), GUILayout.Height (64)};
                var toggleImg = false;
                toggleImg = GUILayout.Toggle(toggleImg, m_Texture, options);
                
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.EndScrollView ();
            GUILayout.EndVertical();
            GUILayout.EndArea();            
        }
    }
}