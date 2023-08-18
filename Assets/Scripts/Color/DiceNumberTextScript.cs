using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceNumberTextScript : MonoBehaviour {

	Text text;
	public static int diceNumber1;
	public static int diceNumber2;
	public static int diceNumber3;
	public static bool spindone = false;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	//	text.text = diceNumber.ToString ();
	}
}
