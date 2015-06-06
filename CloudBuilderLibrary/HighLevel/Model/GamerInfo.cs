﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBuilderLibrary {

	/**
	 * Info about a player.
	 * Can be enriched with information, accessible using the index operator [].
	 * Typically contains a profile field, with displayname, email and lang. You can fetch this by doing
	 * `string name = GamerInfo["profile"]["displayname"];`
	 */
	public class GamerInfo: PropertiesObject {
		/**
		 * Id of the gamer.
		 */
		public string GamerId {
			get { return Properties["gamer_id"]; }
		}

		#region Private
		internal GamerInfo(Bundle serverData) : base(serverData) {}
		#endregion
	}
}
