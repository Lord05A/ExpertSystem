using UnityEngine;
using System.Collections;

public class addtext : MonoBehaviour {

    public GUIText t;
    public double r;

    public addtext(Canvas c)
    {
        t = c.GetComponent<GUIText>();
    }

    public void foo(double e)
    {
       // GUIText t = text.GetComponent<GUIText>();
        t.text = "Middle sensor = " + e;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //GUIText t = text.GetComponent<GUIText>();
        t.text = "Middle sensor = " + r;
	}
}
