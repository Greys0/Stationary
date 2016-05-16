using UnityEngine;

namespace StationaryVessels
{
	public class StationaryModule : PartModule
	{
		[KSPField(isPersistant = true)]
		public bool isFrozen;
		private StationaryVesselModule SVM;

		public void Start()
		{
			SVM = vessel.GetComponent<StationaryVesselModule>();

		}

		public void UpdateButton()
		{
			Events["ToggleFreeze"].guiName = isFrozen ? "Is Frozen: No" : "Is Frozen: Yes";
		}

		[KSPEvent(guiActive = true, guiName = "Is Frozen: No")]
		public void ToggleFreeze()
		{
			SVM.setFreeze(!isFrozen);
		}

		[KSPAction("Toggle Freeze")]
		public void ToggleFreezeAction(KSPActionParam param)
		{
			ToggleFreeze();
		}
	}
}
