﻿using UnityEngine;
using UnityEngine.UI;
using KSP;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StationaryVessels
{
	
	public class StationaryVesselModule : VesselModule
	{
		private Vessel vessel;
		private List<StationaryModule> modules;

		new public void Awake ()
		{
			Debug.Log ("StationaryAwake");
			vessel = GetComponent<Vessel> ();

			// Add StationaryModule to all controllable parts on vessel
			if (!vessel.isEVA)
			{
				foreach (Part p in vessel.parts)
				{
					if (p.Modules.Contains ("ModuleCommand")
					&& !p.Modules.Contains ("StationaryModule"))
					{
						p.AddModule ("StationaryModule");
					}
				}
			}
		}

		public void Start ()
		{
			Debug.Log ("StationaryStart");

			RebuildModulesList();

			// Establish loaded state
			if (modules.FindAll(x => x.isFrozen == true).Count > 0)
			{
				setFreeze(true);
			}
		}

		// Todo: Will probably need to call this to fix the list after some events like docking and breaks
		private void RebuildModulesList()
		{
			if (modules == null) { modules = new List<StationaryModule>(); }
			List<Part> parts = vessel.parts.FindAll(p => p.Modules.Contains("StationaryModule") == true);

			foreach (Part p in parts)
			{
				modules.AddRange(p.Modules.GetModules<StationaryModule>());
			}
		}

		public void setFreeze (bool state)
		{
			foreach (Part p in vessel.parts)
			{
				// Skip parts without Rigidbodys
				if (p.GetComponent<Rigidbody> () == null) continue;
				p.GetComponent<Rigidbody> ().isKinematic = state;
			}
			Propogate(state);
		}

		private void Propogate(bool state)
		{
			modules.ForEach(x => x.isFrozen = state);
			modules.ForEach(x => x.UpdateButton());
		}
	}
}
