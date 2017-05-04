using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ShadowTools : MonoBehaviour {

    Renderer[] renders;

    public ShadowCastingMode castShadow = UnityEngine.Rendering.ShadowCastingMode.Off;
    public bool receiveShadow = false;

	// Use this for initialization
	void Start () {
        renders = GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR	
        foreach (Renderer r in renders)
        {
            r.shadowCastingMode = castShadow;
            r.receiveShadows = receiveShadow;
        }
#endif
    }
}
