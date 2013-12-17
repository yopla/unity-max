// mu (myu) Max-Unity Interoperability Toolkit
// Ivica Ico Bukvic <ico@vt.edu> <http://ico.bukvic.net>
// Ji-Sun Kim <hideaway@vt.edu>
// Keith Wooldridge <kawoold@vt.edu>
// With thanks to Denis Gracanin

// Virginia Tech Department of Music
// DISIS Interactive Sound & Intermedia Studio
// Collaborative for Creative Technologies in the Arts and Design

// Copyright DISIS 2008.
// mu is distributed under the GPL license v3 (http://www.gnu.org/licenses/gpl.html)

using UnityEngine;
using System.Collections;

public class JitCustomEvents : MonoBehaviour {
	public Material[] mats;
	
	
	//private int stop_spawn;

	void Start() {
		//This is simply to prevent infinite loop of spawning and destruction of objects
		//Do one or the other
		//stop_spawn = 0;	
	
	}
	
	
	

	// You can define here various custom functions that can be called from Max
	// These can affect any object
	public void run(GameObject target, string varA, float[] varB) {
		//if (varA != 0) {
			switch (varA) {
				
				// Parse according to requested event ID
				
			//	case 2:
		//			changeMaterial(target, varB);
	//				break;
				case "ouverture":
					changeSpot(target, varB);
					break;
				case "couleur":
					changeSpotColor(target, varB);
					break;
				case "force":
					changeSpotPower(target, varB);
					break;
					
				case "foc":
					changeCamField(target, varB);
					break;
			
			
				case "frust":
					changeCamFrust(target, varB);
					break;
			
				
			//}
		}
	}
	
	// Custom functions
	// In this case changeColor changes color of the object given RGB values
	// changeMaterial changes object's material
	void changeColor(GameObject tgt, float[] val) {
		
		
		// It is always a good idea to check sanity of the arguments
		if (val.Length == 3) {		
			// Create color from arguments and apply it
			Color tmp = new Color(val[0]/255.0f, val[1]/255.0f, val[2]/255.0f);
			tgt.renderer.material.color = tmp;
		}
	}
	
	void changeMaterial(GameObject tgt, float[] val) {
		
	//	var materials : tgt.renderer.Material[];
		//Once again sanity check
		if (val.Length == 1) {	
			
			//Material tmp =  new Material ("defaultMat");
			//Material tmp2 = Material("defaultMat2");
			//
		
			if (val[0] == 0.0f) tgt.renderer.material = mats[0];
			else if (val[0] == 1.0f) tgt.renderer.material = mats[1];	
			else if (val[0] == 2.0f) tgt.renderer.material= mats[2];
			//else if (val[0] == 3.0f) tgt.renderer.material.shader = Shader.Find ("backgroundshadowcatcher");
			
			/*if (val[0] == 0.0f) tgt.renderer.material.shader = Shader.Find ("Diffuse");
			else if (val[0] == 1.0f) tgt.renderer.material.shader = Shader.Find ("Transparent/Diffuse");	
			else if (val[0] == 2.0f) tgt.renderer.material.shader = Shader.Find ("FX/Matte Shadow");
			else if (val[0] == 3.0f) tgt.renderer.material.shader = Shader.Find ("backgroundshadowcatcher");
			*/
		}	
	}
	
	void changeSpot(GameObject tgt, float[] val) {
		 
		// It is always a good idea to check sanity of the arguments
		if (val.Length == 1) {			
			tgt.light.spotAngle = val[0];
		}
	}
	
		void changeSpotColor(GameObject tgt, float[] val) {
		if (val.Length == 3) {	
			Color tmp = new Color(val[0]/255.0f, val[1]/255.0f, val[2]/255.0f);
			tgt.light.color = tmp;
		//	tgt.((Behaviour)GetComponent("Halo")).enabled = true;
			
		}
	}
	void changeSpotPower(GameObject tgt, float[] val) {
		if (val.Length == 1) {	
			tgt.light.intensity = val[0];
		}
	}
	
	void changeCamField(GameObject tgt, float[] val) {
		if (val.Length == 1) {	
			tgt.camera.fieldOfView = val[0];
		}
	}
	
	void changeCamFrust(GameObject tgt, float[] val) {
		///if (val.Length == 1) {	
		//	tgt.camera.fieldOfView = val[0];
		//}
		
		/*float left = val[0] //-0.2;
		float right  = val[1] //0.2;
		float bottom  = val[2] //-0.2;
		float top  = val[3] //0.2;
		*/
		
		Camera cam = tgt.camera;
    	Matrix4x4 m = PerspectiveOffCenter(val[0], val[1], val[2], val[3],
        cam.nearClipPlane, cam.farClipPlane );
    	cam.projectionMatrix = m;
		
	}

	
	
	static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	 {		       
	    float x  = (float) (2.0 * near) / (right - left);
	    float y  =  (float)(2.0 * near) / (top - bottom);
	    float a =  (float)(right + left) / (right - left);
	    float b  = (float) (top + bottom) / (top - bottom);
	    float c  = -(float)(far + near) / (far - near);
	    float d  = -(float)(2.0 * far * near) / (far - near);
	    float e  = (float)(-1.0);
	
	    Matrix4x4 m =new Matrix4x4();
	    m[0,0] = x;  m[0,1] = 0;  m[0,2] = a;  m[0,3] = 0;
	    m[1,0] = 0;  m[1,1] = y;  m[1,2] = b;  m[1,3] = 0;
	    m[2,0] = 0;  m[2,1] = 0;  m[2,2] = c;  m[2,3] = d;
	    m[3,0] = 0;  m[3,1] = 0;  m[3,2] = e;  m[3,3] = 0;
				
	    return m;
		
		Debug.Log("sfzefze");
	}
}
