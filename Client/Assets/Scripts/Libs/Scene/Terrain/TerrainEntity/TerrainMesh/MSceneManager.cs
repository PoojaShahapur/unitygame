using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class MSceneManager
    {
        public class MListener
        {
            public MListener()
            {

            }

            public virtual void preUpdateSceneGraph(MSceneManager source, MCamera camera)
            {

            }

            public virtual void postUpdateSceneGraph(MSceneManager source, MCamera camera)
            {

            }

            public virtual void sceneManagerDestroyed(MSceneManager source)
            {

            }
        }

        public enum RequestType
        {
            SCENE_CULL,
            NUM_REQUESTS
        }

        protected string mName;
        protected Dictionary<string, MSceneNode> mSceneNodes;
        protected MSceneNode mSceneRoot;
        protected HashSet<MSceneNode> mAutoTrackingSceneNodes;

        protected ulong mLastFrameNumber;
        protected MMatrix4[] mTempXform;
        protected bool mResetIdentityView;
        protected bool mResetIdentityProj;

        protected Dictionary<string, MCamera> mCameras;
        protected bool mDisplayNodes;
        protected bool mShowBoundingBoxes;
        protected MList<MListener> mListeners;

        protected uint mNumWorkerThreads;
        MBarrier mWorkerThreadsBarrier;
        protected MList<MThread> mWorkerThreads;
        protected RequestType mRequestType;
        protected MUpdateTransformRequest mUpdateTransformRequest;
        protected CoroutineSceneUpdateTask mCoroutineSceneUpdateTask;

        public MSceneManager(string name)
        {
            mName = name;
            mSceneRoot = null;

            mDisplayNodes = false;
            mShowBoundingBoxes = false;
            mSceneNodes = new Dictionary<string, MSceneNode>();
            mAutoTrackingSceneNodes = new HashSet<MSceneNode>();
            mCameras = new Dictionary<string, MCamera>();
            mListeners = new MList<MListener>();

            if (MacroDef.MULTITHREADING_CULL)
            {
                mNumWorkerThreads = 1;
                mWorkerThreads = new MList<MThread>();
                mUpdateTransformRequest = new MUpdateTransformRequest();
                startWorkerThreads();
            }
        }

        virtual public MCamera createCamera(string name)
        {
            if (mCameras != null)
            {
                // Error
            }

            // TODO:
            //MCamera c = new MCamera(name, this);
            MCamera c = new MCamera(null);
            mCameras.Add(name, c);

            return c;
        }

        public MCamera getCamera(string name)
        {
            if (!mCameras.ContainsKey(name))
            {
                // Error
                return null;
            }
            else
            {
                return mCameras[name];
            }
        }

        public bool hasCamera(string name)
        {
            return (mCameras.ContainsKey(name));
        }

        void destroyCamera(MCamera cam)
        {
            if (cam == null)
            {
                // Error
            }

            destroyCamera(cam.getName());
        }

        public void destroyCamera(string name)
        {
            if (mCameras.ContainsKey(name))
            {
                mCameras.Remove(name);
            }
        }

        virtual public void clearScene()
        {
            getRootSceneNode().removeAllChildren();
            getRootSceneNode().detachAllObjects();
        }

        virtual public MSceneNode createSceneNodeImpl()
        {
            return new MSceneNode(this);
        }

        virtual public MSceneNode createSceneNodeImpl(string name)
        {
            return new MSceneNode(this, name);
        }

        public MSceneNode createSceneNode()
        {
            MSceneNode sn = createSceneNodeImpl();
            UtilApi.assert(!mSceneNodes.ContainsKey(sn.getName()));
            mSceneNodes[sn.getName()] = sn;
            return sn;
        }

        public MSceneNode createSceneNode(string name)
        {
            if (mSceneNodes.ContainsKey(name))
            {
                // error
            }

            MSceneNode sn = createSceneNodeImpl(name);
            mSceneNodes[sn.getName()] = sn;
            return sn;
        }

        virtual public void destroySceneNode(string name)
        {
            if (!mSceneNodes.ContainsKey(name))
            {
                // error
            }

            MNode parentNode = mSceneNodes[name].getParent();
            if (parentNode != null)
            {
                parentNode.removeChild(mSceneNodes[name]);
            }

            mSceneNodes.Remove(name);
        }

        public void destroySceneNode(MSceneNode sn)
        {
            if (sn == null)
            {
                // Error
            }

            destroySceneNode(sn.getName());
        }

        public MSceneNode getRootSceneNode()
        {
            if (mSceneRoot == null)
            {
                mSceneRoot = createSceneNodeImpl("SceneRoot");
                mSceneRoot._notifyRootNode();
            }

            return mSceneRoot;
        }

        public MSceneNode getSceneNode(string name)
        {
            if (!mSceneNodes.ContainsKey(name))
            {
                // Error
            }

            return mSceneNodes[name];
        }

        public bool hasSceneNode(string name)
        {
            return (mSceneNodes.ContainsKey(name));
        }

        virtual public void _updateSceneGraph(MCamera cam)
        {
            firePreUpdateSceneGraph(cam);

            MNode.processQueuedUpdates();

            getRootSceneNode()._update(true, false);

            firePostUpdateSceneGraph(cam);
        }

        virtual public void _findVisibleObjects(MCamera cam)
        {
            Ctx.m_instance.m_logSys.log("10000  _findVisibleObjects");
            this.getRootSceneNode()._findVisibleObjects(cam, true);
        }

        public void addListener(MSceneManager.MListener newListener)
        {
            mListeners.Add(newListener);
        }

        public void removeListener(MSceneManager.MListener delListener)
        {
            mListeners.Remove(delListener);
        }

        public void firePreUpdateSceneGraph(MCamera camera)
        {
            foreach (MListener listener in mListeners.list())
            {
                listener.preUpdateSceneGraph(this, camera);
            }
        }

        public void firePostUpdateSceneGraph(MCamera camera)
        {
            foreach (MListener listener in mListeners.list())
            {
                listener.postUpdateSceneGraph(this, camera);
            }
        }

        public void showBoundingBoxes(bool bShow)
        {
            mShowBoundingBoxes = bShow;
        }

        public bool getShowBoundingBoxes()
        {
            return mShowBoundingBoxes;
        }

        public void cullScene()
        {
            if(MacroDef.MULTITHREADING_CULL)
            {
                mRequestType = RequestType.SCENE_CULL;
                fireWorkerThreadsAndWait();
            }
            else
            {
                cullSceneThread(null, 0);
            }
        }

        public void cullSceneThread(MUpdateTransformRequest request, int threadIdx)
        {
            _updateSceneGraph(Ctx.m_instance.m_camSys.getLocalCamera());
            _findVisibleObjects(Ctx.m_instance.m_camSys.getLocalCamera());
        }

        public long updateWorkerThread(MSceneThread threadHandle)
        {
            MSceneManager sceneManager = threadHandle.getSceneManager();
            return sceneManager._updateWorkerThread(threadHandle);
        }

        public void fireWorkerThreadsAndWait()
        {
            Ctx.m_instance.m_logSys.log("10000  fireWorkerThreadsAndWait");
            Ctx.m_instance.m_logSys.log("10000  fireWorkerThreadsAndWait mWorkerThreadsBarrier.sync_1");
            mWorkerThreadsBarrier.sync();
            Ctx.m_instance.m_logSys.log("10000  fireWorkerThreadsAndWait mWorkerThreadsBarrier.sync_2");
            mWorkerThreadsBarrier.sync();
        }

        public void startWorkerThreads()
        {
            mWorkerThreadsBarrier = new MBarrier( mNumWorkerThreads+1 );
            MThread thread = null;
            for( int i = 0; i< mNumWorkerThreads; ++i )
            {
                thread = new MSceneThread(updateWorkerThread, i, this);
                thread.start();
                mWorkerThreads.Add(thread);
            }
        }

        public void stopWorkerThreads()
        {
            int idx = 0;
            while(idx < mWorkerThreads.length())
            {
                mWorkerThreads[idx].ExitFlag = true;
                ++idx;
            }
            fireWorkerThreadsAndWait();

            idx = 0;
            while(idx < mWorkerThreads.length())
            {
                mWorkerThreads[idx].join();
                ++idx;
            }

            mWorkerThreadsBarrier = null;
        }

        public long _updateWorkerThread(MSceneThread threadHandle)
        {
            int threadIdx = threadHandle.getThreadIdx();
            Ctx.m_instance.m_logSys.log("10000  _updateWorkerThread mWorkerThreadsBarrier.sync_1");
            mWorkerThreadsBarrier.sync();
            switch (mRequestType)
            {
                case RequestType.SCENE_CULL:
                    try
                    {
                        Ctx.m_instance.m_logSys.log("10000  _updateWorkerThread cullSceneThread");
                        cullSceneThread(mUpdateTransformRequest, threadIdx);
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                    break;
                default:
                    break;
            }
            Ctx.m_instance.m_logSys.log("10000  _updateWorkerThread mWorkerThreadsBarrier.sync_2");
            mWorkerThreadsBarrier.sync();

            return 0;
        }

        public void addUpdateTask()
        {
            mCoroutineSceneUpdateTask = new CoroutineSceneUpdateTask();
            Ctx.m_instance.mCoroutineTaskMgr.addTask(mCoroutineSceneUpdateTask);
        }

        public void runUpdateTask()
        {
            mCoroutineSceneUpdateTask.Start();
        }
    }
}