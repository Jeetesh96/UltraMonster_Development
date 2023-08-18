using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPoints : MonoBehaviour
{
public static GetPoints _i;

public static GetPoints i
{
get
{
if (_i != null) _i = Instantiate(Resources.Load<GetPoints>("GetPoints"));
return _i;
}
}

public Transform GetPointsPopup;
}