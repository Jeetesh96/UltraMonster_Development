using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomescreenManager : MonoBehaviour
{

    [SerializeField] Image loadingBar;
    [SerializeField] GameObject registrationPanelGb;
    [SerializeField] ConnectToServer server;

    [Header("User Profile") , Space(10)]
    [SerializeField] Text[] username;
    [SerializeField] Text[] userId;
    [SerializeField] Text loadingPercentageText;

    private void Start()
    {
        StartCoroutine(LoadSplashScreen());
        SetProfilePicture();
        InitializeSound();
    }



    IEnumerator LoadSplashScreen()
    {
        while(loadingBar.fillAmount < 1)
        {
            loadingBar.fillAmount += Time.deltaTime / 4;
            loadingPercentageText.text = ((int)(loadingBar.fillAmount * 100)).ToString() + "%";
            yield return null;
        }


        if (PlayerPrefs.GetString("REMEMBERUSER", "NO") == "YES")
            HomeScreenTransit();
        else if(PlayerPrefs.GetString("REMEMBERUSER" , "NO") == "NO")
            registrationPanelGb.SetActive(true);
    }


    #region Registration 

    #region Variables

    [Header("Register User") , Space(10)]
    [SerializeField] private InputField userNameRegistrationInputField;
    [SerializeField] private InputField passwordRegistrationInputField;
    [SerializeField] private InputField AgentCodeInputField;
    [SerializeField] private Text registerInfo;

    [Header("Login") , Space(10)]
    [SerializeField] private InputField userNameLoginInputField;
    [SerializeField] private InputField passwordLoginInputField;
    [SerializeField] private Text loginInfo;
    [SerializeField] private GameObject seePassword;
    [SerializeField] private GameObject hidePassword;
    
    [SerializeField] private GameObject passwordInputFieldText;


    [SerializeField] private GameObject passwordTextGb;
    [SerializeField] private GameObject tickGb;
    [SerializeField] private GameObject loginScreenGb;

    [Header("Reset Password"), Space(10)]
    [SerializeField] private InputField newPasswordInputField;
    [SerializeField] private InputField confirmNewPasswordInputField;
    [SerializeField] private Text resetPasswordInfo;

    #endregion

    public void RegisterUser()
    {
        if(!string.IsNullOrEmpty(userNameRegistrationInputField.text) &&
            !string.IsNullOrEmpty(passwordRegistrationInputField.text) &&
            !string.IsNullOrEmpty(AgentCodeInputField.text))
        {
            SignUp signUp = new SignUp
            {
                username = userNameRegistrationInputField.text,
                password = passwordRegistrationInputField.text,
                referral_code = AgentCodeInputField.text

            };

            string data = JsonUtility.ToJson(signUp);
            server.RegisterUser(data);

            
            server.gotResponse += (init) =>
            {
                userNameRegistrationInputField.text = "";
                passwordRegistrationInputField.text = "";
                AgentCodeInputField.text = "";
                if(server.signUpResponse.status == "200" || server.signUpResponse.status == "201")
                {
                    registerInfo.text = server.signUpResponse.message;
                }
                else
                {
                    registerInfo.text = "either format error or wrong agent code";
                }

                StartCoroutine(ClearInfo());
                
            };
        }
    }


    public void Login()
    {
        if (!string.IsNullOrEmpty(userNameLoginInputField.text) &&
           !string.IsNullOrEmpty(passwordLoginInputField.text))
        {
            //Debug.Log("User name : " + userNameRegistrationInputField.text + " Password : " + passwordLoginInputField.text);
            Login login = new Login
            {
                username = userNameLoginInputField.text,
                password = passwordLoginInputField.text
            };

            string data = JsonUtility.ToJson(login);

            server.SignIn(data);

            server.gotResponse += (init) =>
            {
                //Debug.Log(server.response.data);
                if (server.webRequestSuccessfullLogin.message == "Login success" || 
                server.webRequestWrongPassword.message == "Login success" || 
                server.webRequestWrongUser.message == "Login success")
                {
                    Debug.Log("login successful");
                    loginScreenGb.SetActive(false);
                    HomeScreenTransit();

                }
                else
                {
                    if(server.responseWrongUser.user != null)
                    {
                        loginInfo.text = server.responseWrongUser.user;
                    }

                    if (server.responseWrongPassword.password  != null)
                    {
                        loginInfo.text = server.responseWrongPassword.password;
                    }

                    StartCoroutine(ClearInfo());
                }
            };
            
        }
        else
        {
            loginInfo.text = "username or password cannot be null";
            StartCoroutine(ClearInfo());
        }
    }

    public void ResetPassword()
    {
        if(!string.IsNullOrEmpty(newPasswordInputField.text) && !string.IsNullOrEmpty(confirmNewPasswordInputField.text))
        {
            if (newPasswordInputField.text == confirmNewPasswordInputField.text)
            {
                ResetPassword reset = new ResetPassword()
                {
                    user_id = PlayerPrefs.GetInt("USERID").ToString(),
                    password = newPasswordInputField.text,
                    confirm_password = confirmNewPasswordInputField.text
                };

                string jsonData = JsonUtility.ToJson(reset);
                server.ResetPassword(jsonData);

                server.gotResponse += (init) =>
                {
                    Debug.Log(server.resetPasswordResponse.status);
                    if (server.resetPasswordResponse.status == "200")
                    {
                        resetPasswordInfo.text = "Password changed successfully";
                    }
                    else
                    {
                        resetPasswordInfo.text = "Please check password format";
                    }
                };
            }
            else
                resetPasswordInfo.text = "Both password does not match";
        }



        StartCoroutine(ClearInfo());

    }

    public void ShowPassword()
    {
        if(!string.IsNullOrEmpty(passwordLoginInputField.text))
        {
            if (seePassword.activeInHierarchy)
            {
                seePassword.SetActive(false);
                hidePassword.SetActive(true);
                passwordInputFieldText.SetActive(false);
                passwordTextGb.GetComponent<Text>().text = passwordLoginInputField.text;
                passwordTextGb.SetActive(true);
            }
            else
            {
                hidePassword.SetActive(false);
                seePassword.SetActive(true);
                passwordInputFieldText.SetActive(true);
                passwordTextGb.GetComponent<Text>().text = "";
                passwordTextGb.SetActive(false);
            }
        }
       
    }

    public void RememberMe()
    {
        if (tickGb.activeInHierarchy)
        {
            PlayerPrefs.SetString("REMEMBERUSER", "NO");
            tickGb.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString("REMEMBERUSER", "YES");
            tickGb.SetActive(true);
        }
    }

    IEnumerator ClearInfo()
    {
        yield return new WaitForSeconds(2);
        loginInfo.text = "";
        registerInfo.text = "";
        resetPasswordInfo.text = "";
    }

    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        tickGb.SetActive(false);
        userNameLoginInputField.text = "";
        passwordLoginInputField.text = "";

        if (GameManager.Instance.bgMusicSource.isPlaying)
            PlayerPrefs.GetString("MUSIC", "ON");
        else
            PlayerPrefs.GetString("MUSIC", "OFF");
    }

    #endregion

    #region HomeScreen

    [Header("Home Screen") , Space(10)]
    [SerializeField] private GameObject homeScreenGb;

    void HomeScreenTransit()
    {
        homeScreenGb.SetActive(true);
        foreach(Text t in username)
        {
            t.text = PlayerPrefs.GetString("USERNAME");
        }

        foreach(Text t in userId)
        {
            t.text = "ID:" + PlayerPrefs.GetInt("USERID").ToString();
        }
    }

    public void PlayGame(int _sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneNumber);
    }

    #endregion


    #region Sound Manager

    [Header("Music") , Space(10)]
    [SerializeField] Slider volumeAmountControl;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle soundToggle;
    [SerializeField] Text musicText;
    [SerializeField] Text soundText;
    [SerializeField] GameObject musicOnGb;
    [SerializeField] GameObject soundOnGb;   


    void InitializeSound()
    {
        volumeAmountControl.value = PlayerPrefs.GetFloat("VOLUME", 1);

        if (PlayerPrefs.GetString("MUSIC", "ON") == "ON")
        {
            GameManager.Instance.bgMusicSource.Play();
            musicToggle.isOn = true;
            musicOnGb.SetActive(false);
            musicText.text = "ON";
        }
        else if(PlayerPrefs.GetString("MUSIC", "ON") == "OFF")
        {
            musicToggle.isOn = false;
            musicOnGb.SetActive(true);
            musicText.text = "OFF";
        }

        if (PlayerPrefs.GetString("SOUND", "ON") == "ON")
        {
            soundToggle.isOn = true;
            soundOnGb.SetActive(false);
            soundText.text = "ON";
        }
        else if (PlayerPrefs.GetString("SOUND", "ON") == "OFF")
        {
            soundToggle.isOn = false;
            soundOnGb.SetActive(true);
            soundText.text = "OFF";
        }
    }

    public void ManageVolume()
    {
        PlayerPrefs.SetFloat("VOLUME", volumeAmountControl.value);
        GameManager.Instance.bgMusicSource.volume = PlayerPrefs.GetFloat("VOLUME", 1);
        
    }

    public void Music()
    {
        if(GameManager.Instance.bgMusicSource.isPlaying)
        {
            PlayerPrefs.SetString("MUSIC" , "OFF");
            GameManager.Instance.bgMusicSource.Stop();
            musicText.text = "OFF";
        }
        else
        {
            PlayerPrefs.SetString("MUSIC", "ON");
            GameManager.Instance.bgMusicSource.Play();
            musicText.text = "ON";
        }

        if (musicToggle.isOn)
        {
            musicOnGb.SetActive(false);
        }
        else
        {
            musicOnGb.SetActive(true);
        }
    }

 

    public void Sound()
    {
        if (GameManager.Instance.soundSource.isPlaying)
        {
            PlayerPrefs.SetString("SOUND", "OFF");
            soundText.text = "OFF";
        }
        else
        {
            PlayerPrefs.SetString("SOUND", "ON");
            soundText.text = "ON";
        }

        if (soundToggle.isOn)
        {
            soundOnGb.SetActive(false);
        }
        else
        {
            soundOnGb.SetActive(true);
        }
    }


    #endregion

    #region Avatar 
    [Header("Avatar") , Space(10)]

    [SerializeField] Sprite[] avatars;
    [SerializeField] Image[] profilePictures;
    [SerializeField] GameObject avatarSelectionFrame;
    [SerializeField] Transform[] avatarSelectedButtons;


    public void ChooseAvatar(Button buttonClicked)
    {
        int avatarNumber = int.Parse(buttonClicked.name);

        PlayerPrefs.SetInt("AVATAR", avatarNumber - 1);
        Debug.Log(PlayerPrefs.GetInt("AVATAR", 0));
        
        avatarSelectionFrame.transform.SetParent(buttonClicked.transform);
        avatarSelectionFrame.transform.localPosition = Vector3.zero;

    }


    public void SetProfilePicture()
    {
        //Debug.Log(PlayerPrefs.GetInt("AVATAR", 0));
        for(int i = 0; i < profilePictures.Length; i++)
        {
            profilePictures[i].sprite = avatars[PlayerPrefs.GetInt("AVATAR", 0)];
        }

        avatarSelectionFrame.transform.SetParent(avatarSelectedButtons[PlayerPrefs.GetInt("AVATAR", 0)]);
        avatarSelectionFrame.transform.localPosition = Vector3.zero;
    }

    #endregion

    [Header("Feedback"), Space(10)]
    [SerializeField] Text email;
    public void CopyEmailToClipboard()
    {
        GUIUtility.systemCopyBuffer = email.text;
    }


    public void Share()
    {
        new NativeShare().SetText("http://nmsgames.com").Share();
    }

    #region Wheel

    [Header("Spin Wheel"), Space(10)]
    [SerializeField] Image wheelUI;
    float timeToStop;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject spinButton;
    float currentSpeed;


    public void SpinWheel()
    {
        timeToStop = Random.Range(5, 10);
        currentSpeed = rotateSpeed;
        StartCoroutine(SpinRoutine());
        spinButton.SetActive(false);
    }

    IEnumerator SpinRoutine()
    {
        while(true)
        {
            yield return null;

            wheelUI.transform.Rotate(Vector3.forward * Time.deltaTime * currentSpeed);

            currentSpeed -= 1;

            if(currentSpeed <= 0)
            {
                break;
            }
        }
        spinButton.SetActive(true);

    }

    #endregion
}
