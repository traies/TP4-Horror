using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashlightManager : MonoBehaviour {
	public Light Spotlight;
	public AudioSource AudioSource;
	private bool _on;
	public float FullCharge, BatteryDrainPerSecond;
	public int Batteries;
	private float _battery;
	public Image BatteryBar;
	public TextMeshProUGUI BatteriesText;
	public AudioClip TurnOnOff, ChargeBattery;

	// Use this for initialization
	void Start () {
		_on = true;
		_battery = FullCharge;
	}

	// Update is called once per frame
	void Update () {

		BatteryBar.fillAmount = _battery / FullCharge;
		BatteriesText.text = Batteries.ToString();
		if (Input.GetButtonDown("Flashlight")) {
			_on = !_on;
			AudioSource.clip = TurnOnOff;
			AudioSource.Play();
		}

		if (Input.GetButtonDown("Battery") && Batteries > 0) {
			Batteries--;
			_battery = FullCharge;
			AudioSource.clip = ChargeBattery;
			AudioSource.Play();
		}

		if (_on) {
			if (_battery > 0) {
				Spotlight.enabled = true;
				_battery -= BatteryDrainPerSecond * Time.deltaTime;
			} else {
				Spotlight.enabled = false;
			}
		} else {
			Spotlight.enabled = false;
		}
	}

	public void AddBattery() {
		Batteries++;
	}
}
