using UnityEngine.UI;
using UnityEngine;

//This class is used to store all the info about the characters
[System.Serializable]
public class CharacterClass
{
	//Name of the characters
	public enum charName
	{
		
		Empty,
		
		Galilei,
		
		Allycra,

		
		
	} 
	public string prefabPath;
	public charName characterName;
	public Sprite charPortrait;

	public DmgDataclass basic;
	public DmgDataclass q;
	public DmgDataclass w;
	public DmgDataclass e;
	public DmgDataclass r;
}


//This class is used to store the data about each character spells
[System.Serializable]
public class DmgDataclass {


	//Name of the spell
	public enum name
	{
		
		_basic,
		
		_Q,

		_W,

		_E,

		_R

		
	}
	//Type of spell
	public enum type
	{
		
		skillShot_floor,
		
		skillShot_front,
		
		lockedShot_rangue,
		
		aoe_instant

	}
	//Launch position of the spell
	public enum launchPos
	{
		
		_floor,
		
		_b,

		_q,

		_w,

		_e,

		_r,

		_sky,
		
		center
		
		
		
	}

	public int weaponLvl;
	public name weaponName;
	public type weaponType;
	public launchPos LaunchPos;
	public float manaCost;
	public float ad;
	public float ap;
	public float rangue;
	public float cdr;
	public float cdrTStamp;


	
	public DmgDataclass Constructor ()
	{
		DmgDataclass capture = new DmgDataclass ();
		capture.weaponName = weaponName;
		capture.weaponType = weaponType;
		capture.LaunchPos = LaunchPos;
		capture.rangue = rangue;
		capture.cdr = cdr;
		capture.weaponLvl = weaponLvl;
		capture.manaCost = manaCost;
		capture.ad = ad;
		capture.ap = ap;

		
		return capture;


	}
	

}

