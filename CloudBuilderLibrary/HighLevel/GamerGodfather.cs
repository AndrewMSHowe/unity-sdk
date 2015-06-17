﻿using System;
using System.Collections.Generic;

namespace CotcSdk {

	public class GamerGodfather {

		/**
		 * Changes the domain affected by the next operations.
		 * You should typically use it this way: `gamer.Godfather.Domain("private").Associate(...);`
		 * @param domain domain on which to scope the next operations.
		 */
		public GamerGodfather Domain(string domain) {
			this.domain = domain;
			return this;
		}

		/**
		 * Method to call in order to generate a temporary code that can be passed to another gamer so he can
		 * add us as a godfather.
		 * 
		 * The domain as specified by the #Domain method is the domain in which the godfather link should be
		 * established. "private" means it's local to this game only.
		 * 
		 * @param done callback invoked when the operation has finished, either successfully or not. The attached
		 *     string is the generated code.
		 */
		public void GenerateCode(ResultHandler<string> done) {
			UrlBuilder url = new UrlBuilder("/v2.6/gamer/godfather").Path(domain);
			HttpRequest req = Gamer.MakeHttpRequest(url);
			req.Method = "PUT";
			Common.RunHandledRequest(req, done, (HttpResponse response) => {
				Common.InvokeHandler(done, response.BodyJson["godfathercode"], response.BodyJson);
			});
		}

		/**
		 * This method can be used to retrieve the gamer who have added you as a godfather.
		 * @param done callback invoked when the operation has finished, either successfully or not.
		 */
		public void GetGodchildren(ResultHandler<List<GamerInfo>> done) {
			UrlBuilder url = new UrlBuilder("/v2.6/gamer/godchildren").Path(domain);
			HttpRequest req = Gamer.MakeHttpRequest(url);
			Common.RunHandledRequest(req, done, (HttpResponse response) => {
				List<GamerInfo> result = new List<GamerInfo>();
				foreach (Bundle b in response.BodyJson["godchildren"].AsArray()) {
					result.Add(new GamerInfo(b));
				}
				Common.InvokeHandler(done, result, response.BodyJson);
			});
		}

		/**
		 * This method can be used to retrieve the godfather of the gamer.
		 * @param done callback invoked when the operation has finished, either successfully or not.
		 */
		public void GetGodfather(ResultHandler<GamerInfo> done) {
			UrlBuilder url = new UrlBuilder("/v2.6/gamer/godfather").Path(domain);
			HttpRequest req = Gamer.MakeHttpRequest(url);
			Common.RunHandledRequest(req, done, (HttpResponse response) => {
				Common.InvokeHandler(done, new GamerInfo(response.BodyJson["godfather"]), response.BodyJson);
			});
		}

		/**
		 * Call this to attribute a godfather to the currently logged in user.
		 * @param done callback invoked when the operation has finished, either successfully or not. The attached
		 *     boolean indicates success.
		 * @param code is a string as generated by #GetCode.
		 * @param rewardTx a transaction Json rewarding the godfather formed as follows:
		 *  { transaction : { "unit" : amount},
		 *    description : "reward transaction",
		 *    domain : "com.clanoftcloud.text.DOMAIN" }
		 * where description and domain are optional.
		 * @param notification optional OS notification to be sent to the godfather who generated the code.
		 *     The godfather will reveive an event of type 'godchildren' containing the id of the godchildren
		 *     and the balance/achievements field if rewarded.
		 */
		public void UseCode(ResultHandler<bool> done, string code, Bundle rewardTx = null, PushNotification notification = null) {
			UrlBuilder url = new UrlBuilder("/v2.6/gamer/godfather").Path(domain);
			HttpRequest req = Gamer.MakeHttpRequest(url);
			Bundle config = Bundle.CreateObject();
			config["godfather"] = code;
			config["osn"] = notification != null ? notification.Data : null;
			config["reward"] = rewardTx;
			req.BodyJson = config;
			Common.RunHandledRequest(req, done, (HttpResponse response) => {
				Common.InvokeHandler(done, response.BodyJson["done"], response.BodyJson);
			});
		}

		#region Private
		internal GamerGodfather(Gamer parent) {
			Gamer = parent;
		}
		private string domain = Common.PrivateDomain;
		private Gamer Gamer;
		#endregion
	}
}
