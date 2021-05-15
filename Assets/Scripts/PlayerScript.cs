using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DobbleConsoleProject;

public class PlayerScript : MonoBehaviour
{
    //public GameObject card1;
    //public GameObject card2;

    public List<GameObject> guiCards;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            guiCards.Add(child.gameObject);
        }

        updateGuiCards();
    }

    // Update is called once per frame
    void Update()
    {
        DetectSpritesClickedOn();

        if (Input.GetKeyUp(KeyCode.X))
        {
            updateGuiCards();
        }
    }
    private void updateGuiCards()
    {
        List<Card> deck = GenerateDeck.deck;

        List<int> idxUsedList = new List<int>();

        foreach(GameObject g in guiCards)
        {
            // This gives the GuiCard a new Dobble card
            // i.e. a new set of images 
            // this is random and really just as a test
            // for when playing I want to implement it as a game

            /// Pick a random Dobble card
            int rndIdx;

            /// finds another integer if the random one chosen
            /// has already been used and therefore already drawn
            /// so no duplicate cards
            do
            {
                rndIdx = Random.Range(0, deck.Count -1);

            } while (idxUsedList.Contains(rndIdx));

            idxUsedList.Add(rndIdx);

            Card card = new Card(deck[rndIdx].imageIndexs);

            List<Object> spritesList = GenerateDeck.GetSpritesForCard(card.imageIndexs);

            g.GetComponent<GuiCardScript>().UpdateGuiCard(card, GenerateDeck.GetSpritesForCard(card.imageIndexs));
        }
    }
    private void DetectSpritesClickedOn()
    {
        /// Detect if sprites are clicked on
        /// 
        /// https://www.codegrepper.com/code-examples/csharp/unity+2d+detect+click+on+sprite
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("getmousedown");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                //Debug.Log("found sprite: " + hit.collider.gameObject.name);
                Debug.Log("sprite - name of image used: " 
                    + hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name);

                //hit.collider.attachedRigidbody.AddForce(Vector2.up);
            }
        }
    }
}
