using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Squre
{
    public class SqureList : MonoBehaviour
    {
        private Transform m_SqureList;
        private GameObject m_SqureBase;
        
        private GridLayoutGroup m_GridLayoutGroup;

        private Vector2 m_SqureNum = Vector2.zero;

        public void Awake()
        {
            m_GridLayoutGroup = this.GetComponent<GridLayoutGroup>();

            m_SqureList = this.transform;
            m_SqureBase = m_SqureList.Find("SqureBase").gameObject;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="startCorner">最初のマス目の位置</param>
        /// <param name="squreSize">マス目のサイズ</param>
        /// <param name="colCount">水平方向のマス数</param>
        /// <param name="rowCount">鉛直方向のマス数</param>
        public void Init(GridLayoutGroup.Corner startCorner, Vector2 squreSize, int colCount, int rowCount)
        {
            setStartCorner(startCorner);
            setConstraint(GridLayoutGroup.Constraint.FixedColumnCount, colCount);
            setSqureSize(squreSize);
            setSequreNum(colCount, rowCount);
        }

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            setupSqure();
        }

        private UnityEngine.Events.UnityAction<Vector3> m_OnClick;
        public void SetOnClick(UnityEngine.Events.UnityAction<Vector3> onClick)
        {
            m_OnClick = onClick;
        }
        
        /// <summary>
        /// 最初のマス目をどこにするかを設定する
        /// </summary>
        /// <param name="corner">最初の位置</param>
        private void setStartCorner(GridLayoutGroup.Corner corner)
        {
            m_GridLayoutGroup.startCorner = corner;
        }
        
        /// <summary>
        /// GridLayoutGroupの制約を設定する
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="constraintNum"></param>
        private void setConstraint(GridLayoutGroup.Constraint constraint, int constraintNum)
        {
            m_GridLayoutGroup.constraint = constraint;
            m_GridLayoutGroup.constraintCount = constraintNum;
        }

        /// <summary>
        /// マス目のサイズを設定する
        /// </summary>
        /// <param name="squreSize">マス目のサイズ</param>
        private void setSqureSize(Vector2 squreSize)
        {
            m_GridLayoutGroup.cellSize = squreSize;
        }

        /// <summary>
        /// マス目の数を設定する
        /// </summary>
        /// <param name="colCount">水平方向のマス目</param>
        /// <param name="rowCount">鉛直方向のマス目</param>
        private void setSequreNum(int colCount, int rowCount)
        {
            m_SqureNum = new Vector2(colCount, rowCount);
        }

        /// <summary>
        /// マス目のセットアップ
        /// </summary>
        private void setupSqure()
        {
            lineupMapSqure(m_SqureBase, m_SqureList, (int)m_SqureNum.x, (int)m_SqureNum.y, m_OnClick);
            m_SqureBase.SetActive(false);
        }

        /// <summary>
        /// マス目を配置する
        /// <param name="squreBase">マス目のベースオブジェクト</param>
        /// <param name="squreParent">マス目の親Transform</param>
        /// <param name="colCount">水平方向のマス数</param>
        /// <param name="rowCount">鉛直方向のマス数</param>
        /// </summary>
        private static void lineupMapSqure(GameObject squreBase, Transform squreParent, int colCount, int rowCount, UnityEngine.Events.UnityAction<Vector3> onClick)
        {
            RectTransform rect = squreBase.GetComponent<RectTransform>();
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    GameObject go = GameObject.Instantiate(squreBase, squreParent);
                    int idx = y * colCount + x;
                    go.name = idx.ToString();
                    go.transform.Find("Button/Text").GetComponent<Text>().text = idx.ToString();

                    SqureObject squre = go.GetComponent<SqureObject>();
                    squre.SetButtonEnter(() => { squre.SetButtonColor(new Color(1f, 0f, 0f, 0.5f)); });
                    squre.SetButtonExit(() => { squre.SetButtonColor(new Color(1f, 1f, 1f, 0.5f)); });
                    
                    squre.SetButtonClick(() => {
                        if (onClick != null)
                        {
                            onClick.Invoke(squre.transform.position);
                        }
                    });
                }
            }
        }
    }
}