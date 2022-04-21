using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Functions that run when an interactable is used on a character tab
 * @author Lucas_C_Wright
 * @start 04-01-2022
 * @version 04-16-2022
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

        healthTitle.text = "HEALTH (Max " + nMax + ")";
        maxHeatlh = nMax;
        nameText.text = nName;
        healthText.text = "" + nMax;
        armorText.text = "" + nArmor;
        initiativeText.text = "" + nInt;

        mainPanel.changesMade();
    }

    public void startChar(string nName, int nMax, int nArmor, int nInt, int cHealth, int cTemp) {
        startChar(nName, nMax, nArmor, nInt);
        healthText.text = "" + cHealth;
        startTemp = "" + cTemp;
    }

    public void multiSliderChange() {
        sliderText.text = "" + multiSlider.value;
        sliderVal = (int)multiSlider.value;
    }

    //event calles for the different buttons on the character panel
    public void plusHealth() {
        if (int.Parse(healthText.text) < maxHeatlh) {
            healthText.text = "" + (int.Parse(healthText.text) + 1);
            mainPanel.changesMade();
        }
    }

    public void minusHealth() {
        if (int.Parse (healthText.text) > 0) {
            healthText.text = "" + (int.Parse(healthText.text) - 1);
            mainPanel.changesMade();
        }
    }

    public void multiPlus() {
        if (int.Parse(healthText.text) <= maxHeatlh - sliderVal) {
            healthText.text = "" + (int.Parse(healthText.text) + sliderVal);
            mainPanel.changesMade();
        }
    }

    public void multiMinus() {
        if (int.Parse(healthText.text) >= 0 + sliderVal) {
            healthText.text = "" + (int.Parse(healthText.text) - sliderVal);
            mainPanel.changesMade();
        }
    }

    public void tempPlus() { 
        tempText.text = "" + (int.Parse(tempText.text) + 1);
        mainPanel.changesMade();
    }

    public void tempMinus() {
        if (int.Parse(tempText.text) > 0) {
            tempText.text = "" + (int.Parse(tempText.text) - 1);
            mainPanel.changesMade();
        }
    }

    public void tempMultiP() { 
        tempText.text = "" + (int.Parse(tempText.text) + sliderVal);
        mainPanel.changesMade();
    }

    public void tempMultiM() {
        if (int.Parse(tempText.text) >= 0 + sliderVal) {
            tempText.text = "" + (int.Parse(tempText.text) - sliderVal);
            mainPanel.changesMade();
        }
    }

    public void resetHealth() {
        healthText.text = "" + maxHeatlh;
        tempText.text = "0";
        mainPanel.changesMade();
    }

    //public void exitCharacter() {
    //    EncounterStructure.removeChar(gameObject);
    //    Destroy(gameObject);
    //    mainPanel.changesMade();
    //}

    //public void lockCharacter() {
    //    gameObject.GetComponent<CharacterDragDrop>().changeLock();
    //}

    //public void minimizeChar() {
    //    GetComponent<CharacterDragDrop>().changeClamp();

    //    miniChar.SetActive(true);
    //    miniChar.GetComponentInChildren<TextMeshProUGUI>().text = nameText.text;
    //    maxChar.SetActive(false);

    //    //Debug.Log(miniChar.transform.GetComponent<RectTransform>().rect.width + " " + miniChar.transform.GetComponent<RectTransform>().rect.height);
    //}

    //public void expandChar() {
    //    GetComponent<CharacterDragDrop>().changeClamp();

    //    miniChar.SetActive(false);
    //    maxChar.SetActive(true);

    //    //Debug.Log(maxChar.transform.GetComponent<RectTransform>().rect.width + " " + maxChar.transform.GetComponent<RectTransform>().rect.height);
    //}

    //fields for the buttons. used only when changing the functionality
    //[SerializeField]
    //private Button plusOne;
    //[SerializeField]
    //private Button plusMulti;
    //[SerializeField]
    //private Button minusOne;
    //[SerializeField]
    //private Button minusMulti;
    //[SerializeField]
    //private Button tempPOne;
    //[SerializeField]
    //private Button tempMOne;
    //[SerializeField]
    //private Button tempPMulti;
    //[SerializeField]
    //private Button tempMMulti;
    //[SerializeField]
    //private Button btReset;

    public void setCharacterFunctionality_DEPRECATED(bool functionality) { 
        
    }
    //    plusOne.enabled = functionality;
    //    minusOne.enabled = functionality;  
    //    plusMulti.enabled = functionality;
    //    minusMulti.enabled = functionality;
    //    tempPOne.enabled = functionality;   
    //    tempMOne.enabled = functionality;
    //    tempPMulti.enabled = functionality;
    //    tempMMulti.enabled = functionality;
    //    btReset.enabled = functionality;
}
