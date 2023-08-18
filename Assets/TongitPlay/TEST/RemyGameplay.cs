using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyGameplay : MonoBehaviour {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private GameObject selectionPrefab;
	private GameObject newSelection;
	public bool isSelected;

	private void qOnMouseDown()
	{
		if (newSelection == null)
		{

			newSelection = Instantiate(selectionPrefab, transform.position, Quaternion.identity);
			newSelection.transform.SetParent(gameObject.transform);
			newSelection.SetActive(false);
		}
		isSelected = !isSelected;

		if (isSelected)
		{
			newSelection.SetActive(true);
		}
		else
		{
			newSelection.SetActive(false);
		}

	}

}
