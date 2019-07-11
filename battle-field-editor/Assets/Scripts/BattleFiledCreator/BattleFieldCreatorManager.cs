using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BattleFiledCreator
{
    public class BattleFieldCreatorManager : SingletonMonoBehaviour<BattleFieldCreatorManager>
    {
        private Transform m_Root;

        private FieldController m_FieldControllerBase;
        private FieldController m_FieldController;

        private Sprite m_BgTexture;

        public Sprite bgTexture
        {
            get { return m_BgTexture; }
        }

        public void Init()
        {
            m_Root = transform.Find("Root");

            if (m_FieldControllerBase == null)
            {
                loadFieldPrefab();
            }
        }

        /// <summary>
        /// フィールドのPrefabをロードする
        /// </summary>
        private void loadFieldPrefab()
        {
            m_FieldControllerBase = Resources.Load<FieldController>("Prefabs/BattleFieldCreator/FieldBasePrefab");
        }

        /// <summary>
        /// バトルフィールドを作成するのに必要なGameObjectを生成する
        /// </summary>
        public void CreateBaseObject()
        {
            m_FieldController = GameObject.Instantiate<FieldController>(m_FieldControllerBase, m_Root);
            m_FieldController.Init();
            m_FieldController.name = "FieldController";
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
            m_FieldController.SetBgTexture(m_BgTexture);
        }

        /// <summary>
        /// ステージを出力する
        /// </summary>
        public void OutputStage(string directory, string name)
        {
            savePrefab(directory, name, m_FieldController.gameObject);
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