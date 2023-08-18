using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Login_Register : MonoBehaviour {
	public UserData UData;
	public WWWForm form;
	public GameObject Login_Email;
	public GameObject Login_Password;

	public Text Text_Login_Email;
	public Text Text_Login_Password;
	public Text Text_Login_Feedback;

	public GameObject Reg_Email;
	public GameObject Reg_Password;
	public GameObject Reg_Password2;
	public GameObject Reg_Username;

	public Text Text_Reg_Email;
	public Text Text_Reg_Password;
	public Text Text_Reg_2Password;
	public Text Text_Reg_Username;
	public Text Text_Reg_Feedback;

	public bool isRegFormActive;
	public Animator LoginAnimator;
	public Scene MainMenu;




    public void RegistrationButton(){
		Text_Reg_Feedback.text = "Registration...";
		StartCoroutine (RequestUserRegistration());
	}
	public void LoginButton(){
		Text_Login_Feedback.text = "Connecting...";
		StartCoroutine (RequestLogin());
		StartCoroutine (GetUserName());
		StartCoroutine (GetUserDescription ());
		StartCoroutine (GetUserAvatar ());
		StartCoroutine (GetUserCard ());
	}

	public IEnumerator RequestLogin(){
		string email = Text_Login_Email.text.ToString();
		string password = Login_Password.GetComponent<InputField> ().text;

		form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);

		WWW w = new WWW ("http://65.1.1.23/gameserver/gameserver/action_login.php", form);
		yield return w;
		Debug.Log (w.text.ToString());
		if(string.IsNullOrEmpty (w.error)){
			//success...
			if(w.text.Contains("Wrong username or password")){
				Text_Login_Feedback.text = "An input error occurred. Check the information you entered. 1";	
			}
			else{
                PhotonNetwork.AuthValues = new AuthenticationValues();; 
				PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
				PhotonNetwork.AuthValues.AddAuthParameter ("email", email);
				PhotonNetwork.AuthValues.AddAuthParameter ("password",password);
                PhotonNetwork.NickName = email;
                PhotonNetwork.AuthValues.UserId = email;
				PhotonNetwork.ConnectUsingSettings();// ("Remy");

				Debug.Log("Successful login!!!");
                Text_Login_Feedback.text = "Successful login.";
                GameObject.FindGameObjectWithTag ("UserData").GetComponent<UserData> ().UserEmail = email;
				if (isRegFormActive) {
					LoginAnimator.SetTrigger ("End_Reg_Active");
				}
				else{
					LoginAnimator.SetTrigger ("End_Reg_InActive");	
				}
			}
		}
		else{
			//error
			Text_Login_Feedback.text = "An input error occurred. Check the information you entered. 2";
		}

	}

	public IEnumerator GetUserName(){
		string email = Text_Login_Email.text.ToString ();
		string password = Login_Password.GetComponent<InputField> ().text;

		form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);

		WWW w = new WWW ("http://65.1.1.23/gameserver/gameserver/action_read_username.php", form);
		yield return w;
		Debug.Log (w.text.ToString ());
		if (string.IsNullOrEmpty (w.error)) {
			string Result = w.text;
			Result = Result.Substring (9);
			int resultLenght = Result.Length;
			Result = Result.Remove (Result.Length - 2);
			UData.UserName = Result;
		} else {
			//error	
		}
	}
		public IEnumerator GetUserDescription(){
			string email = Text_Login_Email.text.ToString();
			string password = Login_Password.GetComponent<InputField> ().text;

			form = new WWWForm ();
			form.AddField ("email", email);
			form.AddField ("password", password);

			WWW w = new WWW ("http://65.1.1.23/gameserver/gameserver/action_read_description.php", form);
			yield return w;
			Debug.Log (w.text.ToString());
			if(string.IsNullOrEmpty (w.error)){
				string Result = w.text;
				Result = Result.Substring (9);
				int resultLenght = Result.Length;
				Result = Result.Remove (Result.Length - 2);
				UData.Description = Result;
			}
			else{
				//error	
			}

	}

	public IEnumerator GetUserAvatar(){
		string email = Text_Login_Email.text.ToString();
		string password = Login_Password.GetComponent<InputField> ().text;

		form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);

		WWW w = new WWW ("http://65.1.1.23/gameserver/gameserver/action_read_avatar.php", form);
		yield return w;
		Debug.Log (w.text.ToString());
		if(string.IsNullOrEmpty (w.error)){
			string Result = w.text;
			Result = Result.Substring (9);
			int resultLenght = Result.Length;
			Result = Result.Remove (Result.Length - 2);
			UData.Avatar = int.Parse(Result);
		}
		else{
			//error	
		}

	}

	public IEnumerator GetUserCard(){
		string email = Text_Login_Email.text.ToString();
		string password = Login_Password.GetComponent<InputField> ().text;

		form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);

		WWW w = new WWW ("http://65.1.1.23/gameserver/gameserver/action_read_card.php", form);
		yield return w;
		Debug.Log (w.text.ToString());
		if(string.IsNullOrEmpty (w.error)){
			string Result = w.text;
			Result = Result.Substring (9);
			int resultLenght = Result.Length;
			Result = Result.Remove (Result.Length - 2);
			Debug.Log ("KARTA: " + Result);
			UData.CardDesign = int.Parse(Result);
		}
		else{
			//error	
		}

	}
	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
	}

	public IEnumerator RequestUserRegistration(){
		string email = Text_Reg_Email.text;
		string regPassword = Reg_Password.GetComponent<InputField> ().text;
		string reg2Password = Reg_Password2.GetComponent<InputField> ().text;
		string username = Text_Reg_Username.text;
		if(!email.Contains("@") || !email.Contains(".")){
			Text_Reg_Feedback.text = "Please enter a valid e-mail";
			yield break;
		}
		if(regPassword.Length < 8) {
			Text_Reg_Feedback.text = "The code must be longer than 8 characters";
			yield break;
			}
		if (regPassword != reg2Password) {
			Text_Reg_Feedback.text = "The codes do not match";
			yield break;
			}
		if (username.Length < 3) {
			Text_Reg_Feedback.text = "Nickname must be longer than 3 characters";
			yield break;
		}

			form = new WWWForm ();
			form.AddField ("email", email);
			form.AddField ("password", regPassword);
			form.AddField ("username", username);

			WWW w = new WWW ("http://65.1.1.23/gameserver/gameserver/action_registration.php", form);
			yield return w;

			Debug.Log (w.text.ToString());
			Debug.Log ("TEST!");
			if(string.IsNullOrEmpty (w.error)){
				//success...
				if(w.text.Contains("invalid email or password")){
					Text_Reg_Feedback.text = "An input error occurred. Check the information you entered. 3";	
				}
				else{
					Text_Reg_Feedback.text = "Successful registration.";

				}
			}

			else{
				//error
				Text_Reg_Feedback.text = "An input error occurred. Check the information you entered. 4";
			}
	}
     

}

