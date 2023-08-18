using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PayValues", menuName = "ScriptableObjects/PayValues")]
public class PayValues : ScriptableObject
{

	// The dictionaries can be accessed throught a property
	[SerializeField]
	StringStringDictionary m_stringStringDictionary;
	public IDictionary<string, string> StringStringDictionary
	{
		get { return m_stringStringDictionary; }
		set { m_stringStringDictionary.CopyFrom(value); }
	}


}
