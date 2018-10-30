
var node1 : Transform;
var minionPrefab : GameObject;
var spawning : boolean = true;
     
function Start () {
     yield WaitForSeconds(60);
     spawnMinion();
}
     
function Update () {
     
}
     
function spawnMinion () {
     
	while(spawning) {
     		 
	    for (var i=0;i<7;i++)  {
	    yield WaitForSeconds(0.5f);
	    Instantiate (minionPrefab, node1.position, node1.rotation) ; }
    yield WaitForSeconds(30);
	}
}