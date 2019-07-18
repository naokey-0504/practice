using System;
using UnityEngine;
using UnityEngine.UI;

namespace AlterEditor.QuestEditor
{
    public class GridBase : MonoBehaviour
    {
        [SerializeField] private Text m_Text;

        private Button m_Button;

        private Vector2 m_GridPos = Vector2.zero;
        
        public void Awake()
        {
            m_Button = transform.GetComponent<Button>();
            m_Button.onClick.AddListener(() =>
            {
                QuestEditorManager.Instance.ShowGridWindow();
            });
        }

        public void SetGridPos(Vector2 pos)
        {
            m_GridPos = pos;
        }

        public void SetText(string str)
        {
            m_Text.text = str;
        }
    }
}