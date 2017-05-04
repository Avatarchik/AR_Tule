using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System;
#endif
public class AssetBundleLoadAsynOperation : IEnumerator
{

    public delegate void OnAssetBundleLoadedDelegate(object obj, object param);


    protected string m_AssetBundleName;
    protected string m_AssetName;
    protected string m_DownloadingError;
    protected OnAssetBundleLoadedDelegate m_LoadedCallback;
    protected object m_ParamData;

    protected bool m_IsDone = false;

    public object Current
    {
        get
        {
            return null;
        }
    }
    public bool MoveNext()
    {
        return Update();
    }

    public void Reset()
    {
    }

    public AssetBundleLoadAsynOperation(string bundle, string asset,OnAssetBundleLoadedDelegate cb)
    {
        m_AssetBundleName = bundle;
        m_AssetName = asset;
        m_LoadedCallback = cb;
    }

    //return if Operation will done:  false process will end  , true process continue
    public virtual bool Update()
    {
        return true;
    }
    public virtual void DoCallback()
    {
       
    }

    public virtual bool IsDone()
    {
        return m_IsDone;
    }
}

public class AssetBundleLoadLevelAsynOperation : AssetBundleLoadAsynOperation
{
    protected bool m_IsAdditive;

    protected AsyncOperation m_Request;

    public AssetBundleLoadLevelAsynOperation(string assetbundleName, string assetName, bool isAdditive, OnAssetBundleLoadedDelegate cb)
        : base(assetbundleName, assetName,cb)
    {
        m_IsAdditive = isAdditive;
    }

    public override bool Update()
    {
#if UNITY_EDITOR

        if (AssetBundleLoadManager.SimulateAssetBundleInEditor)
        {
            string[] levelPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(m_AssetBundleName, m_AssetName);
            if (levelPaths.Length == 0)
            {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...

                Debug.LogError("There is no scene with name \"" + m_AssetName + "\" in " + m_AssetBundleName);
                return true;
            }


            if (m_IsAdditive)
                m_Request = EditorApplication.LoadLevelAdditiveAsyncInPlayMode(levelPaths[0]);
            else
                m_Request = EditorApplication.LoadLevelAsyncInPlayMode(levelPaths[0]);

            m_IsDone = true;
            DoCallback();
            return false;
        }
#endif



        if (m_Request != null && m_Request.isDone)
        {
            m_IsDone = true;
            DoCallback();
            return false;
        }
        else if (m_Request != null && !m_Request.isDone)
        {
            return true;
        }

        LoadedAssetBundle bundle = AssetBundleLoadManager.Instance.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
        if (bundle != null)
        {
            if (m_IsAdditive)
                m_Request = Application.LoadLevelAdditiveAsync(m_AssetName);
            else
                m_Request = Application.LoadLevelAsync(m_AssetName);
            return true;
        }
        else
            return true;
    }


    public override void DoCallback()
    {
        bool isdone = IsDone();
        if (isdone)
        {
            if (m_LoadedCallback != null)
            {
                m_LoadedCallback(null, null);
            }
        }
        else
        {
            Debug.LogError(string.Format("Asset: {0} - {1} Load Operation not done while do callback",m_AssetBundleName,m_AssetName));
        }
    }
}




public class AssetBundleLoadAssetAsynOperation : AssetBundleLoadAsynOperation
{
    protected AssetBundleRequest m_Request;
    System.Type m_Type;

    public AssetBundleLoadAssetAsynOperation(string assetbundleName, string assetName, System.Type type, OnAssetBundleLoadedDelegate cb,object data)
        : base(assetbundleName, assetName,cb)
    {
        m_Type = type;
        m_ParamData = data;
    }


    public override bool Update()
    {
#if UNITY_EDITOR
        if (AssetBundleLoadManager.SimulateAssetBundleInEditor)
        {
            m_IsDone = true;
            DoCallback();
            return false;
        }
#endif
        if (m_Request != null && m_Request.isDone)
        {
            m_IsDone = true;
            DoCallback();
            return false;
        }
        else if (m_Request != null && !m_Request.isDone)
        {
            return true;
        }

        LoadedAssetBundle bundle = AssetBundleLoadManager.Instance.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
        if (bundle != null)
        {
            m_Request = bundle.m_AssetBundle.LoadAssetAsync(m_AssetName, m_Type);
            return true;
        }
        else
        {
            return true;
        }

    }


    public UnityEngine.Object GetAsset()
    {

#if UNITY_EDITOR
        if (AssetBundleLoadManager.SimulateAssetBundleInEditor)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(m_AssetBundleName, m_AssetName);
            if (assetPaths.Length == 0)
            {
                Debug.LogError("There is no asset with name \"" + m_AssetName + "\" in " + m_AssetBundleName);
                return null;
            }

            // @TODO: Now we only get the main object from the first asset. Should consider type also.
            UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
            return target;
        }
#endif

        LoadedAssetBundle bundle = AssetBundleLoadManager.Instance.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
        if(bundle == null)
        {
            Debug.Log("GetAsset fail:" + m_AssetBundleName);
            return null;
        }
        if (m_Request != null && m_Request.isDone)
        {
            Debug.Log("GetAsset succ:" + m_AssetBundleName);
            return m_Request.asset;
        }
        else
        {
            return null;
        }
    }


    public override void DoCallback()
    {

        if (m_LoadedCallback != null)
        {
            UnityEngine.Object obj = GetAsset();
            m_LoadedCallback(obj, m_ParamData);
        }

    }

}
