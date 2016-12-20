using UnityEngine;
using UnityEngine.UI;
using SDK.Lib;

namespace UnitTest
{
    /**
     * @brief 测试 UGui
     */
    public class TestUGui
    {
        public Button button;
        public Image image;

        public void run()
        {

        }

        /**
         * @brief 测试 UGui 添加任意事件
         */
        protected void testEventTriggerListener()
        {
            GameObject go = null;
            button = go.transform.Find("Button").GetComponent<Button>();
            image = go.transform.Find("Image").GetComponent<Image>();
            EventTriggerListener.Get(button.gameObject).onClick = OnButtonClick;
            EventTriggerListener.Get(image.gameObject).onClick = OnButtonClick;
        }

        private void OnButtonClick(GameObject go)
        {
            //在这里监听按钮的点击事件
            if (go == button.gameObject)
            {
                Debug.Log("DoSomeThings");
            }
        }
    }
}