using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class SpiderlingCon : MonoBehaviour {

		public static SpiderlingCon Instance;


		[Range( 0, 5 )]
		public float FullSpeed = 3.5f;
		[Range( 0, 1 )]
		public float StoppedSpeed = .1f;


		private void Awake() {
			if( Instance == null )
				Instance = this;
			if( Instance != this )
				Destroy( gameObject );
		}
	}
}