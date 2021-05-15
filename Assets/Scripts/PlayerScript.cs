using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
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
                //Debug.Log("found sprite: " + hit.collider.gameObject.name);
                Debug.Log("sprite - name of image used: " 
                    + hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name);

                //hit.collider.attachedRigidbody.AddForce(Vector2.up);
            }
        }
    }
}
