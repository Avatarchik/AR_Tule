using UnityEngine;
#if UNITY_EDITOR	
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;


public class LoadedAssetBundle
{
    public AssetBundle m_AssetBundle;

    public LoadedAssetBundle(AssetBundle assetBundle)
    {
        m_AssetBundle = assetBundle;
    }
}


public class AssetBundleLoadManager : MonoSingleton<AssetBundleLoadManager>
{
  

    public bool m_ManifestFromHttp = false;
    AssetBundleManifest m_AssetBundleManifest = null;
#if UNITY_EDITOR	
    static int m_SimulateAssetBundleInEditor = -1;
	const string kSimulateAssetBundles = "SimulateAssetBundles";

    public static bool SimulateAssetBundleInEditor
    {
        get
        {
            if (m_SimulateAssetBundleInEditor == -1)
                m_SimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

            return m_SimulateAssetBundleInEditor != 0;
        }
        set
        {
            int newValue = value ? 1 : 0;
            if (newValue != m_SimulateAssetBundleInEditor)
            {
                m_SimulateAssetBundleInEditor = newValue;
                EditorPrefs.SetBool(kSimulateAssetBundles, value);
            }
        }
    }
#endif

    Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();
    Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
    Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();


    public bool Working
    {
        get
        {
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor == true)
            {
                return true;
            }
#endif
            if (m_AssetBundleManifest == null)
            {
                return false;
            }
            return true;
        }
    }

#region Unity Functions

    void Start()
    {
        StartCoroutine(Initialize());
    }
    void Update()
    {
        // Collect all the finished WWWs.
        var keysToRemove = new List<string>();
        foreach (var keyValue in m_DownloadingWWWs)
        {
            WWW download = keyValue.Value;

            // If downloading fails.
            if (m_DownloadingErrors.ContainsKey(keyValue.Key))
            {
                continue;
            }
            else
            {
                if (download.error != null)
                {
                    m_DownloadingErrors.Add(keyValue.Key, download.error);
                    keysToRemove.Add(keyValue.Key);
                    continue;
                }
                // If downloading succeeds.
                if (download.isDone)
                {
                    //Debug.Log("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
                    m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle));
                    keysToRemove.Add(keyValue.Key);
                }

            }



        }

        // Remove the finished WWWs.
        foreach (var key in keysToRemove)
        {

            if (m_DownloadingWWWs.ContainsKey(key))
            {
                WWW download = m_DownloadingWWWs[key];

                m_DownloadingWWWs.Remove(key);
                download.Dispose();
            }
        }

    }

    
#endregion


#region WWW Operation
    private IEnumerator Initialize()
    {

#if UNITY_EDITOR
        // If we're in Editor simulation mode, we don't need the manifest assetBundle.
        if (SimulateAssetBundleInEditor)
        {
            yield break;
        }
#endif

        string bundleName = AssetBundlePlatformPathManager.GetPlatformAssetbundlePath();
        string assetName = "AssetBundleManifest";
        WWW download = null;
        string url = "";
        if (m_ManifestFromHttp == true)
        {
            url = AssetBundlePlatformPathManager.GetDownloadingHttpAssetBundleURL() + bundleName;
        }
        else
        {
            url = AssetBundlePlatformPathManager.GetAssetBundleDownloadingURL_StreamingAsset() + bundleName;
        }
        Debug.Log("AssetBundle Manifest :" + url);


        download = new WWW(url);
        while (download.isDone != true)
        {
            yield return 0;
        }
        if (download.error != null)
        {
            Debug.LogError(string.Format("AssetBundle Manifest load Error:{0}", download.error));
        }
        else
        {
            m_AssetBundleManifest = download.assetBundle.LoadAsset<AssetBundleManifest>(assetName);
            if (m_AssetBundleManifest == null)
            {
                Debug.LogError(string.Format("AssetBundle Manifest load Failed"));
            }
            else
            {
            }
        }

    }

    IEnumerator WaitWorking(string assetBundleName, bool fromHttp)
    {
        yield return 0;
        StartLoadAssetBundle(assetBundleName,fromHttp);
    }

    // Load AssetBundle and its dependencies.
    public void StartLoadAssetBundle(string assetBundleName, bool fromHttp)
    {
        if (!Working)
        {
            Debug.LogError("AssetBundleLoadManager not Working");
            StartCoroutine(WaitWorking(assetBundleName,fromHttp));
            return;
        }

        if (m_DownloadingWWWs.ContainsKey(assetBundleName))
        {
            Debug.Log(string.Format("AssetBundleLoadManager already process AssetBundle:{0}",assetBundleName));
            return;
        }
        if (m_LoadedAssetBundles.ContainsKey(assetBundleName))
        {
            Debug.Log(string.Format("AssetBundleLoadManager already loaded AssetBundle:{0}", assetBundleName));
            return;
        }

#if UNITY_EDITOR
        // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
        if (SimulateAssetBundleInEditor)
            return;
#endif
        // Check if the assetBundle has already been processed.
        bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, fromHttp);

        // Load dependencies.
        if (!isAlreadyProcessed)
            LoadDependencies(assetBundleName, fromHttp);
    }

    private bool LoadAssetBundleInternal(string assetBundleName, bool fromHttp)
    {
        if (m_DownloadingWWWs.ContainsKey(assetBundleName))
            return true;

        WWW download = null;
        string url = "";
        if (fromHttp == true)
        {

            url = AssetBundlePlatformPathManager.GetDownloadingHttpAssetBundleURL() + assetBundleName;
        }
        else
        {

            url = AssetBundlePlatformPathManager.GetAssetBundleDownloadingURL_StreamingAsset() + assetBundleName;
        }
        
        download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0);
        m_DownloadingWWWs.Add(assetBundleName, download);
        return false;
    }
    private void LoadDependencies(string assetBundleName, bool fromHttp)
    {
        string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
            return;

        m_Dependencies.Add(assetBundleName, dependencies);
        for (int i = 0; i < dependencies.Length; i++)
            LoadAssetBundleInternal(dependencies[i], fromHttp);
    }



#endregion

#region  AssetBundle Operation
    public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
    {
        if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
        {
            Debug.LogError(string.Format("LoadedAssetBundel DownloadingErrors :{0}", error));
            return null;
        }
        LoadedAssetBundle bundle = null;
        m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if (bundle == null)
        {
            //Debug.LogError(string.Format("LoadedAssetBundle :{0} not Found", assetBundleName));
            return null;
        }
        // No dependencies are recorded, only the bundle itself is required.
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
        {
            Debug.Log("GetLoadedAssetBundle no dependency" + assetBundleName);
            return bundle;
        }
        // Make sure all dependencies are loaded
        foreach (var dependency in dependencies)
        {
            // Wait all the dependent assetBundles being loaded.
            LoadedAssetBundle dependentBundle;
            m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null)
            {
                //Debug.LogError(string.Format("LoadedAssetBundel Dependency :{0} not Found", dependency));
                return null;
            }
        }

        Debug.Log("GetLoadedAssetBundle with dependency" + assetBundleName);
        return bundle;
    }


    public void UnloadAssetBundle(string assetBundleName, bool unloadAllLoadedObjects)
    {
#if UNITY_EDITOR
        // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
        if (SimulateAssetBundleInEditor)
            return;
#endif
        UnloadAssetBundleInternal(assetBundleName, unloadAllLoadedObjects);
        UnloadDependencies(assetBundleName, unloadAllLoadedObjects);
    }

    private void UnloadDependencies(string assetBundleName, bool unloadAllLoadedObjects)
    {
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            return;

        foreach (var dependency in dependencies)
        {
            UnloadAssetBundleInternal(dependency, unloadAllLoadedObjects);
        }

        m_Dependencies.Remove(assetBundleName);
    }

    private void UnloadAssetBundleInternal(string assetBundleName, bool unloadAllLoadedObjects)
    {
        string error;
        LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
        if (bundle == null)
            return;


        Debug.Log("Unload:" + assetBundleName);
        bundle.m_AssetBundle.Unload(unloadAllLoadedObjects);
        m_LoadedAssetBundles.Remove(assetBundleName);

    }

    public float GetDownloadingWWWProgress(string fullbundle)
    {
        if (m_DownloadingWWWs.ContainsKey(fullbundle))
        {
            return m_DownloadingWWWs[fullbundle].progress;
        }
        if (m_LoadedAssetBundles.ContainsKey(fullbundle))
        {
            return 1;
        }
        return 0;
    }


#endregion
   

} 