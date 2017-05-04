/********************************************************************
	created:	2015/05/29
	created:	29:5:2015   11:26
	filename: 	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem\BaseEffectMgr.cs
	file path:	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem
	file base:	BaseEffectMgr
	file ext:	cs
	author:		michael
	
	purpose:	效果管理基类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseEffectMgr
{
    protected class CallBackData
    {
        public bool firstframe = true;
        public GameObject target;
        public string methodname;
        public GameObject param;
        public float time;
        public float currenttime;
    }

    protected string mResourcePath = "";

    public int MaxLimit = 20;
    public List<GameObject> ObjectQueue = new List<GameObject>();

    public void AddObjectToQueue(GameObject obj)
    {
        if (ObjectQueue.Count >= MaxLimit)
        {
            GameObject first = ObjectQueue[0];
            Remove(first);
            ObjectQueue.Add(obj);
        }
        else
        {
            ObjectQueue.Add(obj);
        }
    }

    public void ClearPool()
    {
    }

    Dictionary<GameObject, CallBackData> mObjects = new Dictionary<GameObject, CallBackData>();
    public void Reset()
    {
        foreach (GameObject mobject in mObjects.Keys)
        {
            if (mobject != null)
              mobject.SetActive(false);
        }
        mObjects.Clear();
        ClearPool();
    }
    protected GameObject GetStyle(string style,Vector3 offset,Transform parent)
    {
        GameObject model = Resources.Load(mResourcePath + style) as GameObject;
        offset.z = -5;
        GameObject gb = null;
       

        return gb;

    }
    protected GameObject ReadyPlay(string style, Vector3 offset, Transform parent, GameObject target, string methodname, float time)
    {
        GameObject obj = GetStyle(style,offset,parent);
        if (obj == null)
        {
            return null;
        }
       

        CallBackData cbd = new CallBackData();
        cbd.methodname = methodname;
        cbd.target = target;
        cbd.param = obj;
        cbd.time = time;


        mObjects.Add(obj, cbd);
        AddObjectToQueue(obj);

        return obj;
    }

    protected virtual bool IsPlayOver(GameObject obj, CallBackData cbd)
    {
        if (cbd != null)
        {
            if (cbd.currenttime > cbd.time && cbd.time > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void OnDestroyParentObject(GameObject obj)
    {
        if (mObjects.ContainsKey(obj))
        {
            mObjects.Remove(obj);
        }
    }
    public void Remove(GameObject obj)
    {
        if (mObjects.ContainsKey(obj) == true)
        {
            obj.SetActive(false);
            mObjects.Remove(obj);

			
        }
        if (ObjectQueue.Contains(obj) == true)
        {
            ObjectQueue.Remove(obj);
        }
    }

    List<GameObject> removelist = new List<GameObject>();
    public virtual void Update()
    {
        removelist.Clear();
        foreach (KeyValuePair<GameObject, CallBackData> pair in mObjects)
        {
            GameObject obj = pair.Key;
            CallBackData cbd = pair.Value;
            cbd.currenttime += Time.deltaTime;
        
            if (cbd.firstframe == true)
            {
                cbd.firstframe = false;
                continue;
            }
           
            if (IsPlayOver(obj,cbd) == true)
            {
                removelist.Add(obj);
            }
        }
        foreach (GameObject obj in removelist)
        {
           
            if (mObjects.ContainsKey(obj))
            {
                CallBackData cbd = mObjects[obj];
                if (cbd.target != null)
                {
                    cbd.target.SendMessage(cbd.methodname, cbd.param, SendMessageOptions.DontRequireReceiver);

                }

            }

			Remove(obj);
        }
    }


}
