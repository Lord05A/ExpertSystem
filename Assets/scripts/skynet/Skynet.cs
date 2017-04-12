using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Skynet : MonoBehaviour {

    public InterfaceController I_C;

    public float SensorDist = 100f;
    public float SensorTurnAngle = 45;
    public GameObject sensor;
    public Canvas text;
    public GameObject target;
    private float full_turn_to_target = 0.0f;
    
    private float TurnAngleRadian;
    private Vector2 direction = new Vector2(0, 1);
    private float speed;

    public Rigidbody2D rgb;

    private Bridge bridge;

    private bool find_target = false;
    private bool find_target_turn = true;

    static private float boost = 1;

    int z = 0;

	// Use this for initialization
	void Start () {
        Time.timeScale = 0;
        TurnAngleRadian = SensorTurnAngle * Mathf.PI / 180;
        bridge = new Bridge(target.transform.position.x, target.transform.position.y, 800);
        bridge.setCoordinate(transform.position.x, transform.position.y);
        I_C = this.GetComponentInChildren<InterfaceController>();        
	}

    void FixedUpdate()
    {        
        
    }

	// Update is called once per frame
	void Update () {
        //test
        //draw_sensor();
        //test end
       // if (Time.timeScale != 0)
       // {
        speed = 0;
            if (find_target)
            {
                Point target = bridge.get_target();
                direction = new Vector2((float)target.x - transform.position.x,
                    (float)target.y - transform.position.y);
                float turn = 0;

                if (bridge != null)
                {
                    turn = (float)bridge.getAngle(new Point(direction.x, direction.y),
                        new Point(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180),
                    Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180))) * 0.3f;
                }
                find_target_turn = false;

                Debug.Log(direction);
                Debug.Log(turn);
                if (bridge != null)
                    speed = bridge.get_velocity() / 15f;
                transform.Rotate(new Vector3(0, 0, 1), -turn);
                rgb.velocity = direction * Time.deltaTime * speed * boost;
                find_target = false;
                //draw_sensor();
            }
            else
            {
                bridge.setCoordinate(transform.position.x, transform.position.y);
                set_direction();
                set_target_direction();

                Vector2 SensorLeft = calculate_sensor_left();
                Vector2 SensorRight = calculate_sensor_right();

                float dist1 = get_info_from_sensor(SensorLeft);
                float dist2 = get_info_from_sensor(transform.up);
                float dist3 = get_info_from_sensor(SensorRight);
              
                speed = (float)bridge.getVelocity(dist1, dist2, dist3);
                //if (float.IsNaN(speed) || float.IsInfinity(speed))
                //{
                //    speed = 1f;
                //}
                //if (dist2 != -1)
                //    Debug.Log(dist1 + ";" + dist2 + ";" + dist3);
                //z++;
                // if (z % 5 == 0)
                // {
                //   z = 0;
                float turn = (float)bridge.getAngle(dist1, dist2, dist3);
                //if (dist1 == -1 && dist2 == -1 && dist3 == -1)
                //   full_turn_to_target = turn;



                Vector2 v1 = Vector2.zero;
                Vector2 v2 = Vector2.zero;
                if ((dist1 == -1) && (dist2 == -1) && (dist3 == -1))
                {
                    if (turn != 0)
                    {
                        // Debug.Log(turn);
                        v1 = calculate_vector_turn(direction, turn);
                        v2 = calculate_vector_turn(direction, -turn);
                        //Debug.Log(v1 + " " + v2);

                        //turn = bridge.getAngle2(new Point((double)v1.x, (double)v1.y), bridge.get_target()) >
                        //bridge.getAngle2(new Point((double)v2.x, (double)v2.y), bridge.get_target()) ? turn : -turn;
                        //Debug.Log(bridge.getAngle(new Point((double)v1.x, (double)v1.y), bridge.get_target()) + " & " +
                        //  bridge.getAngle(new Point((double)v2.x, (double)v2.y), bridge.get_target()));            
                    }
                }

                // Debug.Log(turn);
                if (turn == float.NaN)
                    rgb.velocity = new Vector2(0, 0);
                if (!float.IsInfinity(turn) && !float.IsNaN(turn))
                {
                    //Point target = bridge.get_target();
                    //Point tar = new Point(target.x - transform.position.x,
                    //    target.y - transform.position.y);
                    //Point cur = new Point(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180),
                    //   Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180));
                    //if (dist1 == -1 && dist2 == -1 && dist3 ==- 1)
                    //{
                    //    float rotate = 0;
                    //    Debug.Log("Rotate = " + full_turn_to_target);
                    //    if (full_turn_to_target < 0)
                    //    {
                    //        full_turn_to_target++;
                    //        rotate = 1;
                    //    }
                    //    else if (full_turn_to_target > 0)
                    //    {
                    //        full_turn_to_target--;
                    //        rotate = -1;
                    //    }
                    //    transform.Rotate(new Vector3(0, 0, 1), rotate);
                    //}
                    //else
                    // if (speed != 0)
                    if (Mathf.Abs(turn) > 0.1)
                        transform.Rotate(new Vector3(0, 0, 1), -turn);
                }

              //  (text.transform.FindChild("TextButt").GetComponent<Text>() as Text).text =
              //      "Middle sensor = " + dist2 +
               //     "        Left sensor = " + dist1 +
               //     "  Right sensor = " + dist3;
                //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + TurnRadian * 180 / Mathf.PI);
                direction = calculate_direction(turn * Mathf.PI / 180);
                //}
                //Debug.Log(direction);
                //Debug.Log(transform.rotation);
                //Debug.Log(transform.rotation.z);
                // if (dist2 != -1)
                {
                    // Debug.Log(dist1 + " " + dist2 + " " + dist3);
                    // Debug.Log("turn " + turn);
                }
                //Debug.Log(dist1 + " " + dist2 + " " + dist3);
                if (!float.IsInfinity(direction.x) && !float.IsNaN(direction.x) &&
                    !float.IsInfinity(direction.y) && !float.IsNaN(direction.y))
                {
                    rgb.velocity = direction * Time.deltaTime * speed * boost;
                }
                draw_sensor();

                //// поворот, если едем от цели
                //Point target = bridge.get_target();
                //Point tar = new Point(target.x - transform.position.x,
                //    target.y - transform.position.y);

                //Point cur = new Point(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180),
                //   Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180));
                //if (bridge.getAngle(tar, cur) == 0)
                //    if (tar.x / cur.x <= 0 && tar.y / cur.y <= 0)
                //        transform.Rotate(new Vector3(0, 0, 1), 180);
            }
      //  }
	}

    Vector2 calculate_direction(float TurnRadian)
    {
        Vector2 result = new Vector2();
        result.x = transform.up.x * Mathf.Cos(TurnRadian) - transform.up.y * Mathf.Sin(TurnRadian);
        result.y = transform.up.x * Mathf.Sin(TurnRadian) + transform.up.y * Mathf.Cos(TurnRadian);
        return result;
    }

    Vector2 calculate_sensor_left()
    {
        Vector2 result = new Vector2();
        result.x = transform.up.x * Mathf.Cos(TurnAngleRadian) - transform.up.y * Mathf.Sin(TurnAngleRadian);
        result.y = transform.up.x * Mathf.Sin(TurnAngleRadian) + transform.up.y * Mathf.Cos(TurnAngleRadian);     
        return result;
    }

    Vector2 calculate_sensor_right()
    {
        Vector2 result = new Vector2();
        result.x = transform.up.x * Mathf.Cos(-TurnAngleRadian) - transform.up.y * Mathf.Sin(-TurnAngleRadian);        
        result.y = transform.up.x * Mathf.Sin(-TurnAngleRadian) + transform.up.y * Mathf.Cos(-TurnAngleRadian);
        return result;
    }

    Vector2 calculate_vector_turn(Vector2 v, float t)
    {
        Vector2 result = new Vector2();
        result.x = v.x * Mathf.Cos(t) - v.y * Mathf.Sin(t);
        result.y = v.x * Mathf.Sin(t) + v.y * Mathf.Cos(t);
        return result;
    }

    float get_info_from_sensor(Vector2 direction)
    {
        
        RaycastHit2D hit = Physics2D.Raycast(sensor.transform.position, direction, SensorDist);

        //.DrawLine(sensor.transform.position, direction, Color.red);
              
        if (hit.collider != null && hit.transform != sensor.transform)
        {            
            float distance = Mathf.Pow(hit.point.y - sensor.transform.position.y, 2) +
                Mathf.Pow(hit.point.x - sensor.transform.position.x, 2);
            if (distance <= SensorDist)
            {
                //Debug.Log("hit " + hit.point);
                //Debug.Log(sensor.transform.position);
                //Debug.Log(distance);
                if (hit.collider.tag == "target")
                {
                    find_target = true;
                }           

                if (distance > 800)
                {
                    distance = -1;
                }

                return distance;
            }
        }

        return -1;
    }

    public void set_direction()
    {
        bridge.set_direction(new Point(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180),
            Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180)));
        //Debug.Log("direction " + Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180) + " "
          //  + Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180));
        //Debug.Log("z = " + (transform.rotation.eulerAngles.z + 90));
        //Debug.Log("cos = " + Mathf.Cos(60 * Mathf.PI / 180));
    }

    public void set_target_direction()
    {
        Point target = bridge.get_target();
        bridge.set_target_direction(new Point(target.x - transform.position.x, 
            target.y - transform.position.y));
       // Debug.Log("target direction " + (target.x - transform.position.x) + " " + (target.y - transform.position.y));
    }


    static public void set_boost(float b)
    {
        boost = b;
    }

    void draw_sensor()
    {
        float SensorDist1 = Mathf.Pow(SensorDist, 1f/2f);
        Vector2 v1 = new Vector2(I_C.transform.position.x, I_C.transform.position.y);
        Vector2 v2 = new Vector2(I_C.transform.position.x + direction.x * SensorDist1, I_C.transform.position.y + direction.y * SensorDist1);

        Vector2 direction1 = calculate_vector_turn(direction, SensorTurnAngle);
        Vector2 direction2 = calculate_vector_turn(direction, -SensorTurnAngle);

        Vector2 v3 = new Vector2(I_C.transform.position.x + direction1.x * SensorDist1, I_C.transform.position.y + direction1.y * SensorDist1);
        Vector2 v4 = v1;
        Vector2 v5 = new Vector2(I_C.transform.position.x + direction2.x * SensorDist1, I_C.transform.position.y + direction2.y * SensorDist1);
        Vector2 v6 = v2;
       
        I_C.draw_sensor_line(v1, v2, v3, v4, v5, v6);
       // v_start1 = calculate_vector_turn(v_start, -SensorTurnAngle);
       // v_finish1 = calculate_vector_turn(v_start, -SensorTurnAngle);
        
    }

    public void set_new_target(Vector3 new_target) {
        //Time.timeScale = 0;
        new_target.z = 1;
        if (target == null)
        {
            target = new GameObject();
        }
        target.transform.position = new_target;
        bridge.set_target(new Point(new_target.x, new_target.y));
        bridge.setCoordinate(transform.position.x, transform.position.y);
        //Time.timeScale = 1;
        
    }

    public void set_new_position(Vector3 new_pos)
    {
        new_pos.z = 1;
        transform.position = new_pos;
    }

    public void change_speed(float new_speed)
    {
        if (new_speed >= 0)
            bridge.set_velocity(new_speed);
        bridge.changeMaxVelocity(new_speed);
    }

    public void change_sensor(float new_sensor)
    {
        if (new_sensor >= 0)
            SensorDist = new_sensor;
        bridge.changeMaximumSensorLength(800);
    }

    public void change_direction(float new_direction)
    {
        transform.Rotate(new Vector3(0, 0, 1), -new_direction);
    }

}
