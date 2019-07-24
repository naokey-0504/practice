using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace AlterEditor.SimulationMapEditor
{
    public class SME_Palette : EditorWindow
    {
        private const int kSpace = 20;
        private const int kCowNum = 5;

        private Sprite m_BgSprite;
        private Texture m_Texture;
        private Vector2 scrollPos;

        int m_SelectedToggle;
        private bool m_IsGridMode = false;
        
        private string m_PrefabPath = "Prefabs/Parts/";

        private Dictionary<string, List<GameObject>> m_PartsDic = new Dictionary<string, List<GameObject>>();

        [MenuItem("Window/Editor/SimulationMapEditor/Palette")]
        static void Open()
        {
            EditorWindow.GetWindow<SME_Palette>(SME_Common.kPrefix + "_Palette");
        }

        public void OnEnable()
        {
        }

        void OnGUI()
        {
            int screenW = Screen.width;
            
            GUILayout.Label( "プレハブのパス" );
            m_PrefabPath = GUILayout.TextField(m_PrefabPath);
            
            GUILayout.Label( "グリッドモード" );
            string btnText = "グリッドモードを" + (m_IsGridMode ? "OFF" : "ON") + "にする";
            if (GUILayout.Button(btnText))
            {
                m_IsGridMode = !m_IsGridMode;
                SimulationMapEditorManager.Instance.SetIsGridMode(m_IsGridMode);
            }
            
            GUILayout.Label( "プレハブのインポート" );
            if (GUILayout.Button("Import"))
            {
                destroyPartsDic();
                loadPartsPrefabs();

                m_SelectedToggle = 0;
            }

            
            m_SelectedToggle = GUILayout.Toolbar (m_SelectedToggle, m_PartsDic.Keys.ToArray(), EditorStyles.toolbarButton);

            GUILayout.BeginArea(new Rect(0, 100, screenW, 500));
            GUILayout.BeginVertical();
 
            scrollPos = GUILayout.BeginScrollView(scrollPos,false,true, GUILayout.Width(screenW),GUILayout.MinHeight(200),GUILayout.MaxHeight(1000),GUILayout.ExpandHeight(true));

            if (0 < m_PartsDic.Count)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                var prefabArr = m_PartsDic.Values.ToArray()[m_SelectedToggle];
                for (int i = 0; i < prefabArr.Count; i++)
                {
                    var prefab = prefabArr[i];
                    var options = new[] {GUILayout.Width(64), GUILayout.Height(64)};
                    var toggleImg = false;
                    toggleImg = GUILayout.Toggle(toggleImg, prefab.GetComponent<SpriteRenderer>().sprite.texture, options);

                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView ();
            GUILayout.EndVertical();
            GUILayout.EndArea();            
        }

        private void destroyPartsDic()
        {
            foreach (var pairs in m_PartsDic)
            {
                var list = pairs.Value;
                for (int i = 0; i < list.Count; ++i)
                {
                    GameObject go = list[i];
                    if (go != null)
                    {
                        GameObject.Destroy(go);
                    }
                    list[i] = null;
                }
                list.Clear();
            }
            m_PartsDic.Clear();
        }

        private void loadPartsPrefabs()
        {
            var directoryArr = getPartsDirectoriesName();
            foreach (var directory in directoryArr)
            {
                var list = new List<GameObject>();
                    
                string partsPath = Application.dataPath + "/Resources/" + m_PrefabPath + "/" + directory;
                var fileArr = System.IO.Directory.GetFiles(partsPath, "*.prefab");
                for (int i = 0; i < fileArr.Length; ++i)
                {
                    fileArr[i] = fileArr[i].Split('/').Last().Split('.').First();
                    string prefabPath = m_PrefabPath + directory + "/" + fileArr[i];
                    list.Add(Resources.Load<GameObject>(prefabPath));
                }

                m_PartsDic.Add(directory, list);
            }            
        }
        
        private string[] getPartsDirectoriesName()
        {
            var directories = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/" + m_PrefabPath);
            for(int i = 0; i < directories.Length; ++i)
            {
                directories[i] = directories[i].Split('/').Last();
            }
            return directories;
        }
    }
}