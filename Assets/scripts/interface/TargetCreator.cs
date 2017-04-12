using UnityEngine;
using System.Collections;

public class TargetCreator : MonoBehaviour {

    private bool ready_for_adding = false;
    //public GameObject target;
    public Skynet skynet;
    // Update is called once per frame
    void Update()
    {
        if (ready_for_adding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                float mousex = (Input.mousePosition.x);
                float mousey = (Input.mousePosition.y);
                Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
                mouseposition.z = 1;  
                skynet.set_new_target(mouseposition);
                ready_for_adding = !ready_for_adding;
            }
        }
    }

    public void ClickOn()
    {
        ready_for_adding = !ready_for_adding;
    }
}
