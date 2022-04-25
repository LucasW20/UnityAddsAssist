using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles the interaction of the turn order panel
 * @author Lucas_C_Wright
 * @start 04-23-2022
 * @version 04-24-2022
 */
public class OrderInteract : MonoBehaviour {
    [SerializeField]
    private GameObject miniPanel;
    [SerializeField]
    private GameObject maxPanel;

    public void minimize() {
        maxPanel.SetActive(false);
        miniPanel.SetActive(true);
    }

    public void maximize() {
        maxPanel.SetActive(true);
        miniPanel.SetActive(false);
    }

    public void closePanel() {
        gameObject.SetActive(false);
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
