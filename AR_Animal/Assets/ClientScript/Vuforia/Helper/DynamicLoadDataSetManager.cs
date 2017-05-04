using UnityEngine;
using System.Collections;
using System.IO;
using Vuforia;
using System;
using System.Collections.Generic;

public class DynamicLoadDataSetManager : MonoSingleton<DynamicLoadDataSetManager> {

    public delegate void OnImageTargetCreatedDelegate(TrackableBehaviour track);

    string QCARSubPath = "/VuforiaDataSet/";

    List<string> _LoadDataSetList;

    public IEnumerator StartLoadDataSet(List<string> list, OnImageTargetCreatedDelegate cb, bool bhttp = false)
    {
        Debug.Log("StartLoadDataSet" + list.ToArray());

        foreach (string name in list)
        {

            Debug.Log("StartLoadDataSet ing");
            yield return StartCoroutine(LoadDataSet(name));
        }

        Debug.Log("StartLoadDataSet End" );
        InitImageTarget(cb);

        yield return 0;
    }



    IEnumerator CopyFileFromStream2Device(string src,string dest)
    {
        if (File.Exists(dest))
        {
            yield break;
        }

        

#if UNITY_IOS
         WWW wwwxml = new WWW(src);

        Debug.LogError(string.Format("CopyFileFromStream2Device from:{0}", src));
        while (!wwwxml.isDone)
        {
            yield return 0;
        }
        if (wwwxml.error == null)
        {
            string dir = AssetBundlePlatformPathManager.GetOnlyDirectoryName(dest);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            Debug.LogError(string.Format("CopyFileFromStream2Device to:{0}",dest));
            MiscUtility.WriteFile(wwwxml.bytes, dest);
        }
        else
        {
            Debug.LogError(string.Format("DynamicLoadDataSet CopyFileFromStream2Device error:{0}",wwwxml.error));
        }
#else



        WWW wwwxml = new WWW(src);

        Debug.Log(string.Format("CopyFileFromStream2Device from:{0}", src));
        while (!wwwxml.isDone)
        {
            yield return 0;
        }
        if (wwwxml.error == null)
        {
            string dir = AssetBundlePlatformPathManager.GetOnlyDirectoryName(dest);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            Debug.Log(string.Format("CopyFileFromStream2Device to:{0}",dest));
            //MiscUtility.WriteFile(wwwxml.bytes, dest);
        }
        else
        {
            Debug.LogError(string.Format("DynamicLoadDataSet CopyFileFromStream2Device error:{0}",wwwxml.error));
        }
#endif
    }
 
    

    public IEnumerator GetDataSetXmlDataFile(string name, bool bhttp)
    {

        yield return new WaitForEndOfFrame();

        if (bhttp == false)
        {
            string srcPathXml = AssetBundlePlatformPathManager.GetStreamingAssetsPathForWWW(QCARSubPath + name + @".xml");
            string destPathXml = AssetBundlePlatformPathManager.GetFullSavePathOnDevice(QCARSubPath + name + @".xml");
            yield return StartCoroutine(CopyFileFromStream2Device(srcPathXml, destPathXml));


            string srcPathDat = AssetBundlePlatformPathManager.GetStreamingAssetsPathForWWW(QCARSubPath + name + @".dat");
            string destPathDat = AssetBundlePlatformPathManager.GetFullSavePathOnDevice(QCARSubPath + name + @".dat");
            yield return StartCoroutine(CopyFileFromStream2Device(srcPathDat, destPathDat));

        }
        else
        {
            string srcPathXml = AssetBundlePlatformPathManager.GetDownloadingHttpURL(QCARSubPath + name + @".xml");
            string destPathXml = AssetBundlePlatformPathManager.GetFullSavePathOnDevice(QCARSubPath + name + @".xml");
            //yield return StartCoroutine(DownloadFromHttp(srcPathXml, destPathXml));


            string srcPathDat = AssetBundlePlatformPathManager.GetDownloadingHttpURL(QCARSubPath + name + @".dat");
            string destPathDat = AssetBundlePlatformPathManager.GetFullSavePathOnDevice(QCARSubPath + name + @".dat");
            //yield return StartCoroutine(DownloadFromHttp(srcPathDat, destPathDat));
        }
    }

  

#region Operation Unit
    private bool LoadDataSetStreamasset(string dataSetName)
    {
        // Check if the data set exists at the given path.
        if (!DataSet.Exists(dataSetName))
        {
            Debug.LogError("Data set " + dataSetName + " does not exist.");
            return false;
        }

        // Request an ImageTracker instance from the TrackerManager.
        //TrackerManager.Instance.InitTracker<ObjectTracker>();
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>() as ObjectTracker;



        // Create a new empty data set.
        DataSet dataSet = objectTracker.CreateDataSet();

        // Load the data set from the given path.
        if (!dataSet.Load(dataSetName))
        {
            Debug.LogError("Failed to load data set " + name + ".");
            return false;
        }


        objectTracker.Stop();

        // (Optional) Activate the data set.
        objectTracker.ActivateDataSet(dataSet);


        if (!objectTracker.Start())
        {
            Debug.LogError("Tracker Failed to Start.");
            return false;
        }


        Debug.Log(string.Format("Load Data Set and Start Success : {0}", dataSetName));
        return true;
    }
    private IEnumerator LoadDataSet(string dataSetName)
    {
        yield return new WaitForEndOfFrame();

        VuforiaUnity.StorageType storageType = VuforiaUnity.StorageType.STORAGE_ABSOLUTE;


        string destPathXml = AssetBundlePlatformPathManager.GetFullSavePathOnDevice(QCARSubPath + dataSetName + @".xml");

        Debug.Log("Try Exist:" + destPathXml);
        // Check if the data set exists at the given path.
        if (!DataSet.Exists(destPathXml, storageType))
        {
            Debug.LogError("Data set " + destPathXml + " does not exist.");
            yield break;
        }

        Debug.Log("Try Get Tracker");
        // Request an ImageTracker instance from the TrackerManager.
        //TrackerManager.Instance.InitTracker<ObjectTracker>();
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>() as ObjectTracker;


        Debug.Log("Try Create TrackerDataset");

        // Create a new empty data set.
        DataSet dataSet = objectTracker.CreateDataSet();

        Debug.LogError("Start Load data set :" + dataSetName); 

        // Load the data set from the given path.
        if (!dataSet.Load(destPathXml, storageType))
        {
            Debug.LogError("Failed to load data set " + destPathXml + ".");
            yield break;
        }


        objectTracker.Stop();

        // (Optional) Activate the data set.
        objectTracker.ActivateDataSet(dataSet);


        if (!objectTracker.Start())
        {
            Debug.LogError("Tracker Failed to Start.");
            yield break;
        }
        Debug.Log(string.Format("Load Data Set and Start Success : {0}", dataSetName));
        
    }
    private void InitImageTarget(OnImageTargetCreatedDelegate cb)
    {
        IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
        foreach (TrackableBehaviour tb in tbs)
        {
            // change generic name to include trackable name
            tb.gameObject.name = "DynamicImageTarget-" + tb.TrackableName;

            if (cb != null)
            {
                cb(tb);
            }
        }


    }
#endregion


}
