using UnityEngine;
using System.Collections;

public class ZoomScript : MonoBehaviour
{
    public float maxSize = 5f;
    public float minSize = 0f;
    public float deltaSize;
    public float verticalSpeed = 40f;
    public float horizontalSpeed = 40f;

	void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            GetComponent<Camera>().orthographicSize -= deltaSize;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            GetComponent<Camera>().orthographicSize += deltaSize;
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, minSize, maxSize);

        
	}
}
