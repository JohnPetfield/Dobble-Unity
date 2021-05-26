using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DobbleConsoleProject;
using System;

using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    /**/
    /**/
    /**/
    public static bool debug = false;
    /**/
    /**/
    /**/

    public List<GameObject> guiCards;
    public GameObject centralComparisonCard;

    public GameObject players;

    public List<GameObject> playerGOlist;

    private bool gameHasEnded = false;
    public float restartDelay = 0f;

    private void Awake()
    {
        // each player Game Object stored in a list
        foreach (Transform child in players.transform)
        {
            //Debug.Log(child.tag + " " + child.name);

            if (child.gameObject.CompareTag("Player"))
            {
                playerGOlist.Add(child.gameObject);
            }
        }
    }

    private void Start()
    {
        // divide the main deck amongst the players
        assignPlayerDecks();

        // Central card is always index 0 of GenerateDeck.deck
        setInitialCentralComparisonCard();

        // Normal cards should always use the last card in the player deck
        // pretty sure can pop() the last card and then updating is always
        // using the last card
        updateGuiCards();

        /*
        foreach(GameObject go in playerGOlist)
        {
            Debug.Log(go.name + " cards in deck: " +
                go.GetComponent<PlayerScript>().NoCardsRemaining);
        }*/
    }

    private void assignPlayerDecks()
    {
        int numPlayerDecksNeeded = playerGOlist.Count;
        int numCardsPerDeck = (GenerateDeck.deck.Count - 1) / numPlayerDecksNeeded;

    // ignore the element of index 0 (of GenerateDeck.deck) as this is the card assigned
    // to the centralComparisonCard 

    //GenerateDeck.deck

    ////
    /// https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size
    ///

    List<List<Card>> listOfPlayersDecks = SplitList(GenerateDeck.deck, numCardsPerDeck);

        // assign each player a playerdeck
        int i = 0;
        foreach(List<Card> list in listOfPlayersDecks)
        {
            playerGOlist[i].GetComponent<PlayerScript>().playerdeck = list;
            i++;
        }
    }
    public static List<List<Card>> SplitList(List<Card> locations, int nSize)
    {
        var list = new List<List<Card>>();

        // 0 is for central card so start from 1
        for (int i = 1; i < locations.Count; i += nSize)
        {
            list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
        }

        return list;
    }

    private void setInitialCentralComparisonCard()
    {
        // Now deck is shuffled the initial index can be fixed
        // which will allow to distribute the cards amongst players easier
        int Idx = 0;
        List<Card> deck = GenerateDeck.deck;

        //Debug.Log("deck.Count: " + deck.Count);

        //int rndIdx = Random.Range(0, deck.Count - 1);
        //Debug.Log("rndIdx: " + rndIdx);

        Card card = new Card(deck[Idx].imageIndexs);

        //Debug.Log("card.imageIndexs.Count: " + card.imageIndexs.Count);
        //Debug.Log("GenerateDeck.GetSpritesForCard: " + GenerateDeck.GetSpritesForCard(card.imageIndexs));

        centralComparisonCard.GetComponent<GuiCardScript>().UpdateGuiCard(card, GenerateDeck.GetSpritesForCard(card.imageIndexs));
    }

    private void updateGuiCards()
    {
        /*
        List<Card> deck = GenerateDeck.deck;

        List<int> idxUsedList = new List<int>();

        foreach (GameObject g in guiCards)
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
                rndIdx = UnityEngine.Random.Range(0, deck.Count -1);

            } while (idxUsedList.Contains(rndIdx));

            idxUsedList.Add(rndIdx);

            Card card = new Card(deck[rndIdx].imageIndexs);

            g.GetComponent<GuiCardScript>().UpdateGuiCard(card, GenerateDeck.GetSpritesForCard(card.imageIndexs));

        }
        */

        foreach(GameObject playerGo in playerGOlist)
        {

            drawSingleCard(playerGo);
            /*
            // Get last card from the player's deck
            PlayerScript ps = playerGo.GetComponent<PlayerScript>();
            List<Card> deck = ps.playerdeck;
            int idxLastCard = deck.Count - 1;

            Card card = new Card(deck[idxLastCard].imageIndexs);
            
            ps.GetComponentInChildren<GuiCardScript>().UpdateGuiCard(card, GenerateDeck.GetSpritesForCard(card.imageIndexs));*/
        }
    }

    private void drawSingleCard(GameObject playerGO)
    {
        // Get last card from the player's deck
        PlayerScript ps = playerGO.GetComponent<PlayerScript>();
        List<Card> deck = ps.playerdeck;
        int idxLastCard = deck.Count - 1;

        Card card = new Card(deck[idxLastCard].imageIndexs);

        ps.GetComponentInChildren<GuiCardScript>().UpdateGuiCard(card, GenerateDeck.GetSpritesForCard(card.imageIndexs));
    }

    // Update is called once per frame
    void Update()
    {
        DetectSpritesClickedOn();
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
                string imageName = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name;
                string parentNameOfImage = hit.collider.gameObject.transform.parent.name;

                bool matchFound = testMatch(imageName /*, parentNameOfImage*/ );

                if(debug == true)
                {
                    matchFound = true;
                    Debug.LogWarning("debug set to true.");
                }

                if(matchFound)
                {
                    /// g is the Game Object that is the GuiCard thats the parent
                    /// of the image that was clicked on
                    GameObject g = hit.collider.gameObject.transform.parent.gameObject;
                    

                    PlayerScript ps = g.transform.parent.GetComponent<PlayerScript>();


                    // has to have one card to delete from the deck
                    if (ps.NoCardsRemaining >= 1)
                    {
                        // Delete the last card in the players deck
                        ps.Pop();

                        if (ps.NoCardsRemaining == 0)
                        {
                            this.EndGame();
                        }
                        else
                        {
                            // 'Move' the card from the player to the centre one (visual only) i.e. 
                            // Turn the central comparison card into the card that was used to find the match
                            centralComparisonCard.GetComponent<GuiCardScript>().UpdateGuiCard(g);

                            // Update the cards? (this should really only be the card
                            // that needs updating, because they need a new one,
                            // the player who didn't guess should not be redrawn
                            //updateGuiCards();
                            drawSingleCard(g.transform.parent.gameObject);
                        }
                    }
                    else // no more cards in deck so that player has won the game
                    {
                        this.EndGame();
                    }

                }
            }
        }
    }

    /// <summary>
    ///  This checks all cards, so by default it checks the card that the image
    ///  that was clicked on also. This probably should be better and in a 
    /// a scenario where there are 3 cards this isn't correct at all and
    /// will need rewritting
    /// </summary>
    private bool testMatch(string imageName /*, string parentName */)
    {
        // look through outer images, in the central comparison card
        foreach (GameObject childImageGO in centralComparisonCard.GetComponent<GuiCardScript>().childOuterImages)
        {
            if (imageName == childImageGO.GetComponent<SpriteRenderer>().sprite.name)
            {
                return true;
            }
        }

        // look for centre image, in the central comparison card
        if (imageName == centralComparisonCard.GetComponent<GuiCardScript>().centreImage.GetComponent<SpriteRenderer>().sprite.name)
        {
            return true;
        }

        return false;
        /*
        foreach(GameObject guiCardGO in guiCards)
        {
            if (guiCardGO.name == parentName)
            {
                continue;
            }

            bool foundImage = false;
            foreach(GameObject childImageGO in guiCardGO.GetComponent<GuiCardScript>().childOuterImages)
            {
                if (imageName == childImageGO.GetComponent<SpriteRenderer>().sprite.name)
                {
                    foundImage = true;
                    break;
                }
            }

            // look for centre image, above is outer images only
            if (imageName == guiCardGO.GetComponent<GuiCardScript>().centreImage.GetComponent<SpriteRenderer>().sprite.name)
            {
                foundImage = true;
            }

            // if false here then we have a card that doesn't contain the image
            // we're looking for, therefore we can stop and return false
            // as the image has to be in all cards to be a match
            if (foundImage == false)
            {
                return false;
            }
        }

        // this code is only reachable if there is an image in each card
        // because it returns at the end of each card if an image isn't found
        return true;
        */
    }


    /// <summary>
    /// https://www.youtube.com/watch?v=VbZ9_C4-Qbo&list=PLhwwh6WoCwTKJebe8LBwqhjgfIiiGZOXt&index=2
    /// </summary>
    void EndGame()
    {
        if(gameHasEnded == false)
        {
            Debug.Log("game over");
            this.gameHasEnded = true;
            //this.Restart();
            //Invoke("Restart", restartDelay);
            Invoke("OpenEndGameMenu", restartDelay);
            //this.OpenEndGameMenu();
        }
    }
    void OpenEndGameMenu()
    {
        SceneManager.LoadScene("GameOver");
    }

    /*
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}
