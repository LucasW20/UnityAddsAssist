using UnityEngine;
using TMPro;

/**
 * Handles the functionality for the utility buttons on character panels
 * @author Lucas_C_Wright
 * @start 04-15-2022
 * @version 04-22-2022
 */
public class CharacterUtility : MonoBehaviour {
    [SerializeField]
    private GameObject miniChar;
    [SerializeField]
    private GameObject maxChar;
    [SerializeField]
    private GameObject MPObect;
    [SerializeField]
    private GameObject blurCharacter;
    [SerializeField]
    private GameObject teamMenu;
    [SerializeField]
    private GameObject teamSword;
    [SerializeField]
    private GameObject teamShield;
    [SerializeField]
    private GameObject teamFire;
    [SerializeField]
    private GameObject teamSkull;
    private MPButtonBehaviour mainPanel;
    private GameObject currentTeam;

    // Start is called before the first frame update
    void Start() {
        currentTeam = teamSkull;
        mainPanel = MPObect.GetComponent<MPButtonBehaviour>();
    }

    //event call for the 'X' button. Removes the panel from the scene and the list of characters
    public void exitCharacter() {
        EncounterStructure.removeChar(gameObject);
        Destroy(gameObject);
        mainPanel.changesMade();
    }

    //event call for the minimize button. Minimizes the character panel for better organization
    public void minimizeChar() {
        //update specifics for the minimized panel. Clamp area and team.
        GetComponent<CharacterDragDrop>().changeClamp();
        miniChar.transform.GetChild(2).Find(currentTeam.name).gameObject.SetActive(true);

        miniChar.SetActive(true);
        miniChar.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<CharaterInteract>().nameText.text;
        maxChar.SetActive(false); 
    }

    //event call for the maximize button. Maximizes the character panel for better organization
    public void expandChar() {
        //update specifics
        GetComponent<CharacterDragDrop>().changeClamp();

        //turn off the team in the mini panel when expanding to ready for a team change
        miniChar.transform.GetChild(2).Find(currentTeam.name).gameObject.SetActive(false);

        miniChar.SetActive(false);
        maxChar.SetActive(true);
    }

    /**
     * changes the functionality of the character by turning on the blur image, blocking the interactables
     * @param functionality true for turning on function
     */
    public void setCharacterFunctionality(bool functionality) {
        //FINAL in the functionality call line
        blurCharacter.SetActive(!functionality);
    }

    //event call for the team button. Opens the team menu to change the team the character is on 
    public void openTeamMenu() {
        setCharacterFunctionality(false);
        teamMenu.SetActive(true);
    }

    //event calls for all the buttons in the team window. Changes the team that the character is on.
    public void selectTeam(string newTeam) {
        currentTeam.SetActive(false);

        //swap the team
        switch (newTeam) {
            case "Sword":
                currentTeam = teamSword;
                break;
            case "Shield":
                currentTeam = teamShield;
                break;
            case "Skull":
                currentTeam = teamSkull;
                break;
            case "Fire":
                currentTeam = teamFire;
                break;
            default:
                break;
        }

        currentTeam.SetActive(true);
        setCharacterFunctionality(true);
        teamMenu.SetActive(false);
    }
}
