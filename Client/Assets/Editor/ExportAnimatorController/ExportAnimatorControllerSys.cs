using EditorTool;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EditorTool
{
    public class ExportAnimatorControllerSys
    {
        static public ExportAnimatorControllerSys m_instance;
        //protected XmlAnimatorController m_curXmlAnimatorController;         // 当前处理的动画控制器

        public static ExportAnimatorControllerSys instance()
        {
            if (m_instance == null)
            {
                m_instance = new ExportAnimatorControllerSys();
            }
            return m_instance;
        }

        protected List<XmlAnimatorController> m_controllerList = new List<XmlAnimatorController>();
        protected List<XmlPath> m_xmlPathList = new List<XmlPath>();                // 保存所有的目录

        public List<XmlAnimatorController> controllerList
        {
            get
            {
                return m_controllerList;
            }
            set
            {
                m_controllerList = value;
            }
        }

        //public XmlAnimatorController curXmlAnimatorController
        //{
        //    get
        //    {
        //        return m_curXmlAnimatorController;
        //    }
        //    set
        //    {
        //        m_curXmlAnimatorController = value;
        //    }
        //}

        public void clear()
        {
            m_controllerList.Clear();
            m_xmlPathList.Clear();
        }

        // 解析 Xml
        public void parseXml()
        {
            clear();

            string path = ExportUtil.getDataPath("Res/Config/Tool/ExportSkelAnimatorController.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode rootNode = xmlDoc.SelectSingleNode("Root");

            XmlNodeList controllerNodeList = rootNode.SelectNodes("Controller");
            XmlElement controllerElem;
            XmlAnimatorController controller;

            foreach (XmlNode controllerNode in controllerNodeList)
            {
                controllerElem = (XmlElement)controllerNode;
                controller = new XmlAnimatorController();
                //m_curXmlAnimatorController = controller;
                m_controllerList.Add(controller);
                controller.parseXml(controllerElem);
            }

            XmlNodeList pathNodeList = rootNode.SelectNodes("Path");
            XmlElement pathElem = null;
            XmlPath xmlPath = null;

            foreach (XmlNode pathNode in pathNodeList)
            {
                pathElem = (XmlElement)pathNode;
                xmlPath = new XmlPath();
                m_xmlPathList.Add(xmlPath);
                xmlPath.parseXml(pathElem);
            }
        }

        // 打包 Controller
        public void exportControllerAsset()
        {
            foreach(var item in m_controllerList)
            {
                //m_curXmlAnimatorController = item;
                RuntimeAnimatorController runtimeAsset = ExportAnimatorControllerUtil.BuildAnimationController(item);
                SOAnimatorController soAnimator = ScriptableObject.CreateInstance<SOAnimatorController>();
                soAnimator.addAnimator(item.controllerFullPath, runtimeAsset);

                // 创建预制，并且添加到编辑器中，以便进行检查
                AssetDatabase.CreateAsset(soAnimator, item.assetFullPath);
                //刷新编辑器
                AssetDatabase.Refresh();
            }
        }
    }
}