using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class ScrawlMeshBehaviour : MonoBehaviour
{
    public bool _Realtime ;
    RenderTextureCamera _RegionRenderTexture;
    Texture2D _CurrentTexture;
    

    public List<int> _NeedScrawlMaterialIndex = new List<int>();
    private void Start()
    {
        _Realtime = true;
      //  UIController.GetInatance().toggel.isOn = false;
    }
    void OnDestroy()
    {
        if (_CurrentTexture != null)
        {
            GameObject.DestroyImmediate(_CurrentTexture, true);
        }
    }

    void Update()
    {

        RenderTextureCamera rrt = RenderTextureCamera.Instance;
        if (rrt != null &&
            _CurrentTexture != null &&
             rrt.GetRenderTexture() )
            
        {
            if(_CurrentTexture.width != rrt.GetRenderTexture().width ||
            _CurrentTexture.height != rrt.GetRenderTexture().height)
            {
                CopyTextureFromOutputTexture();
                Scrawl();

            }
        }

        if (_Realtime == true)
        {
            CopyTextureFromOutputTexture();
        }

    }

    Texture2D CopyTextureFromOutputTexture()
    {
        RenderTextureCamera rrt = RenderTextureCamera.Instance;
       
        if (rrt != null && rrt.GetRenderTexture() != null)
        {
            if (_CurrentTexture == null)
            {
                _CurrentTexture = new Texture2D(rrt.GetRenderTexture().width, rrt.GetRenderTexture().height);
            }
            else if (_CurrentTexture.width != rrt.GetRenderTexture().width ||
                _CurrentTexture.height != rrt.GetRenderTexture().height)
            {

                GameObject.DestroyImmediate(_CurrentTexture, true);

                _CurrentTexture = new Texture2D(rrt.GetRenderTexture().width, rrt.GetRenderTexture().height);
            }

            RenderTexture.active = rrt.GetRenderTexture();
            _CurrentTexture.ReadPixels(new Rect(0, 0, rrt.GetRenderTexture().width, rrt.GetRenderTexture().height), 0, 0);
            _CurrentTexture.Apply();
            RenderTexture.active = null;

            return _CurrentTexture;
        }
        return null;
    }
    public void Scrawl()
    {
        StartCoroutine(Scrawling());
    }
    IEnumerator Scrawling()
    {
        Region_Capture rc = RenderTextureCamera.Instance.gameObject.GetComponent<Region_Capture>();
        if (rc != null)
        {
            ImageTargetBehaviour itb = gameObject.GetComponentInParent<ImageTargetBehaviour>();
            if (itb != null)
            {
                rc.StartInitialize(itb.gameObject);
                RenderTextureCamera.Instance.RecalculateTextureSize();
            }
        }


        while(!RenderTextureCamera.Instance.GetRenderTexture())
        {
            yield return 0;
        }


        Texture2D tex2d = CopyTextureFromOutputTexture();
        if (tex2d == null)
        {
            yield break;
        }
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {


            if (_NeedScrawlMaterialIndex.Count == 0)
            {
                r.material.mainTexture = null;
                r.material.SetTexture("_DetailAlbedoMap", tex2d);
                //r.material.SetInt("_UVSec", 1);

            }
            else
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
                    if (_NeedScrawlMaterialIndex.Contains(i))
                    {
                        Material mat = r.materials[i];
                        mat.mainTexture = null;
                        mat.SetTexture("_DetailAlbedoMap", null);
                        //r.material.SetInt("_UVSec", 1);
                    }

                }
            }
        }
    }
  
    public void OnFound()
    {
        UIController uIController = UIController.GetInatance();
        uIController.scraw = this;
        Scrawl();
    }

    public void OnLost()
    {

    }
    public void OnTracking()
    {

    }
}