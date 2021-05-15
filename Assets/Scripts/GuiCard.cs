using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DobbleConsoleProject;


public class GuiCard
{
    public List<GameObject> GOspritesList;
    public List<int> imageIndexs;
    public Vector2 centrePoint;

    public string name;

    public GuiCard(Vector2 _centrePoint, List<int> _imageIndexs, string _name)
    {
        GOspritesList = new List<GameObject>();

        name = _name;
        centrePoint = _centrePoint;
        imageIndexs = _imageIndexs;
    }

    public void UpdateGuiCard(Card newCard, List<Object> spriteList)
    {
        // offset all individual angles with a single value
        // so that images aren't always in the same place
        int rndAngle1 = Random.Range(0, 359);
        Vector3 zAxis = new Vector3(0, 0, 1);

        /// Replace the existing sprites with the new Dobble card images
        for (int i = 0; i < spriteList.Count; i++)
        {
            this.GOspritesList[i].GetComponent<SpriteRenderer>().sprite = (Sprite)spriteList[i];
            this.GOspritesList[i].GetComponent<Transform>().RotateAround(this.centrePoint, zAxis, rndAngle1);
        }

    }
}
