using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Tableau.Base;

namespace Tableau.Base.Net {
	
	public class Player : NetworkBehaviour {

        [SyncVar]
        public int playerId;

	}
}
