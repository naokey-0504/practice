using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AlterEditor
{
    public class GridBase : MonoBehaviour
    {
        [SerializeField] private Text m_Text;
        [SerializeField] private Button m_Button;

        private Vector2 m_GridPos = Vector2.zero;

        public void SetGridPos(Vector2 pos)
        {
            m_GridPos = pos;
        }

        public void SetText(string str)
        {
            m_Text.text = str;
        }

        public void AddOnClick(UnityAction action)
        {
            if (m_Button != null)
            {
                m_Button.onClick.AddListener(action);
            }
        }
    }
}