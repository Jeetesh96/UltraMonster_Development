using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointPopUp : MonoBehaviour
{
public static PointPopUp Create(Vector3 position, int PointsAmount)
{
Transform PointsPopUpTransform = Instantiate(GetPoints.i.GetPointsPopup, position, Quaternion.identity);
PointPopUp pointPopUp = PointsPopUpTransform.GetComponent<PointPopUp>();
pointPopUp.Setup(PointsAmount);

return pointPopUp;
}
private TextMeshPro textmesh;

private void Awake()
{
textmesh = transform.GetComponent<TextMeshPro>();
}

public void Setup(int PointAmount)
{
textmesh.SetText(PointAmount.ToString());
}

private void Update()
{
float moveYSpeed = 20;
transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
}
}