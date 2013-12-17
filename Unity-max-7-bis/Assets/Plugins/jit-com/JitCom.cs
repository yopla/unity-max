using UnityEngine;
using System.Collections;

public class JitCom : MonoBehaviour {
	
	
	
		void Start() {
		//This is simply to prevent infinite loop of spawning and destruction of objects
		//Do one or the other
		//stop_spawn = 0;	
		}
	
	
	
	
	public void reposition(GameObject tgt, string xLoc, string yLoc, string zLoc) {
		Vector3 newLoc = new Vector3((float)System.Convert.ToDouble(xLoc),
			(float)System.Convert.ToDouble(yLoc), (float)System.Convert.ToDouble(zLoc));
		tgt.transform.position = newLoc;
	}
	

	public void scale(GameObject tgt, string xVal, string yVal, string zVal) {
		Vector3 newScale = new Vector3((float)System.Convert.ToDouble(xVal),
			(float)System.Convert.ToDouble(yVal), (float)System.Convert.ToDouble(zVal));
		tgt.transform.localScale += newScale;		
	}

	public void absoluteScale(GameObject tgt, string xVal, string yVal, string zVal) {
		Vector3 newScale = new Vector3((float)System.Convert.ToDouble(xVal),
			(float)System.Convert.ToDouble(yVal), (float)System.Convert.ToDouble(zVal));
		tgt.transform.localScale = newScale;
	}

	
	
	public void move(GameObject tgt, string xVal, string yVal, string zVal) {
		tgt.transform.Translate((float)System.Convert.ToDouble(xVal), 
			(float)System.Convert.ToDouble(yVal), -(float)System.Convert.ToDouble(zVal));
	}
	
	public void rotate(GameObject tgt, string xVal, string yVal, string zVal) {

		tgt.transform.Rotate((float)System.Convert.ToDouble(xVal), 
			(float)System.Convert.ToDouble(yVal), (float)System.Convert.ToDouble(zVal));
	}
	
	public void absoluteRotate(GameObject tgt, string xVal, string yVal, string zVal) {
		float toX = (float)System.Convert.ToDouble(xVal);
		float toY = (float)System.Convert.ToDouble(yVal);
		float toZ = (float)System.Convert.ToDouble(zVal);
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3(toX, toY, toZ);
		tgt.transform.rotation = rot;
	}
	
	
	
}