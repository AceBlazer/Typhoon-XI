using UnityEngine;
[System.Serializable]
//A class used to hold attackers info when a player or enemy gets attacked
public class AttackersDataClass {
	
	public string _Name;
	public GameObject _Obj;
	public float _time;

	public AttackersDataClass Constructor ()
	{
		AttackersDataClass capture = new AttackersDataClass ();
		capture._Name = _Name;
		capture._Obj = _Obj;
		capture._time = _time;
		return capture;
	}

}