using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/**
 * Handles the drag and drop mechanics
 * @author Lucas_C_Wright
 * @start 04-02-2022
 * @version 04-09-2022
 */
public class CharacterDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler {
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject mini;
    //[SerializeField]
    //private float ySize = 200;
    //[SerializeField]
    //private float yMiniSize = 50;
    private RectTransform rectTrans;
    private bool locked = false;
    private bool expanded = true;
    private float XMin;
    private float XMax;
    private float miniYMin;
    private float miniYMax;
    private float expaYMin;
    private float expaYMax;
    private const float xSize = 173;


    // Start is called before the first frame update
    void Start() {
        rectTrans = GetComponent<RectTransform>();
        //XMin = 0;
        //XMax = Screen.width;

        //expaYMin = ;
        //expaYMax = ;

        //miniYMin = ;
        //miniYMax = ;
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

        if (expanded) {
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
        } else {
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
        //check if locked
        if (!locked) {
            //Debug.Log("OnDrag");
            rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    //swaps the locked boolean. When false the player can move around this character panel. When true they can't
    public void changeLock() {
        locked = !locked;
    }

    public void setLock(bool lk) {
        locked = lk;
    }

    //changes the values for the clamp when the character gets minimized or expanded
    public void changeClamp() {
        expanded = !expanded;

    }

    //other functions for Drag/Drop. Not currently used.
    public void OnEndDrag(PointerEventData eventData) {
        //Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData) {
        gameObject.transform.SetAsLastSibling();
        //Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData) {
        //Debug.Log("OnBeginDrag");
    }
}
