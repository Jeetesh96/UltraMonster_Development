using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


#region Register
[System.Serializable]
public class SignUp
{
    public string username;
    public string password;
    public string referral_code;
}

[System.Serializable]
public class SignUpResponse
{
    public string status;
    public string message;
}
#endregion


#region Login

[System.Serializable]
public class Login
{
    public string username;
    public string password;
}


#region Response String According to Payload
[System.Serializable]
public class UnityWebRequestResponeWrongPass
{
    public string status;
    public string message;
    public ResponseDataWrongPassword data;
}

[System.Serializable]
public class UnityWebRequestResponeWrongUser
{
    public string status;
    public string message;
    public ResponseDataWrongUser data;
}

[System.Serializable]
public class UnityWebRequestResponeSuccessfulLogin
{
    public string status;
    public string message;
    public ResponseDataLoginSuccessful data;
}
#endregion

#region Payload Data

[System.Serializable]
public class ResponseDataWrongPassword
{
    public string password;
}

[System.Serializable]
public class ResponseDataWrongUser
{
    public string user;
}

[System.Serializable]
public class ResponseDataLoginSuccessful
{
    public int user_id;
    public string username;
}
#endregion

#endregion

#region Reset Password

[System.Serializable]
public class ResetPassword
{
    public string user_id;
    public string password;
    public string confirm_password;
}

[System.Serializable]
public class ResetPasswordServerResponse
{
    public string status;
    public string message;
    public string error;
}

#endregion