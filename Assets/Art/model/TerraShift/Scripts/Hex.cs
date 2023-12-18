using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Hex : MonoBehaviour, IPointerClickHandler{

    //Class that stores information about this hexagon tile

    //X and Y Coordinates of the tile in the hexagon map
    public int x, y;
    //reference to the GameManager Script
    private GameManager gm;

    // current number of houses on the tile
    public int numHouses = 0;
    //current green level of the tile, to simulate the landscape changes
    public float greenLevel = 0;

    //House Prefab reference
    public GameObject house;
    //List of House GameObjects on this hexagon tile
    public List<GameObject> houses;

    void Start()
    {
        //get the reference to the GameManager
        gm = GameObject.Find("GameManager").transform.GetComponent<GameManager>();
    }

    // Gets called when the hexagon tile is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // if not the left mouse button pressed, return
        if (eventData.button != 0) return;

        // set the selected gameObject in the GameManager to this and call the clickHex function
        GameManager.selected = gameObject;
        gm.clickHex();
    }

    //Gets Called when the Mouse hovers over the hexagon tile
    void OnMouseOver()
    {
        //if not currently performing an action, set the selected GameObject to this
        if(!gm.inAction)GameManager.selected = gameObject;
    }

    //function removes and adds n House GameObjects on this tile
    public void changeHouse(int n)
    {
        if (n == 0) return;
        for(int i = 0; i < Mathf.Abs(n); i++)
        {
            if (n < 0) removeHouse();
            else addHouse();
        }
    }

    // add one house object
    public void addHouse()
    {
        if (numHouses < 10)
        {
            numHouses++;
            GameObject g = Instantiate(house);
            Vector2 r = Random.insideUnitCircle * 0.75f;
            g.transform.position = new Vector3(transform.position.x + r.x, transform.position.y + 0.57f, transform.position.z + r.y);
            g.transform.localEulerAngles = new Vector3(g.transform.localEulerAngles.x, g.transform.localEulerAngles.y, Random.Range(0, 360));
            g.transform.SetParent(transform);
            houses.Add(g);
        }
    }

    //remove one house object
    public void removeHouse()
    {
        if(numHouses > 0)
        {
            numHouses--;
            DestroyImmediate(houses[houses.Count - 1]);
            houses.RemoveAt(houses.Count - 1);
        }
    }
}
