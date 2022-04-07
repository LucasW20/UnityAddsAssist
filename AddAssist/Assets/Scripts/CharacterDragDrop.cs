using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Handles the drag and drop mechanics
 * @author Lucas_C_Wright
 * @start 04-02-2022
 * @version 04-03-2022
 */
public class CharacterDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler {
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float ySize = 200;
    [SerializeField]
    private float yMiniSize = 50;
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
        XMin = canvas.GetComponent<RectTransform>().rect.xMin + canvas.GetComponent<RectTransform>().offsetMin.x + xSize;
        XMax = canvas.GetComponent<RectTransform>().rect.xMax + canvas.GetComponent<RectTransform>().offsetMax.x - xSize;
        expaYMin = canvas.GetComponent<RectTransform>().rect.yMin + canvas.GetComponent<RectTransform>().offsetMin.y + ySize;
        expaYMax = canvas.GetComponent<RectTransform>().rect.yMax + canvas.GetComponent<RectTransform>().offsetMax.y - ySize;
        miniYMin = canvas.GetComponent<RectTransform>().rect.yMin + canvas.GetComponent<RectTransform>().offsetMin.y - yMiniSize;
        miniYMax = canvas.GetComponent<RectTransform>().rect.yMax + canvas.GetComponent<RectTransform>().offsetMax.y + yMiniSize;
    }

    // Update is called once per frame
    void Update() {
        //every frame check that the player is within the screen bounds 
        if (expanded) {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, XMin, XMax), Mathf.Clamp(transform.position.y, expaYMin, expaYMax), 0);
        } else {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, XMin, XMax), Mathf.Clamp(transform.position.y, miniYMin, miniYMax), 0);
        }
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
        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData) {
        gameObject.transform.SetAsLastSibling();
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
    }
}
