using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class MamaCon : MonoBehaviour {

		public static MamaCon Instance;


		[Range( 0, 10 )]
		public float MaxMamaVelocity = 5;
		[Range( 0, 100 )]
		public float MamaSpeedForce = 5;

		[Header( "Obj Refs" )]
		public Mama Mama;

		private void Awake() {
			if( Instance == null )
				Instance = this;
			if( Instance != this )
				Destroy( gameObject );
		}
	}
}