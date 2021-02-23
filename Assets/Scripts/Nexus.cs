using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class Nexus : MonoBehaviour {

		[System.Serializable]
		public class CnxnInfo {
			[SerializeField]
			public CnxnType Type = CnxnType.None;
			[SerializeField]
			public CnxnStatus Status = CnxnStatus.Unconnected;

			[SerializeField]
			public Nexus ConnectedTo = null;

			public Segment Segment;
		}
		public enum CnxnType {
			None = 0,
			Web
		}

		public enum CnxnStatus {
			None = 0,
			Unconnected,
			Connected
		}


		public List<CnxnInfo> Cnxns = new List<CnxnInfo>();


		public void OnDestroy() {
			foreach( CnxnInfo cnxn in Cnxns ) {
				Destroy( cnxn.Segment );
			}
		}



	}
}