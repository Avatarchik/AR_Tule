using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController :MonoBehaviour {
    private static UIController _intance;
    public  static UIController GetInatance() {
            return _intance;
    }
    public Toggle toggel;
    public ScrawlMeshBehaviour scraw;

    // Use this for initialization
    void Start () {
        _intance = this;
     //   toggel.onValueChanged.AddListener(IsEveryTimeUpdate);
    }
    //public void IsEveryTimeUpdate(bool isEveryTimeUpdate)
    //{
    //    if (scraw != null) {
    //      scraw._Realtime = isEveryTimeUpdate;
    //    }
    //}
}
