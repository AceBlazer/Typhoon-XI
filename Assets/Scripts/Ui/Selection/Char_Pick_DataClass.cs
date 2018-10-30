using UnityEngine;
using System.Collections;

public class Char_Pick_DataClass  {


	public CharacterClass.charName characterName;
	public string charPortraitDir;
	public string charPrefabDir;
	public bool _available;
	public string toolTip;

	public Char_Pick_DataClass Constructor ()
	{
		Char_Pick_DataClass capture = new Char_Pick_DataClass ();
		capture.characterName = characterName;
		capture._available = _available;
		capture.charPortraitDir = charPortraitDir;
		capture.charPrefabDir = charPrefabDir;
		capture.toolTip = toolTip;
		return capture;
		
		
	}
	

}
