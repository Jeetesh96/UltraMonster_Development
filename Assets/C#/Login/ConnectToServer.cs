using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.NetworkInformation;

using System;
using System.Text;
using System.Net;
//using System.ComponentModel;
//using System.Threading;

public class ConnectToServer : MonoBehaviour
{

    [SerializeField] string registerUserUrl;
    [SerializeField] string signInUrl;
    [SerializeField] string resetPasswordUrl;
    [SerializeField] UnityEngine.UI.Text pingText;

    public  System.Action<bool> gotResponse;
    string responseData;

    #region Login In
    [HideInInspector] public UnityWebRequestResponeSuccessfulLogin webRequestSuccessfullLogin;
    [HideInInspector] public UnityWebRequestResponeWrongPass webRequestWrongPassword;
    [HideInInspector] public UnityWebRequestResponeWrongUser webRequestWrongUser;

    [HideInInspector] public ResponseDataWrongUser responseWrongUser;
    [HideInInspector] public ResponseDataWrongPassword responseWrongPassword;
    [HideInInspector] public ResponseDataLoginSuccessful responseLoginSuccessful;
    #endregion

    [HideInInspector] public SignUpResponse signUpResponse;

    [HideInInspector] public ResetPasswordServerResponse resetPasswordResponse;

    private enum CommunicationType
    {
        Register,
        Login,
        ResetPassword
    }


    /// <summary>
    /// Register Button Functionality
    /// </summary>
    /// <param name="jsonData"></param>
    public void RegisterUser(string jsonData)
    {
        StartCoroutine(CommunicateWithServer(jsonData, registerUserUrl , CommunicationType.Register));
    }


    /// <summary>
    /// Sign in button functionality
    /// </summary>
    /// <param name="jsonData"></param>
    public void SignIn(string jsonData)
    {
        StartCoroutine(CommunicateWithServer(jsonData, signInUrl , CommunicationType.Login));
    }


    public void ResetPassword(string jsonData)
    {
        StartCoroutine(CommunicateWithServer(jsonData, resetPasswordUrl, CommunicationType.ResetPassword));
    }


    /// <summary>
    /// Communicate with server to register and login
    /// </summary>
    /// <param name="jsonData"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator CommunicateWithServer(string jsonData , string url , CommunicationType type)
    {

        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        UnityWebRequest request = new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(string.Format("Error: {0}", request.error));
        }


        responseData = request.downloadHandler.text;
        Debug.Log(responseData);

        switch(type)
        {
            case CommunicationType.Login:

                #region Deserialize Response Data 
                webRequestSuccessfullLogin = JsonUtility.FromJson<UnityWebRequestResponeSuccessfulLogin>(responseData);
                webRequestWrongPassword = JsonUtility.FromJson<UnityWebRequestResponeWrongPass>(responseData);
                webRequestWrongUser = JsonUtility.FromJson<UnityWebRequestResponeWrongUser>(responseData);
                #endregion

                #region Get Payload JSON String from Response Data
                string wrongUserData = JsonUtility.ToJson(webRequestWrongUser.data);
                string wrongPasswordData = JsonUtility.ToJson(webRequestWrongPassword.data);
                string successfullLoginData = JsonUtility.ToJson(webRequestSuccessfullLogin.data);
                #endregion

                #region Deserialize Payload Data
                responseWrongUser = JsonUtility.FromJson<ResponseDataWrongUser>(wrongUserData);
                responseWrongPassword = JsonUtility.FromJson<ResponseDataWrongPassword>(wrongPasswordData);
                responseLoginSuccessful = JsonUtility.FromJson<ResponseDataLoginSuccessful>(successfullLoginData);
                #endregion

                if (responseLoginSuccessful.username != null)
                {
                    //Debug.Log(responseLoginSuccessful.user_id);
                    PlayerPrefs.SetString("USERNAME", responseLoginSuccessful.username);
                    PlayerPrefs.SetInt("USERID", responseLoginSuccessful.user_id);
                  
                }

                
                break;
            case CommunicationType.Register:
                signUpResponse = JsonUtility.FromJson<SignUpResponse>(responseData);
                break;
            case CommunicationType.ResetPassword:
                resetPasswordResponse = JsonUtility.FromJson<ResetPasswordServerResponse>(responseData);
                break;
        }
      
        gotResponse(request.downloadHandler.isDone);
    }


    private void Start()
    {
        StartCoroutine(PingRoutine());
    }

    IEnumerator  PingRoutine()
    {
        while(true)
        {
            System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send("8.8.8.8", timeout, buffer, options);

            GameManager.Instance.pingInMs = reply.RoundtripTime;
            pingText.text = reply.RoundtripTime.ToString() + " ms";

            yield return new WaitForSecondsRealtime(1);
        }
        
    }
}
