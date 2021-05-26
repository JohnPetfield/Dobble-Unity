using UnityEngine;

using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public GameObject player;
    public Text score;

    private void Start()
    {
        this.score = GetComponent<Text>(); 
    }
    // Update is called once per frame
    void Update()
    {
        this.score.text = player.GetComponent<PlayerScript>().NoCardsRemaining.ToString();
    }
}
