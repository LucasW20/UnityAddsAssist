using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/**
 * Handles the drag and drop mechanics
 * @author Lucas_C_Wright
 * @start 04-02-2022
 * @version 04-16-2022
 */
public class CharacterDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler {
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject mini;
    private RectTransform rectTrans;
    private bool expanded = true;

    // Start is called before the first frame update
    void Start() {
        rectTrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        //lock right x
        if (anchoredPosition.x + (transform.GetComponent<RectTransform>().rect.width / 2) > background.GetComponent<RectTransform>().rect.width) {
            Debug.Log("Hit Right");
            anchoredPosition.x = background.GetComponent<RectTransform>().rect.width - (transform.GetComponent<RectTransform>().rect.width / 2);
        }
        //lock left x
        if (anchoredPosition.x - transform.GetComponent<RectTransform>().rect.width / 2 < 0) {
            Debug.Log("Hit Left: ");
            anchoredPosition.x = transform.GetComponent<RectTransform>().rect.width / 2;
        }

        if (expanded) { //clamps for the maximized panel
            //lock bottom y
            if (anchoredPosition.y - transform.GetComponent<RectTransform>().rect.height / 2 < 0) {
                Debug.Log("Hit Bottom");
                anchoredPosition.y = transform.GetComponent<RectTransform>().rect.height / 2;
            }
            //lock top y
            if (anchoredPosition.y + transform.GetComponent<RectTransform>().rect.height / 2 > background.GetComponent<RectTransform>().rect.height) {
                Debug.Log("Hit Top");
                anchoredPosition.y = background.GetComponent<RectTransform>().rect.height - transform.GetComponent<RectTransform>().rect.height / 2;
            }
        } else { //clamps for the minimized panel
            //lock bottom y
            if (anchoredPosition.y + transform.GetComponent<RectTransform>().rect.height / 2 - mini.GetComponent<RectTransform>().rect.height * 1.25f < 0) {
                Debug.Log("Hit Bottom");
                anchoredPosition.y = -transform.GetComponent<RectTransform>().rect.height / 2 + mini.GetComponent<RectTransform>().rect.height * 1.25f;
            }
            //lock top y
            if (anchoredPosition.y + transform.GetComponent<RectTransform>().rect.height / 2 > background.GetComponent<RectTransform>().rect.height) {
                Debug.Log("Hit Top");
                anchoredPosition.y = background.GetComponent<RectTransform>().rect.height - transform.GetComponent<RectTransform>().rect.height / 2;
            }
        }
        
        transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }


    //Called every frame that the player is 'draging' the mouse across the screen. Moves the character panel along with the mouse
    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //changes the values for the clamp when the character gets minimized or expanded
    public void changeClamp() {
        expanded = !expanded;

    }

    //event call when the played clicks on the panel
    public void OnPointerDown(PointerEventData eventData) {
        gameObject.transform.SetAsLastSibling();
    }

    //other functions for Drag/Drop. Not currently used.
    public void OnEndDrag(PointerEventData eventData) { }
    public void OnBeginDrag(PointerEventData eventData) { }
}
