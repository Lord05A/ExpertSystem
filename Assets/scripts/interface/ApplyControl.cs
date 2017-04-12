using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ApplyControl : MonoBehaviour {

    public Text inputSpeed;
    public Text inputSensor;
    public Text inputDirection;
    public Skynet skynet;

    public void ClickOn()
    {
        change_speed(inputSpeed.text);
        change_sensor(inputSensor.text);
        change_direction(inputDirection.text);
    }

    private void change_speed(string new_speed)
    {
        if (!string.IsNullOrEmpty(new_speed))
            skynet.change_speed(float.Parse(new_speed));
    }
    
    private void change_sensor(string new_sensor) 
    {
        if (!string.IsNullOrEmpty(new_sensor))
            skynet.change_sensor(float.Parse(new_sensor));
    }

    private void change_direction(string new_direction)
    {
        if (!string.IsNullOrEmpty(new_direction))
            skynet.change_direction(float.Parse(new_direction));
    }

}
