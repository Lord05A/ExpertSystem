using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour {

    public int boost = 1;

    public void ClickOn()
    {
        Skynet.set_boost(boost);
    }
	
}
