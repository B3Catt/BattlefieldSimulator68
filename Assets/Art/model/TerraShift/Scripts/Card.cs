using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    //Rect Transform of the Card on the Canvas
    private RectTransform t;
    //Reference to the GameManager Script
    private GameManager gm;

	void Start () {
        //Set up References
        gm = GameObject.Find("GameManager").transform.GetComponent<GameManager>();
        t = transform.GetComponent<RectTransform>();
        //Set the callback function of the cards UI-Button to the click() function
        transform.GetComponent<Button>().onClick.AddListener(() => { click(); });
    }

    //Called when the card gets clicked
    void click()
    {
        //If not performing an action, set the selected Card to this card
        if (!gm.inAction) GameManager.selectedCard = gameObject;
    }

    void Update()
    {
        //Scale up the card if it is selected
        if(GameManager.selectedCard == gameObject)
        {
            t.sizeDelta = new Vector2(175f, 250f);
        }
        else
        {
            t.sizeDelta = new Vector2(100f, 175f);
        }
    }


}
