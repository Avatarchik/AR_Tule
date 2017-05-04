/********************************************************************
	created:	2015/05/29
	created:	29:5:2015   10:37
	filename: 	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem\BHSpecialEffect.cs
	file path:	D:\AllStar_Proj\trunk\Art\Assets\ClientScript\Client\EffectSystem
	file base:	BHSpecialEffect
	file ext:	cs
	author:		michael
	
	purpose:	粒子特效集合类
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSpecialEffect : MonoBehaviour
{
    [HideInInspector]
    public ParticleEffectMgr.PlayEffectData mData;
    ISMStateMachine<ParticleSpecialEffect> mISM;
    public bool IsLoop = false;
    public float LastTime = 3.0f;
    public float ParticleScale = 1.0f;
    public delegate void PSEDelegate(ParticleSpecialEffect entity);


    [HideInInspector]
    public PSEDelegate _OnStartCallback;
    [HideInInspector]
    public PSEDelegate _OnEndCallback;

    [HideInInspector]
    public ParticleSystem[] Pss;

    public EllipsoidParticleEmitter[] EPss;

    void MakeISM()
    {
        mISM = new ISMStateMachine<ParticleSpecialEffect>(this);

        mISM.CreateAndAdd<PSEInitState>("Init", this);
        mISM.CreateAndAdd<PSEStoppedState>("Stopped", this);
        mISM.CreateAndAdd<PSEPausedState>("Paused", this);
        mISM.CreateAndAdd<PSELoopingState>("Looping", this);
        mISM.CreateAndAdd<PSEPlayingState>("Playing", this);
        //don't push Any state init state willbe push while first DoPlay Called
    }
    void Awake()
    {

        MakeISM();
    }
   
    protected void Start()
    {


        Pss = GetComponentsInChildren<ParticleSystem>(true);
        if (Pss == null)
        {
            Pss = new ParticleSystem[1];
            Pss[0] = new ParticleSystem();
            Pss[0].gameObject.transform.parent = transform;
        }

        EPss = GetComponentsInChildren<EllipsoidParticleEmitter>(true);
       

    }
    void Update()
    {

        if (mISM != null)
        {
            mISM.Update();
        }
    }
    public void SetScale(float scale)
    {
        ParticleScale = scale;

        
        Pss = GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem system in Pss)
        {
            system.startSpeed *= ParticleScale;
            system.startSize *= ParticleScale;
            system.gravityModifier *= ParticleScale;
            //system.transform.localScale *= ParticleScale;
        }



        //get all emitters we need to do scaling on
        ParticleEmitter[] emitters = GetComponentsInChildren<ParticleEmitter>();

        //get all animators we need to do scaling on
        ParticleAnimator[] animators = GetComponentsInChildren<ParticleAnimator>();

        //apply scaling to emitters
        if (emitters != null)
        {
            foreach (ParticleEmitter emitter in emitters)
            {
                emitter.minSize *= scale;
                emitter.maxSize *= scale;
                emitter.worldVelocity *= scale;
                emitter.localVelocity *= scale;
                emitter.rndVelocity *= scale;
            }
        }
        if (animators != null)
        {
            //apply scaling to animators
            foreach (ParticleAnimator animator in animators)
            {
                animator.force *= scale;
                animator.rndForce *= scale;
            }
        }

        
    }
    public void DoPlay(bool loop)
    {
        IsLoop = loop;
        if (mISM != null)
        {
            mISM.Push("Init");
        }
    }

    public void Stop(bool immediate = false)
    {
        if (mISM == null)
        {
            return;
        }
        if (mISM.ContainsState("Playing"))
        {
            if (immediate)
            {
                mISM.Push("Stopped");
            }
        }
        else if (mISM.ContainsState("Paused"))
        {
            if (immediate)
            {
                mISM.Push("Stopped");
            }
            else
            {
                Resume();
            }
            
        }
        else if (mISM.ContainsState("Looping"))
        {
            if (immediate)
            {
                mISM.Push("Stopped");
            }
            else
            {
                mISM.Push("Playing");
            }
        }
        else if (mISM.ContainsState("Init"))
        {
            mISM.Push("Stopped");
        }
    }

    public void Pause()
    {
        if (mISM == null)
        {
            return;
        }
        PSEPausedState state = mISM.GetFromDic<PSEPausedState>("Paused");
        if (mISM.ContainsState("Playing"))
        {
            state.IsFromLoop = false;
            mISM.Push(state);
        }
        else if(mISM.ContainsState("Looping"))
        {
            state.IsFromLoop = true;
            mISM.Push(state);
        }

    }

    public void Resume()
    {
        if (mISM == null)
        {
            return;
        }
        if (mISM.ContainsState("Paused"))
        {
            PSEPausedState state = mISM.GetState<PSEPausedState>("Paused");
            if (state.IsFromLoop)
            {
                mISM.Push("Looping");
            }
            else
            {
                mISM.Push("Playing");
            }
        }
      
    }
    public void DoStartCallback()
    {
        if (_OnStartCallback != null)
        {
            _OnStartCallback(this);
        }
    }
    public void DoEndCallback()
    {
        if (_OnEndCallback != null)
        {
            _OnEndCallback(this);
        }
    }
    public void SetStartCallback(PSEDelegate cb)
    {
        _OnStartCallback = cb;
    }
    public void SetEndCallback(PSEDelegate cb)
    {
        _OnEndCallback = cb;
    }


#region Unit Operation
    public void SetLayerRecursive(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; i++)
        {
            Transform ct = t.GetChild(i);
            SetLayerRecursive(ct, layer);
        }
    }
    void ClearAllChildren(GameObject go)
    {
        List<Transform> rlist = new List<Transform>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            rlist.Add(go.transform.GetChild(i));
        }
        foreach (Transform t in rlist)
        {
            DestroyImmediate(t.gameObject);
        }
    }
#endregion

    void OnDestroy()
    {
        mISM = null;
        if (ParticleEffectMgr.Instance != null)
        {
            ParticleEffectMgr.Instance.StopInEffect(mData.ID);
        }
    }
}



#region Scale Helper
public static class PSEScaler
{
    public static void Scale(ParticleSpecialEffect ef ,float scaleFactor)
    {
        //scale shuriken particle systems
        ScaleShurikenSystems(ef , scaleFactor);

        //scale trail renders
        ScaleTrailRenderers(ef,scaleFactor);


    }

    static void ScaleShurikenSystems(ParticleSpecialEffect effect, float scaleFactor)
    {
        foreach (ParticleSystem system in effect.Pss)
        {
            system.startSpeed *= scaleFactor;
            system.startSize *= scaleFactor;
            system.gravityModifier *= scaleFactor;
            system.transform.localScale *= scaleFactor;
        }
    }

    static void ScaleTrailRenderers(ParticleSpecialEffect effect, float scaleFactor)
    {
        //get all animators we need to do scaling on
        TrailRenderer[] trails = effect.gameObject.GetComponentsInChildren<TrailRenderer>();

        //apply scaling to animators
        foreach (TrailRenderer trail in trails)
        {
            trail.startWidth *= scaleFactor;
            trail.endWidth *= scaleFactor;
        }
    }
}

#endregion