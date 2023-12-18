using UnityEngine;
using System.Collections;

public class ScaleOut : MonoBehaviour {

    //Simple Script animating the tiles to scale down and destroyed

    //Speed of the animation
    public float speed = 10f;
    //Timer Variable
    private float t;
    //Starting Scale of the object
    private float startS;

	void Start () {
        //Set up Parameters
        t = Mathf.PI / 2f;
        startS = transform.localScale.x;
    }
	
	void Update () {
        t += Time.deltaTime * speed;
        //If Objects scale is equal to 0, Destroy it
        if (t > Mathf.PI) GameObject.Destroy(gameObject);
        //Animate the Sacling down using a Sine Wave
        float s = startS * Mathf.Sin(t);
        transform.localScale = new Vector3(s, s, s);
	}
}
