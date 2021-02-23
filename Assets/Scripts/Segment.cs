using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class Segment : MonoBehaviour {

		public Nexus.CnxnType MyCnxnType = Nexus.CnxnType.None;

		public Nexus[] Parents;

		private void OnDestroy() {
			if( Parents != null ) {
				Nexus parentA = Parents[0];
				Nexus parentB = Parents[1];

				int pBIdx = parentA.Cnxns.FindIndex( cnxnInfo => cnxnInfo.ConnectedTo == parentB );
				int pAIdx = parentB.Cnxns.FindIndex( cnxnInfo => cnxnInfo.ConnectedTo == parentA );
				if( parentA.Cnxns.Count - 1 > pBIdx && pBIdx != -1 )
					parentA.Cnxns.RemoveAt( pBIdx );
				if( parentB.Cnxns.Count - 1 > pAIdx && pAIdx != -1 )
					parentB.Cnxns.RemoveAt( pAIdx );
			}
		}
	}
}