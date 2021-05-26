using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DobbleConsoleProject;

public class GenerateDeck : MonoBehaviour
{
    public static Object[] spritesArray;
    public static List<Card> deck;

    public GameObject guiCardPrefab;
    readonly int n = 7;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("GenerateDeck.awake() - s");
        Dobble d = new Dobble(n);
        
        //deck = d.createDeck();
        deck = d.createShuffledDeck();

        /// https://docs.unity3d.com/ScriptReference/Resources.LoadAll.html
        spritesArray = Resources.LoadAll("DobbleImages", typeof(Sprite));

        //Debug.Log("deck.Count: " + deck.Count);
        //Debug.Log("GenerateDeck.awake() - e");
    }


    /// Returns a list of sprite images for a given list of ints
    /// that are the indexes for the sprite images
    public static List<Object> GetSpritesForCard(List<int> imageIndexList)
    {
        List<Object> returnSpriteList = new List<Object>();

        foreach(int idx in imageIndexList)
        {
            returnSpriteList.Add(spritesArray[idx]);
        }

        return returnSpriteList;
    }
}