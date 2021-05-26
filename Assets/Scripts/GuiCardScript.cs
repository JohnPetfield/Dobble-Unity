using System.Collections.Generic;
using UnityEngine;
using DobbleConsoleProject;

public class GuiCardScript : MonoBehaviour
{
    public List<GameObject> GOspritesList;
    public List<int> imageIndexs;
    public Vector2 centrePoint;

    //double n = 7;

    public List<GameObject> childOuterImages;
    public GameObject centreImage;

    List<double> anglesList = new List<double>();

    private void Awake()
    {
        anglesList = CalculateImageAngles(7);

        Vector3 zAxis = new Vector3(0, 0, 1);
        int rnd = Random.Range(0, 359);
        int i = 0;
        /// Get all child images with tag 'OuterImage'
        /// in prefab all the outer images are in the same place
        /// i.e. are all placed on top of each other.
        /// 
        /// Rotate them around the centre of the white circle
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("OuterImage"))
            {
                childOuterImages.Add(child.gameObject);
                //child.position = new Vector2(this.transform.position.x, 1.25f + card.centrePoint.y);
                child.transform.RotateAround(this.transform.position, zAxis, (float)anglesList[i] + rnd);
                i++;
            }
            /// store centre image seperately
            else if (child.gameObject.CompareTag("CentreImage"))
            {
                centreImage = child.gameObject;
                centreImage.transform.RotateAround(this.transform.position, zAxis, rnd);
            }
        }

        //Debug.Log("guicardscrip.start() - childOuterImages.Count: " + childOuterImages.Count);
    }
    private static List<double> CalculateImageAngles(double n)
    {
        if (n == 0f)
            return null;
        double angle = 360 / n;
        List<double> anglesList = new List<double>();

        for (int i = 0; i < n; i++)
        {
            anglesList.Add(i * angle);
        }

        return anglesList;
    }

    public void applyRotationalOffset()
    {
        Vector3 zAxis = new Vector3(0, 0, 1);
        int rnd = Random.Range(0, 359);

        foreach (GameObject g in childOuterImages)
        {
            g.GetComponent<Transform>().RotateAround(this.transform.position, zAxis, rnd);
        }
    }

    /// This is for updating the centralComparisonGuiCard to take
    /// in a GuiCard as a parameter and copy the images that GuiCard input has
    public void UpdateGuiCard(GameObject inputGuiCard)
    {
        // Apply centre image of input card to this one (presumably the centralComparisonGuiCard)
        centreImage.GetComponent<SpriteRenderer>().sprite = 
            inputGuiCard.GetComponent<GuiCardScript>().centreImage.GetComponent<SpriteRenderer>().sprite;

        for(int i = 0; i < this.childOuterImages.Count;i++)
        {
            this.childOuterImages[i].GetComponent<SpriteRenderer>().sprite = 
                inputGuiCard.GetComponent<GuiCardScript>().childOuterImages[i].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void UpdateGuiCard(Card newCard, List<Object> spriteList)
    {

        //Debug.Log("spriteList: " + spriteList.Count);
        //Debug.Log("newCard: " + (newCard == null));
        //Debug.Log("childOuterImages: " + childOuterImages.Count);
        // offset all individual angles with a single value
        // so that images aren't always in the same place
        int rndAngle1 = Random.Range(0, 359);
        Vector3 zAxis = new Vector3(0, 0, 1);

        /// Replace the existing sprites with the new Dobble card images
        for (int i = 0; i < spriteList.Count; i++)
        {
            if (i == spriteList.Count - 1)
            {
                // update centre image
                centreImage.GetComponent<SpriteRenderer>().sprite = (Sprite)spriteList[i];
                centreImage.transform.RotateAround(this.transform.position, zAxis, rndAngle1);
            }
            else // update and rotate outer images
            {
                //Debug.Log(childOuterImages[i]);
                //Debug.Log(childOuterImages[i].GetComponent<SpriteRenderer>().sprite == null);
                this.childOuterImages[i].GetComponent<SpriteRenderer>().sprite = (Sprite)spriteList[i];
                this.childOuterImages[i].GetComponent<Transform>().RotateAround(this.transform.position, zAxis, rndAngle1);
            }
        }
    }
}