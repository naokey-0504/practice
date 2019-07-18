using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace AlterEditor.QuestEditor
{
    public class MetaWindow : MonoBehaviour
    {
        public static readonly string[] kDropDownList = new string[]
        {
            "なし",
            
            "森",
            "草原",
            "川",
            "橋",
        };
        
        [SerializeField] private Button m_CloseBtn;
        [SerializeField] private Dropdown m_Dropdown;
        [SerializeField] private Transform m_EventListContens;
        [SerializeField] private EventBase m_EventBase;

        public void Awake()
        {
            this.gameObject.SetActive(false);
            
            m_CloseBtn.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

            for (int i = 0; i < 3; ++i)
            {
                GameObject.Instantiate(m_EventBase, m_EventListContens);
            }
        }

        public void Init()
        {
            m_Dropdown.ClearOptions();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void SetDropDownOpitons(string[] strArr)
        {
            m_Dropdown.AddOptions(strArr.ToList());
        }
    }
}