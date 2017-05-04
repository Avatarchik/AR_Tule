using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseAssetLoader : MonoSingleton<BaseAssetLoader>
{

    List<AssetBundleLoadAsynOperation> m_InProgressOperations = new List<AssetBundleLoadAsynOperation>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < m_InProgressOperations.Count; )
        {
            if (m_InProgressOperations[i].IsDone())
            {
                m_InProgressOperations.RemoveAt(i);
            }
            else
                i++;
        }
        

	}


    public AssetBundleLoadAsynOperation StartLoadAsset(string assetBundle, string asset, AssetBundleLoadAssetAsynOperation.OnAssetBundleLoadedDelegate cb,object  data = null,bool bhttp = false)
    {
        AssetBundleLoadManager.Instance.StartLoadAssetBundle(assetBundle, bhttp);

        AssetBundleLoadAssetAsynOperation operation = new AssetBundleLoadAssetAsynOperation(assetBundle, asset, typeof(Object), cb,data);
        StartCoroutine(operation);

        m_InProgressOperations.Add(operation);


        return operation;
    }
    public AssetBundleLoadAsynOperation StartLoadLevel(string assetBundle, string asset, AssetBundleLoadAssetAsynOperation.OnAssetBundleLoadedDelegate cb, bool additive = false, bool bhttp = false)
    {
        AssetBundleLoadManager.Instance.StartLoadAssetBundle(assetBundle, bhttp);

        AssetBundleLoadLevelAsynOperation operation = new AssetBundleLoadLevelAsynOperation(assetBundle, asset, additive, cb);
        StartCoroutine(operation);

        m_InProgressOperations.Add(operation);

        return operation;
    }


  
    public void CancelProgress(AssetBundleLoadAsynOperation operation)
    {
        if (m_InProgressOperations.Contains(operation))
        {
            StopCoroutine(operation);
            m_InProgressOperations.Remove(operation);
        }

    }


    public void Unload(string assetBundle)
    {
        AssetBundleLoadManager.Instance.UnloadAssetBundle(assetBundle, true);
    }
}
