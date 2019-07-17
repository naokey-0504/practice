using UnityEngine;
using UnityEngine.UI;

namespace AlterEditor.QuestEditor
{
    public class GridBase : MonoBehaviour
    {
        [SerializeField] private Text m_Text;

        public void SetText(string str)
        {
            m_Text.text = str;
        }
    }
}