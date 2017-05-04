using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenameTools : MonoBehaviour {

    public string SrcString;
    public string DestString;

    public bool bExecute = false;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame

    [ExecuteInEditMode]
    void Update () {

        if (bExecute == true)
        {

            Transform[] tarr = gameObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in tarr)
            {
                string name = t.gameObject.name;
               
                if (name.Contains(SrcString))
                {
                    name = name.Replace(SrcString, DestString);

                    t.gameObject.name = name;

                }

            }

            bExecute = false;
        }
        
	}
}
