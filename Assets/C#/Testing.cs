using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
void Start()
{
PointPopUp.Create(Vector3.zero, 300);
}
private void Update()
{
if (Input.GetMouseButtonDown(0))
{
PointPopUp.Create(UtilsClass.GetMouseWorldPosition(), 100);
}
}
}