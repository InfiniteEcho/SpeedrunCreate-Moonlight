using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class GameCon : MonoBehaviour {

		public static GameCon Instance;

		private void Awake() {
			if( Instance == null )
				Instance = this;
			if( Instance != this )
				Destroy( gameObject );
		}
	}
}