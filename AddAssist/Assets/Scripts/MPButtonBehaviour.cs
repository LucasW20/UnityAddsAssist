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
 * @version 04-24-2022
 */
public class MPButtonBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject createPanel;
    [SerializeField]
    private GameObject savePanel;
    [SerializeField]
    private GameObject loadPanel;
    [SerializeField]
    private GameObject resetConfirmationPanel;
    [SerializeField]
    private TextMeshProUGUI loadError;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject characterTemplate;
    [SerializeField]
    private GameObject bossCharTemplate;
    [SerializeField]
    private GameObject mpBlurImage;
    [SerializeField]
    private GameObject turnPanel;
    [SerializeField]
    private TextMeshProUGUI turnText;
    private bool saved = true;
    private float fadeTime = 3.5f;

    //event call for the Create Combatant button
    public void createButton() {
        createPanel.SetActive(true);
        setFuncationality(false);
    }

    //event call for the save button
    public void saveButton() {
        setFuncationality(false);
        Debug.Log("Save Hit!");
        string encounter = EncounterStructure.saveList();
        savePanel.SetActive(true);
        savePanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = encounter;
        savePanel.transform.SetAsLastSibling();
    }

    //event call for the load button
    public void loadButton() {
        setFuncationality(false);
        Debug.Log("Load Hit!");
        loadPanel.SetActive(true);
        loadPanel.transform.SetAsLastSibling();
    }

    //event call for the clear button
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

    //event call for the Turn order button
    public void generateInitiativeOrder() {
        //only run if there are current characters
        if (EncounterStructure.charListCount() == 0) { 
            return; 
        }

        //get the string of order from the EncounterStucture
        string order = EncounterStructure.getInitiativeOrder();

        //paste the string to a new initiative order panel
        turnText.text = order;

        //enable the panel
        turnPanel.SetActive(true);
    }

    //sub buttons
    //event call for the clear button in the save panel
    public void saveClearExit() {
        setFuncationality(true);
        saved = true;
        EncounterStructure.clearEncounter();
        savePanel.SetActive(false);
    }

    //event call for the exit button in the save panel
    public void saveExit() {
        setFuncationality(true);
        saved = true;
        savePanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
        savePanel.SetActive(false);
    }

    //event call for the load button in the load panel
    public void loadLoadExit() {
        try {
            //clear the encounter of any existing characters so we don't go over the maximum
            EncounterStructure.clearEncounter();

            //load the input into an array split by the lines
            string[] lines = loadPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text.Split('\n');

            //max of 20 characters
            if (lines.Length > 20) { throw new Exception("Too many characters!"); }

            //for each line convert it into an array that is a character
            for (int i = 0; i < lines.Length; i++) {
                if (!lines[i].Equals("")) {
                    string[] ch = lines[i].Split(',');
                    this.create(ch[0], ch[1], ch[2], ch[3], ch[4], ch[5], ch[6]);  //create the UI element for the character
                }
            }

            //after a successfull creation of each character turn everything on
            setFuncationality(true);
            loadPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
            loadPanel.SetActive(false);
            EncounterStructure.activateEncounter();
            saved = false;
        } catch (Exception e) {
            //if the creation fails for any reason abort and clear the encounter
            Debug.LogException(e);
            StopAllCoroutines();
            StartCoroutine(FadeOutNotification("One of the lines is Invalid!"));
            EncounterStructure.clearEncounter();
        }
    }

    //event call for the exit button in the load panel
    public void loadExit() {
        setFuncationality(true);
        loadPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = "";
        loadPanel.SetActive(false);
    }

    //called by other scripts to let this script know when changes have been made 
    public void changesMade() {
        saved = false;
    }

    //event call for the cancel button in the reset confirm panel
    public void resetConfirmCancel() {
        setFuncationality(true);
        resetConfirmationPanel.SetActive(false);
    }

    //event call for the continue button in the reset confirm panel
    public void resetConfirmContinue() {
        setFuncationality(true);
        resetConfirmationPanel.SetActive(false);
        EncounterStructure.clearEncounter();
    }

    //general fuctions    
    //variables for testing inputs
    //TODO find a better way to handle these variables
    string chName;
    string armor;
    string initiative;
    string hp;
    string cHp;
    string cTemp;
    string cBoss;
    int successArmor;
    int successHealth;
    int successInt;
    int successCurr;
    int successTemp;
    bool successIsBoss; 

    //verifies that the inputs in the fields are valid for the correct values. Return false if any part is bad
    private bool verifyInput() {
        //catch a format exception for when a parse fails
        try {
            successHealth = int.Parse(hp);
            successArmor = int.Parse(armor);
            successInt = int.Parse(initiative);
            successCurr = int.Parse(cHp);
            successTemp = int.Parse(cTemp);
            int tempBoss = int.Parse(cBoss);
            if (tempBoss == 1) { successIsBoss = true; } else { successIsBoss = false; }
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

    //create functions specifically for loading an encounter
    public void create(string nameVal, string hpVal, string armorVal, string initiativeVal, string cHpVal, string cTempVal, string sIsCharBoss) {
        //get the values in the field. store them in class variables for multiple access points
        chName = nameVal;
        armor = armorVal;
        initiative = initiativeVal;
        hp = hpVal;
        cHp = cHpVal;
        cTemp = cTempVal;
        cBoss = sIsCharBoss;

        //check that the values are good
        if (verifyInput()) {
            //make the new UI element
            GameObject nChar;

            if (successIsBoss) {
                //create boss UI
                nChar = Instantiate(bossCharTemplate);

                //set the UI elements position/size/scale
                nChar.transform.parent = canvas.transform;
                nChar.GetComponent<RectTransform>().sizeDelta = bossCharTemplate.GetComponent<RectTransform>().sizeDelta;
                nChar.GetComponent<RectTransform>().localScale = bossCharTemplate.GetComponent<RectTransform>().localScale;
                nChar.GetComponent<RectTransform>().position = bossCharTemplate.GetComponent<RectTransform>().position;
            } else {
                //create normal UI
                nChar = Instantiate(characterTemplate);

                //set the UI elements position/size/scale
                nChar.transform.parent = canvas.transform;
                nChar.GetComponent<RectTransform>().sizeDelta = characterTemplate.GetComponent<RectTransform>().sizeDelta;
                nChar.GetComponent<RectTransform>().localScale = characterTemplate.GetComponent<RectTransform>().localScale;
                nChar.GetComponent<RectTransform>().position = characterTemplate.GetComponent<RectTransform>().position;
            }

            //finally call the startChar function for that characters script
            nChar.GetComponent<CharaterInteract>().startChar(chName, successHealth, successArmor, successInt, successCurr, successTemp);

            //finally add the UI gameobject to the singleton
            EncounterStructure.addChar(nChar, successHealth, successArmor, successInt, chName, successIsBoss);
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

    //FIRST call in the functionality line. Disables certain parts while other panels are open. 
    public void setFuncationality(bool functionality) {
        mpBlurImage.SetActive(!functionality);
        EncounterStructure.setEncounterFunctionality(functionality);
    }
}