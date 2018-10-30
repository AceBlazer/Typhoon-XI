

var CamSpeed = 0.50;
var GUIsize = 25;
var target : GameObject;




function Start() {

}

function Update () {

   

  
        var recdown = Rect (0, 0, Screen.width, GUIsize);
        var recup = Rect (0, Screen.height-GUIsize, Screen.width, GUIsize);
        var recleft = Rect (0, 0, GUIsize, Screen.height);
        var recright = Rect (Screen.width-GUIsize, 0, GUIsize, Screen.height);


        if(recup.Contains(Input.mousePosition)) {
        if(transform.position.z > 0) {
        transform.Translate(0 ,0 ,-CamSpeed ,Space.World ); }
        }
        if(recdown.Contains(Input.mousePosition)) {
        if(transform.position.z < 25) {
        transform.Translate(0 ,0 ,CamSpeed ,Space.World ); }
        }
        if(recright.Contains(Input.mousePosition)) {
        if(transform.position.x > -43) {
        transform.Translate(-CamSpeed ,0 ,0 ,Space.World ); }
        }
        if(recleft.Contains(Input.mousePosition)) {
        if(transform.position.x < 18) {
        transform.Translate(CamSpeed ,0 ,0 ,Space.World ); }
        }

   
    



}