using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : Collectable {

	protected override void OnRabitHit (HeroRabbit rabit) {
		this.CollectedHide ();
		LevelController.current.addCoins (1);

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
