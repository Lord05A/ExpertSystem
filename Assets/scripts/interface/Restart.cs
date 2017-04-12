using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {
        
    public void Reload()
    {        
        Application.LoadLevel(Application.loadedLevelName);
    }
}
