using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    public int boost = 1;
    public LineRenderer lr_front;
    
    void Start()
    {

    }

    public void set_boost()
    {
        Skynet.set_boost(boost);
    }

    public void draw_sensor_line(Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Vector3 pos5, Vector3 pos6)
    {
        pos1.z = 1;
        pos2.z = 1;
        pos3.z = 1;
        pos4.z = 1;
        pos5.z = 1;
        pos6.z = 1;
        lr_front.SetVertexCount(0);
        lr_front.SetVertexCount(6);
        lr_front.SetPosition(0, pos1);
        lr_front.SetPosition(1, pos2);
        lr_front.SetPosition(2, pos3);
        lr_front.SetPosition(3, pos4);
        lr_front.SetPosition(4, pos5);
        lr_front.SetPosition(5, pos6); 
    }

    

    
}
