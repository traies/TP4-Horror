using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private bool mainMenu = true;
    private bool play = false;
    private bool highScores = false;
    private bool controls = false;
    private AudioSource audioSource;
    public GUISkin skin;
    public AudioClip audioClip;

    // UI variables
    public float menuCoordX;
    public float menuCoordY;
    public float menuWidth;
    public float menuHeight;
    public float buttonWidth;
    public float buttonHeight;
    public float allButtonsPosX;
    public float firstButtonPosY;
    public float secondButtonPosY;
    public float thirdButtonPosY;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.Box(new Rect(Screen.width * menuCoordX, Screen.height * menuCoordY, Screen.width * menuWidth, Screen.height * menuHeight), "");

        if (mainMenu)
        {
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * firstButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Play", "Play")))
            {
                mainMenu = false;
                play = true;
                Application.LoadLevel("GenerationScene");
            }
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * secondButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Controls", "Controls")))
            {
                mainMenu = false;
                controls = true;
            }
            //if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * thirdButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Controls", "Controls")))
            //{
            //    mainMenu = false;
            //    controls = true;
            //}
        }
        //else if (highScores)
        //{
        //    GUI.Label(new Rect(Screen.width * allButtonsPosX, Screen.height * firstButtonPosY, Screen.width * buttonWidth, Screen.height * 3 * buttonHeight),
        //        "Current highscore\n");
        //    if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * thirdButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Return", "Return")))
        //    {
        //        mainMenu = true;
        //        highScores = false;
        //    }
        //}
        else if (controls)
        {
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * thirdButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Return", "Return")))
            {
                mainMenu = true;
                controls = false;
            }
            GUI.Label(new Rect(Screen.width * allButtonsPosX, Screen.height * firstButtonPosY, Screen.width * buttonWidth, Screen.height * 3 * buttonHeight),
                "Move : \n" +
                "Aim : \n" +
                "Shoot : \n" +
                "Change Weapon : \n" +
                "Charge weapon : \n" +
                "Change flashlight battery : \n" +
                "Pick up object : \n" +
                "Run : \n");
        }
    }
}
