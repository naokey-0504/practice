using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using BattleFiledCreator;
using UnityEngine.UI;

namespace AlterEditor.BattleFieldCreator
{
    public class BFC_Main : EditorWindow
    {
        private const int kSpace = 20;

        private Sprite m_BgSprite;
        private string m_PrefabDirectory = "";
        private string m_PrefabName = "";
	    
        [MenuItem("Window/Editor/BattleFieldCreator/Main")]
        static void Open()
        {
            EditorWindow.GetWindow<BFC_Main>( BFC_Common.kPrefix + "_Main" );
        }

        public void OnEnable()
        {
            m_PrefabDirectory = "/Resources/Prefabs";
        }

        void OnGUI()
        {
            GUILayout.Label( "0. 初期化する" );
            if (GUILayout.Button("Init"))
            {
                BattleFieldCreatorManager.Instance.Init();
            }
            
            GUILayout.Space(kSpace);
            GUILayout.Label( "1. 作成に必要なGameObjectを配置する" );
            if (GUILayout.Button("Create Base"))
            {
                BattleFieldCreatorManager.Instance.CreateBaseObject();
            }
		    
            GUILayout.Space(kSpace);
            GUILayout.Label( "2. 背景を設定する" );
            var options = new []{GUILayout.Width (64), GUILayout.Height (64)};
            var sprite = EditorGUILayout.ObjectField (m_BgSprite, typeof(Sprite), false, options) as Sprite;
            if (m_BgSprite != sprite)
            {
                m_BgSprite = sprite;
                if (sprite != null)
                {
                    BattleFieldCreatorManager.Instance.SetBgTexture(sprite);
                    BattleFieldCreatorManager.Instance.ReflectBgTexture();
                }
            }
		    
            GUILayout.Space(kSpace);
            GUILayout.Label( "3. オブジェクトを配置する" );
		    
            GUILayout.Space(kSpace);
            GUILayout.Label( "4. ステージを出力する" );
            GUILayout.Label("Application.dataPath");
            GUILayout.TextField( Application.dataPath + "/" );
            GUILayout.Label( "ディレクトリ" );
            string dirPath = GUILayout.TextField(m_PrefabDirectory);
            if (dirPath != m_PrefabDirectory)
            {
                m_PrefabDirectory = dirPath;
            }
            GUILayout.Label( "プレハブ名" );
            m_PrefabName = GUILayout.TextField(m_PrefabName);
            if (GUILayout.Button("Output Stage"))
            {
                BattleFieldCreatorManager.Instance.OutputStage(Application.dataPath + "/" + m_PrefabDirectory, m_PrefabName);
            }
        }
    }
}