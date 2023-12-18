using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour {

    //VARIABLES AND REFERENCES
    //------------------------------------------------------------------------------------------------

    //Canvas shown at the end of the game (either after 30 turns or with 0 houses)
    public GameObject gameOverScreen;
    //Plate hovering above the currently selected hexagon tile
    public GameObject selectionPlate;
    //GridManager that holds the hexagon map
    public GridManager grid;

    //Land types 0 to 6 and their corresponding names
    public enum Tiles : int { Void = 0, Grass = 1, Forest = 2, Water = 3, Mountain = 4, Desert = 5, Dirt = 6 };

    //Array of card prefabs that can get drawn
    public GameObject[] cards;
    //mainCanvas shows the cards to play and turnCanvas displays the current year after each turn
    public GameObject mainCanvas, turnCanvas;

    //Icons shown above the selected hexagon tile after using an ability (or showing positions of beasts)
    public GameObject meteorIcon, fireIcon, rainIcon, stormIcon, wolfIcon, dragonIcon;

    //Count of the current turn (year)
    public int turnCount = 0;

    //Color of the selectionPlate (blue if valid position for the ability, red if invalid)
    public Color colorTrue, colorFalse;

    //Currently selected tile of the hexagon map
    [HideInInspector]public static GameObject selected;
    //Currently selected cardin the mainCanvas
    [HideInInspector]public static GameObject selectedCard;

    //Number of cards left to play this turn (2 out of 3 cards get played)
    private int cardsLeft = 2;

    //damping hardness of the selectionPlate following the mouse (smoother look / animation)
    private float hardness = 0.3f;
    //damping hardness to animate the scale of the plate when clicking a tile
    private float hardnessScale = 0.5f;
    //timer to determine when to animate back to the usual plate scale, after click
    private float PlateBackTimer = 0;
    //scale of the plate that it is currently animating to
    private float targetPlateScale = 1f;

    //Storing data for simulation, for every tile store the new land type and the change of houses
    private int[,] nextTurn, nextHouseChange;

    //Timer until Environment Simulation after the turn end
    private float turnTimer = -2f;

    //Timer until Village Simulation after the Environment Simulation, and timer for cards to appear after Village Simulation
    private float villageTimer = -2, cardTimer = -2;

    //X and Y Coordinates (in the hexagon map) of the tile an card has been used on
    private int actionX, actionY;
    //while performing the animation and changin after using a card
    [HideInInspector]public bool inAction = false;
    
    //GameObject holding the icon of the ability used (to give player visual feedback)
    private GameObject actionIcon;
    //GameObject holding the icon for the wolf and the targeted tile to get attacked
    private GameObject wolf = null, wolfTarget = null;
    //wolfs X and Y Coordinates in the hexagon map
    private int wolfX, wolfY;
    //Counting number of turns without wolf to increase the spawning rate
    private int turnsWithoutWolf = 0;
    //GameObject holding the icon for the dargon and the targeted tile to get attacked
    private GameObject dragon = null, dragonTarget = null;
    //Position of the beasts in the world to create smooth animations using damped movement
    private Vector3 dragonTargetPos, wolfTargetPos;
    //dragons X and Y Coordinates in the hexagon map
    private int dragonX, dragonY;
    //Counting number of turns without dragon to increase the spawning rate
    private int turnsWithoutDragon = 0;

    //maximum number of houses achieved this game (Highscore of the current round)
    private int maxHouses = 0;

    //------------------------------------------------------------------------------------------------

    //set up everything for the game to start
    void Start () {
        //fill the canvas with 3 new cards
        fillCards();

        //Disable the gameOver Canvas until it needs to be shown
        gameOverScreen.SetActive(false);
    }

    //function will instantiate 3 random cards from the array and place them in the mainCanvas
    private void fillCards()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject g = GameObject.Instantiate(cards[Random.Range(0, cards.Length)]);
            g.transform.SetParent(mainCanvas.transform);
            RectTransform t = g.transform.GetComponent<RectTransform>();
            t.localScale = new Vector3(1, 1, 1);
            t.position = new Vector3(mainCanvas.GetComponent<Canvas>().pixelRect.width / 4f * (i+1), 40, 0);
            //Add a script to animate the cards to fly in from the bottom with a speed of 2
            g.AddComponent<CardYIn>().speed = 2f;
        }
    }

    //ACTIONS OF THE ABILITIES - METEOR - FIRE - RAIN - STORM
    //------------------------------------------------------------------------------------------------
    // Using coroutines these actions can be timed easily while the update method animates everything
    // Enables to run this code parallel to the main code
    // All 4 actions are structured the same

    IEnumerator meteorAction()
    {
        actionIcon.AddComponent<ScaleIn>().speed = 6f;
        grid.map[actionX, actionY].GetComponent<Hex>().changeHouse(-grid.map[actionX, actionY].GetComponent<Hex>().numHouses);
        yield return new WaitForSeconds(0.5f);
        if (selected.name.Contains("Dirt") || selected.name.Contains("Grass") || selected.name.Contains("Forest")) grid.setHex((int)Tiles.Mountain, actionX, actionY);
        else if (selected.name.Contains("Mountain") || selected.name.Contains("Desert")) grid.setHex((int)Tiles.Dirt, actionX, actionY);
        else if (selected.name.Contains("Water")) grid.setHex((int)Tiles.Grass, actionX, actionY);
        if(wolf != null && wolfX == actionX && wolfY == actionY)
        {
            DestroyImmediate(wolf);
            wolf = null;
        }
        if (dragon != null && dragonX == actionX && dragonY == actionY)
        {
            DestroyImmediate(dragon);
            dragon = null;
        }
        yield return new WaitForSeconds(0.2f);
        inAction = false;
        actionIcon.AddComponent<ScaleOut>().speed = 6f;
    }

    IEnumerator fireAction()
    {
        actionIcon.AddComponent<ScaleIn>().speed = 6f;
        grid.map[actionX, actionY].GetComponent<Hex>().changeHouse(-grid.map[actionX, actionY].GetComponent<Hex>().numHouses);
        yield return new WaitForSeconds(0.5f);
        if (selected.name.Contains("Dirt") || selected.name.Contains("Grass")) grid.setHex((int)Tiles.Desert, actionX, actionY);
        else if (selected.name.Contains("Forest")) grid.setHex((int)Tiles.Dirt, actionX, actionY);
        if (wolf != null && wolfX == actionX && wolfY == actionY)
        {
            DestroyImmediate(wolf);
            wolf = null;
        }
        yield return new WaitForSeconds(0.2f);
        inAction = false;
        actionIcon.AddComponent<ScaleOut>().speed = 6f;
    }

    IEnumerator rainAction()
    {
        actionIcon.AddComponent<ScaleIn>().speed = 6f;
        yield return new WaitForSeconds(0.5f);
        if (selected.name.Contains("Grass") || selected.name.Contains("Forest")) grid.setHex((int)Tiles.Water, actionX, actionY);
        else if (selected.name.Contains("Dirt")) grid.setHex((int)Tiles.Forest, actionX, actionY);
        else if (selected.name.Contains("Desert")) grid.setHex((int)Tiles.Grass, actionX, actionY);
        yield return new WaitForSeconds(0.2f);
        inAction = false;
        actionIcon.AddComponent<ScaleOut>().speed = 6f;
    }

    IEnumerator stormAction()
    {
        actionIcon.AddComponent<ScaleIn>().speed = 6f;
        grid.map[actionX, actionY].GetComponent<Hex>().changeHouse(-grid.map[actionX, actionY].GetComponent<Hex>().numHouses / 2);
        yield return new WaitForSeconds(0.5f);
        if (selected.name.Contains("Forest")) grid.setHex((int)Tiles.Dirt, actionX, actionY);
        if (dragon != null && dragonX == actionX && dragonY == actionY)
        {
            DestroyImmediate(dragon);
            dragon = null;
        }
        yield return new WaitForSeconds(0.2f);
        inAction = false;
        actionIcon.AddComponent<ScaleOut>().speed = 6f;
    }

    //------------------------------------------------------------------------------------------------

    //function to run when a tile gets clicked, depending on the selected card an action gets performed
    public void clickHex()
    {
        targetPlateScale = 0.3f;
        PlateBackTimer = 0.1f;
        if (inAction) return;
        Hex hex = selected.transform.GetComponent<Hex>();
        if (selectedCard == null)
        {
            return;
        }
        if (selectedCard.name.Contains("Fire"))
        {
            if (selected.name.Contains("Void") || selected.name.Contains("Water") || selected.name.Contains("Desert") || selected.name.Contains("Mountain")) return;
            actionX = hex.x;
            actionY = hex.y;
            inAction = true;
            actionIcon = Instantiate(fireIcon);
            Vector3 p = grid.map[hex.x, hex.y].transform.position;
            actionIcon.transform.position = new Vector3(p.x, p.y + 2f, p.z);
            StartCoroutine(fireAction());
        }
        else if (selectedCard.name.Contains("Rain"))
        {
            if (selected.name.Contains("Void") || selected.name.Contains("Water")) return;
            actionX = hex.x;
            actionY = hex.y;
            inAction = true;
            actionIcon = Instantiate(rainIcon);
            Vector3 p = grid.map[hex.x, hex.y].transform.position;
            actionIcon.transform.position = new Vector3(p.x, p.y + 2f, p.z);
            StartCoroutine(rainAction());
        }
        else if (selectedCard.name.Contains("Meteor"))
        {
            if (selected.name.Contains("Void")) return;
            actionX = hex.x;
            actionY = hex.y;
            inAction = true;
            actionIcon = Instantiate(meteorIcon);
            Vector3 p = grid.map[hex.x, hex.y].transform.position;
            actionIcon.transform.position = new Vector3(p.x, p.y + 2f, p.z);
            StartCoroutine(meteorAction());
        }
        else if (selectedCard.name.Contains("Storm"))
        {
            if (selected.name.Contains("Void") || selected.name.Contains("Mountain")) return;
            actionX = hex.x;
            actionY = hex.y;
            inAction = true;
            actionIcon = Instantiate(stormIcon);
            Vector3 p = grid.map[hex.x, hex.y].transform.position;
            actionIcon.transform.position = new Vector3(p.x, p.y + 2f, p.z);
            StartCoroutine(stormAction());
        }
        else if (selectedCard.name.Contains("Grass"))
        {
            if (!selected.name.Contains("Void")) return;
            grid.setHex(1, selected.GetComponent<Hex>().x, selected.GetComponent<Hex>().y);
        }
        else if (selectedCard.name.Contains("Forest"))
        {
            if (!selected.name.Contains("Void")) return;
            grid.setHex(2, selected.GetComponent<Hex>().x, selected.GetComponent<Hex>().y);
        }
        else if (selectedCard.name.Contains("Water"))
        {
            if (!selected.name.Contains("Void")) return;
            grid.setHex(3, selected.GetComponent<Hex>().x, selected.GetComponent<Hex>().y);
        }
        else if (selectedCard.name.Contains("Mountain"))
        {
            if (!selected.name.Contains("Void")) return;
            grid.setHex(4, selected.GetComponent<Hex>().x, selected.GetComponent<Hex>().y);
        }
        else if (selectedCard.name.Contains("Desert"))
        {
            if (!selected.name.Contains("Void")) return;
            grid.setHex(5, selected.GetComponent<Hex>().x, selected.GetComponent<Hex>().y);
        }
        else if (selectedCard.name.Contains("Dirt"))
        {
            if (!selected.name.Contains("Void")) return;
            grid.setHex(6, selected.GetComponent<Hex>().x, selected.GetComponent<Hex>().y);
        }

        //Destroy the used card object and if only one card is left end the turn
        GameObject.Destroy(selectedCard);
        cardsLeft--;
        if (cardsLeft <= 0)
        {
            for (int i = 0; i < mainCanvas.transform.childCount; i++)
            {
                if (mainCanvas.transform.GetChild(i).name.Contains("Card"))
                {
                    //destroy the last card in the canvas
                    GameObject.Destroy(mainCanvas.transform.GetChild(i).gameObject);
                }
            }
            turnTimer = 2f;
            turnCount++;
            //show the canvas with the year
            turnCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Year " + turnCount;
        }
    }

	void Update () {
        checkGameOver();
        float dragonHardness = 0.2f;
        if(dragon != null)dragon.transform.position = new Vector3(dragon.transform.position.x + dragonHardness * (dragonTargetPos.x - dragon.transform.position.x), dragon.transform.position.y + dragonHardness * (dragonTargetPos.y - dragon.transform.position.y), dragon.transform.position.z + dragonHardness * (dragonTargetPos.z - dragon.transform.position.z));
        if(wolf != null)wolf.transform.position = new Vector3(wolf.transform.position.x + dragonHardness * (wolfTargetPos.x - wolf.transform.position.x), wolf.transform.position.y + dragonHardness * (wolfTargetPos.y - wolf.transform.position.y), wolf.transform.position.z + dragonHardness * (wolfTargetPos.z - wolf.transform.position.z));

        // Determine if the current selection is valid
        if (inAction)
        {
            //invalid if currently performing an action
            selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorFalse;
            return;
        }
        else
        {
            selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorTrue;
            if (selectedCard != null && selected != null)
            {
                //Determine if the selected hexagon tile is valid for the selected card
                if (selectedCard.name.Contains("Desert") || selectedCard.name.Contains("Mountain") || selectedCard.name.Contains("Grass") || selectedCard.name.Contains("Water") || selectedCard.name.Contains("Forest") || selectedCard.name.Contains("Dirt"))
                {
                    if (!selected.name.Contains("Null") && !selected.name.Contains("Void")) selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorFalse;
                }
                else if (selectedCard.name.Contains("Fire"))
                {
                    if (!selected.name.Contains("Dirt") && !selected.name.Contains("Grass") && !selected.name.Contains("Forest")) selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorFalse;
                }
                else if (selectedCard.name.Contains("Rain"))
                {
                    if (!selected.name.Contains("Desert") && !selected.name.Contains("Dirt") && !selected.name.Contains("Grass") && !selected.name.Contains("Forest")) selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorFalse;
                }
                else if (selectedCard.name.Contains("Meteor"))
                {
                    if (selected.name.Contains("Void")) selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorFalse;
                }
                else if (selectedCard.name.Contains("Storm"))
                {
                    if (selected.name.Contains("Void") || selected.name.Contains("Mountain")) selectionPlate.GetComponent<MeshRenderer>().sharedMaterial.color = colorFalse;
                }
            }

            //after a click, reset the target scale of the selectionPlate
            if (PlateBackTimer > 0) PlateBackTimer -= Time.deltaTime;
            else
            {
                targetPlateScale = 1f;
            }
            if (selected != null)
            {
                //animate the selectionPlate to the target position, and the scale after a click
                selectionPlate.transform.position = new Vector3(selectionPlate.transform.position.x + hardness * (selected.transform.position.x - selectionPlate.transform.position.x), selectionPlate.transform.position.y + hardness * (selected.transform.position.y + 0.7f - selectionPlate.transform.position.y), selectionPlate.transform.position.z + hardness * (selected.transform.position.z - selectionPlate.transform.position.z));
                float s = selectionPlate.transform.localScale.x + hardnessScale * (targetPlateScale - selectionPlate.transform.localScale.x);
                selectionPlate.transform.localScale = new Vector3(s, s, selectionPlate.transform.localScale.z);

            }
        }

        //Define the functions called between turns and time everything to create small breaks
        if (turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
            float t = (2 - turnTimer)/2f * Mathf.PI;
            turnCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Sin(t);
        }
        else if(turnTimer > -1 && turnTimer <= 0)
        {
            turnTimer = -2f;
            turnCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            doTurn();
            villageTimer = 1f;
        }

        if (villageTimer > 0)
        {
            villageTimer -= Time.deltaTime;
        }
        else if (villageTimer > -1 && villageTimer <= 0)
        {
            villageTimer = -2f;
            doVillage();
            cardTimer = 0.2f;
        }

        if (cardTimer > 0)
        {
            cardTimer -= Time.deltaTime;
        }
        else if (cardTimer > -1 && cardTimer <= 0)
        {
            cardTimer = -2f;
            cardsLeft = 2;
            fillCards();
        }

        //if (selectedCard != null && Input.GetMouseButtonDown(0)) selectedCard = null;
    }

    //function simply restarts the current scene (in this case the  game)
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //deal with game Over conditions and Highscores
    private void checkGameOver()
    {
        int tilesWithHouses = 0;
        int numHouses = 0;
        //Count all the houses and check if it is a new highscore
        for (int y = 0; y < grid.gridHeightInHexes; y++)
        {
            for (int x = 0; x < grid.gridWidthInHexes; x++)
            {
                if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                if (grid.map[x, y].GetComponent<Hex>().numHouses > 0)
                {
                    tilesWithHouses++;
                    numHouses += grid.map[x, y].GetComponent<Hex>().numHouses;
                }
            }
        }
        if (numHouses > maxHouses) maxHouses = numHouses;
        //  if no houses are left or 30 turns have passed the game ends
        if(tilesWithHouses <= 0 || turnCount > 30)
        {
            //show the game over canvas
            gameOverScreen.SetActive(true);
            if (turnCount > 30) gameOverScreen.transform.GetChild(1).GetComponent<Text>().text = "30 Years have passed... Now let's see how you  did! In its best Time the Village Contained";
            gameOverScreen.transform.GetChild(4).GetComponent<Text>().text = "" + maxHouses;
        }
    }

    //VILLAGE SIMULATION
    //------------------------------------------------------------------------------------------------
    //Simulatie the village / Houses, as well as wolf and dragon AI
    private void doVillage()
    {
        nextHouseChange = new int[grid.gridWidthInHexes, grid.gridHeightInHexes];

        for (int y = 0; y < grid.gridHeightInHexes; y++)
        {
            for (int x = 0; x < grid.gridWidthInHexes; x++)
            {
                if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                Hex hex = grid.map[x, y].GetComponent<Hex>();
                if (hex.numHouses > 0)
                {
                    //Destroy all houses if invalid position
                    if(grid.map[x, y].name.Contains("Water") || grid.map[x, y].name.Contains("Mountain"))
                    {
                        grid.map[x, y].GetComponent<Hex>().changeHouse(-10);
                    }

                    //Destroy Houses and prevent spreading if on a desert tile
                    if (hex.greenLevel <= -0.5f)
                    {
                        nextHouseChange[x, y] = -3;
                        continue;
                    }

                    //Possible Neighbours
                    List<GameObject> possible = new List<GameObject>();
                    GameObject[] n = grid.getNeighbours(x, y);
                    for (int k = 0; k < 6; k++)
                    {
                        if (n[k] == null || n[k].name.Contains("Null") || n[k].name.Contains("Void") || n[k].name.Contains("Mountain") || n[k].name.Contains("Water"))
                        {
                            continue;
                        }
                        if (n[k].GetComponent<Hex>().numHouses >= 10) continue;
                        if (n[k].GetComponent<Hex>().greenLevel > 0) possible.Add(n[k]);
                    }

                    if (possible.Count <= 0) continue;
                    //Spawn Houses on Neighbour Tiles

                    List<GameObject> sortedP = possible.OrderByDescending(o => o.GetComponent<Hex>().greenLevel).ToList();

                    int tileCount = (possible.Count / 2);
                    if (tileCount <= 0) tileCount = 1;
                    int housesPerTile = (hex.numHouses) / tileCount;

                    if (housesPerTile == 0) housesPerTile = 1;

                    int houseCount = (hex.numHouses / 2);
                    if (houseCount <= 0) houseCount = 1;
                    int housesLeft = hex.numHouses / houseCount;

                    for (int i = 0; i < sortedP.Count; i++)
                    {
                        if (housesLeft < housesPerTile) housesPerTile = housesLeft;
                        nextHouseChange[sortedP[i].GetComponent<Hex>().x, sortedP[i].GetComponent<Hex>().y] += housesPerTile;
                        housesLeft -= housesPerTile;
                        if (housesLeft <= 0) break;
                    }

                    //Destroy some houses if on dirt tile
                    if (hex.greenLevel <= 0.5f)
                    {
                        nextHouseChange[x, y] = -3;
                    }

                }
            }
        }

        //WOLF AI
        //------------------------------------------------------------------------------------------------
        if (wolf == null && turnCount >= 4)
        {
            if (Random.Range(0, 30 / (turnsWithoutWolf + turnCount)) == 0)
            {
                turnsWithoutWolf = 0;
                List<GameObject> possibleTiles = new List<GameObject>();
                for (int y = 0; y < grid.gridHeightInHexes; y++)
                {
                    for (int x = 0; x < grid.gridWidthInHexes; x++)
                    {
                        if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                        if (grid.map[x, y].GetComponent<Hex>().numHouses > 0) continue;
                        possibleTiles.Add(grid.map[x, y]);
                    }
                }

                GameObject tile = possibleTiles[Random.Range(0, possibleTiles.Count)];
                wolfX = tile.GetComponent<Hex>().x;
                wolfY = tile.GetComponent<Hex>().y;

                wolf = Instantiate(wolfIcon);
                Vector3 p = grid.map[wolfX, wolfY].transform.position;
                wolfTargetPos = new Vector3(p.x, p.y + 1f, p.z);
            }
            else
            {
                turnsWithoutWolf++;
            }
        }else if(wolf != null)
        {
            if(grid.map[wolfX, wolfY].GetComponent<Hex>().numHouses > 0)
            {
                nextHouseChange[wolfX, wolfY] -= 30;
            }
            else
            {
                if (wolfTarget == null || wolfTarget.GetComponent<Hex>().numHouses <= 0)
                {
                    //set new target
                    List<GameObject> possibleTiles = new List<GameObject>();
                    for (int y = 0; y < grid.gridHeightInHexes; y++)
                    {
                        for (int x = 0; x < grid.gridWidthInHexes; x++)
                        {
                            if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                            if (grid.map[x, y].GetComponent<Hex>().numHouses <= 0) continue;
                            possibleTiles.Add(grid.map[x, y]);
                        }
                    }
                    wolfTarget = possibleTiles[Random.Range(0, possibleTiles.Count)];
                }
                if (wolfX < wolfTarget.GetComponent<Hex>().x) wolfX++;
                else if(wolfX > wolfTarget.GetComponent<Hex>().x) wolfX--;
                else if (wolfY < wolfTarget.GetComponent<Hex>().y) wolfY++;
                else if (wolfY > wolfTarget.GetComponent<Hex>().y) wolfY--;

                Vector3 p = grid.map[wolfX, wolfY].transform.position;
                wolfTargetPos = new Vector3(p.x, p.y + 1f, p.z);
                if (grid.map[wolfX, wolfY].GetComponent<Hex>().numHouses > 0)
                {
                    nextHouseChange[wolfX, wolfY] -= 30;
                }
            }
        }

        //DRAGON AI
        //------------------------------------------------------------------------------------------------
        if (dragon == null && turnCount >= 8)
        {
            if (Random.Range(0, 60 / (turnsWithoutDragon + turnCount)) == 0)
            {
                turnsWithoutDragon = 0;
                List<GameObject> possibleTiles = new List<GameObject>();
                for (int y = 0; y < grid.gridHeightInHexes; y++)
                {
                    for (int x = 0; x < grid.gridWidthInHexes; x++)
                    {
                        if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                        if (grid.map[x, y].GetComponent<Hex>().numHouses > 0) continue;
                        possibleTiles.Add(grid.map[x, y]);
                    }
                }

                GameObject tile = possibleTiles[Random.Range(0, possibleTiles.Count)];
                dragonX = tile.GetComponent<Hex>().x;
                dragonY = tile.GetComponent<Hex>().y;

                dragon = Instantiate(dragonIcon);
                Vector3 p = grid.map[dragonX, dragonY].transform.position;
                dragonTargetPos = new Vector3(p.x, p.y + 1f, p.z);
            }
            else
            {
                turnsWithoutDragon++;
            }
        }
        else if (dragon != null)
        {
            if (grid.map[dragonX, dragonY].GetComponent<Hex>().numHouses > 0)
            {
                nextHouseChange[dragonX, dragonY] -= 30;
            }
            else
            {
                if (dragonTarget == null || dragonTarget.GetComponent<Hex>().numHouses <= 0)
                {
                    //set new target
                    List<GameObject> possibleTiles = new List<GameObject>();
                    for (int y = 0; y < grid.gridHeightInHexes; y++)
                    {
                        for (int x = 0; x < grid.gridWidthInHexes; x++)
                        {
                            if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                            if (grid.map[x, y].GetComponent<Hex>().numHouses <= 0) continue;
                            possibleTiles.Add(grid.map[x, y]);
                        }
                    }
                    dragonTarget = possibleTiles[Random.Range(0, possibleTiles.Count)];
                }
                if (dragonX < dragonTarget.GetComponent<Hex>().x) dragonX++;
                else if (dragonX > dragonTarget.GetComponent<Hex>().x) dragonX--;
                else if (dragonY < dragonTarget.GetComponent<Hex>().y) dragonY++;
                else if (dragonY > dragonTarget.GetComponent<Hex>().y) dragonY--;

                Vector3 p = grid.map[dragonX, dragonY].transform.position;
                dragonTargetPos = new Vector3(p.x, p.y + 1f, p.z);
                if (grid.map[dragonX, dragonY].GetComponent<Hex>().numHouses > 0)
                {
                    nextHouseChange[dragonX, dragonY] -= 30;
                }
            }
        }

        //Apply changes

        for (int y = 0; y < grid.gridHeightInHexes; y++)
        {
            for (int x = 0; x < grid.gridWidthInHexes; x++)
            {
                if (nextHouseChange[x, y] != 0)
                {
                    grid.map[x, y].GetComponent<Hex>().changeHouse(nextHouseChange[x, y]);
                }
               
                //TODO: spawn Icons, that have decay script
            }
        }
    }

    //LANDSCAPE SIMULATION
    //------------------------------------------------------------------------------------------------
    //Simulate the landscape / Hexagon Tiles, based on green level
    private void doTurn()
    {
        nextTurn = new int[grid.gridWidthInHexes, grid.gridHeightInHexes];

        for (int y = 0; y < grid.gridHeightInHexes; y++)
        {
            for (int x = 0; x < grid.gridWidthInHexes; x++)
            {
                if (grid.map[x, y] == null || grid.map[x, y].name.Contains("Null") || grid.map[x, y].name.Contains("Void")) continue;
                if (grid.map[x, y].name.Contains("Mountain") || grid.map[x, y].name.Contains("Water")) continue;
                int nCount = 6;
                float green = 0;
                GameObject[] n = grid.getNeighbours(x, y);
                for(int i = 0; i < 6; i++)
                {
                    if (n[i] == null || n[i].name.Contains("Null") || n[i].name.Contains("Void"))
                    {
                        nCount--;
                        continue;
                    }
                    if (n[i].name.Contains("Desert"))
                    {
                        green -= 2;
                    }else if (n[i].name.Contains("Mountain"))
                    {
                        green -= 1;
                    }
                    else if (n[i].name.Contains("Dirt"))
                    {
                        green += 0;
                    }
                    else if (n[i].name.Contains("Grass"))
                    {
                        green += 1;
                    }
                    else if (n[i].name.Contains("Forest"))
                    {
                        green += 2;
                    }
                    else if (n[i].name.Contains("Water"))
                    {
                        green += 3;
                    }
                }

                green = green / nCount;

                grid.map[x, y].GetComponent<Hex>().greenLevel = green;

                if(green <= -0.5f)
                {
                    if(grid.map[x,y].name.Contains("Dirt"))nextTurn[x, y] = (int)Tiles.Desert;//<--
                    if (grid.map[x, y].name.Contains("Grass")) nextTurn[x, y] = (int)Tiles.Dirt;
                    if (grid.map[x, y].name.Contains("Forest")) nextTurn[x, y] = (int)Tiles.Grass;
                }
                else if (green <= 0.5f)
                {
                    if (grid.map[x, y].name.Contains("Desert"))nextTurn[x, y] = (int)Tiles.Dirt;//<--
                    if (grid.map[x, y].name.Contains("Grass")) nextTurn[x, y] = (int)Tiles.Dirt;
                    if (grid.map[x, y].name.Contains("Forest")) nextTurn[x, y] = (int)Tiles.Grass;
                }
                else if (green <= 1.5f)
                {
                    if (grid.map[x, y].name.Contains("Desert")) nextTurn[x, y] = (int)Tiles.Dirt;
                    if (grid.map[x, y].name.Contains("Dirt")) nextTurn[x, y] = (int)Tiles.Grass;//<--
                    if (grid.map[x, y].name.Contains("Forest")) nextTurn[x, y] = (int)Tiles.Grass;
                }
                else
                {
                    if (grid.map[x, y].name.Contains("Desert")) nextTurn[x, y] = (int)Tiles.Dirt;
                    if (grid.map[x, y].name.Contains("Dirt")) nextTurn[x, y] = (int)Tiles.Grass;
                    if (grid.map[x, y].name.Contains("Grass")) nextTurn[x, y] = (int)Tiles.Forest;//<--
                }
            }
         }

        //Apply Changes
        for (int y = 0; y < grid.gridHeightInHexes; y++)
        {
            for (int x = 0; x < grid.gridWidthInHexes; x++)
            {
                if(nextTurn[x, y] != 0)grid.setHex(nextTurn[x, y], x, y);
            }
        }

     }

    
}
