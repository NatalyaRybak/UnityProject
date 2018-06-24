using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static LevelController current;
	Vector3 startingPosition;
	private int coins = 0;

	void Awake() {
		current = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setStartPosition(Vector3 pos) {
		this.startingPosition = pos;
	}
	public void onRabbitDeath(HeroRabbit rabit) {
		//При смерті кролика повертаємо на початкову позицію
		rabit.transform.position = this.startingPosition;
	}

	public void addCoins(int n) {
		coins += n;
	}
}
