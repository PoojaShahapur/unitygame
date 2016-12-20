using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 通过添加 Entry 实现
     */
    public class EventTriggerEntryListener
    {
        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        protected EventTrigger m_eventTrigger;
        static public Dictionary<EventTrigger, EventTriggerEntryListener> sTrigger2Listener = new Dictionary<EventTrigger, EventTriggerEntryListener>();

        static public EventTriggerEntryListener Get(GameObject go)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = go.AddComponent<EventTrigger>();
                sTrigger2Listener[trigger] = new EventTriggerEntryListener();
                sTrigger2Listener[trigger].m_eventTrigger = trigger;
            }
            else
            {
                return sTrigger2Listener[trigger];
            }

            if (trigger != null)
            {
                EventTrigger.Entry entry;

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry); // 这一行就相当于在 EventTrigger 组件编辑器中点击[Add New Event Type] 添加一个新的事件类型
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnPointerClick);

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnPointerDown);

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnPointerEnter);

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnPointerExit);

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnPointerUp);

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.Select;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnSelect);

                entry = new EventTrigger.Entry();
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.UpdateSelected;
                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(sTrigger2Listener[trigger].OnUpdateSelected);
            }

            return sTrigger2Listener[trigger];
        }

        public void OnPointerClick(BaseEventData eventData)
        {
            PointerEventData pointEventData = eventData as PointerEventData;
            if (onClick != null)
            {
                onClick(m_eventTrigger.gameObject);
            }
        }

        public void OnPointerDown(BaseEventData eventData)
        {
            PointerEventData pointEventData = eventData as PointerEventData;
            if (onDown != null)
            {
                onDown(m_eventTrigger.gameObject);
            }
        }

        public void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointEventData = eventData as PointerEventData;
            if (onEnter != null)
            {
                onEnter(m_eventTrigger.gameObject);
            }
        }

        public void OnPointerExit(BaseEventData eventData)
        {
            PointerEventData pointEventData = eventData as PointerEventData;
            if (onExit != null)
            {
                onExit(m_eventTrigger.gameObject);
            }
        }

        public void OnPointerUp(BaseEventData eventData)
        {
            PointerEventData pointEventData = eventData as PointerEventData;
            if (onUp != null)
            {
                onUp(m_eventTrigger.gameObject);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null)
            {
                onSelect(m_eventTrigger.gameObject);
            }
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null)
            {
                onUpdateSelect(m_eventTrigger.gameObject);
            }
        }
    }
}