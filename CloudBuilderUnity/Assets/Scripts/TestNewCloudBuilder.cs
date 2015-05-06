﻿
using UnityEngine;
using System.Collections;
using CloudBuilderLibrary;

public class TestNewCloudBuilder : MonoBehaviour {
	private Clan Clan;
	private DomainEventLoop EventLoop;
	private Gamer Gamer;

	// Inherited
	void Start() {
/*		CloudBuilder.Setup(
			done: (Result<Clan> result) => {
				Clan = result.Value;
				Debug.Log("Setup done");
			},
			apiKey: "cloudbuilder-key",
			apiSecret: "azerty",
//			environment: "http://10.211.55.2:2000/",
			httpVerbose: true
		);*/
	}

	void OnApplicationFocus(bool focused) {
		if (EventLoop != null) {
			if (focused)	EventLoop.Resume();
			else			EventLoop.Suspend();
		}
		CloudBuilder.OnApplicationFocus(focused);
	}

	void OnApplicationQuit() {
		if (EventLoop != null)
			EventLoop.Stop();
		CloudBuilder.OnApplicationQuit();
	}

	// Responding to events
	public void DoLogin() {
		Clan.LoginAnonymously(done: this.DidLogin);
	}

	public void DoLoginWithFacebook() {
		Clan.LoginWithFacebook(done: this.DidLogin);
	}

	public void DoRestoreSession() {
		Clan.ResumeSession(
			done: this.DidLogin,
			gamerId: "55481f4ed9768e9b3d31864b",
			gamerSecret: "4102d2295329d73c58f96347f42790b320b5b1f1"
		);
	}

	public void DoGetProfile() {
		if (Gamer == null) {
			Debug.Log("Please log in first");
			return;
		}

		Gamer.GetProfile((Result<GamerProfile> result) => {
			if (result.IsSuccessful)
				Debug.Log("Get profile done: " + result.Value.Properties.ToJson());
			else
				Debug.Log("Get profile failed " + result.ToString());
		});
	}

	public void DoSetProfile() {
		if (Gamer == null) {
			Debug.Log("Please log in first");
			return;
		}

		Bundle profile = Bundle.CreateObject();
		profile["displayName"] = "Florian du clan";

		Gamer.SetProfile(
			done: (Result<bool> result) => {
				Debug.Log("Profile set: " + result);
			},
			data: profile
		);
	}

	public void DoGetFacebookFriends() {
		Gamer.FetchFacebookFriends(done: (Result<int> result) => {
			Debug.Log("Fetched fb friends: " + result.ToString() + " with " + result.Value);
		});
	}

	// Private
	private void DidLogin(Result<Gamer> result) {
		if (result.IsSuccessful) {
			Gamer = result.Value;
			// Run an event loop
			if (EventLoop != null) EventLoop.Stop();
			EventLoop = new DomainEventLoop(Gamer).Start();
			Debug.Log("Login done! Welcome " + Gamer.GamerId + "!");
		}
		else
			Debug.Log("Login failed :( " + result.ToString());
	}
}
