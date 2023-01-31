using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DobbleConsoleProject;
using System;

using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public static bool debug = false;

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
        SetInitialCentralComparisonCard();

        // Normal cards should always use the last card in the player deck
        // pretty sure can pop() the last card and then updating is always
        // using the last card
        UpdateGuiCards();
    }

    private void assignPlayerDecks()
    {
        int numPlayerDecksNeeded = playerGOlist.Count;
        int numCardsPerDeck = (GenerateDeck.deck.Count - 1) / numPlayerDecksNeeded;

        // ignore the element of index 0 (of GenerateDeck.deck) as this is the card assigned
        // to the centralComparisonCard 

        /// https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size
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

    private void SetInitialCentralComparisonCard()
    {
        // Now deck is shuffled the initial index can be fixed
        // which will allow to distribute the cards amongst players easier
        int Idx = 0;
        List<Card> deck = GenerateDeck.deck;

        Card card = new Card(deck[Idx].imageIndexs);

        centralComparisonCard.GetComponent<GuiCardScript>().UpdateGuiCard(card, GenerateDeck.GetSpritesForCard(card.imageIndexs));
    }

    private void UpdateGuiCards()
    {
        foreach(GameObject playerGo in playerGOlist)
        {
            DrawSingleCard(playerGo);
        }
    }

    private void DrawSingleCard(GameObject playerGO)
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
        /// https://www.codegrepper.com/code-examples/csharp/unity+2d+detect+click+on+sprite
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                string imageName = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name;

                bool matchFound = TestMatch(imageName);

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

                            DrawSingleCard(g.transform.parent.gameObject);
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

    ///  This checks all cards, so by default it checks the card that the image
    ///  that was clicked on also. This probably should be better and in a 
    /// a scenario where there are 3 cards this isn't correct at all and
    /// will need rewritting
    private bool TestMatch(string imageName /*, string parentName */)
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
    }
    /// https://www.youtube.com/watch?v=VbZ9_C4-Qbo&list=PLhwwh6WoCwTKJebe8LBwqhjgfIiiGZOXt&index=2
    void EndGame()
    {
        if(gameHasEnded == false)
        {
            Debug.Log("game over");
            this.gameHasEnded = true;
            Invoke("OpenEndGameMenu", restartDelay);
        }
    }
    void OpenEndGameMenu()
    {
        SceneManager.LoadScene("GameOver");
    }

    // How to reload current scene
    /*
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}
