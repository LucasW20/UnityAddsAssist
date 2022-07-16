using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Handles the data structures and logic on the different character objects in the scene. Singleton
 * @author Lucas_C_Wright
 * @start 04-02-2022
 * @version 04-24-2022
 */
public static class EncounterStructure {

    //struct for a character. Private because its only used here for the character List
    private struct Character {
        public GameObject ob;
        public int health;
        public int armor;
        public int initiative;
        public string chName;
        public bool isBoss;

        //public constructor but because the struct is private you can't make characters outside of this class
        public Character(GameObject UIObject, int maxHealth, int armorClass, int encounterInitiative, string characterName, bool isCharBoss) {
            ob = UIObject;
            health = maxHealth;
            armor = armorClass;
            initiative = encounterInitiative;
            chName = characterName;
            isBoss = isCharBoss;
        }

        //to string for save/load purposes. prints out all the information about the character including current values to a single line
        public override string ToString() {
            string res = "" + chName + "," + health + "," + armor + "," + initiative + "," +
                ob.transform.GetChild(0).GetChild(9).GetChild(2).GetComponent<TextMeshProUGUI>().text + "," +
                ob.transform.GetChild(0).GetChild(10).GetChild(2).GetComponent<TextMeshProUGUI>().text + ",";

            if (isBoss) { res += "1"; } else { res += "0"; }

            return res;
        }
    }

    //private list to prevent desyncing the list and characters on screen
    private static List<Character> charList = new List<Character>();

    //public method to add a character to the list. this is used to verify a good add. Returns true with a successfull add, false otherwise.
    public static bool addChar(GameObject nObject, int nHealth, int nArmor, int nIni, string nName, bool isCharBoss) {
        try {
            charList.Add(new Character(nObject, nHealth, nArmor, nIni, nName, isCharBoss));
        } catch (Exception e) { 
            //if any exception is thrown for any reason return false as it is a bad add.
            Debug.LogException(e);
            return false;
        }

        return true;
    }

    //method called to set all characters in the list to show on the screen. Used when loading as each character isn't individually added.
    public static bool activateEncounter() {
        for (int i = 0; i < charList.Count; ++i) {
            charList[i].ob.SetActive(true);
        }

        return true;
    }

    //deletes a character from the List to prevent desync.
    public static bool removeChar(GameObject rCh) {
        for (int i = 0; i < charList.Count; i++) {
            //using the gameobject object to find the match.
            if (charList[i].ob == rCh) {
                charList.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    //method to remove all characters from the screen and the list. 
    public static bool clearEncounter() {
        while (charList.Count > 0) {
            GameObject.Destroy(charList[0].ob);
            charList.RemoveAt(0);
        }

        return true;
    }

    //method for saving the list of characters. returns a string of multiple lines, one for each character in the list. 
    public static string saveList() {
        string encounter = "";

        for (int i = 0; i < charList.Count; i++) {
            encounter += charList[i].ToString() + "\n";
        }

        return encounter;
    }

    //changes the functionality of each character in the list/scene. Used for when a window from the main controls are open.
    public static void setEncounterFunctionality(bool functionality) {
        for (int i = 0; i < charList.Count; i++) {
            //turn off the buttons
            charList[i].ob.GetComponent<CharacterUtility>().setCharacterFunctionality(functionality);
        }
    }

    //gets a string of all the characters in order of initiative
    public static string getInitiativeOrder() {
        //create a temporary list and fill it with the singleton character list
        List<Character> tempList = new List<Character>();
        for (int i = 0; i < charList.Count;i++) {
            tempList.Add(charList[i]);
        }

        //sort the list from highest intitiative to lowest
        tempList.Sort((s2, s1) => s1.initiative.CompareTo(s2.initiative));
        string order = "";
        
        //check for duplicates at each index and build the string
        for (int i = 0; i < tempList.Count; i++) {
            bool duplicate = false;

            for (int j = 0; j < tempList.Count; j++) {
                //if a duplicate is found at a different index then make it so the name is surrounded by *'s
                if (i != j && tempList[i].initiative == tempList[j].initiative) {
                    duplicate = true;
                }
            }

            //update the string
            if (duplicate) {
                order += "*" + tempList[i].chName + "*\n";
            }
            else {
                order += i + 1 + ". " + tempList[i].chName + "\n";
            }
        }
        
        return order;
    }

    //getter for the number of characters in the List
    public static int charListCount() {
        return charList.Count;
    }
}
