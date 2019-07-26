using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FontTest
{
    public class FontTestController : MonoBehaviour
    {
        [SerializeField] private string m_String;
        [SerializeField] private Text m_ArialText;
        [SerializeField] private Text m_AbirukusaText;
        [SerializeField] private Text m_FaceText;
        
        public void Update()
        {
            m_ArialText.text = m_String;
            m_AbirukusaText.text = m_String;
            m_FaceText.text = m_String;
        }
    }

}