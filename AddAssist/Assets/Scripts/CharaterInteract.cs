using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Functions that run when an interactable is used on a character tab
 * @author Lucas_C_Wright
 * @start 04-01-2022
 * @version 04-22-2022
 */
public class CharaterInteract : MonoBehaviour {
    //fields to reference values shown to the user
    [SerializeField]
    private Slider multiSlider;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI healthTitle;
    [SerializeField]
    private TextMeshProUGUI tempText;
    [SerializeField]
    private TextMeshProUGUI sliderText;
    [SerializeField]
    private TextMeshProUGUI armorText;
    [SerializeField]
    private TextMeshProUGUI initiativeText;
    [SerializeField]
    public TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject MPObect;
    private int sliderVal;
    private int maxHeatlh;
    private string startTemp = "0";
    private MPButtonBehaviour mainPanel;

    //Start is called before the first frame update
    void Start() {
        //default values for the slider, and temp health. the other defaults will be set when creating a character
        sliderText.text = "1";
        sliderVal = 1;
        tempText.text = startTemp;
    }

    //called when a new character is instanciated by other classes. This is often called the same time as this.Start()
    public void startChar(string nName, int nMax, int nArmor, int nInt) {
        mainPanel = MPObect.GetComponent<MPButtonBehaviour>();

        healthTitle.text = "Health (Max " + nMax + ")";
        maxHeatlh = nMax;
        nameText.text = nName;
        healthText.text = "" + nMax;
        armorText.text = "" + nArmor;
        initiativeText.text = "" + nInt;

        mainPanel.changesMade();
    }

    //called when a new character is instanciated by other classes. Specifically used for when loading existing encounters
    public void startChar(string nName, int nMax, int nArmor, int nInt, int cHealth, int cTemp) {
        startChar(nName, nMax, nArmor, nInt);
        healthText.text = "" + cHealth;
        startTemp = "" + cTemp;
    }

    //event call for when the value of the slider is changed
    public void multiSliderChange() {
        //update the value that the health is affected by
        sliderText.text = "" + multiSlider.value;
        sliderVal = (int)multiSlider.value;
    }

    //event call for the plus 1 button
    public void plusHealth() {
        if (int.Parse(healthText.text) < maxHeatlh) {
            healthText.text = "" + (int.Parse(healthText.text) + 1);
            mainPanel.changesMade();
        }
    }

    //event call for the minus 1 button
    public void minusHealth() {
        if (int.Parse(healthText.text) > 0) {
            healthText.text = "" + (int.Parse(healthText.text) - 1);
            mainPanel.changesMade();
        }
    }

    //event call for the plus multi button
    public void multiPlus() {
        if (int.Parse(healthText.text) <= maxHeatlh - sliderVal) {
            healthText.text = "" + (int.Parse(healthText.text) + sliderVal);
            mainPanel.changesMade();
        }
    }

    //event call for the minus multi button
    public void multiMinus() {
        if (int.Parse(healthText.text) >= 0 + sliderVal) {
            healthText.text = "" + (int.Parse(healthText.text) - sliderVal);
            mainPanel.changesMade();
        }
    }

    //event call for the temp plus 1 button
    public void tempPlus() {
        tempText.text = "" + (int.Parse(tempText.text) + 1);
        mainPanel.changesMade();
    }

    //event call for the temp minus 1 button
    public void tempMinus() {
        if (int.Parse(tempText.text) > 0) {
            tempText.text = "" + (int.Parse(tempText.text) - 1);
            mainPanel.changesMade();
        }
    }

    //event call for the temp plus multi button
    public void tempMultiP() {
        tempText.text = "" + (int.Parse(tempText.text) + sliderVal);
        mainPanel.changesMade();
    }

    //event call for the temp minus multi button
    public void tempMultiM() {
        if (int.Parse(tempText.text) >= 0 + sliderVal) {
            tempText.text = "" + (int.Parse(tempText.text) - sliderVal);
            mainPanel.changesMade();
        }
    }

    //event call for the reset health button
    public void resetHealth() {
        healthText.text = "" + maxHeatlh;
        tempText.text = "0";
        mainPanel.changesMade();
    }

    public void setCharacterFunctionality_DEPRECATED(bool functionality) {

    }
}