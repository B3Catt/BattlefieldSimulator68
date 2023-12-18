using UnityEngine;
using System.Collections;

public class ScaleIn : MonoBehaviour {

    //Simple Script animating the tiles to scale up after creation

    //Speed of  the Animation
    public float speed = 10f;
    //Timer variable
    private float t;
    //Target Scale
    private float endS;

    void Start()
    {
        //Set up starting parameters
        t = 0;
        endS = transform.localScale.x;
        transform.localScale = new Vector3(0, 0, 0);
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        if (t > Mathf.PI / 2f)
        {
            //If animation ends, disable this script and set the final scale
            transform.localScale = new Vector3(endS, endS, endS);
            enabled = false;
            return;
        }
        //Animate the Scaling using a Sine wave
        float s = endS * Mathf.Sin(t);
        transform.localScale = new Vector3(s, s, s);
    }
}
