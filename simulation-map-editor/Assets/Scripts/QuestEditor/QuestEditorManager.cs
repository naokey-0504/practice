using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AlterEditor.QuestEditor
{
    public class QuestEditorManager : SingletonMonoBehaviour<QuestEditorManager>
    {
        private Transform m_Root;
        private GameObject m_StageObj;

        [SerializeField] private GameObject m_GridBase;
        [SerializeField] private Transform m_GridParent;
        [SerializeField] private GridLayoutGroup m_GridLayoutGroup;
        
        public void Init()
        {
            m_Root = transform.Find("Root");
            m_GridLayoutGroup.cellSize = m_GridBase.GetComponent<RectTransform>().sizeDelta;
        }

        public void LoadSimurationStage(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            m_StageObj = GameObject.Instantiate(prefab, m_Root);
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
                }
            }
        }
    }
}