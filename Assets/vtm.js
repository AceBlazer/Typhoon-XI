﻿//****** Donations are greatly appreciated.  ******
//****** You can donate directly to Jesse through paypal at  https://www.paypal.me/JEtzler   ******
        
var smooth:int; 
private var targetPosition:Vector3;

function Update () {

    if(Input.GetKeyDown(KeyCode.Mouse1)) {

        smooth=1;
        var playerPlane = new Plane(Vector3.up, transform.position);
        var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        var hitdist = 0.0;

        if (playerPlane.Raycast (ray, hitdist)) {

            var targetPoint = ray.GetPoint(hitdist);
            targetPosition = ray.GetPoint(hitdist);
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = targetRotation;
        }
    }
    transform.position = Vector3.Slerp (transform.position, targetPosition, Time.deltaTime * smooth);  
}