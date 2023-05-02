using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI location;
    public Button nextButton;
    
    //UI elements
    public GameObject map1, map2, player;
    public Button forward, backward, left, right;
    
    //connect with scriptable objects
    public LocationScriptableObject currentLocation;

    //init File.IO stuff
    const string TEXT_NAME = "textNum.txt";
    const string TEXT_DIR = "/Resources/Texts/";
    const string DATA_DIR = "/Resources/Data/";
    string TEXT_PATH;
    
    int lineIndex = 0;
    int charIndex = 0;
    string[] fileLines;

    public AudioSource typewriterSound;

    //numbers to load texts
    int currentTextFile = 0;
    public int CurrentTextFile
    {
        get { return currentTextFile; }
        set
        {
            currentTextFile = value;

            if (currentTextFile < 4)
            {
                DialogueSystem();
                InvokeRepeating("TypeChar", 0, 0.05f);
            }
            else if (currentTextFile == 4)  //we enter the house here
            {
                //disable next button for now
                nextButton.gameObject.SetActive(false);
                
                //show the player and 1 fl minimap
                map1.gameObject.SetActive(true);
                player.gameObject.SetActive(true);
                
                //enable the rest of UI elements
                location.gameObject.SetActive(true);
                EnableLocationButtons(true);
                
                //load scriptable object files
                UpdateLocation();
            }
        }
    }

    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //no next button when the game begins
        nextButton.gameObject.SetActive(false);
        location.gameObject.SetActive(false);
        
        //disable UI elements until player enters the ground floor
        map1.gameObject.SetActive(false);
        map2.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        EnableLocationButtons(false);

        //define file paths
        TEXT_PATH = Application.dataPath + TEXT_DIR + TEXT_NAME;
        
        //clear the text at the first frame of each scene
        dialogue.text = string.Empty;
        
        //start the dialogue
        DialogueSystem();
        InvokeRepeating("TypeChar", 0, 0.05f);
        
    }

    //controlling all direction buttons in one go to make life easier
    void EnableLocationButtons(bool state)
    {
        forward.gameObject.SetActive(state);
        backward.gameObject.SetActive(state);
        left.gameObject.SetActive(state);
        right.gameObject.SetActive(state);
    }

    //TODO Fix the typing time issue
    void DialogueSystem()
    {
        //play typewriter sound
        typewriterSound.PlayOneShot(typewriterSound.clip);
        
        //define the new text path to load
        string newTextPath = TEXT_PATH.Replace("Num", currentTextFile + "");

        //put each line in the text file into an array
        fileLines = File.ReadAllLines(newTextPath);

        // for (int lineNum = 0; lineNum < fileLines.Length; lineNum++)
        // {
        //     string lineContents = fileLines[lineNum];
        //     
        //     //break down the line into individual characters and put in an array
        //     char[] lineChar = lineContents.ToCharArray();
        //
        //     //start typing individual characters!!
        //      for (int charNum = 0; charNum < lineChar.Length + 1; charNum++)
        //      {
        //          //every character takes 0.05 sec to type
        //          if (charNum < lineChar.Length)
        //          {
        //              dialogue.text += lineChar[charNum];
        //          }
        //          else if (charNum == lineChar.Length)
        //          {
        //              dialogue.text += "\n" + "\n";
        //          }
        //      }
        // }
        nextButton.gameObject.SetActive(true);
        
    }

    //TODO: figure out how to invoke with parameters
    //  void Type(int num, char[] charArray)
    // {
        // //type characters
        // if (num < charArray.Length)
        // {
        //     dialogue.text += charArray[num];
        // }
        // //when we are at the end of the line, make an empty line
        // else if (num == charArray.Length)
        // {
        //     dialogue.text += "\n" + "\n";
        // }
    // }

    void TypeChar()
    {
        //stop typing if we've finished all the lines
        if (lineIndex >= fileLines.Length)
        {
            //don't repeat invoke this function again
            CancelInvoke("TypeChar");
            
            //stop the typewriter sound
            typewriterSound.Stop();
            
            //stop running this code
            return;
        }

        //find the current line
        string typeLine = fileLines[lineIndex];

        //if we haven't finished typing a line
        if (charIndex < typeLine.Length)
        {
            //type the character
            dialogue.text += typeLine[charIndex];
            //char index increases for the next repeat invoke
            charIndex++;
        }
        //if we have finished typing a line
        else
        {
            //add an empty line
            dialogue.text += "\n" + "\n";
            
            //move to the next line
            lineIndex++;
            
            //start at the first character
            charIndex = 0;
        }
    }
     
     
     //TODO incorporate scriptable objects with dialogue system
     void ScrObjWithDialogue()
     {
         //load the text from current scriptable object
         location.text = currentLocation.locationName;
         dialogue.text = currentLocation.locationDescription.text;

     }

     void UpdateLocation()
     {
         //load the text from current scriptable object
         location.text = currentLocation.locationName;
         dialogue.text = currentLocation.locationDescription.text;

         //if currentLocation's forwardLocation doesn't exist
         //forward button is not interactable
         if (!currentLocation.forwardLocation)
         {
             forward.interactable = false;
         }
         else
         {
             forward.interactable = true;
             currentLocation.forwardLocation.backwardLocation = currentLocation;
         }
         
         //if current location's backwardLocation doesn't exist
         if (!currentLocation.backwardLocation)
         {
             backward.interactable = false;
         }
         else
         {
             backward.interactable = true;
             currentLocation.backwardLocation.forwardLocation = currentLocation;
         }
         
         //if current's left doesn't exist
         if (!currentLocation.leftLocation)
         {
             left.interactable = false;
         }
         else
         {
             left.interactable = true;
             currentLocation.leftLocation.rightLocation = currentLocation;
         }
         
         //if current's right doesn't exist
         if (!currentLocation.rightLocation)
         {
             right.interactable = false;
         }
         else
         {
             right.interactable = true;
             currentLocation.rightLocation.leftLocation = currentLocation;
         }
     }
     
     //the button directions
     //TODO: remember to update player position
     public void ButtonDirection(int direction)
     {
         switch (direction)
         {
             case 0:    //forward
                 currentLocation = currentLocation.forwardLocation;
                 break;
             case 1:    //backward
                 currentLocation = currentLocation.backwardLocation;
                 break;
             case 2:    //left
                 currentLocation = currentLocation.leftLocation;
                 break;
             case 3:    //right
                 currentLocation = currentLocation.rightLocation;
                 break;
         }
         
         //update the texts
         UpdateLocation();
     }
     
     
}
