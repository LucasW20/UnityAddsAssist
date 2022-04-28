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

    //Button call for the minimize button
    public void minimize() {
        maxPanel.SetActive(false);
        miniPanel.SetActive(true);
    }

    //Button call for the maximize button
    public void maximize() {
        maxPanel.SetActive(true);
        miniPanel.SetActive(false);
    }

    //Button call for the close button
    public void closePanel() {
        gameObject.SetActive(false);
    }
}
