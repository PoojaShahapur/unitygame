using System.Collections.Generic;

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


        public MSceneManager(string name)
        {
            mName = name;
            mSceneRoot = null;

            mDisplayNodes = false;
            mShowBoundingBoxes = false;
        }


        public MCamera createCamera(string name)
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

        public MSceneNode createSceneNodeImpl()
        {
            return new MSceneNode(this);
        }

        public MSceneNode createSceneNodeImpl(string name)
        {
            return new MSceneNode(this, name);
        }

        public MSceneNode createSceneNode()
        {
            MSceneNode sn = createSceneNodeImpl();
            UtilApi.assert(mSceneNodes.ContainsKey(sn.getName()));
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

        public void destroySceneNode(string name)
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
                mSceneRoot = createSceneNodeImpl("Ogre/SceneRoot");
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

        public void _updateSceneGraph(MCamera cam)
        {
            firePreUpdateSceneGraph(cam);

            MNode.processQueuedUpdates();

            getRootSceneNode()._update(true, false);

            firePostUpdateSceneGraph(cam);
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
    }
}