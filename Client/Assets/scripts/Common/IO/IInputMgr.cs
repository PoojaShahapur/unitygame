using System;
using UnityEngine;
using System.Collections;

namespace SDK.Common
{
    public interface IInputMgr
    {
        void addKeyListener(EventID evtID, Action<KeyCode> cb);
        void removeKeyListener(EventID evtID, Action<KeyCode> cb);
        void addMouseListener(EventID evtID, Action cb);
        void removeMouseListener(EventID evtID, Action cb);
        void addAxisListener(EventID evtID, Action cb);
        void removeAxisListener(EventID evtID, Action cb);

        void postInit();
    }
}