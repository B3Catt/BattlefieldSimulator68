using UnityEngine;
using System.Collections;

public class CardYIn : MonoBehaviour {

    //Simple script to animate the cards moving in upwards

    //Speed of the animation
    public float speed = 10f;
    //Timer variable
    private float t;
    //Target Y Coordinate
    private float endY;
    //Starting Y Coordinate
    private float startY;

    //RectTransform of the CardGameObject (because of GUI System)
    private RectTransform rt;

    void Start()
    {
        //set Start Position and viariables
        rt = transform.GetComponent<RectTransform>();
        t = 0;
        endY = rt.position.y;
        startY = endY - 250;
        rt.position = new Vector3(rt.position.x, startY, rt.position.z);
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        if (t > Mathf.PI / 2f)
        {
            //if animation ends, disable this script and set the final card position
            rt.position = new Vector3(rt.position.x, endY, rt.position.z);
            enabled = false;
            return;
        }
        //using a Sine Wave, animate the card moving upwards
        float y = startY + (endY - startY) * Mathf.Sin(t);
        rt.position = new Vector3(rt.position.x, y, rt.position.z);
    }
}
