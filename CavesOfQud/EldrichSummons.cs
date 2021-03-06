using System;
using System.Collections.Generic;
using XRL.Messages;
using XRL.UI;
using XRL.Core;
using XRL.World.AI;
using XRL.World.AI.GoalHandlers;
using XRL.World.Effects;

namespace XRL.World.Parts.Mutation
{
	[Serializable]
	public class EldrichSummons : BaseMutation
	{
		public Guid ActivatedAbilityCommandSummonEldrichHorror1ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror2ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror3ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror4ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror5ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror6ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror7ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror8ID;
		public Guid ActivatedAbilityCommandSummonEldrichHorror9ID;

		public Guid ActivatedAbilityCommandRecallEldrichHorrorID;
		public Guid ActivatedAbilityCommandDominateEldrichHorrorID;

		public GameObject SummonedElder1;
		public GameObject SummonedElder2;
		public GameObject SummonedElder3;
		public GameObject SummonedElder4;
		public GameObject SummonedElder5;
		public GameObject SummonedElder6;
		public GameObject SummonedElder7;
		public GameObject SummonedElder8;
		public GameObject SummonedElder9;


		public int ActiveElderSlot;

		[NonSerialized] public Brain _pBrain;
		public string pinnedZone;
		public GameObject TargetDomination;

		public EldrichSummons()
		{
			this.Name = "Eldrich Summons";
			this.DisplayName = "Eldrich Summons";
			this.Type = "Mental";
			this.MaxLevel = 9;
		}

		private GameObject GetSumonedElderSlotById(int slotId)
		{
			switch (slotId)
			{
				case 9:
					return SummonedElder9;
				case 8:
					return SummonedElder8;
				case 7:
					return SummonedElder7;
				case 6:
					return SummonedElder6;
				case 5:
					return SummonedElder5;
				case 4:
					return SummonedElder4;
				case 3:
					return SummonedElder3;
				case 2:
					return SummonedElder2;
				default:
					return SummonedElder1;
			}
		}

		private void SetSummonedElderSlotById(int slotId, GameObject go)
		{
			switch (slotId)
			{
				case 9:
					SummonedElder9 = go;
					break;
				case 8:
					SummonedElder8 = go;
					break;
				case 7:
					SummonedElder7 = go;
					break;
				case 6:
					SummonedElder6 = go;
					break;
				case 5:
					SummonedElder5 = go;
					break;
				case 4:
					SummonedElder4 = go;
					break;
				case 3:
					SummonedElder3 = go;
					break;
				case 2:
					SummonedElder2 = go;
					break;
				default:
					SummonedElder1 = go;
					break;
			}
		}

		public override bool AllowStaticRegistration() => true;

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent((IPart) this, "BeforeAITakingAction");
			Object.RegisterPartEvent((IPart) this, "BeginTakeAction");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror1");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror2");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror3");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror4");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror5");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror6");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror7");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror8");
			Object.RegisterPartEvent((IPart) this, "CommandSummonEldrichHorror9");
			Object.RegisterPartEvent((IPart) this, "CommandDominateEldrichHorror");
			Object.RegisterPartEvent((IPart) this, "CommandRecallEldrichHorror");
			Object.RegisterPartEvent((IPart) this, "TakeDamage");
			Object.RegisterPartEvent((IPart) this, "DominationBroken");
			Object.RegisterPartEvent((IPart) this, "PossessedByEldrichHorror");
			base.Register(Object);
		}

		public Brain pBrain
		{
			get
			{
				if (this._pBrain == null)
					this._pBrain = this.ParentObject.pBrain;
				return this._pBrain;
			}
		}

		public override string GetDescription() => "You may draw an Elder horror into this reality to aid you.";

		private GameObject GetExistingOrCreateNewEldrichFormBySlotId(int slotId)
		{
			if (!DoesElderSlotExistAndIsAlive(slotId))
				SetSummonedElderSlotById(slotId, SpawnNewElderHorror(slotId));

			var eldrichHorror = GetSumonedElderSlotById(slotId);
			LevelEldrichHorrorToPlayer(eldrichHorror);
			return eldrichHorror;
		}

		public override bool FireEvent(Event e)
		{
			if (e.ID == "BeginTakeAction")
			{
				if (this.TargetDomination != null && this.TargetDomination.IsNowhere())
					this.TargetDomination = (GameObject) null;
			}
			else if (e.ID == "TakeDamage" || e.ID == "DominationBroken" || e.ID == "ApplyEffect")
			{
				this.DropDomination();
			}
			else if (e.ID == "CommandSummonEldrichHorror1")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(1)))
				{
					this.ActiveElderSlot = 1;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror2")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(2)))
				{
					this.ActiveElderSlot = 2;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror3")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(3)))
				{
					this.ActiveElderSlot = 3;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror4")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(4)))
				{
					this.ActiveElderSlot = 4;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror5")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(5)))
				{
					this.ActiveElderSlot = 5;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror6")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(6)))
				{
					this.ActiveElderSlot = 6;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror7")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(7)))
				{
					this.ActiveElderSlot = 7;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror8")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(8)))
				{
					this.ActiveElderSlot = 8;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}
			else if (e.ID == "CommandSummonEldrichHorror9")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				if (this.PlaceAndActivateEldrichHorror(GetExistingOrCreateNewEldrichFormBySlotId(9)))
				{
					this.ActiveElderSlot = 9;
					this.UseEnergy(1000, "Mental Mutation");
					putAllSummonsOnCooldown(30);
				}
			}


			else if (e.ID == "CommandDominateEldrichHorror")
			{
				this.DropDomination();

				if (ActiveElderSlot > 0 && this.DominateEldrichHorror())
				{
					this.UseEnergy(1000, "Mental Mutation");
					this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandDominateEldrichHorrorID, 100);
				}
			}
			else if (e.ID == "CommandRecallEldrichHorror")
			{
				this.DropDomination();
				this.DeActivateAndRecallEldrichHorror();
				this.UseEnergy(10, "Mental Mutation");
				putAllSummonsOnCooldown(30);
			}

			return base.FireEvent(e);
		}

		private void putAllSummonsOnCooldown(int time)
		{
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror1ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror2ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror3ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror4ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror5ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror6ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror7ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror8ID, time);
			this.CooldownMyActivatedAbility(this.ActivatedAbilityCommandSummonEldrichHorror9ID, time);
		}

		private bool DoesElderSlotExistAndIsAlive(int slot)
		{
			var go = GetSumonedElderSlotById(slot);

			if (go == null)
				return false;

			return go.hitpoints > 0;
		}

		private bool PlaceAndActivateEldrichHorror(GameObject GO)
		{
			try
			{
				Cell cell = this.PickDestinationCell(20, AllowVis.OnlyVisible, false);
				if (cell.IsEmpty())
				{
					XRLCore.Core.Game.ActionManager.AddActiveObject(GO);
					cell.AddObject(GO);
					return true;
				}
			}
			catch (Exception ex)
			{
				IPart.AddPlayerMessage("&RError " + ex.ToString());
			}

			return false;
		}

		private bool DeActivateAndRecallEldrichHorror()
		{
			this.DropDomination();
			try
			{
				var GO = GetSumonedElderSlotById(ActiveElderSlot);

				if (GO == null)
					return false;

				Cell curCell = GO?.pPhysics?.CurrentCell ?? null;
				if (curCell != null)
					curCell.RemoveObject(GO);

				XRLCore.Core.Game.ActionManager.RemoveActiveObject(GO);
				ActiveElderSlot = 0;
				return true;
			}
			catch (Exception ex)
			{
				IPart.AddPlayerMessage("&RError " + ex.ToString());
			}

			return false;
		}

		private void DropDomination()
		{
			try
			{
				if (this.TargetDomination != null && this.TargetDomination.IsNowhere())
					this.TargetDomination = (GameObject) null;
				if (this.TargetDomination != null)
				{
					if (this.TargetDomination.pPhysics.CurrentCell != null &&
					    this.TargetDomination.pPhysics.CurrentCell.ParentZone.IsWorldMap())
						this.TargetDomination.PullDown(false);
					this.TargetDomination.RemoveEffect(nameof(Domination),
						(Predicate<Effect>) (FX => (FX as Dominated).Dominator == this.ParentObject));
					XRLCore.Core.Game.Player.Body = this.ParentObject;
					if (this.ParentObject.IsPlayer())
						Popup.Show("&rYour domination is broken!", true);
					this.TargetDomination = (GameObject) null;
					this.pBrain.Goals.Clear();
					this.Unpin();
				}

				this.TargetDomination = (GameObject) null;
				this._pBrain = (Brain) null;

			}
			catch (Exception ex)
			{
				IPart.AddPlayerMessage("&RError " + ex.ToString());
			}
		}

		private bool DominateEldrichHorror()
		{
			try
			{
				if (ActiveElderSlot < 1)
					return false;

				GameObject GO = null;
				if (DoesElderSlotExistAndIsAlive(ActiveElderSlot))
					GO = GetSumonedElderSlotById(ActiveElderSlot);

				if (GO?.CurrentCell == null || GO?.pPhysics?.CurrentCell == null || GO.hitpoints < 1)
					return false;

				this.TargetDomination = GO;
				if (GO.ApplyEffect((Effect) new Dominated(this.ParentObject, 99999)))
				{
					IPart.AddPlayerMessage("Dominating- " + GO.ToString());
					this.DidXToY("take", "control of", GO, terminalPunctuation: "!", ColorAsGoodFor: this.ParentObject);
					this.Pin();
					this.TargetDomination = GO;
					XRLCore.Core.Game.Player.Body = GO;
					return true;
				}
				else
					IPart.AddPlayerMessage(
						"&RSomething prevents you from dominating " + GO.the + GO.DisplayName + "&R.");
			}
			catch (Exception ex)
			{
				IPart.AddPlayerMessage("&RError " + ex.ToString());
			}

			return false;
		}

		private void LevelEldrichHorrorToPlayer(GameObject go)
		{
			if (go.Statistics.ContainsKey("Level"))
			{
				int parentLevel = this.ParentObject.Statistics["Level"].Value;
				int eldrichHorrorLevel = go.Statistics["Level"].Value;
				int levelUp = parentLevel - eldrichHorrorLevel;

				if (go.HasPart("Leveler"))
				{
					Leveler leveler = go.GetPart("Leveler") as Leveler;
					if (leveler != null)
					{
						for (int i = 0; i <= levelUp; i++)
						{
							leveler.LevelUp();

							if (go.Statistics.ContainsKey("SP"))
								go.Statistics["SP"].BaseValue = 50 + go.Statistics["SP"].BaseValue;
						}
					}
				}
			}
		}

		private GameObject SpawnNewElderHorror(int slotId)
		{
			GameObject GO;
			switch (slotId)
			{
				case 9:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons9");
					break;
				case 8:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons8");
					break;
				case 7:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons7");
					break;
				case 6:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons6");
					break;
				case 5:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons5");
					break;
				case 4:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons4");
					break;
				case 3:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons3");
					break;
				case 2:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons2");
					break;
				default:
					GO = GameObjectFactory.Factory.CreateObject("EldrichSummons1");
					break;
			}

			try
			{
				LevelEldrichHorrorToPlayer(GO);


				if (this.ParentObject.IsPlayer())
				{
					IPart.AddPlayerMessage("You've have summoned an Elder Horror!");
					GO.Statistics["XPValue"].BaseValue = 0; //Can't farm your own dudes.
					(GO.GetPart("Render") as Render).ColorString = "&C"; //Make sure we know whose ours are.
				}
				else
				{
					IPart.AddPlayerMessage(this.ParentObject.The + this.ParentObject.DisplayName +
					                       "summons a swirling and writhing being from beyond the stars.");
				}

				Brain GBrain = GO.GetPart("Brain") as Brain;
				if (GBrain != null)
				{
					GBrain.SetFeeling(this.ParentObject, 200);
					GBrain.PartyLeader = this.ParentObject;
				}

				this.ParentObject.ApplyEffect((Effect) new Exhausted(3));
			}
			catch (Exception ex)
			{
				IPart.AddPlayerMessage("&RError " + ex.ToString());
			}

			return GO;
		}

		public void Pin()
		{
			this.Unpin();
			this.pinnedZone = this.ParentObject.pPhysics.CurrentCell.ParentZone.ZoneID;
			if (XRLCore.Core.Game.ZoneManager.PinnedZones.CleanContains<string>(this.pinnedZone))
				return;
			XRLCore.Core.Game.ZoneManager.PinnedZones.Add(this.pinnedZone);
		}

		public void Unpin()
		{
			if (this.pinnedZone == null)
				return;
			XRLCore.Core.Game.ZoneManager.PinnedZones.Remove(this.pinnedZone);
			this.pinnedZone = (string) null;
		}

		public override string GetLevelText(int Level)
		{
			return "&WSummon Elder Horror modifier: &Cx" + Level;
		}

		public override bool ChangeLevel(int NewLevel)
		{
			this.Unmutate();

			if (this.ActivatedAbilityCommandRecallEldrichHorrorID == Guid.Empty && NewLevel >= 1)
				this.ActivatedAbilityCommandRecallEldrichHorrorID = this.AddMyActivatedAbility("Recall Elder Horror",
					"CommandRecallEldrichHorror", "Mental Mutation", "Recall Elder Horror", "*", "Recall Elder Horror");

			if (this.ActivatedAbilityCommandDominateEldrichHorrorID == Guid.Empty && NewLevel >= 1)
				this.ActivatedAbilityCommandDominateEldrichHorrorID = this.AddMyActivatedAbility(
					"Dominate Elder Horror", "CommandDominateEldrichHorror", "Mental Mutation", "Dominate Elder Horror",
					"\x0003", "Dominate Elder Horror");

			if (this.ActivatedAbilityCommandSummonEldrichHorror1ID == Guid.Empty && NewLevel >= 1)
				this.ActivatedAbilityCommandSummonEldrichHorror1ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the First Gate", "CommandSummonEldrichHorror1", "Mental Mutation",
					"Summon Elder Horror of the First Gate", "+", "Summon Elder Horror of the First Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror2ID == Guid.Empty && NewLevel >= 2)
				this.ActivatedAbilityCommandSummonEldrichHorror2ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Second Gate", "CommandSummonEldrichHorror2", "Mental Mutation",
					"Summon Elder Horror of the Second Gate", "+", "Summon Elder Horror of the Second Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror3ID == Guid.Empty && NewLevel >= 3)
				this.ActivatedAbilityCommandSummonEldrichHorror3ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Third Gate", "CommandSummonEldrichHorror3", "Mental Mutation",
					"Summon Elder Horror of the Third Gate", "+", "Summon Elder Horror of the Third Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror4ID == Guid.Empty && NewLevel >= 4)
				this.ActivatedAbilityCommandSummonEldrichHorror4ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Fourth Gate", "CommandSummonEldrichHorror4", "Mental Mutation",
					"Summon Elder Horror of the Fourth Gate", "+", "Summon Elder Horror of the Fourth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror5ID == Guid.Empty && NewLevel >= 5)
				this.ActivatedAbilityCommandSummonEldrichHorror5ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Fifth Gate", "CommandSummonEldrichHorror5", "Mental Mutation",
					"Summon Elder Horror of the Fifth Gate", "+", "Summon Elder Horror of the Fifth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror6ID == Guid.Empty && NewLevel >= 6)
				this.ActivatedAbilityCommandSummonEldrichHorror6ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Sixth Gate", "CommandSummonEldrichHorror6", "Mental Mutation",
					"Summon Elder Horror of the Sixth Gate", "+", "Summon Elder Horror of the Sixth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror7ID == Guid.Empty && NewLevel >= 7)
				this.ActivatedAbilityCommandSummonEldrichHorror7ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Seventh Gate", "CommandSummonEldrichHorror7", "Mental Mutation",
					"Summon Elder Horror of the Seventh Gate", "+", "Summon Elder Horror of the Sixth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror8ID == Guid.Empty && NewLevel >= 8)
				this.ActivatedAbilityCommandSummonEldrichHorror8ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Eighth Gate", "CommandSummonEldrichHorror8", "Mental Mutation",
					"Summon Elder Horror of the Eighth Gate", "+", "Summon Elder Horror of the Sixth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror9ID == Guid.Empty && NewLevel >= 9)
				this.ActivatedAbilityCommandSummonEldrichHorror9ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Ninth Gate", "CommandSummonEldrichHorror9", "Mental Mutation",
					"Summon Elder Horror of the Ninth Gate", "+", "Summon Elder Horror of the Sixth Gate");

			return base.ChangeLevel(NewLevel);
		}

		public override bool Mutate(GameObject GO, int Level)
		{
			this.Unmutate(GO);
			if (this.ActivatedAbilityCommandRecallEldrichHorrorID == Guid.Empty && Level >= 1)
				this.ActivatedAbilityCommandRecallEldrichHorrorID = this.AddMyActivatedAbility("Recall Elder Horror",
					"CommandRecallEldrichHorror", "Mental Mutation", "Recall Elder Horror", "*", "Recall Elder Horror");

			if (this.ActivatedAbilityCommandDominateEldrichHorrorID == Guid.Empty && Level >= 1)
				this.ActivatedAbilityCommandDominateEldrichHorrorID = this.AddMyActivatedAbility(
					"Dominate Elder Horror", "CommandDominateEldrichHorror", "Mental Mutation", "Dominate Elder Horror",
					"\x0003", "Dominate Elder Horror");

			if (this.ActivatedAbilityCommandSummonEldrichHorror1ID == Guid.Empty && Level >= 1)
				this.ActivatedAbilityCommandSummonEldrichHorror1ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the First Gate", "CommandSummonEldrichHorror1", "Mental Mutation",
					"Summon Elder Horror of the First Gate", "+", "Summon Elder Horror of the First Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror2ID == Guid.Empty && Level >= 2)
				this.ActivatedAbilityCommandSummonEldrichHorror2ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Second Gate", "CommandSummonEldrichHorror2", "Mental Mutation",
					"Summon Elder Horror of the Second Gate", "+", "Summon Elder Horror of the Second Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror3ID == Guid.Empty && Level >= 3)
				this.ActivatedAbilityCommandSummonEldrichHorror3ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Third Gate", "CommandSummonEldrichHorror3", "Mental Mutation",
					"Summon Elder Horror of the Third Gate", "+", "Summon Elder Horror of the Third Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror4ID == Guid.Empty && Level >= 4)
				this.ActivatedAbilityCommandSummonEldrichHorror4ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Fourth Gate", "CommandSummonEldrichHorror4", "Mental Mutation",
					"Summon Elder Horror of the Fourth Gate", "+", "Summon Elder Horror of the Fourth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror5ID == Guid.Empty && Level >= 5)
				this.ActivatedAbilityCommandSummonEldrichHorror5ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Fifth Gate", "CommandSummonEldrichHorror5", "Mental Mutation",
					"Summon Elder Horror of the Fifth Gate", "+", "Summon Elder Horror of the Fifth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror6ID == Guid.Empty && Level >= 6)
				this.ActivatedAbilityCommandSummonEldrichHorror6ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Sixth Gate", "CommandSummonEldrichHorror6", "Mental Mutation",
					"Summon Elder Horror of the Sixth Gate", "+", "Summon Elder Horror of the Sixth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror7ID == Guid.Empty && Level >= 7)
				this.ActivatedAbilityCommandSummonEldrichHorror7ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Seventh Gate", "CommandSummonEldrichHorror7", "Mental Mutation",
					"Summon Elder Horror of the Seventh Gate", "+", "Summon Elder Horror of the Sixth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror8ID == Guid.Empty && Level >= 8)
				this.ActivatedAbilityCommandSummonEldrichHorror8ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Eighth Gate", "CommandSummonEldrichHorror8", "Mental Mutation",
					"Summon Elder Horror of the Eighth Gate", "+", "Summon Elder Horror of the Sixth Gate");

			if (this.ActivatedAbilityCommandSummonEldrichHorror9ID == Guid.Empty && Level >= 9)
				this.ActivatedAbilityCommandSummonEldrichHorror9ID = this.AddMyActivatedAbility(
					"Summon Elder Horror of the Ninth Gate", "CommandSummonEldrichHorror9", "Mental Mutation",
					"Summon Elder Horror of the Ninth Gate", "+", "Summon Elder Horror of the Sixth Gate");

			this.ChangeLevel(Level);
			return base.Mutate(GO, Level);
		}

		public override bool Unmutate(GameObject GO)
		{
			this.Unmutate();
			return base.Unmutate(GO);
		}

		private bool Unmutate()
		{
			if (this.ActivatedAbilityCommandSummonEldrichHorror1ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror1ID);
				this.ActivatedAbilityCommandSummonEldrichHorror1ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror2ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror2ID);
				this.ActivatedAbilityCommandSummonEldrichHorror2ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror3ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror3ID);
				this.ActivatedAbilityCommandSummonEldrichHorror3ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror4ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror4ID);
				this.ActivatedAbilityCommandSummonEldrichHorror4ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror5ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror5ID);
				this.ActivatedAbilityCommandSummonEldrichHorror5ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror6ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror6ID);
				this.ActivatedAbilityCommandSummonEldrichHorror6ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror7ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror7ID);
				this.ActivatedAbilityCommandSummonEldrichHorror7ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror8ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror8ID);
				this.ActivatedAbilityCommandSummonEldrichHorror8ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandSummonEldrichHorror9ID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandSummonEldrichHorror9ID);
				this.ActivatedAbilityCommandSummonEldrichHorror9ID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandDominateEldrichHorrorID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandDominateEldrichHorrorID);
				this.ActivatedAbilityCommandDominateEldrichHorrorID = Guid.Empty;
			}

			if (this.ActivatedAbilityCommandRecallEldrichHorrorID != Guid.Empty)
			{
				this.RemoveMyActivatedAbility(ref this.ActivatedAbilityCommandRecallEldrichHorrorID);
				this.ActivatedAbilityCommandRecallEldrichHorrorID = Guid.Empty;
			}

			return true;
		}
	}
}