using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteEcho.SpeedrunCreate.Moonlight {
	public class ShadowSamplePoint : MonoBehaviour {

		[Header( "Output Vars" )]
		public float ShadowCoverage = 0;
		public bool IsShadowCovered {
			get {
				return ShadowCoverage > ShadowCon.Instance.ShadowCoveredThresh;
			}
		}

		public int Idx;

		public int ScreenPointX;
		public int ScreenPointY;

		public Color SampledColor;
	}
}