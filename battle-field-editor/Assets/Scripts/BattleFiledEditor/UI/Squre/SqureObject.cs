using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Scripts.UI.Squre
{
    public class SqureObject : MonoBehaviour
    {
        private Button m_Button;

        private UnityAction m_OnClick, m_OnPointerEnter, m_OnPointerExit;

        public void Awake()
        {
            Transform btnTrans = transform.Find("Button");
            m_Button = btnTrans.GetComponent<Button>();
            m_Button.onClick.AddListener(() => { onClickDelegate(); });

            EventTrigger trigger = btnTrans.GetComponent<EventTrigger>();
            trigger.triggers.Add(createEventTriggerEntry(EventTriggerType.PointerEnter, (data) => { onPointerEnterDelegate(); }));
            trigger.triggers.Add(createEventTriggerEntry(EventTriggerType.PointerExit, (data) => { onPointerExitDelegate(); }));
        }

        private static EventTrigger.Entry createEventTriggerEntry(EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(callback);
            return entry;
        }

        private void onClickDelegate()
        {
            if (m_OnClick != null)
            {
                m_OnClick.Invoke();                
            }
        }

        private void onPointerEnterDelegate()
        {
            if (m_OnPointerEnter != null)
            {
                m_OnPointerEnter.Invoke();
            }
        }
        
        private void onPointerExitDelegate()
        {
            if (m_OnPointerExit != null)
            {
                m_OnPointerExit.Invoke();
            }
        }

        public void SetButtonClick(UnityEngine.Events.UnityAction onClick)
        {
            m_OnClick = onClick;
        }
        
        public void SetButtonEnter(UnityEngine.Events.UnityAction onEnter)
        {
            m_OnPointerEnter = onEnter;
        }
        
        public void SetButtonExit(UnityEngine.Events.UnityAction onExit)
        {
            m_OnPointerExit = onExit;
        }
        
        public void SetButtonColor(Color color)
        {
            m_Button.image.color = color;
        }
    }
}