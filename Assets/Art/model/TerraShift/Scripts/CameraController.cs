using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    //Class to control the game camera with the mouse

    // change control mode between drag with mouse and drag on screen edge
    public bool drag = true;
    // distance from edge scrolling starts, if drag is false
    public int Boundary = 50; 
    // speed to scroll over map, if drag is false
    public int speed = 5;
    //screen width
    private int theScreenWidth;
    //screen height
    private int theScreenHeight;

    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
    }

    void Update()
    {
        if (drag)
        {   //DRAG MODE
            if(Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                float dx = Input.GetAxis("Mouse X") * Time.deltaTime * -15;
                float dy = Input.GetAxis("Mouse Y") * Time.deltaTime * -15;
                transform.position = new Vector3(transform.position.x + dx, transform.position.y, transform.position.z + dy);
            }
        }
        else
        {   //SCREEN EDGE MODE
            if (Input.mousePosition.x > theScreenWidth - Boundary)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime * (Input.mousePosition.x - theScreenWidth + Boundary) / Boundary, transform.position.y, transform.position.z);
            }
            if (Input.mousePosition.x < 0 + Boundary)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            if (Input.mousePosition.y > theScreenHeight - Boundary)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
            }
            if (Input.mousePosition.y < 0 + Boundary)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
            }
        }
        
    }

}
