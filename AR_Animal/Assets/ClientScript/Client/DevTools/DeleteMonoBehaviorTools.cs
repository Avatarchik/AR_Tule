using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeleteMonoBehaviorTools : MonoBehaviour
{


    public bool bExecute = false;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame

    [ExecuteInEditMode]
    void Update () {

        if (bExecute == true)
        {

            LODGroup[] objs = FindObjectsOfType<LODGroup>();

            foreach (LODGroup obj in objs)
            {
                GameObject.DestroyImmediate(obj);

            }

            bExecute = false;
        }
        
	}
}
