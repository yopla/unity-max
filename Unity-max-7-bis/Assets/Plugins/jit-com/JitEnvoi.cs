using UnityEngine;
using System.Collections;


public class JitEnvoi : MonoBehaviour {
	

	// / / / //////   pour l'envoie
	public GameObject send;
	private	JitSend writer;
	private bool valid = false;
	// envoie !!!
	
	
		void Start() {
		///////// Pour l'envoie !!!
		//
		if (send == null) send = GameObject.Find("Main Camera");
		if (send != null) writer = (JitSend)send.GetComponent("JitSend");
		if (writer != null) valid = true;
		////////// envoie !!
	
	}
	
	public void renvoi(GameObject tgt, string nom) {
		
		//print(tgt);
		//print(nom);
		
		if (valid) {
			
			float[] tmprot = {tgt.transform.eulerAngles.x, tgt.transform.eulerAngles.y, tgt.transform.eulerAngles.z};				
			float[] tmppos = {tgt.transform.position.x, tgt.transform.position.y, tgt.transform.position.z};	
			float[] tmpfield = {tgt.camera.fieldOfView};
			
			write(nom,"M", tmppos);
			write(nom, "R", tmprot);
			write(nom, "foc", tmpfield);
			
			}
	}
	
	
		// // / /envoie !!! / / 
	
	 void write(string nom, string method, float[] var) {
		//print (name);
		string toWrite = nom + " " + method;
		for(int i = 0; i < var.Length; i++){
			toWrite += " " + var[i];
		}
		toWrite += ";\n";
		writer.write(toWrite);
	}
	
// envoie !! //
	
	
	
	
	
}
