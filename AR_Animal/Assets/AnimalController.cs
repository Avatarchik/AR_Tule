using UnityEngine;
using System.Collections;

public class AnimalController : MonoBehaviour {

    private bool flag = false;
   // public GameObject exitPanel;
    // Use this for initialization
    void Start () {
        CurrentAnim = "Act";
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (!flag)
            //{
            //    exitPanel.SetActive(true);
            //    flag = true;
            //}
            //else
            //{
            //    exitPanel.SetActive(false);
            //    flag = false;
            //}
            Application.Quit();
        }
    }
    private Animator anim;
    private string CurrentAnim;

    
    public void OnTap(TapGesture gesture)
    {
        Debug.Log("OErjunTap");
        if (anim != null)
        {
            //获取动画层 0 指Base Layer.
            AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
            //如果正在播放walk动画.
            if (stateinfo.IsName(CurrentAnim))
            {
                return;
            }
        }
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<Animator>()) {
                this.anim = hit.transform.GetComponent<Animator>();
            }
            anim.Play(CurrentAnim);
            AudioSorceController.GetIntance().PlayAudio(hit.transform.name);

        }
    }
}
