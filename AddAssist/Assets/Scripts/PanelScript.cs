using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Handles the behaviour for general panels. Not used by all panels.
 * @author Lucas_C_Wright
 * @start 04-16-2022
 * @version 04-16-2022
 */
public class PanelScript : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData) {
        transform.SetAsLastSibling();
    }
}
