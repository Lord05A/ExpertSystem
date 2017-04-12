using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SquareCreator : MonoBehaviour {

    public Text sizeX;
    public Text sizeY;
    public Text sizeTurn;

    public GameObject prefab_obj;

    private bool ready_for_adding = false;
    static public List<GameObject> pool = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (ready_for_adding)
        {
            Vector3 scale = Vector3.zero;
            scale.z = 1; 
            if (!string.IsNullOrEmpty(sizeX.text))
                scale.x = float.Parse(sizeX.text);
            else
                scale.x = 1;
            if (!string.IsNullOrEmpty(sizeY.text))
                scale.y = float.Parse(sizeY.text);
            else
                scale.y = 1;
            prefab_obj.transform.localScale = scale * 10;

            float turn = 0;
            if (!string.IsNullOrEmpty(sizeTurn.text))
                turn = float.Parse(sizeTurn.text);            

            if (Input.GetMouseButtonDown(0))
            {
                float mousex = (Input.mousePosition.x);
                float mousey = (Input.mousePosition.y);
                Debug.Log(mousex);
                Debug.Log(mousey);
                Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
                mouseposition.z = 1;
                Debug.Log(mouseposition);
                GameObject new_obj = (GameObject)Instantiate(prefab_obj, mouseposition, new Quaternion());
                pool.Add(new_obj);
                new_obj.transform.Rotate(new Vector3(0, 0, 1), -turn);
                ready_for_adding = !ready_for_adding;
            }
        }
    }

    public void ClickOn()
    {
        ready_for_adding = !ready_for_adding;
    }
}
