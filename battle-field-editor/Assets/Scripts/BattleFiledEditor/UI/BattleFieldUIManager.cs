using System;
using Scripts.UI.Squre;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class BattleFieldUIManager : MonoBehaviour
    {
        private Canvas m_Canvas;
        private SqureList m_SqureList;

        public void Awake()
        {
            Transform canvasTrans = transform.Find("Canvas");
            m_Canvas = canvasTrans.GetComponent<Canvas>();
            m_SqureList = canvasTrans.Find("SqureList").GetComponent<SqureList>();
        }

        public void Start()
        {
            m_SqureList.Init(GridLayoutGroup.Corner.UpperLeft, new Vector2(80f, 80f), 10, 5);
            m_SqureList.Setup();
        }
    }
}