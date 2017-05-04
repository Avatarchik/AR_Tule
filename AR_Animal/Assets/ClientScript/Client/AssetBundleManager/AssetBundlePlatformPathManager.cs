using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class AssetBundlePlatformPathManager {

    static string kAssetBundlesPath ="/AssetBundle/";



#if UNITY_EDITOR
    //public static string _Server = "http://120.24.225.193/";
    public static string _Server = @"http://www.23michael45.com/";
    //public static string _Server = @"http://localhost/";
#else
    public static string _Server = @"http://www.23michael45.com/";
#endif



    #region Editor Mode Path

    //这里是编辑器打包时用的函数,真机下无这些功能
#if UNITY_EDITOR
    public static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
   

    public static string GetAssetBundleStorePath()
    {
        string outputPath = Path.Combine(AssetBundlePlatformPathManager.kAssetBundlesPath, AssetBundlePlatformPathManager.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));
        string fullpath = Application.dataPath + "/../../Share/" + outputPath;
        return fullpath;
    }

    public static string GetAppOutputPath()
    {
        string fullpath = Application.dataPath + "/../../Share/BuildApp/";
        return fullpath;
    }


#endif
#endregion


    public static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsWebPlayer:
            case RuntimePlatform.OSXWebPlayer:
                return "WebPlayer";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            // Add more build platform for your own.
            // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
            default:
                return null;
        }
    }

    public static string GetPlatformAssetbundlePath()
    {
          string platformFolderForAssetBundles =
#if UNITY_EDITOR
        GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
        GetPlatformFolderForAssetBundles(Application.platform);
#endif

        return platformFolderForAssetBundles;
    }







    //得到AR压缩包Zip的解压目录 入参带后缀   可以是路径带/ 也可是文件.XXX
    public static string GetFullSavePathOnDevice(string name)
    {
        if (name.StartsWith(@"/"))
        {

            return Application.persistentDataPath + name;
        }
        else
        {

            return Application.persistentDataPath + "/" + name;
        }
    }



    //得到文件名,不包含路径
    public static string GetOnlyFileName(string full)
    {

        return full.Substring(full.LastIndexOf('/') + 1, full.Length - full.LastIndexOf('/') - 1);
    }
    public static string GetOnlyDirectoryName(string full)
    {
        string dir = full.Substring(0, full.LastIndexOf('/'));

        return dir;
    }

    public static string GetStreamingAssetsPathForFileSystem(string name)
    {
        string path = Application.streamingAssetsPath;
        if (name.StartsWith(@"/"))
        {
            return path + name;
        }
        else
        {
            return path + "/" + name;
        }
    }
    public static string GetStreamingAssetsPathForWWW(string name)
    {
        string path = "";
        if (Application.isEditor)
        {
            //string path = "file://" + System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
            //path += "/../Share/";

            path = "file://" + Application.streamingAssetsPath;

        }
        else if (Application.isWebPlayer)
            path = System.IO.Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            path = "file://" + Application.streamingAssetsPath;
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
            path = Application.streamingAssetsPath;
        else // For standalone player.
            path = "file://" + Application.streamingAssetsPath;

        if (name.StartsWith(@"/"))
        {

            return path + name;
        }
        else
        {

            return path + "/" + name;
        }
     }


#region Download Path
    public static string GetAssetBundleDownloadingURL_StreamingAsset()
    {
        
        string url = GetStreamingAssetsPathForWWW(AssetBundlePlatformPathManager.kAssetBundlesPath + GetPlatformAssetbundlePath() + "/");
        return url;
    }
    public static string GetDownloadingHttpURL(string name)
    {
       
        string url = _Server + @"/";
        return url + name;
    }

    public static string GetDownloadingHttpAssetBundleURL()
    {
        
        string url = _Server + AssetBundlePlatformPathManager.kAssetBundlesPath + GetPlatformAssetbundlePath() + "/";
        return url;
    }

    

#endregion

}
