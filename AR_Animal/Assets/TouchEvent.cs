using UnityEngine;
using System.Collections;

public class TouchEvent : MonoBehaviour {

    private float speed = 0.02f;

    private Touch oldTouch1;
    private Touch oldTouch2;
  public  bool isUseRotate = true;
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
        _Rotate();
		}
   
    //public void StopAnim()
    //{
    //    GetComponent<Animator>().enabled = false;
    //} 
         
   
    void _Rotate()
    {

        if (Input.touchCount <= 0)
        {
            return;
        }

        //if (Input.GetMouseButton(0)) {
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        isUseRotate = false;
        //        if (hit.transform.gameObject.tag == "dengxiaoping" || hit.transform.gameObject.tag == "zhouenlai"|| hit.transform.gameObject.tag == "maozhedong")
        //        {
                  
        //            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //            {
        //                //移动
        //                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //              //  transform.Translate(-touchDeltaPosition.x * speed, touchDeltaPosition.y * speed, 0);
        //                transform.Rotate(new Vector3(0, 1, 0) * -touchDeltaPosition.x * 10, Space.Self);
        //            }
        //        } 
        //    }
        //    else
        //    {
        //        //是否开启旋转
        //        isUseRotate = true;
              
        //    }
        //}
        //if (isUseRotate) {
            //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            //{
            //    //旋转
            //    Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //  //  transform.Rotate(new Vector3(0, 1, 0) * -touchDeltaPosition.x * 10, Space.Self);
            //    transform.Translate(-touchDeltaPosition.x * speed, touchDeltaPosition.y * speed, 0);
            //}
        //}
      

    

        if (Input.touchCount == 2)
        {
           
            //多点触摸, 放大缩小  
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            //第2点刚开始接触屏幕, 只记录，不做处理  
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }

            if (newTouch1.phase == TouchPhase.Moved || newTouch2.phase == TouchPhase.Moved)
            {

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / 1000f;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor,
                                            localScale.y + scaleFactor,
                                            localScale.z + scaleFactor);

                //最小缩放到 0.3 倍  
                if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
                {
                    transform.localScale = scale;
                }

                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
        }
       
    }

}

   
