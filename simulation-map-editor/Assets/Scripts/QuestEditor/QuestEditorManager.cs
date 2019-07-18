using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AlterEditor.QuestEditor
{
    public class QuestEditorManager : SingletonMonoBehaviour<QuestEditorManager>
    {
        private Transform m_Root;
        private GameObject m_StageObj;

        [SerializeField] private GridBase m_GridBase;
        [SerializeField] private Transform m_GridParent;
        [SerializeField] private GridLayoutGroup m_GridLayoutGroup;
        [SerializeField] private MetaWindow m_MetaWindow;
        
        public void Init()
        {
            m_Root = transform.Find("Root");
            m_GridLayoutGroup.cellSize = m_GridBase.GetComponent<RectTransform>().sizeDelta;
            
            m_MetaWindow.Init();
            
            //TODO:最終的には、どこかに定義されたマップの種類を取得するようにする
            m_MetaWindow.SetDropDownOpitons(MetaWindow.kDropDownList);
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
                    grid.SetGridPos(new Vector2(c, r));
                }
            }
        }

        public void ShowGridWindow()
        {
            m_MetaWindow.Show();
        }
    }
}