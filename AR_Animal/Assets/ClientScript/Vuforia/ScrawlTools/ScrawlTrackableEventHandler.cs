using UnityEngine;
using System.Collections;
using Vuforia;

public class ScrawlTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{

    #region PRIVATE_MEMBER_VARIABLES
    public string AnimalPath;
    private TrackableBehaviour mTrackableBehaviour;
    private ScrawlMeshBehaviour mCharacterBehaviour;
    private ImageTarget mImageTarget;

    bool mRefound = false;

    TrackableBehaviour.Status _PreStatus = TrackableBehaviour.Status.UNKNOWN;
    TrackableBehaviour.Status _NewStatus = TrackableBehaviour.Status.UNKNOWN;
    

    #endregion // PRIVATE_MEMBER_VARIABLES



    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        //mCharacterBehaviour = GetComponentInChildren<ScrawlMeshBehaviour>();

        mImageTarget = mTrackableBehaviour.Trackable as ImageTarget;
        
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    void Update()
    {
        if (mRefound == true)
        {
            if (_NewStatus == TrackableBehaviour.Status.TRACKED)
            {
                OnTrackingFound();
                mRefound = false;
            }
        }

    }

    #region PUBLIC_METHODS

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format("Tracker:{0} pre:{1} new:{2}",gameObject.name,previousStatus,newStatus));
#endif
        _PreStatus = previousStatus;
        _NewStatus = newStatus;
      
        if (previousStatus == TrackableBehaviour.Status.NOT_FOUND && newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.NOT_FOUND && newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
           
            OnTrackingFound();
                    
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED  && newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            OnTrackingLost();

        }
        else if (previousStatus == TrackableBehaviour.Status.EXTENDED_TRACKED  && newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            OnTrackingLost();

        }

    }

    public void SetRefound()
    {
        mRefound = true;
    }

    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS

    private GameObject anima;
    private void OnTrackingFound()
    {
        anima=Instantiate(Resources.Load(AnimalPath) as GameObject);
        anima.transform.parent = this.gameObject.transform;

        mCharacterBehaviour = anima.GetComponentInChildren<ScrawlMeshBehaviour>();

        
        if (mCharacterBehaviour)
        {
            mCharacterBehaviour.OnFound();
        }
    }


    private void OnTrackingLost()
    {
       
       if(mCharacterBehaviour)
        {
            mCharacterBehaviour.OnLost();
        }
        Destroy(anima);
    }
    private void OnTracking()
    {

        if (mCharacterBehaviour)
        {
            mCharacterBehaviour.OnTracking();
        }
    }


    #endregion // PRIVATE_METHODS
    //private Animator anim;
    //private string CurrentAnim;
    //public void OnTap(TapGesture gesture)
    //{
    //    Debug.Log("OnErjunTap");
    //    if (anim !=null) {
    //        //获取动画层 0 指Base Layer.
    //        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
    //        //如果正在播放walk动画.
    //        if (stateinfo.IsName(CurrentAnim))
    //        {
    //            return;
    //        }
    //    }
         

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        switch (hit.transform.name)
    //        {
    //            case "daxiang":
    //                Animator anim1 = hit.transform.GetComponent<Animator>();
    //                this.anim = anim1;
    //                CurrentAnim = "Act";
    //                anim.Play(CurrentAnim);
    //                AudioSorceController.GetIntance().PlayAudio("daxiangact");
    //                break;

    //                case "baozi":
    //                Animator anim2 = hit.transform.GetComponent<Animator>();
    //                this.anim = anim2;
    //                CurrentAnim = "Act";
    //                anim.Play(CurrentAnim);
    //                break;
                    
    //        }
           
    //    }
    //}
}

