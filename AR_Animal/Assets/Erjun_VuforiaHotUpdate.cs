using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
 

public class Erjun_VuforiaHotUpdate : MonoBehaviour {
  

    private static Erjun_VuforiaHotUpdate _instance;
    public static Erjun_VuforiaHotUpdate GetInstance()
    {
        if (_instance == null) {
            _instance = new Erjun_VuforiaHotUpdate();
        }
        return _instance;
    }
    //private string ContentPath;
    //private int FinishDown;
    IEnumerator wait() {
        yield return new WaitForSeconds(0.5f);
        LoadAR();
    }

    void Start()
    {
        //  StartCoroutine(wait());
        //ContentPath = Erjun_Util.GetContentPath();
        ////Copy本地资源到公共地盘 Application.persistentDataPath  

        //for (int i = 0; i < DateModel.LoadAR_Res.Count; i++)
        //{
        //    //&& File.Exists(Erjun_Util.TestPlaformLocalPath(ContentPath, DateModel.LoadAR_Res[i]))
        print((Application.persistentDataPath + "/" + "Erjun_Demo.xml"));
        if (!File.Exists(Application.persistentDataPath + "/" + "Erjun_Demo.xml")||! File.Exists(Application.persistentDataPath + "/" + "Erjun_Demo.dat"))
        {
            StartCoroutine(loadLocal("Erjun_Demo.xml"));
            StartCoroutine(loadLocal("Erjun_Demo.dat"));
        }
        else
        {
            LoadAR();
        }
    }
    //加载本地资源
    IEnumerator loadLocal(string  loadName)
    {
        string localPath="";
        switch (Application.platform) {
            case RuntimePlatform.Android:
                  localPath = Application.streamingAssetsPath + "/" + loadName;
                break;
            case RuntimePlatform.IPhonePlayer:
                  localPath = Application.streamingAssetsPath + "/" + loadName;
                break;
            default:
                  localPath = "File:///" + Application.streamingAssetsPath + "/" + loadName;
                break;
        }
             
            //加载本地资源
            WWW www = new WWW(localPath);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("==============load not error==========");
            }
            else
            {
                Debug.Log("=============load error==========" + www.error);
                // yield break;
            }
            if (www.isDone)
            {
                if (www.bytesDownloaded > 0)
                {
                    File.WriteAllBytes(Application.persistentDataPath + "/" + loadName, www.bytes);
 
                if (File.Exists(Application.persistentDataPath + "/" + "Erjun_Demo.xml") && File.Exists(Application.persistentDataPath + "/" + "Erjun_Demo.dat")) {
                    LoadAR();
                }
               
                }
            }
   }

    //加载AR
    public void LoadAR() {
      
         
        ////加载识别图地址
        //switch (Application.platform)
        //{
        //    case RuntimePlatform.Android:
        //        LoadDataSet(Application.streamingAssetsPath + "/" + "Erjun_Demo.xml");
        //        break;
        //    case RuntimePlatform.IPhonePlayer:
        //        LoadDataSet(Application.streamingAssetsPath + "/" + "Erjun_Demo.xml");
        //        break;
        //    default:
        //        LoadDataSet( Application.streamingAssetsPath + "/" + "Erjun_Demo.xml");
        //        break;
        //}
        LoadDataSet(Application.persistentDataPath + "/" + "Erjun_Demo.xml");
    }
    public  void LoadDataSet(string dataSetName)
    {
        try
        {
    
            ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            DataSet dataSet = objectTracker.CreateDataSet();
            if (dataSet.Load(dataSetName, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
            {
             
                objectTracker.Stop();//这里必须要停止跟踪才能激活DataSet
                objectTracker.ActivateDataSet(dataSet);
                objectTracker.Start();
                ConfigTrackable();
            }
            else {
                Debug.LogError("没有加载到资源");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("错误:" + e.Message);
           // GameController.GetInstance().UseTip("错误:" + e.Message, 0);
        }
    }

    public  void  ConfigTrackable()
    {
        IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
        foreach (TrackableBehaviour tb in tbs)
        {
            tb.gameObject.AddComponent<TurnOffBehaviour>();
            ScrawlTrackableEventHandler scraw= tb.gameObject.AddComponent<ScrawlTrackableEventHandler>();

            switch (tb.TrackableName)
            {
                case "xionglu":
                    scraw.AnimalPath = "xionglu";
                    break;
                case "shizi":
                    scraw.AnimalPath = "shizi";
                    break;
                case "ma":
                    scraw.AnimalPath = "ma";
                    break;
                case "lingyang":
                    scraw.AnimalPath = "lingyang";
                    break;
                case "laohu":
                    scraw.AnimalPath = "laohu";
                    break;
                case "lang":
                    scraw.AnimalPath = "lang";
                    break;
                case "huli":
                    scraw.AnimalPath = "huli";
                    break;
                case "baozi":
                    scraw.AnimalPath = "baozi";
                    break;
                case "banma":
                    scraw.AnimalPath = "banma";
                    break;
                case "daxiang":
                    scraw.AnimalPath = "daxiang";
                    break;
                case "608345529186171397":
                    scraw.AnimalPath = "daxiang";
                    break;
            }
        }

    }
}
