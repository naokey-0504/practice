using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AlterEditor.SimulationMapEditor
{
    public class SimulationMapEditorManager : SingletonMonoBehaviour<SimulationMapEditorManager>
    {
        private const string kSimulationMapName = "SimulationMap";

        private Transform m_Root;
        private GameObject m_SimulationMap;

        private FiledMapPrefab m_FiledMapPrefabBase;
        private FiledMapPrefab m_FiledMapPrefab;
        
        [SerializeField] private GridBase m_GridBase;
        [SerializeField] private Transform m_GridParent;
        [SerializeField] private GridLayoutGroup m_GridLayoutGroup;
        
        private Sprite m_BgTexture;

        public Sprite bgTexture
        {
            get { return m_BgTexture; }
        }

        public void Init()
        {
            m_Root = transform.Find("Root");
            m_SimulationMap = searchSimulationMap();
            m_GridLayoutGroup.cellSize = m_GridBase.GetComponent<RectTransform>().sizeDelta;
            if (m_FiledMapPrefab == null)
            {
                m_FiledMapPrefab = m_SimulationMap.GetComponent<FiledMapPrefab>();
            }
        }

        public void Update()
        {
        }

        /// <summary>
        /// フィールドのPrefabをロードする
        /// </summary>
        public void LoadFieldPrefab()
        {
            if (m_FiledMapPrefabBase == null)
            {
                m_FiledMapPrefabBase =
                    Resources.Load<FiledMapPrefab>("Prefabs/SimulationMapEditor/FieldBasePrefab");
            }
        }
        
        public void DrawGrid(int col, int row)
        {
            m_GridLayoutGroup.constraintCount = col;
            for (int r = 0; r < row; ++r)
            {
                for (int c = 0; c < col; c++)
                {
                    var grid = GameObject.Instantiate(m_GridBase, m_GridParent).GetComponent<GridBase>();
                    grid.gameObject.SetActive(true);
                    grid.SetText(string.Format("({0}, {1})", c, r));
                    grid.SetGridPos(new Vector2(c, r));
                }
            }
        }

        /// <summary>
        /// バトルフィールドを作成するのに必要なGameObjectを生成する
        /// </summary>
        public void CreateBaseObject()
        {
            m_FiledMapPrefab =
                GameObject.Instantiate<FiledMapPrefab>(m_FiledMapPrefabBase, m_Root);
            m_FiledMapPrefab.Init();
            m_FiledMapPrefab.name = kSimulationMapName;
            m_SimulationMap = searchSimulationMap();
        }

        private GameObject searchSimulationMap()
        {
            var trans = m_Root.Find(kSimulationMapName);
            return trans != null ? trans.gameObject : null;
        }

        /// <summary>
        /// 背景のTextureを設定する
        /// </summary>
        /// <param name="texture">背景</param>
        public void SetBgTexture(Sprite sprite)
        {
            m_BgTexture = sprite;
        }

        /// <summary>
        /// 背景のTextureを反映させる
        /// </summary>
        public void ReflectBgTexture()
        {
            m_FiledMapPrefab.SetBgTexture(m_BgTexture);
        }

        /// <summary>
        /// ステージを出力する
        /// </summary>
        public void OutputStage(string directory, string name)
        {
            if (0 < m_Root.childCount)
            {
                savePrefab(directory, name, m_Root.GetChild(0).gameObject);
            }
        }

        private static void savePrefab(string directory, string name, GameObject gameObj)
        {
            if (!Directory.Exists(directory))
            {
                //prefab保存用のフォルダがなければ作成する
                Directory.CreateDirectory(directory);
            }
            
            //prefabの保存ファイルパス
            string prefabPath = directory + "/" + name + ".prefab";
            if (!File.Exists(prefabPath))
            {
                //prefabファイルがなければ作成する
                File.Create(prefabPath);
            }
            
            //prefabの保存
            UnityEditor.PrefabUtility.CreatePrefab(prefabPath, gameObj);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}