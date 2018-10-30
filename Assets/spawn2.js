

	var  node1:Transform;
	var  minionPrefab:GameObject;
	var  spawn:boolean=true;
	var  did:boolean = false;
	var  sec:int;
	var  gm:GameObject;


	
	// Update is called once per frame
	function Update () {
		sec= gm.GetComponent("gametime").intsec;
		if (sec==15 || sec==45) {
		    if (!did) {
			spawnMinion ();
			did = true;
		    }
		}
	}


		
	function spawnMinion() {
		var i = 0;
		do {
			yield WaitForSeconds (1.5f);
			i++;
			Instantiate (minionPrefab, node1.position, Quaternion.identity);
		} while(i < 7);

		yield WaitForSeconds (10);
		did=false;
	}



