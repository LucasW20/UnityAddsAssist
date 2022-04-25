using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Handles the create character system
 * @author Lucas_C_Wright
 * @start 04-02-2022
 * @version 04-23-2022
 */
public class CreateSystem : MonoBehaviour {
    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private TMP_InputField armorInput;
    [SerializeField]
    private TMP_InputField healthInput;
    [SerializeField]
    private TMP_InputField iniInput;
    [SerializeField]
    private GameObject bossToggle;
    [SerializeField]
    private GameObject characterTemplate;
    [SerializeField]
    private GameObject bossCharTemplate;
    [SerializeField]
    private GameObject canvas;
    [SerializeField] 
    private TextMeshProUGUI notificationText;
    private float fadeTime = 3.5f;

    // Start is called before the first frame update
    void Start() {
        //make sure that the input fields are empty
        nameInput.text = "";
        armorInput.text = "";
        healthInput.text = "";
        iniInput.text = "";
    }

    public void cancelCreate() {
        gameObject.SetActive(false);
        canvas.transform.GetChild(1).GetComponent<MPButtonBehaviour>().setFuncationality(true);
    }

    //variables for testing inputs
    //private string chName;
    //private string armor;
    //private string initiative;
    //private string hp;
    private int successArmor;
    private int successHealth;
    private int successInt;

    //verifies that the inputs in the fields are valid for the correct values. Return false if any part is bad
    private bool verifyInput(string chName, string armor, string initiative, string hp) {
        //catch a format exception for when a parse fails
        try {
            successHealth = int.Parse(hp);
            successArmor = int.Parse(armor);
            successInt = int.Parse(initiative);
        } catch (System.FormatException e) {
            Debug.LogException(e);
            return false;
        }

        //each value to see if they match what is required
        if (chName == null || chName.Equals("")) { return false; }
        if (successArmor <= 0) { return false; }
        if (successHealth <= 0) { return false; }
        if (successInt <= 0) { return false; }
        
        return true;
    }

    //called by the Create button. Gets the necessary info from the input fields and then creates the UI element. 
    public void create() {
        //get the values in the field. store them in class variables for multiple access points
        //chName = nameInput.text;
        //armor = armorInput.text;
        //initiative = iniInput.text;
        //hp = healthInput.text;

        //check that the values are good
        if (verifyInput(nameInput.text, armorInput.text, iniInput.text, healthInput.text)) {
            //make the new UI element
            GameObject nChar;

            if (bossToggle.GetComponent<Toggle>().isOn) {
                //create boss panel
                nChar = Instantiate(bossCharTemplate);

                //set the UI elements position/size/scale and finally call the startChar function for that characters script
                nChar.transform.parent = canvas.transform;
                nChar.GetComponent<RectTransform>().sizeDelta = bossCharTemplate.GetComponent<RectTransform>().sizeDelta;
                nChar.GetComponent<RectTransform>().localScale = bossCharTemplate.GetComponent<RectTransform>().localScale;
                nChar.GetComponent<RectTransform>().position = bossCharTemplate.GetComponent<RectTransform>().position;
                nChar.GetComponent<CharaterInteract>().startChar(nameInput.text, successHealth, successArmor, successInt);

                //add to singleton as boss character
                EncounterStructure.addChar(nChar, successHealth, successArmor, successInt, nameInput.text, true);
            } else {
                //create normal panel
                nChar = Instantiate(characterTemplate);

                //set the UI elements position/size/scale and finally call the startChar function for that characters script
                nChar.transform.parent = canvas.transform;
                nChar.GetComponent<RectTransform>().sizeDelta = characterTemplate.GetComponent<RectTransform>().sizeDelta;
                nChar.GetComponent<RectTransform>().localScale = characterTemplate.GetComponent<RectTransform>().localScale;
                nChar.GetComponent<RectTransform>().position = characterTemplate.GetComponent<RectTransform>().position;
                nChar.GetComponent<CharaterInteract>().startChar(nameInput.text, successHealth, successArmor, successInt);

                //add to singleton as normal character
                EncounterStructure.addChar(nChar, successHealth, successArmor, successInt, nameInput.text, false);
            }

            //Show in the screen, and set the functunality to on.
            nChar.SetActive(true);
            canvas.transform.GetChild(1).GetComponent<MPButtonBehaviour>().setFuncationality(true);

            //finally reset the fields after a successfull creation and hide the create window
            nameInput.text = "";
            armorInput.text = "";
            healthInput.text = "";
            iniInput.text = "";
            gameObject.SetActive(false);  
        } else {
            //If there was anything wrong with the inputs then throw out an error message
            StopAllCoroutines();
            StartCoroutine(FadeOutNotification("One of the fields has an invalid input!", notificationText));
        }
    }
    
    //the coroutine method
    private IEnumerator FadeOutNotification(string message, TextMeshProUGUI element) {
        element.text = message;    //set the text of the gameobject to the message wanted
        float time = 0;            //base time for when we start the fade out

        //set the base alpha and wait for a few seconds to give player time to read. Makes the message POP into view, catching the players attention
        element.color = new Color(element.color.r, element.color.g, element.color.b, 1f);
        yield return new WaitForSeconds(1.5f);

        //loop to change the alpha gradually
        while (time < fadeTime) {
            time += Time.unscaledDeltaTime;     //update the time
            element.color = new Color(element.color.r,    //update the color
                                  element.color.g,
                                  element.color.b,
                                  Mathf.Lerp(1f, 0f, time / fadeTime));
            yield return null;                  //finish the loop
        }
    }
}
