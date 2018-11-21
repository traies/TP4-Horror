using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private bool mainMenu = true;
    private bool play = false;
    private bool difficulty = false;
    private bool controls = false;
    private Difficulty _difficulty;
    private AudioSource audioSource;
    public GUISkin skin;
    public AudioClip audioClip;

    // UI variables
    public float menuCoordX;
    public float menuCoordY;
    public float menuWidth;
    public float menuHeight;
    public float titlePosX;
    public float titlePosY;
    public float titleWidth;
    public float titleHeight;
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
        CrossScenesData.difficulty = Difficulty.Normal; // As a default value
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.Box(new Rect(Screen.width * menuCoordX, Screen.height * menuCoordY, Screen.width * menuWidth, Screen.height * menuHeight), "");
        GUI.Label(new Rect(Screen.width * titlePosX, Screen.height * titleHeight, Screen.width * titleWidth, Screen.height * titleHeight), new GUIContent("Inside", "Inside"));
        
        if (mainMenu)
        {
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * firstButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Play", "Play")))
            {
                mainMenu = false;
                play = true;
                Application.LoadLevel("GenerationScene");
            }
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * secondButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Difficulty", "Difficulty")))
            {
                mainMenu = false;
                difficulty = true;
            }
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * thirdButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Controls", "Controls")))
            {
                mainMenu = false;
                controls = true;
            }
        }
        else if (difficulty)
        {
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * firstButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Easy", "Easy")))
            {
                difficulty = false;
                mainMenu = true;
                _difficulty = Difficulty.Easy;
            }
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * secondButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Normal", "Normal")))
            {
                difficulty = false;
                mainMenu = true;
                _difficulty = Difficulty.Normal;
            }
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * thirdButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Hard", "Hard")))
            {
                difficulty = false;
                mainMenu = true;
                _difficulty = Difficulty.Hard;
            }
            CrossScenesData.difficulty = _difficulty;
        }
        else if (controls)
        {
            if (GUI.Button(new Rect(Screen.width * allButtonsPosX, Screen.height * thirdButtonPosY, Screen.width * buttonWidth, Screen.height * buttonHeight), new GUIContent("Return", "Return")))
            {
                mainMenu = true;
                controls = false;
            }
            GUI.Label(new Rect(Screen.width * allButtonsPosX, Screen.height * firstButtonPosY, Screen.width * buttonWidth, Screen.height * 3 * buttonHeight),
                "Move: WASD or Arrow keys \n" +
                "Aim: Right Mouse \n" +
                "Shoot: Left Mouse\n" +
                "Change Weapon: Q \n" +
                "Reload: R\n" +
                "Use health pack: V\n"+
                "Toggle flashlight: F\n" +
                "Change flashlight battery: T\n" +
                "Pick up object: E\n" +
                "Run : Shift\n");
        }
    }
}
