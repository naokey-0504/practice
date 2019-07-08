using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.PartsList
{
    public class ListObject: MonoBehaviour

    {
        private Image m_Image;
        private Button m_Button;
        
        public void Awake()
        {
            m_Button = transform.Find("Button").GetComponent<Button>();
        }
    }
}