using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DobbleConsoleProject;


public class GuiCardScript : MonoBehaviour
{
    public List<GameObject> GOspritesList;
    public List<int> imageIndexs;
    public Vector2 centrePoint;

    //double n = 7;

    //public string name;
    /*
    public GuiCard(Vector2 _centrePoint, List<int> _imageIndexs, string _name)
    {
        GOspritesList = new List<GameObject>();

        name = _name;
        centrePoint = _centrePoint;
        imageIndexs = _imageIndexs;
    }*/

    public List<GameObject> childOuterImages;
    public GameObject centreImage;

    List<double> anglesList = new List<double>();

    private void Start()
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

    public void UpdateGuiCard(Card newCard, List<Object> spriteList)
    {
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
                childOuterImages[i].GetComponent<SpriteRenderer>().sprite = (Sprite)spriteList[i];
                childOuterImages[i].GetComponent<Transform>().RotateAround(this.transform.position, zAxis, rndAngle1);
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            //applyRotationalOffset();
        }
    }
}
