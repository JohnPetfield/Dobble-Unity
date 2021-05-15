using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DobbleConsoleProject;

public class GenerateDeck : MonoBehaviour
{
    public GameObject cardBackground;

    List<Card> deck;
    List<double> anglesList = new List<double>();
    int n = 7;

    // Array of all the images that go on cards (needs to be cast to Sprite)
    public Object[] spritesArray;

    public GuiCard card1;
    public GuiCard card2;

    // Start is called before the first frame update
    void Start()
    {
        Dobble d = new Dobble(n);
        deck = d.createDeck();
        anglesList = CalculateImageAngles();

        /// https://docs.unity3d.com/ScriptReference/Resources.LoadAll.html
        spritesArray = Resources.LoadAll("DobbleImages", typeof(Sprite));

        // Card 1 
        int rndCard1 = Random.Range(0, deck.Count - 1);
        Vector2 circleCentre1 = new Vector2(-2, 0);
        card1 = new GuiCard(circleCentre1, deck[rndCard1].imageIndexs, "GuiCard1");
        DrawCard(card1);

        // Card 2
        int rndCard2 = Random.Range(0, deck.Count - 1);
        Vector2 circleCentre2 = new Vector2(2, 0);
        card2 = new GuiCard(circleCentre2, deck[rndCard2].imageIndexs, "GuiCard2");
        DrawCard(card2);

        //Debug.Log(card1.name + " card.GOspritesList.count: " + card1.GOspritesList.Count);
        //Debug.Log(card2.name + " card.GOspritesList.count: " + card2.GOspritesList.Count);
        //Debug.Log("anglesList count: " + anglesList.Count);
        //Debug.Log("no. cards in deck: " + deck.Count);
        //Debug.Log("spritesArray: " + spritesArray.Length);
        //Debug.Log("Screen.width: " + Screen.width);
    }

    private void DrawCard(GuiCard card)
    {
        DrawCircleBackground(card.centrePoint);

        DrawCentreImage(card);

        DrawRotatedImages(card);
    }

    private void DrawRotatedImages(GuiCard card)
    {
        // offset all individual angles with a single value
        // so that images aren't always in the same place
        int rnd = Random.Range(0, 359);
        
        Vector3 zAxis = new Vector3(0, 0, 1);
        int i = 0;

        foreach (int angle in anglesList)
        {
            GameObject sprite = new GameObject("Sprite" + i, typeof(SpriteRenderer), typeof(BoxCollider2D) /*, typeof(Transform)*/);

            SpriteRenderer centreSpriteSpriteRenderer = sprite.GetComponent<SpriteRenderer>();
            centreSpriteSpriteRenderer.sprite = (Sprite)spritesArray[card.imageIndexs[i]];

            Transform spriteTransform = sprite.GetComponent<Transform>();

            // allow it to be clickable i.e. boxcollider needed for the raycast

            sprite.GetComponent<BoxCollider2D>().size = new Vector2(0.81f,0.6f);


            // spawns all images at point (0 , 1.25f) then rotates
            // them around the centre point of the Dobble card
            spriteTransform.position = new Vector2(0 + card.centrePoint.x, 1.25f + card.centrePoint.y);
            spriteTransform.RotateAround(card.centrePoint, zAxis, angle + rnd);
            i++;

            // Add the GO so I can access the sprites later
            // to update them with new images
            card.GOspritesList.Add(sprite);

            //Debug.Log(card.name + " card.GOspritesList.count: " + card.GOspritesList.Count);
        }
    }

    private void DrawCircleBackground(Vector2 circleCentre)
    {
        // Creates an instance of the prefab that dragged the GameObject 
        // variable in the editor (once I declared a public GameObject
        GameObject cardObj = Instantiate(cardBackground, circleCentre, Quaternion.identity);
        SpriteRenderer backgroundSpriteRenderer = cardObj.GetComponent<SpriteRenderer>();

        backgroundSpriteRenderer.sortingLayerName = "Background";
    }

    private void DrawCentreImage(GuiCard card)
    {
        GameObject centreSprite = new GameObject("Sprite" + n, typeof(SpriteRenderer), typeof(BoxCollider2D));

        centreSprite.GetComponent<BoxCollider2D>().size = new Vector2(0.81f, 0.6f);

        SpriteRenderer centreSpriteSpriteRenderer = centreSprite.GetComponent<SpriteRenderer>();

        // Centre sprite is always n in imageIndexs
        // (all outsprites are 0 to n - 1 in imageIndexs)
        centreSpriteSpriteRenderer.sprite = (Sprite)spritesArray[card.imageIndexs[n]];

        centreSprite.GetComponent<Transform>().position = card.centrePoint;

        card.GOspritesList.Add(centreSprite);
        //Debug.Log(card.name + " card.GOspritesList.count: " + card.GOspritesList.Count);
    }

    private List<double> CalculateImageAngles()
    {
        double angle = 360 / n;
        List<double> anglesList = new List<double>();

        for (int i = 0; i < n; i++)
        {
            anglesList.Add(i * angle);
        }

        return anglesList;
    }

    private List<Object> GetSpritesForCard(List<int> imageIndexList)
    {
        List<Object> returnSpriteList = new List<Object>();

        foreach(int idx in imageIndexList)
        {
            returnSpriteList.Add(spritesArray[idx]);
        }

        return returnSpriteList;
    }

    // Run once a frame
    public void Update()
    {
        List<int> cardsCurrentlyUsed = new List<int>();

        if (Input.GetKeyUp(KeyCode.X))
        {
            // This gives the GuiCard a new Dobble card
            // i.e. a new set of images 
            // this is random and really just as a test
            // for when playing I want to implement it as a game

            /// Card 1
            /// 
            /// Pick a random Dobble card
            int rndCard1 = Random.Range(0, deck.Count - 1);
            Card card = new Card(deck[rndCard1].imageIndexs);

            cardsCurrentlyUsed.Add(rndCard1);
            
            /// Ensure this randomly picked card isn't the same 
            /// as the above randomly picked card (card1)
            int rndCard2;
            do
            {
                rndCard2 = Random.Range(0, deck.Count - 1);

            } while (cardsCurrentlyUsed.Contains(rndCard2));

            card1.UpdateGuiCard(card, GetSpritesForCard(card.imageIndexs));

            card = new Card(deck[rndCard2].imageIndexs);

            card2.UpdateGuiCard(card, GetSpritesForCard(card.imageIndexs));
        }
    }
}
