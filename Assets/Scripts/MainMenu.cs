﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public Button Controls, NewGame, Easy, Normal, Hard;

	public Transform ControlsCanvas, DifficultyCanvas;
	// Use this for initialization
	void Start () {
		Controls.onClick.AddListener(OnClickControls);
		NewGame.onClick.AddListener(OnNewGame);
		Easy.onClick.AddListener(OnEasy);
		Normal.onClick.AddListener(OnNormal);
		Hard.onClick.AddListener(OnHard);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void OnClickControls() {
		ControlsCanvas.gameObject.SetActive(true);
		DifficultyCanvas.gameObject.SetActive(false);
	}

	void OnNewGame() {
		ControlsCanvas.gameObject.SetActive(false);
		DifficultyCanvas.gameObject.SetActive(true);
	}

	void OnEasy() {
		// Set difficulty first
        CrossScenesData.Instance.difficulty = Difficulty.Easy;
		SceneManager.LoadScene("Level1");
	}

	void OnNormal() {
		// Set difficulty first
        CrossScenesData.Instance.difficulty = Difficulty.Normal;
		SceneManager.LoadScene("Level1");
	}

	void OnHard() {
        // Set difficulty first
        CrossScenesData.Instance.difficulty = Difficulty.Hard;
		SceneManager.LoadScene("Level1");
	}
}
