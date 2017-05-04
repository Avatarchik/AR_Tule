


using UnityEngine;
public class PSEInitState : ISMState<ParticleSpecialEffect>
{
    public PSEInitState(string name, ParticleSpecialEffect entity, ISMStateMachine<ParticleSpecialEffect> parentISM, int priority)
        : base(name, entity, parentISM, priority, "Exclusive")
    {
    }


    public override bool Enter()
    {
         return base.Enter();
    }

    public override void Execute()
    {
        if (mEntity.IsLoop)
        {
            mParentISM.Push("Looping");
        }
        else
        {
            mParentISM.Push("Playing");
        }
        base.Execute();
    }
    public override void Exit()
    {
        mEntity.DoStartCallback();
        base.Exit();
    }

}

public class PSEStoppedState : ISMState<ParticleSpecialEffect>
{
    bool bHasStopInMgr = false;
    public PSEStoppedState(string name, ParticleSpecialEffect entity, ISMStateMachine<ParticleSpecialEffect> parentISM, int priority)
        : base(name, entity,parentISM,priority,"Exclusive")
    {
    }
    public override bool Enter()
    {
        bHasStopInMgr = false;

        GameObject go = mEntity.gameObject;
        if (go != null)
        {
            
            mEntity.DoEndCallback();
            ParticleSystem[] pss = go.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in pss)
            {
                p.Stop();
                p.time = 0;
            }

        }

        return base.Enter();
    }

    public override void Execute()
    {
        if (bHasStopInMgr == false)
        {
            foreach (ParticleSystem p in mEntity.Pss)
            {
                if (null == p)
                {
                    continue;
                }
                p.GetComponent<Renderer>().enabled = false;
            }
            ParticleEffectMgr.Instance.StopInEffect(mEntity.mData.ID);
            bHasStopInMgr = true;

        }
        base.Execute();
    }
}

public class PSEPausedState : ISMState<ParticleSpecialEffect>
{
    public bool IsFromLoop = false;
    public PSEPausedState(string name, ParticleSpecialEffect entity, ISMStateMachine<ParticleSpecialEffect> parentISM, int priority)
        : base(name, entity, parentISM, priority, "Exclusive")
    {
    }

    public override bool Enter()
    {
         foreach (ParticleSystem p in mEntity.Pss)
         {
             if (p.isPlaying)
             {
                 p.Pause();
             }
         }
         return base.Enter();
    }


    public override void Exit()
    {
         foreach (ParticleSystem p in mEntity.Pss)
         {
             if (p.isPaused)
             {
                 p.Play();
             }
         }
        base.Exit();
    }
}


public class PSEBasePlayState : ISMState<ParticleSpecialEffect>
{
    public PSEBasePlayState(string name, ParticleSpecialEffect entity, ISMStateMachine<ParticleSpecialEffect> parentISM, int priority)
            : base(name, entity,parentISM,priority,"Exclusive")
        {
        }
    #region Play Function


    protected void DoPlayOnGameOjbect(GameObject go,bool loop)//return if need to recursive
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.GetComponent<Renderer>().enabled = true;
            ps.loop = loop;
            ps.Play();
        }


    }

    protected void RecursiveChildren(GameObject go,bool loop)
    {

        DoPlayOnGameOjbect(go, loop);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform t = go.transform.GetChild(i);
            RecursiveChildren(t.gameObject, loop);
        }
    }

    protected void Play(bool loop)
    {
        RecursiveChildren(mEntity.gameObject,loop);
    }

    protected override bool IsOver()
    {
        return base.IsOver();
    }
    
    #endregion
}
public class PSEPlayingState : PSEBasePlayState
{
    public PSEPlayingState(string name, ParticleSpecialEffect entity, ISMStateMachine<ParticleSpecialEffect> parentISM, int priority)
            : base(name, entity,parentISM,priority)
        {
        }


    public override bool Enter()
    {
        Play(false);
        return base.Enter();
    }
    

    protected override bool IsOver()
    {
        bool bIsOver = true;

        foreach (ParticleSystem ps in mEntity.Pss)
        {
            if (null == ps)
            {
                continue;
            }
            if (ps.enableEmission == false)
            {
                continue;
            }


            if (ps.isPlaying == true && ps.GetComponent<Renderer>().enabled == true)
            {
                bIsOver = false;
                break;
            }

            if (ps.particleCount > 0)
            {
                bIsOver = false;
                break;
            }

        }
        if (mEntity.EPss != null)
        {

            if (mEntity.GetComponentsInChildren<EllipsoidParticleEmitter>().Length > 0)
           {
               bIsOver = false;
           }
        }
      
        if (bIsOver)
        {
            mParentISM.Push("Stopped");
        }
        return bIsOver;
    }

}
public class PSELoopingState : PSEBasePlayState
{
    CrudeElapsedTimer timer;
    public PSELoopingState(string name, ParticleSpecialEffect entity, ISMStateMachine<ParticleSpecialEffect> parentISM, int priority)
            : base(name, entity,parentISM,priority)
        {
        }


    public override bool Enter()
    {
        timer = new CrudeElapsedTimer(mEntity.LastTime);
        Play(true);
        return base.Enter();
    }

    public override void Execute()
    {
        timer.Advance(Time.deltaTime);
        base.Execute();
    }

    protected override bool IsOver()
    {
        if (timer.TimeOutCount > 0)
        {
            return true;
        }
        return false;
    }

}

