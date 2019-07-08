using System;
using Scripts.UI.Squre;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class BattleFieldUIManager : MonoBehaviour
    {
        private static readonly Vector2 kSqureSize = new Vector2(80f, 80f);
        
        private Canvas m_Canvas;
        private SqureList m_SqureList;
        private Transform m_PartsTop;

        public void Awake()
        {
            Transform canvasTrans = transform.Find("Canvas");
            m_Canvas = canvasTrans.GetComponent<Canvas>();
            m_SqureList = canvasTrans.Find("SqureList").GetComponent<SqureList>();
            m_PartsTop = canvasTrans.Find("PartsTop");
        }

        public void Start()
        {
            m_SqureList.Init(GridLayoutGroup.Corner.UpperLeft, kSqureSize, 10, 5);
            m_SqureList.SetOnClick((position) =>
            {
                GameObject go = new GameObject("parts", typeof(Image));
                go.transform.SetParent(m_PartsTop);
                go.transform.position = position;
                go.GetComponent<RectTransform>().sizeDelta = kSqureSize;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                go.GetComponent<Image>().color = Color.green;
            });
            m_SqureList.Setup();
        }
    }
}