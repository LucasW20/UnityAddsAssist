using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/**
 * Functions that run when a button is pressed on the main panel. 
 * @author Lucas_C_Wright
 * @start 04-01-2022
 * @version 04-15-2022
 */
public class MPButtonBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject createPanel;
    [SerializeField]
    private GameObject savePanel;
    [SerializeField]
    private GameObject loadPanel;
    //[SerializeField]
    //private GameObject exitConfirmationPanel;
    [SerializeField]
    private GameObject resetConfirmationPanel;
    [SerializeField]
    private TextMeshProUGUI loadError;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject characterTemplate;
    [SerializeField]
    private GameObject mpBlurImage;
    private bool saved = true;
    private float fadeTime = 3.5f;

    //// Start is called before the first frame update
    //void Start() {

    //}

    //// Update is called once per frame
    //void Update() { 

    //}

    public void createButton() {
        createPanel.SetActive(true);
        setFuncationality(false);
    }

    //public void exitButton() {
    //    if (saved) {
    //        Application.Quit();
    //    } else {
    //        exitConfirmationPanel.SetActive(true);
    //        exitConfirmationPanel.transform.SetAsLastSibling();
    //    }
    //}

    public void saveButton() {
        setFuncationality(false);
        Debug.Log("Save Hit!");
        string encounter = EncounterStructure.saveList();
        savePanel.SetActive(true);
        savePanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = encounter;
        savePanel.transform.SetAsLastSibling();
    }

    public void loadButton() {
        setFuncationality(false);
        Debug.Log("Load Hit!");
        loadPanel.SetActive(true);
        loadPanel.transform.SetAsLastSibling();
    }

    public void resetButton() {
        setFuncationality(false);
        if (saved) {
            EncounterStructure.clearEncounter();
            setFuncationality(true);
        } else {
            resetConfirmationPanel.SetActive(true);
            resetConfirmationPanel.transform.SetAsLastSibling();
        }
    }

    //sub buttons

    public void saveClearExit() {
        setFuncationality(true);
        saved = true;
        EncounterStructure.clearEncounter();
        savePanel.SetActive(false);
    }

    public void saveExit() {
        setFuncationality(true);
        saved = true;
        savePanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
        savePanel.SetActive(false);
    }

    public void loadLoadExit() {
        try {
            //clear the encounter of any existing characters so we don't go over the maximum
            EncounterStructure.clearEncounter();

            string[] lines = loadPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text.Split('\n');

            if (lines.Length > 20) { throw new Exception("Too many characters!"); }

            for (int i = 0; i < lines.Length; i++) {
                if (!lines[i].Equals("")) {
                    string[] ch = lines[i].Split(',');
                    this.create(ch[0], ch[1], ch[2], ch[3], ch[4], ch[5]);
                }
            }

            setFuncationality(true);
            loadPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
            loadPanel.SetActive(false);
            EncounterStructure.activateEncounter();
            saved = false;
        } catch (Exception e) {
            Debug.LogException(e);
            StopAllCoroutines();
            StartCoroutine(FadeOutNotification("One of the lines is Invalid!"));
            EncounterStructure.clearEncounter();
        }
    }

    public void loadExit() {
        setFuncationality(true);
        loadPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
        loadPanel.SetActive(false);
    }

    public void changesMade() {
        saved = false;
    }

    public void resetConfirmCancel() {
        setFuncationality(true);
        resetConfirmationPanel.SetActive(false);
    }

    public void resetConfirmContinue() {
        setFuncationality(true);
        resetConfirmationPanel.SetActive(false);
        EncounterStructure.clearEncounter();
    }

    //general fuctions    
    //variables for testing inputs
    string chName;
    string armor;
    string initiative;
    string hp;
    string cHp;
    string cTemp;
    int successArmor;
    int successHealth;
    int successInt;
    int successCurr;
    int successTemp;

    //verifies that the inputs in the fields are valid for the correct values. Return false if any part is bad
    private bool verifyInput() {
        //catch a format exception for when a parse fails
        try {
            successHealth = int.Parse(hp);
            successArmor = int.Parse(armor);
            successInt = int.Parse(initiative);
            successCurr = int.Parse(cHp);
            successTemp = int.Parse(cTemp);
        } catch (System.FormatException e) {
            Debug.LogException(e);
            return false;
        }

        //each value to see if they match what is required
        if (chName == null || chName.Equals("")) { return false; }
        if (successArmor <= 0) { return false; }
        if (successHealth <= 0) { return false; }
        if (successInt <= 0) { return false; }
        if (successCurr <= 0 || successCurr > successHealth) { return false; }
        if (successTemp < 0) { return false; }

        return true;
    }

    public void create(string nameVal, string hpVal, string armorVal, string initiativeVal, string cHpVal, string cTempVal) {
        //get the values in the field. store them in class variables for multiple access points
        chName = nameVal;
        armor = armorVal;
        initiative = initiativeVal;
        hp = hpVal;
        cHp = cHpVal;
        cTemp = cTempVal;

        //check that the values are good
        if (verifyInput()) {
            //make the new UI element
            GameObject nChar = Instantiate(characterTemplate);

            //set the UI elements position/size/scale and finally call the startChar function for that characters script
            nChar.transform.parent = canvas.transform;
            nChar.GetComponent<RectTransform>().sizeDelta = characterTemplate.GetComponent<RectTransform>().sizeDelta;
            nChar.GetComponent<RectTransform>().localScale = characterTemplate.GetComponent<RectTransform>().localScale;
            nChar.GetComponent<RectTransform>().position = characterTemplate.GetComponent<RectTransform>().position;
            nChar.GetComponent<CharaterInteract>().startChar(chName, successHealth, successArmor, successInt, successCurr, successTemp);

            //finally add the UI gameobject to the singleton
            EncounterStructure.addChar(nChar, successHealth, successArmor, successInt, chName);
        } else {
            //If there was anything wrong with the inputs then throw out an error message
            throw new Exception("Invalid Input Expection!");
        }
    }

    //the coroutine method
    private IEnumerator FadeOutNotification(string message) {
        loadError.text = message;    //set the text of the gameobject to the message wanted
        float time = 0;                     //base time for when we start the fade out

        //set the base alpha and wait for a few seconds to give player time to read
        loadError.color = new Color(loadError.color.r, loadError.color.g, loadError.color.b, 1f);
        yield return new WaitForSeconds(1.5f);

        //loop to change the alpha gradually
        while (time < fadeTime) {
            time += Time.unscaledDeltaTime;     //update the time
            loadError.color = new Color(loadError.color.r,    //update the color
                                  loadError.color.g,
                                  loadError.color.b,
                                  Mathf.Lerp(1f, 0f, time / fadeTime));
            yield return null;                  //finish the loop
        }
    }

    //[SerializeField]
    //private Button btCreate;
    //[SerializeField]
    //private Button btSave;
    //[SerializeField]
    //private Button btLoad;
    //[SerializeField]
    //private Button btClear;

    public void setFuncationality(bool functionality) {
        //btCreate.enabled = functionality;
        //btSave.enabled = functionality;
        //btLoad.enabled = functionality;
        //btClear.enabled = functionality;
        mpBlurImage.SetActive(!functionality);
        EncounterStructure.setEncounterFunctionality(functionality);
    }
}