/********************************************************************
	created:	2015/05/29
	created:	29:5:2015   11:28
	filename: 	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem\TextEffectMgr.cs
	file path:	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem
	file base:	TextEffectMgr
	file ext:	cs
	author:		michael
	
	purpose:	text effect mananger
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextEffectMgr : BaseEffectMgr
{
   
    
    public TextEffectMgr()
    {
        
    }
    

    public GameObject Play(string text, string style, Vector3 offset, Transform parent = null, GameObject target = null, string methodname = null, float time = 0)
    {
        GameObject obj = ReadyPlay(style, offset, parent, target, methodname,time);
        if (obj != null)
        {
            //EffectManager effect = SetText(text, obj);
            //if (effect != null)
            //{
            //    effect.PlayAnimation();
            //}


            //iTweenEvent[] ites = obj.GetComponents<iTweenEvent>();
            //int i = 0;

            ////把foreach改为for
            ////foreach (iTweenEvent ite in ites)
            //for (i = 0; i < ites.Length; ++i )
            //{
            //    //ite.Play();
            //    ites[i].Play();
            //}
            Animator ani = obj.GetComponentInChildren<Animator>();
            if (ani)
            {
                ani.SetTrigger("Play");
            }

         
            SetText(text, obj);
        }
        return obj;
    }

    TextMesh SetText(string text, GameObject obj)
    {
        //EffectManager effect = obj.GetComponent<EffectManager>();
        //if (effect != null)
        //{
        //    effect.SetText(text);
        //}

        TextMesh tm = obj.GetComponent<TextMesh>();
        if (tm != null)
        {
            //Debug.Log(text);
            tm.text = text;
        }

      
        return tm;
    }

    protected override bool IsPlayOver(GameObject obj,CallBackData cbd)
    {
        if (base.IsPlayOver(obj, cbd) == true)
        {
            return true;
        }

        /*
        iTween[] tween = obj.GetComponents<iTween>();
        if (tween.Length > 0)
        {
            return false;
        }

        iTween[] tweenchildren = obj.GetComponentsInChildren<iTween>();

        if (tweenchildren.Length > 0)
        {
            return false;
        }*/
		if(obj == null)
		{
			Debug.LogError("IsPlayOver obj is null!");
			return true;
		}

        Animator ani = obj.GetComponentInChildren<Animator>();
        if (ani)
        {
            
            AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Play") || ani.IsInTransition(0))
            {
                return false;
            }
            else if (state.IsName("Idle"))
            {
                return true;
            }
        }
      
        
        return true;
    }

}
