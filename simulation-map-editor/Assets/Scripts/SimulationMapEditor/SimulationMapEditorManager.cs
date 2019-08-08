using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
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

        [SerializeField] private Camera m_Camera;
        private float m_CameraDistance = 0f;
        
        [SerializeField] private GridBase m_GridBase;
        private Vector3 m_GridSpace;
        private List<GridBase> m_GridList = new List<GridBase>();
        [SerializeField] private Transform m_GridParent;
        [SerializeField] private GridLayoutGroup m_GridLayoutGroup;
        
        private Sprite m_BgTexture;
        
        private bool m_IsGridMode = false;
        public bool isGridMode
        {
            get { return m_IsGridMode; }
        }
        
        public Sprite bgTexture
        {
            get { return m_BgTexture; }
        }

        public void Init()
        {
            m_Root = transform.Find("Root");
            m_SimulationMap = searchSimulationMap();
            m_GridList.Clear();
            m_GridLayoutGroup.cellSize = m_GridBase.GetComponent<RectTransform>().sizeDelta;
            m_CameraDistance = m_Camera.WorldToScreenPoint(GameObject.Find("Object").transform.position).z;
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
                    grid.AddOnClick(() => { onClickGrid(grid); });
                    m_GridList.Add(grid);
                }
            }
        }

        public void CalcGridSpace(int col)
        {
            if (0 < m_GridList.Count)
            {
                m_GridSpace.x = m_GridList[1].transform.position.x - m_GridList[0].transform.position.x;
                m_GridSpace.y = 0f;
                m_GridSpace.z = m_GridList[col].transform.position.z - m_GridList[0].transform.position.z;
            }
            else
            {
                m_GridSpace = Vector3.zero;
            }
        }

        private void onClickGrid(GridBase grid)
        {
            var activeGameObject = Selection.activeGameObject != null
                ? Selection.activeGameObject.GetComponent<MapObject>()
                : null;
            if (activeGameObject != null)
            {
                if (SimulationMapEditorManager.Instance.isGridMode)
                {
                    //マス目に合わせて吸着するように
                    // ただし、偶数サイズの場合、マス目に合うようにオフセットする
                    var position = grid.transform.position;
                    position.x += (int) activeGameObject.gridSize.x % 2 == 0 ? m_GridSpace.x / 2f : 0f;
                    position.z += (int) activeGameObject.gridSize.y % 2 == 0 ? m_GridSpace.z / 2f : 0f;
                    activeGameObject.transform.position = position;
                }
            }
        }

        public Vector2 GetGridSize()
        {
            return m_GridBase.GetComponent<RectTransform>().sizeDelta;
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

        public void SetIsGridMode(bool flg)
        {
            m_IsGridMode = flg;
        }
    }
}