using System.Collections.Generic;
using UnityEngine;
using DobbleConsoleProject;
public class PlayerScript : MonoBehaviour
{
    public GameObject playerCard;
    // This is the card pool for this player
    public List<Card> playerdeck;

    // this is probably going to be the score i.e. to be displayed
    // as once the player gets to zero cards they've won.
    private int noCardsRemaining;
    public int NoCardsRemaining
    {
        get { return this.playerdeck.Count;}
    }

    public void Pop()
    {
        int idxLastCard = this.playerdeck.Count - 1;
        if(this.playerdeck.Count >= 1)
        {
            this.playerdeck.RemoveAt(idxLastCard);
        }
        else
        {
            Debug.LogError("playerscript.pop - trying to pop with empty array");
        }
    }
}
