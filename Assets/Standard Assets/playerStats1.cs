using UnityEngine;
using System.Collections;

public class playerStats1 : MonoBehaviour {

	public int health;
	public int maxhealth;
	float pourcentage;
	int barlength;
	public int armor;
	public int resist;
	public bool shielded;
	public int shield;
	public GameObject canvas;
	public GUITexture hpbar;

	// Use this for initialization
	void Start () {
		maxhealth = 600;
		health = maxhealth;
		shielded = false;
		shield = 0;
		armor = 30;
		resist = 50;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		pourcentage = health / maxhealth;
		barlength = (int)pourcentage * 100;
		hpbar.GetComponent<GUITexture>().pixelInset = new Rect(hpbar.pixelInset.x,hpbar.pixelInset.y,barlength,hpbar.pixelInset.height);

		if (health <= 0) {
			health = 0;
			die ();
		}

		if (shield == 0) {
			shielded = false;
		}*/

	}

	public void getHit(int damage, int magic,int truedmg,int crit,int armpen, int magpen) {
		
			health = health - (magic - (resist - magpen));
			health = health - ((damage + crit) - (armor - armpen));
			health = health - truedmg;
		
	}

	public void die () {
		//die
	}
}
