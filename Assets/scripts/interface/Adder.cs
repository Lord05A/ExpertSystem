using UnityEngine;
using System.Collections;

public class Adder : MonoBehaviour {

    private bool ready_for_adding = false;
    public GameObject prefab_obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ready_for_adding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                float mousex = (Input.mousePosition.x);
                float mousey = (Input.mousePosition.y);
                Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
                mouseposition.z = 1;
                Debug.Log(mouseposition);
                Instantiate(prefab_obj, mouseposition, new Quaternion());
                ready_for_adding = !ready_for_adding;
            }
        }
	}

    public void ClickOn()
    {
        ready_for_adding = !ready_for_adding;
    }
}
