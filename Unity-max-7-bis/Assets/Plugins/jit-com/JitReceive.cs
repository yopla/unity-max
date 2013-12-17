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
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class JitReceive : MonoBehaviour {
	
	JitCom jitcom;
	JitCustomEvents jitCustom;
	JitEnvoi jitenvoi;
	
	
	
	
	public int portNo;
	public int maxObjects;
	
	//struct for pointing to various objects
	private struct objectList {
		public string objName;		public GameObject objPointer;
		
		public void set(string s, GameObject gp) {
			objName = s;
			objPointer = gp;
		}	}
	private int numObjects;
	private objectList[] o;
	
	
	
	
	private TcpClient incoming_client;
	private NetworkStream netStream;
	private TcpListener server;
	private bool waiting;

	// Use this for initialization
	void Start () {
		
		jitCustom = (JitCustomEvents)GetComponent("JitCustomEvents");
		jitcom = (JitCom)GetComponent("JitCom");
		jitenvoi = (JitEnvoi)GetComponent("JitEnvoi");
		
	
		
		
		
		if (portNo == 0) portNo = 32003;
		if (maxObjects == 0) maxObjects = 1024;
		waiting = false;
		server = new TcpListener(IPAddress.Any, portNo);
		server.Start();
		numObjects = 0;
		o = new objectList[maxObjects];
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
				
		
		string s;
		string[] values;
		
		if (server.Pending()) {
			incoming_client = server.AcceptTcpClient();
			netStream = incoming_client.GetStream();

			waiting = true;
		}
		while (waiting && netStream.DataAvailable) {
			try {
				int numread = 0;
				byte[] tmpbuf = new byte[1024];
				numread = netStream.Read(tmpbuf, 0, tmpbuf.Length);

				s = Encoding.ASCII.GetString(tmpbuf, 0, numread);
				s = s.Replace("\n","");
				values = s.Split(';');
			
				if (values.Length > 1) {
					for (int i = 0; i < (values.Length-1); i++) {
						Parse(values[i]);
					}
				}
				else Parse(values[0]);
			}
			//Called when netStream fails to read from the stream.
			catch (IOException e) {
				waiting = false;
				netStream.Close();
				incoming_client.Close();
			}
			//Called when netStream has been closed already.
			catch (ObjectDisposedException e) {
				waiting = false;
				incoming_client.Close();
			}
		}	
	}
	

	
	
	public void Parse(string toParse) {

		GameObject target = null;
		int i;
		bool found = false;
		
		string[] values = toParse.Split(' ');

		if (numObjects > 0) {
			for (i = 0; i < numObjects && !found; i++) {
				if (values[0].Equals(o[i].objName)) {
					found = true;
					target = o[i].objPointer;
				}
			}
		}
		
		if (numObjects == 0 || !found) {
			target = GameObject.Find(values[0]);
			if (target) {
				o[numObjects].set(values[0], target);
				numObjects++;
			}
		}

		if (!target) {
			print("Requested object " + values[0] + " not found.");
		}
		else {
			switch (values[1]) {
				
				case "take": 			
					jitenvoi.renvoi(target, values[0]);
				break;
				
				case "c":
					if (jitCustom) {
						string[] val = new string[values.Length - 3];
						for (int j = 0; j < values.Length - 3; j++) {
							val[j] = values[j + 3];
						}
						custom(target, values[2], val);
					}
					else {
						Debug.Log("Custom script not found.");
					}
					break;
				
				
				case "m":
					if (values.Length == 5) {
						jitcom.move(target, values[2], values[3], values[4]);
					}
					break;
				case "M":
					if (values.Length == 5) {
						jitcom.reposition(target, values[2], values[3], values[4]);
					}
					break;
				case "r":
					if (values.Length == 5) {
						jitcom.rotate(target, values[2], values[3], values[4]);
					}
					break;
				case "R":
					if (values.Length == 5) {
						jitcom.absoluteRotate(target, values[2], values[3], values[4]);
					}
					break;
				case "s":
					if (values.Length == 5) {
						jitcom.scale(target, values[2], values[3], values[4]);
					}
					break;
				case "S":
					if (values.Length == 5) {
						jitcom.absoluteScale(target, values[2], values[3], values[4]);
					}
					break;
				
			}
		}
	}
	
	
	void custom(GameObject tgt, string method, string[] val) {
		int sz = val.Length;
		float[] param = new float[sz];
		for (int i = 0; i < val.Length; i++) {
			param[i] = (float)System.Convert.ToDouble(val[i]);
		}
		jitCustom.run (tgt, method, param);
	}
	
	
	
	
	
}
