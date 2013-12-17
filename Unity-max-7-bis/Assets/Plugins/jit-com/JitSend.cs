// mu (myu) Max-Unity Interoperability Toolkit
// Ivica Ico Bukvic <ico@vt.edu> <http://ico.bukvic.net>
// Ji-Sun Kim <hideaway@vt.edu>
// Keith Wooldridge <kawoold@vt.edu>
// With thanks to Denis Gracanin

// Virginia Tech Department of Music
// DISIS Interactive Sound & Intermedia Studio
// Collaborative for Creative Technologies in the Arts and Design

// Copyright DISIS 2008.
// mu is distributed under the GPL license (http://www.gnu.org/licenses/gpl.html)

using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class JitSend : MonoBehaviour {

public int portNo = 32000;
public string addr = "127.0.0.1";
public int packetTreshold = 100;

private TcpClient client;
private NetworkStream netStream;
private Queue updateSend;

private int msgCounter;
private float connectionAttempt;
private bool connection;

	void Update() {
		if (updateSend.Count != 0) {
			try {
				if (!connection) {
					connectionAttempt += Time.deltaTime;
					if (connectionAttempt > 1.0f) {
						connectionAttempt = 0.0f;
						try {
							client.Connect(addr, portNo);
							connection = true;
						}
						catch(Exception e) {
							connection = false;
						}
					}
					if (connection) {
						netStream = client.GetStream();
					}
					else if (msgCounter > packetTreshold) {
						updateSend.Clear();
						msgCounter = 0;
					}
				}
				if (connection) {
					string toWrite = (string)updateSend.Peek();
					byte[] output;
					output = Encoding.ASCII.GetBytes(toWrite);
					netStream.Write(output, 0, toWrite.Length);
					updateSend.Dequeue();
				}
			}
			//Called when netStream has been closed.
			catch (Exception e) {
				netStream.Close();
				client.Close();
				client = new TcpClient();
				connection = false;
				try {
					client.Connect(addr, portNo);
					connection = true;
				}
				catch(Exception se) {
					connection = false;
				}
				if (connection) netStream = client.GetStream();
			}
		}
	}

	// Use this for initialization
	void Start () {
		client = new TcpClient();
		updateSend = new Queue();
		msgCounter = 0;
		connectionAttempt = 0;
		connection = false;
		if (packetTreshold == 0) packetTreshold = 1;
	}
	
	// Externally called from jitMessenger script associated with various objects
	public void write (string toWrite) {
		updateSend.Enqueue(toWrite);
		msgCounter++;
	}
}
