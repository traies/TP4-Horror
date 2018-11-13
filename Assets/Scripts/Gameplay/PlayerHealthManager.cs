using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthManager : MonoBehaviour {
	private PlayerManager _playerManager;
	private Animator _animator;
	public float HealthPoints;
	private float _hp;
	public Image HealthBar;

	private int _healthPacks;
	public int InitialHealthPacks;
	
	public AudioSource UseHealthPackSound;
	public AudioSource NoHealthPackSound;
	public AudioSource DeathSound;
	public TextMeshProUGUI HealthPacksUI;
	public Camera PlayerCamera;
	public int HitSmoothFrames;
	public float HitDesviation;
	private bool dead;

	// Use this for initialization
	void Start () {
		_hp = HealthPoints;
		_animator = GetComponent<Animator>();
		_playerManager = GetComponent<PlayerManager>();
		_healthPacks = InitialHealthPacks;
	}
	
	// Update is called once per frame
	void Update () {
		HealthBar.fillAmount = _hp / HealthPoints;
		HealthPacksUI.text = _healthPacks.ToString();
		if(Input.GetButtonDown("UseHealthPack")) {
			if (_hp < HealthPoints && _healthPacks > 0) {
				// Use health pack	
				UseHealthPackSound.Play();
				_healthPacks--;
				_hp = HealthPoints;
			} else {
				// No health pack sound.
				NoHealthPackSound.Play();
			}
		}
	}

	private void DeathAnimation()
	{
		_animator.SetTrigger("Dead");
		DeathSound.Play();
		_playerManager.Die();
		_animator.SetLayerWeight(1, 0);
	}

	public void TakeDamage(float damage) {
		if (!dead) {
			_hp -= damage;
			if (_hp <= 0) {
				dead = true;
				DeathAnimation();
			} else {
				// Camera alteration
				// PlayerCamera.transform.Rotate();
				StartCoroutine(HitCameraImpact());
			}
		}
	}

	private IEnumerator HitCameraImpact() {
		var randomVec = Random.insideUnitSphere;
		randomVec.z = 0;
		for (int i = 0; i < HitSmoothFrames; i++) {
			if (dead) {
				break;
			}
			PlayerCamera.transform.Rotate((randomVec / HitSmoothFrames) * (HitSmoothFrames - i) * HitDesviation);
			yield return null;
		}
		// Camera.transform.Rotate(Vector3.left * RecoilAmount * RecoilSmoothness);
		// for (int i = 1; i <= RecoilAmount; i++) {
		// 	yield return null;
		// 	Camera.transform.Rotate(Vector3.left * (RecoilAmount - i) * RecoilSmoothness);
		// }
	}

	public void AddHealthPack() {
		_healthPacks++;
	}
}
