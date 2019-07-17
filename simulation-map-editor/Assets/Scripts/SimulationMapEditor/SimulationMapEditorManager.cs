using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SimulationMapEditor
{
    public class SimulationMapEditorManager : SingletonMonoBehaviour<SimulationMapEditorManager>
    {
        private const string kSimulationMapName = "SimulationMap";

        private Transform m_Root;
        private GameObject m_SimulationMap;

        private SimulationMapController m_SimulationMapControllerBase;
        private SimulationMapController m_SimulationMapController;

        private Sprite m_BgTexture;

        public Sprite bgTexture
        {
            get { return m_BgTexture; }
        }

        public void Init()
        {
            m_Root = transform.Find("Root");
            m_SimulationMap = searchSimulationMap();
            if (m_SimulationMapController == null)
            {
                m_SimulationMapController = m_SimulationMap.GetComponent<SimulationMapController>();
            }
        }

        /// <summary>
        /// フィールドのPrefabをロードする
        /// </summary>
        public void LoadFieldPrefab()
        {
            if (m_SimulationMapControllerBase == null)
            {
                m_SimulationMapControllerBase = Resources.Load<SimulationMapController>("Prefabs/SimulationMapEditor/FieldBasePrefab");
            }
        }

        /// <summary>
        /// バトルフィールドを作成するのに必要なGameObjectを生成する
        /// </summary>
        public void CreateBaseObject()
        {
            m_SimulationMapController = GameObject.Instantiate<SimulationMapController>(m_SimulationMapControllerBase, m_Root);
            m_SimulationMapController.Init();
            m_SimulationMapController.name = kSimulationMapName;
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
            m_SimulationMapController.SetBgTexture(m_BgTexture);
        }

        /// <summary>
        /// ステージを出力する
        /// </summary>
        public void OutputStage(string directory, string name)
        {
            savePrefab(directory, name, m_SimulationMapController.gameObject);
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