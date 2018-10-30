
/*

var	 range:int ;
var	 gun:GameObject ;
var	 sample:GameObject ;
var	 bullet:GameObject ;
var	 moveSpeed : int;
var	 controller: CharacterController;
var	firing:boolean=false;
var	canmove: boolean=true;
var	newposition : Vector3;
var	 instantiated:boolean = false;
var	cp:GameObject ;
var	enemy:GameObject ;
var	basic:GameObject ;

	

	function Update () {
		enemy = GameObject.FindWithTag ("BluePlayerTag");
		if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {
			transform.LookAt (enemy.transform);
			GetComponent<Animation> ().Play ("Alycra - Basic");
			attack (); 	
		}


		var RMB = Input.GetMouseButtonDown(1);

		if (RMB) {

			var hit:RaycastHit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit) && hit.transform.tag == "BluePlayerTag") {
				enemy = GameObject.FindWithTag ("BluePlayerTag");

				print ("enemy selected");
				RecievedMove (hit.point);

				if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {
					transform.LookAt (enemy.transform);
					GetComponent<Animation> ().Play ("Alycra - Basic");
					attack (); 	
				}
				else {
					RecievedMove (hit.point); 
				}
			}
			if (Physics.Raycast (ray, out hit) && hit.transform.tag == "Ground" && cp) {
				Destroy (cp);
			//	canmove = false;
			    instantiated = false;
		    }

		}

	}


	function RecievedMove( movePos: Vector3){
		newposition = movePos;
		if (!instantiated ) {
			instantiated = true;
			cp = Instantiate (sample, enemy.transform.position, enemy.transform.rotation) as GameObject; 
			if (Vector3.Distance (transform.position, enemy.transform.position) > range && canmove) {
				transform.LookAt (enemy.transform);
				rm.canmove = true;
				rm.newposition = enemy.transform.position;

			} 

		}
	}

	function attack() {
		if (!firing) {
			firing=true;
			basic = Instantiate (bullet, gun.transform.position, gun.transform.rotation) as GameObject;
			yield WaitForSeconds (1);
			firing=false; }
	}


		
	
	*/

