/********************************************************************
	created:	2015/05/29
	created:	29:5:2015   10:45
	filename: 	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem\BHEffectMgr.cs
	file path:	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem
	file base:	BHEffectMgr
	file ext:	cs
	author:		michael
	
	purpose:	特效粒子管理器
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ParticleEffectMgr : Singleton<ParticleEffectMgr>
{
    #region Members
    public int MaxSameEffectCount = 8;
    Dictionary<long, ParticleSpecialEffect> EffectPlayingList = new Dictionary<long,ParticleSpecialEffect>();

    #endregion


    public class PlayEffectData
    {
        public bool IsLoop = false;
        public Vector3 Position = Vector3.zero;
        public Transform ParentTranform = null;
        public string LayerName = "Default";
        public float ParticleScale = 1.0f;
        public long ID;

        public PlayEffectData(long id,float scale,Vector3 v, Transform t,bool loop = false,string layer = "Default")
        {
            Position = v;
            ParentTranform = t;
            IsLoop = loop;
            LayerName = layer;
            ParticleScale = scale;
            ID = id;
        }
    }

    void OnDestroy()
    {
        foreach (KeyValuePair<long,ParticleSpecialEffect> kv in EffectPlayingList)
        {
            GameObject.DestroyImmediate(kv.Value.gameObject);
        }
        EffectPlayingList.Clear();
    }


    public void Stop(ParticleSpecialEffect effect, bool immediate = false)
    {  
        if (effect)
        {
            effect.Stop(immediate);
        }
    }

    public void StopInEffect(long eid)
    {
        if (EffectPlayingList.ContainsKey(eid))
        {
            if (EffectPlayingList[eid] != null &&
                EffectPlayingList[eid].gameObject != null)
            {
                GameObject.Destroy(EffectPlayingList[eid].gameObject);

            }
            EffectPlayingList.Remove(eid);
        }
    }


    public long StartPlayEffect(string bundle, string asset, Vector3 pos, Transform parent = null, float scale = 1.0f, bool loop = false, string layer = "Default")
    {
        long id = DateTime.Now.Ticks;
        while (EffectPlayingList.ContainsKey(id))
        {
            id++;
        }
        EffectPlayingList[id] = null;

        PlayEffectData data = new PlayEffectData(id,scale, pos, parent, loop, layer);
        BaseAssetLoader.Instance.StartLoadAsset(bundle, asset, OnLoadedEffect, data,false);

        return id;
    }
    public void OnLoadedEffect(object objloaded , object objparam)
    {
        PlayEffectData data = (PlayEffectData)objparam;
        GameObject prefab = objloaded as GameObject;

        Vector3 pos = data.Position;
        Quaternion rot = Quaternion.identity;

        if (data.ParentTranform != null)
        {
            pos = data.ParentTranform.position + pos;
            rot = data.ParentTranform.rotation;
        }

        GameObject effectgo = GameObject.Instantiate(prefab,pos,rot) as GameObject;
        if (effectgo != null)
        {
            ParticleSpecialEffect pse = effectgo.GetComponent<ParticleSpecialEffect>();
            if (pse)
            {
                pse.gameObject.SetActive(true);
                pse.mData = data;
                pse.transform.parent = data.ParentTranform;
                pse.SetLayerRecursive(pse.transform, LayerMask.NameToLayer(data.LayerName));
                pse.SetScale(data.ParticleScale);
                pse.DoPlay(data.IsLoop);

                EffectPlayingList[data.ID] = pse;
            }
            else
            {
                Debug.LogError("No ParticleSpecialEffect Component in Prefab!Do you Forget Add it?");
            }
        }
    }
}
