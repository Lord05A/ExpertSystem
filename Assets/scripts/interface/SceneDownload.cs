using UnityEngine;
using System.Collections;

public class SceneDownload : MonoBehaviour
{
    public string scene_name;

    public void ClickOn()
    {
        Application.LoadLevel(scene_name);
    }

}
