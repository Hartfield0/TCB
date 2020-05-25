// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomBattle.CustomBattleMenuVM
// Assembly: TaleWorlds.MountAndBlade.CustomBattle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BE5DA056-FF60-485A-AA32-12EF43DB1898
// Assembly location: G:\steam\steamapps\common\Mount & Blade II Bannerlord\Modules\CustomBattle\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.CustomBattle.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
  public class CustomBattleMenuVM : ViewModel
  {
    private List<BasicCultureObject> _factionList;
    private List<CustomBattleSceneData> _customBattleScenes;
    private List<BasicCharacterObject> _charactersList;
    private CustomBattleState _customBattleState;
    private CustomBattleMenuSideVM _enemySide;
    private CustomBattleMenuSideVM _playerSide;
    private bool _isAttackerCustomMachineSelectionEnabled;
    private bool _isDefenderCustomMachineSelectionEnabled;
    private GameTypeSelectionGroup _gameTypeSelectionGroup;
    private MapSelectionGroup _mapSelectionGroup;
    private string _randomizeButtonText;
    private string _backButtonText;
    private string _startButtonText;
    private string _titleText;
    private MBBindingList<CustomBattleSiegeMachineVM> _attackerMeleeMachines;
    private MBBindingList<CustomBattleSiegeMachineVM> _attackerRangedMachines;
    private MBBindingList<CustomBattleSiegeMachineVM> _defenderMachines;

    public CustomBattleMenuVM(CustomBattleState battleState)
    {
      ////this.\u002Ector();
      this._customBattleState = battleState;
      List<BasicCultureObject> basicCultureObjectList = new List<BasicCultureObject>();
      basicCultureObjectList.Add((BasicCultureObject) Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire"));
      basicCultureObjectList.Add((BasicCultureObject) Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia"));
      basicCultureObjectList.Add((BasicCultureObject) Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai"));
      basicCultureObjectList.Add((BasicCultureObject) Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia"));
      basicCultureObjectList.Add((BasicCultureObject) Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania"));
      basicCultureObjectList.Add((BasicCultureObject) Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait"));
      this._factionList = basicCultureObjectList;
      this._customBattleScenes = new List<CustomBattleSceneData>();
      List<BasicCharacterObject> basicCharacterObjectList = new List<BasicCharacterObject>();
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_1"));
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_2"));
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_3"));
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_4"));
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_5"));
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_6"));
      basicCharacterObjectList.Add((BasicCharacterObject) Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_7"));
      this._charactersList = basicCharacterObjectList;
      this.EnemySide = new CustomBattleMenuSideVM(new TextObject("{=35IHscBa}ENEMY", (Dictionary<string, TextObject>) null), false);
      this.PlayerSide = new CustomBattleMenuSideVM(new TextObject("{=BC7n6qxk}PLAYER", (Dictionary<string, TextObject>) null), true);
      List<string> stringList = new List<string>();
      List<string> list = ((IEnumerable<BasicCultureObject>) this._factionList).Select<BasicCultureObject, string>((Func<BasicCultureObject, string>) (f => ((object) f.Name).ToString())).ToList<string>();
      this.EnemySide.FactionSelectionGroup.Refresh((IEnumerable<string>) list, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnEnemyFactionSelection));
      this.PlayerSide.FactionSelectionGroup.Refresh((IEnumerable<string>) list, 0, new Action<SelectorVM<SelectorItemVM>>(this.OnPlayerFactionSelection));
      this.IsAttackerCustomMachineSelectionEnabled = false;
      List<MapSelectionElement> elementList = new List<MapSelectionElement>();
      this._customBattleScenes = CustomGame.Current.CustomBattleScenes.ToList<CustomBattleSceneData>();
      using (List<CustomBattleSceneData>.Enumerator enumerator = this._customBattleScenes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CustomBattleSceneData current = enumerator.Current;
          elementList.Add(new MapSelectionElement((current.Name).ToString(), ((CustomBattleSceneData) current).IsSiegeMap, ((CustomBattleSceneData) current).IsVillageMap));
        }
      }
      this.MapSelectionGroup = new MapSelectionGroup(((object) new TextObject("{=fXhblsky}MAP", (Dictionary<string, TextObject>) null)).ToString(), elementList);
      this.PlayerSide.CharacterSelectionGroup.Refresh((IEnumerable<string>) ((IEnumerable<BasicCharacterObject>) this._charactersList).Select<BasicCharacterObject, string>((Func<BasicCharacterObject, string>) (p => ((object) p.Name).ToString())).ToList<string>(), 0, new Action<SelectorVM<SelectorItemVM>>(this.OnPlayerCharacterSelection));
      this.EnemySide.CharacterSelectionGroup.Refresh((IEnumerable<string>) ((IEnumerable<BasicCharacterObject>) this._charactersList).Select<BasicCharacterObject, string>((Func<BasicCharacterObject, string>) (p => ((object) p.Name).ToString())).ToList<string>(), 1, new Action<SelectorVM<SelectorItemVM>>(this.OnEnemyCharacterSelection));
      this.OnPlayerCharacterSelection(this.PlayerSide.CharacterSelectionGroup);
      this.OnEnemyCharacterSelection(this.EnemySide.CharacterSelectionGroup);
      this.AttackerMeleeMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
      for (int index = 0; index < 3; ++index)
        ((Collection<CustomBattleSiegeMachineVM>) this.AttackerMeleeMachines).Add(new CustomBattleSiegeMachineVM((SiegeEngineType) null, new Action<CustomBattleSiegeMachineVM>(this.OnMeleeMachineSelection)));
      this.AttackerRangedMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
      for (int index = 0; index < 4; ++index)
        ((Collection<CustomBattleSiegeMachineVM>) this.AttackerRangedMachines).Add(new CustomBattleSiegeMachineVM((SiegeEngineType) null, new Action<CustomBattleSiegeMachineVM>(this.OnAttackerRangedMachineSelection)));
      this.DefenderMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
      for (int index = 0; index < 4; ++index)
        ((Collection<CustomBattleSiegeMachineVM>) this.DefenderMachines).Add(new CustomBattleSiegeMachineVM((SiegeEngineType) null, new Action<CustomBattleSiegeMachineVM>(this.OnDefenderRangedMachineSelection)));
      this.GameTypeSelectionGroup = new GameTypeSelectionGroup("Game Type", this.MapSelectionGroup, new Action<bool>(this.OnPlayerTypeChange));
      base.RefreshValues();
    }

    internal void SetActiveState(bool isActive)
    {
      if (isActive)
      {
        this.EnemySide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
        this.EnemySide.CurrentSelectedCharacter.FillFrom(this._charactersList[this.EnemySide.CharacterSelectionGroup.SelectedIndex], -1);
        this.PlayerSide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
        this.PlayerSide.CurrentSelectedCharacter.FillFrom(this._charactersList[this.PlayerSide.CharacterSelectionGroup.SelectedIndex], -1);
      }
      else
      {
        this.EnemySide.CurrentSelectedCharacter = (CharacterViewModel) null;
        this.PlayerSide.CurrentSelectedCharacter = (CharacterViewModel) null;
      }
    }

    private void OnPlayerTypeChange(bool isCommander)
    {
      this.PlayerSide.OnPlayerTypeChange(isCommander);
    }

    public override void RefreshValues()
    {
      base.RefreshValues();
      this.RandomizeButtonText = ((object) GameTexts.FindText("str_randomize", (string) null)).ToString();
      this.StartButtonText = ((object) GameTexts.FindText("str_start", (string) null)).ToString();
      this.BackButtonText = ((object) GameTexts.FindText("str_back", (string) null)).ToString();
      this.TitleText = ((object) GameTexts.FindText("str_custom_battle", (string) null)).ToString();
      ((ViewModel) this.EnemySide).RefreshValues();
      ((ViewModel) this.PlayerSide).RefreshValues();
      this.AttackerMeleeMachines.ApplyActionOnAllItems((Action<CustomBattleSiegeMachineVM>) (x => x.RefreshValues()));
      this.AttackerRangedMachines.ApplyActionOnAllItems((Action<CustomBattleSiegeMachineVM>) (x => x.RefreshValues()));
      this.DefenderMachines.ApplyActionOnAllItems((Action<CustomBattleSiegeMachineVM>) (x => x.RefreshValues()));
      this.MapSelectionGroup.RefreshValues();
    }

    private void OnMeleeMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
    {
      List<InquiryElement> inquiryElementList = new List<InquiryElement>();
      inquiryElementList.Add(new InquiryElement((object) null, "Empty", (ImageIdentifier) null));
      using (IEnumerator<SiegeEngineType> enumerator = this.GetAllAttackerMeleeMachines().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          SiegeEngineType current = enumerator.Current;
          inquiryElementList.Add(new InquiryElement((object) current, ((object) current.Name).ToString(), (ImageIdentifier) null));
        }
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(((object) new TextObject("{=MVOWsP48}Select a Melee Machine", (Dictionary<string, TextObject>) null)).ToString(), string.Empty, inquiryElementList, false, true, ((object) GameTexts.FindText("str_done", (string) null)).ToString(), "", (Action<List<InquiryElement>>) (selectedElements => selectedSlot.SetMachineType(((IEnumerable<InquiryElement>) selectedElements).First<InquiryElement>().Identifier as SiegeEngineType)), (Action<List<InquiryElement>>) null, ""), false);
    }

    private void OnAttackerRangedMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
    {
      List<InquiryElement> inquiryElementList = new List<InquiryElement>();
      inquiryElementList.Add(new InquiryElement((object) null, "Empty", (ImageIdentifier) null));
      using (IEnumerator<SiegeEngineType> enumerator = this.GetAllAttackerRangedMachines().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          SiegeEngineType current = enumerator.Current;
          inquiryElementList.Add(new InquiryElement((object) current, ((object) current.Name).ToString(), (ImageIdentifier) null));
        }
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(((object) new TextObject("{=SLZzfNPr}Select a Ranged Machine", (Dictionary<string, TextObject>) null)).ToString(), string.Empty, inquiryElementList, false, true, ((object) GameTexts.FindText("str_done", (string) null)).ToString(), "", (Action<List<InquiryElement>>) (selectedElements => selectedSlot.SetMachineType(selectedElements[0].Identifier as SiegeEngineType)), (Action<List<InquiryElement>>) null, ""), false);
    }

    private void OnDefenderRangedMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
    {
      List<InquiryElement> inquiryElementList = new List<InquiryElement>();
      inquiryElementList.Add(new InquiryElement((object) null, "Empty", (ImageIdentifier) null));
      using (IEnumerator<SiegeEngineType> enumerator = this.GetAllDefenderRangedMachines().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          SiegeEngineType current = enumerator.Current;
          inquiryElementList.Add(new InquiryElement((object) current, ((object) current.Name).ToString(), (ImageIdentifier) null));
        }
      }
      InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(((object) new TextObject("{=SLZzfNPr}Select a Ranged Machine", (Dictionary<string, TextObject>) null)).ToString(), string.Empty, inquiryElementList, false, true, ((object) GameTexts.FindText("str_done", (string) null)).ToString(), "", (Action<List<InquiryElement>>) (selectedElements => selectedSlot.SetMachineType(selectedElements[0].Identifier as SiegeEngineType)), (Action<List<InquiryElement>>) null, ""), false);
    }

    private void OnPlayerCharacterSelection(SelectorVM<SelectorItemVM> selector)
    {
      this.PlayerSide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
      this.PlayerSide.CurrentSelectedCharacter.FillFrom(this._charactersList[selector.SelectedIndex], -1);
      ((IEnumerable<SelectorItemVM>) this.EnemySide.CharacterSelectionGroup.ItemList).ToList<SelectorItemVM>().ForEach((Action<SelectorItemVM>) (i => i.CanBeSelected = true));
      if (((Collection<SelectorItemVM>) this.EnemySide.CharacterSelectionGroup.ItemList).Count <= selector.SelectedIndex)
        return;
      ((Collection<SelectorItemVM>) this.EnemySide.CharacterSelectionGroup.ItemList)[selector.SelectedIndex].CanBeSelected = false;
    }

    private void OnEnemyCharacterSelection(SelectorVM<SelectorItemVM> selector)
    {
      this.EnemySide.CurrentSelectedCharacter = new CharacterViewModel((CharacterViewModel.StanceTypes) 1);
      this.EnemySide.CurrentSelectedCharacter.FillFrom(this._charactersList[selector.SelectedIndex], -1);
      ((IEnumerable<SelectorItemVM>) this.PlayerSide.CharacterSelectionGroup.ItemList).ToList<SelectorItemVM>().ForEach((Action<SelectorItemVM>) (i => i.CanBeSelected = true));
      if (((Collection<SelectorItemVM>) this.PlayerSide.CharacterSelectionGroup.ItemList).Count <= selector.SelectedIndex)
        return;
      ((Collection<SelectorItemVM>) this.PlayerSide.CharacterSelectionGroup.ItemList)[selector.SelectedIndex].CanBeSelected = false;
    }

    private void OnEnemyFactionSelection(SelectorVM<SelectorItemVM> selector)
    {
      BasicCultureObject faction = this._factionList[this.EnemySide.FactionSelectionGroup.SelectedIndex];
      this.EnemySide.CompositionGroup.SetCurrentSelectedCulture(this._factionList[this.EnemySide.FactionSelectionGroup.SelectedIndex]);
      this.EnemySide.CurrentSelectedCultureID = ((MBObjectBase) faction).StringId;
    }

    private void OnPlayerFactionSelection(SelectorVM<SelectorItemVM> selector)
    {
      BasicCultureObject faction = this._factionList[this.PlayerSide.FactionSelectionGroup.SelectedIndex];
      this.PlayerSide.CompositionGroup.SetCurrentSelectedCulture(faction);
      this.PlayerSide.CurrentSelectedCultureID = ((MBObjectBase) faction).StringId;
    }

    private void ExecuteRandomizeAttackerSiegeEngines()
    {
      List<SiegeEngineType> siegeEngineTypeList = new List<SiegeEngineType>();
      siegeEngineTypeList.AddRange(this.GetAllAttackerMeleeMachines());
      siegeEngineTypeList.Add((SiegeEngineType) null);
      foreach (CustomBattleSiegeMachineVM attackerMeleeMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerMeleeMachines)
        attackerMeleeMachine.SetMachineType((SiegeEngineType) TaleWorlds.Core.Extensions.GetRandomElement<SiegeEngineType>((IEnumerable<SiegeEngineType>) siegeEngineTypeList));
      siegeEngineTypeList.Clear();
      siegeEngineTypeList.AddRange(this.GetAllAttackerRangedMachines());
      siegeEngineTypeList.Add((SiegeEngineType) null);
      foreach (CustomBattleSiegeMachineVM attackerRangedMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerRangedMachines)
        attackerRangedMachine.SetMachineType((SiegeEngineType) TaleWorlds.Core.Extensions.GetRandomElement<SiegeEngineType>((IEnumerable<SiegeEngineType>) siegeEngineTypeList));
    }

    private void ExecuteRandomizeDefenderSiegeEngines()
    {
      List<SiegeEngineType> siegeEngineTypeList = new List<SiegeEngineType>();
      siegeEngineTypeList.AddRange(this.GetAllDefenderRangedMachines());
      siegeEngineTypeList.Add((SiegeEngineType) null);
      foreach (CustomBattleSiegeMachineVM defenderMachine in (Collection<CustomBattleSiegeMachineVM>) this._defenderMachines)
        defenderMachine.SetMachineType((SiegeEngineType) TaleWorlds.Core.Extensions.GetRandomElement<SiegeEngineType>((IEnumerable<SiegeEngineType>) siegeEngineTypeList));
    }

    private void ExecuteBack()
    {
      Debug.Print("EXECUTE BACK - PRESSED", 0, (Debug.DebugColor) 4, 17592186044416UL);
      Game.Current.GameStateManager.PopState(0);
    }

    private void ExecuteStart()
    {
      int armySize1 = this.PlayerSide.CompositionGroup.ArmySize;
      int armySize2 = this.EnemySide.CompositionGroup.ArmySize;
      bool flag1 = (int) this.GameTypeSelectionGroup.GetCurrentPlayerSide() == 1;
      bool flag2 = this.GameTypeSelectionGroup.GetCurrentPlayerType() == GameTypeSelectionGroup.PlayerType.Commander;
      BasicCharacterObject basicCharacterObject = (BasicCharacterObject) null;
      if (!flag2)
      {
        List<BasicCharacterObject> list = ((IEnumerable<BasicCharacterObject>) this._charactersList).ToList<BasicCharacterObject>();
        list.Remove(this._charactersList[this.PlayerSide.CharacterSelectionGroup.SelectedIndex]);
        list.Remove(this._charactersList[this.EnemySide.CharacterSelectionGroup.SelectedIndex]);
        basicCharacterObject = (BasicCharacterObject) TaleWorlds.Core.Extensions.GetRandomElement<BasicCharacterObject>((IEnumerable<BasicCharacterObject>) list);
        --armySize1;
      }
      int num1 = armySize1 - 1;
      int num2 = (int) Math.Round((double) this.PlayerSide.CompositionGroup.ArmyComposition2Value / 100.0 * (double) num1);
      int num3 = (int) Math.Round((double) this.PlayerSide.CompositionGroup.ArmyComposition3Value / 100.0 * (double) num1);
      int num4 = (int) Math.Round((double) this.PlayerSide.CompositionGroup.ArmyComposition4Value / 100.0 * (double) num1);
      int num5 = num1 - (num2 + num3 + num4);
      int num6 = armySize2 - 1;
      int num7 = (int) Math.Round((double) this.EnemySide.CompositionGroup.ArmyComposition2Value / 100.0 * (double) num6);
      int num8 = (int) Math.Round((double) this.EnemySide.CompositionGroup.ArmyComposition3Value / 100.0 * (double) num6);
      int num9 = (int) Math.Round((double) this.EnemySide.CompositionGroup.ArmyComposition4Value / 100.0 * (double) num6);
      int num10 = num6 - (num7 + num8 + num9);
      CustomBattleCombatant[] customBattleParties = this._customBattleState.GetCustomBattleParties(this._charactersList[this.PlayerSide.CharacterSelectionGroup.SelectedIndex], basicCharacterObject, this._charactersList[this.EnemySide.CharacterSelectionGroup.SelectedIndex], this._factionList[this.PlayerSide.FactionSelectionGroup.SelectedIndex], new int[4]
      {
        num5,
        num2,
        num3,
        num4
      }, new List<BasicCharacterObject>[4]
      {
        this.PlayerSide.CompositionGroup.SelectedMeleeInfantryTypes,
        this.PlayerSide.CompositionGroup.SelectedRangedInfantryTypes,
        this.PlayerSide.CompositionGroup.SelectedMeleeCavalryTypes,
        this.PlayerSide.CompositionGroup.SelectedRangedCavalryTypes
      }, this._factionList[this.EnemySide.FactionSelectionGroup.SelectedIndex], new int[4]
      {
        num10,
        num7,
        num8,
        num9
      }, new List<BasicCharacterObject>[4]
      {
        this.EnemySide.CompositionGroup.SelectedMeleeInfantryTypes,
        this.EnemySide.CompositionGroup.SelectedRangedInfantryTypes,
        this.EnemySide.CompositionGroup.SelectedMeleeCavalryTypes,
        this.EnemySide.CompositionGroup.SelectedRangedCavalryTypes
      }, (flag1 ? 1 : 0) != 0);
      Game.Current.PlayerTroop = this._charactersList[this.PlayerSide.CharacterSelectionGroup.SelectedIndex];
      bool isSiege = this.GameTypeSelectionGroup.GetCurrentGameType() == GameTypeSelectionGroup.GameType.Siege;
      MapSelectionElement selectedMap = this.MapSelectionGroup.SelectedMap;
      MapSelectionElement mapWithName = this.MapSelectionGroup.GetMapWithName(this.MapSelectionGroup.SearchText);
      if (mapWithName != null && mapWithName != selectedMap)
        selectedMap = mapWithName;
      CustomBattleSceneData customBattleSceneData = selectedMap != null ? ((IEnumerable<CustomBattleSceneData>) this._customBattleScenes).Single<CustomBattleSceneData>((Func<CustomBattleSceneData, bool>) (s => ((object) ((CustomBattleSceneData)  s).Name).ToString() == selectedMap.MapName)) : ((IEnumerable<CustomBattleSceneData>) this._customBattleScenes).First<CustomBattleSceneData>((Func<CustomBattleSceneData, bool>) (cbs => ((CustomBattleSceneData)  cbs).IsSiegeMap == isSiege));
      float num11 = 6f;
      if (this.MapSelectionGroup.IsCurrentMapSiege)
      {
        Dictionary<SiegeEngineType, int> dictionary1 = new Dictionary<SiegeEngineType, int>();
        foreach (CustomBattleSiegeMachineVM attackerMeleeMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerMeleeMachines)
        {
          if (attackerMeleeMachine.SiegeEngineType != null)
          {
            SiegeEngineType siegeWeaponType = CustomBattleMenuVM.GetSiegeWeaponType(attackerMeleeMachine.SiegeEngineType);
            if (!dictionary1.ContainsKey(siegeWeaponType))
              dictionary1.Add(siegeWeaponType, 0);
            dictionary1[siegeWeaponType]++;
          }
        }
        foreach (CustomBattleSiegeMachineVM attackerRangedMachine in (Collection<CustomBattleSiegeMachineVM>) this._attackerRangedMachines)
        {
          if (attackerRangedMachine.SiegeEngineType != null)
          {
            SiegeEngineType siegeWeaponType = CustomBattleMenuVM.GetSiegeWeaponType(attackerRangedMachine.SiegeEngineType);
            if (!dictionary1.ContainsKey(siegeWeaponType))
              dictionary1.Add(siegeWeaponType, 0);
            dictionary1[siegeWeaponType]++;
          }
        }
        Dictionary<SiegeEngineType, int> dictionary2 = new Dictionary<SiegeEngineType, int>();
        foreach (CustomBattleSiegeMachineVM defenderMachine in (Collection<CustomBattleSiegeMachineVM>) this._defenderMachines)
        {
          if (defenderMachine.SiegeEngineType != null)
          {
            SiegeEngineType siegeWeaponType = CustomBattleMenuVM.GetSiegeWeaponType(defenderMachine.SiegeEngineType);
            if (!dictionary2.ContainsKey(siegeWeaponType))
              dictionary2.Add(siegeWeaponType, 0);
            dictionary2[siegeWeaponType]++;
          }
        }
        int num12;
        float num13 = (float) (num12 = int.Parse(((Collection<SelectorItemVM>) this.MapSelectionGroup.WallHitpointSelection.ItemList)[this.MapSelectionGroup.WallHitpointSelection.SelectedIndex].StringItem)) / 100f;
        float[] numArray = new float[2];
        if (num12 == 50)
        {
          int index = MBRandom.RandomInt(2);
          numArray[index] = 0.0f;
          numArray[1 - index] = 1f;
        }
        else
        {
          numArray[0] = num13;
          numArray[1] = num13;
        }
        BannerlordMissions.OpenSiegeMissionWithDeployment(((CustomBattleSceneData)  customBattleSceneData).SceneID, this._charactersList[this.PlayerSide.CharacterSelectionGroup.SelectedIndex], customBattleParties[0], customBattleParties[1], this.GameTypeSelectionGroup.GetCurrentPlayerType() == GameTypeSelectionGroup.PlayerType.Commander, numArray, ((IEnumerable<CustomBattleSiegeMachineVM>) this._attackerMeleeMachines).Any<CustomBattleSiegeMachineVM>((Func<CustomBattleSiegeMachineVM, bool>) (mm => mm.SiegeEngineType == DefaultSiegeEngineTypes.SiegeTower)), dictionary1, dictionary2, flag1, int.Parse(this.MapSelectionGroup.SceneLevelSelection.GetCurrentItem().StringItem), this.MapSelectionGroup.SeasonSelection.GetCurrentItem().StringItem.ToLower(), this.MapSelectionGroup.IsSallyOutSelected, false, num11);
      }
      else
        BannerlordMissions.OpenCustomBattleMission(((CustomBattleSceneData)  customBattleSceneData).SceneID, this._charactersList[this.PlayerSide.CharacterSelectionGroup.SelectedIndex], customBattleParties[0], customBattleParties[1], flag2, basicCharacterObject, "", this.MapSelectionGroup.SeasonSelection.GetCurrentItem().StringItem.ToLower(), num11);
      Debug.Print("P-Ranged: " + (object) num2 + " P-Mounted: " + (object) num3 + " P-HorseArcher: " + (object) num4 + " P-Infantry: " + (object) num5, 0, (Debug.DebugColor) 5, 17592186044416UL);
      Debug.Print("E-Ranged: " + (object) num7 + " E-Mounted: " + (object) num8 + " E-HorseArcher: " + (object) num9 + " E-Infantry: " + (object) num10, 0, (Debug.DebugColor) 5, 17592186044416UL);
      Debug.Print("EXECUTE START - PRESSED", 0, (Debug.DebugColor) 4, 17592186044416UL);
    }

    private void ExecuteRandomize()
    {
      this.GameTypeSelectionGroup.RandomizeAll();
      this.MapSelectionGroup.RandomizeAll();
      this.PlayerSide.Randomize();
      this.EnemySide.Randomize();
      if (this.MapSelectionGroup.IsCurrentMapSiege)
      {
        this.ExecuteRandomizeAttackerSiegeEngines();
        this.ExecuteRandomizeDefenderSiegeEngines();
      }
      Debug.Print("EXECUTE RANDOMIZE - PRESSED", 0, (Debug.DebugColor) 4, 17592186044416UL);
    }

    private void ExecuteDoneDefenderCustomMachineSelection()
    {
      this.IsDefenderCustomMachineSelectionEnabled = false;
    }

    private void ExecuteDoneAttackerCustomMachineSelection()
    {
      this.IsAttackerCustomMachineSelectionEnabled = false;
    }

    [DataSourceProperty]
    public bool IsAttackerCustomMachineSelectionEnabled
    {
      get
      {
        return this._isAttackerCustomMachineSelectionEnabled;
      }
      set
      {
        if (value == this._isAttackerCustomMachineSelectionEnabled)
          return;
        this._isAttackerCustomMachineSelectionEnabled = value;
        this.OnPropertyChanged(nameof (IsAttackerCustomMachineSelectionEnabled));
      }
    }

    [DataSourceProperty]
    public bool IsDefenderCustomMachineSelectionEnabled
    {
      get
      {
        return this._isDefenderCustomMachineSelectionEnabled;
      }
      set
      {
        if (value == this._isDefenderCustomMachineSelectionEnabled)
          return;
        this._isDefenderCustomMachineSelectionEnabled = value;
        this.OnPropertyChanged(nameof (IsDefenderCustomMachineSelectionEnabled));
      }
    }

    [DataSourceProperty]
    public string RandomizeButtonText
    {
      get
      {
        return this._randomizeButtonText;
      }
      set
      {
        if (!(value != this._randomizeButtonText))
          return;
        this._randomizeButtonText = value;
        this.OnPropertyChanged(nameof (RandomizeButtonText));
      }
    }

    [DataSourceProperty]
    public string TitleText
    {
      get
      {
        return this._titleText;
      }
      set
      {
        if (!(value != this._titleText))
          return;
        this._titleText = value;
        this.OnPropertyChanged(nameof (TitleText));
      }
    }

    [DataSourceProperty]
    public string BackButtonText
    {
      get
      {
        return this._backButtonText;
      }
      set
      {
        if (!(value != this._backButtonText))
          return;
        this._backButtonText = value;
        this.OnPropertyChanged(nameof (BackButtonText));
      }
    }

    [DataSourceProperty]
    public string StartButtonText
    {
      get
      {
        return this._startButtonText;
      }
      set
      {
        if (!(value != this._startButtonText))
          return;
        this._startButtonText = value;
        this.OnPropertyChanged(nameof (StartButtonText));
      }
    }

    [DataSourceProperty]
    public CustomBattleMenuSideVM EnemySide
    {
      get
      {
        return this._enemySide;
      }
      set
      {
        if (value == this._enemySide)
          return;
        this._enemySide = value;
        this.OnPropertyChanged(nameof (EnemySide));
      }
    }

    [DataSourceProperty]
    public CustomBattleMenuSideVM PlayerSide
    {
      get
      {
        return this._playerSide;
      }
      set
      {
        if (value == this._playerSide)
          return;
        this._playerSide = value;
        this.OnPropertyChanged(nameof (PlayerSide));
      }
    }

    [DataSourceProperty]
    public GameTypeSelectionGroup GameTypeSelectionGroup
    {
      get
      {
        return this._gameTypeSelectionGroup;
      }
      set
      {
        if (value == this._gameTypeSelectionGroup)
          return;
        this._gameTypeSelectionGroup = value;
        this.OnPropertyChanged(nameof (GameTypeSelectionGroup));
      }
    }

    [DataSourceProperty]
    public MapSelectionGroup MapSelectionGroup
    {
      get
      {
        return this._mapSelectionGroup;
      }
      set
      {
        if (value == this._mapSelectionGroup)
          return;
        this._mapSelectionGroup = value;
        this.OnPropertyChanged(nameof (MapSelectionGroup));
      }
    }

    [DataSourceProperty]
    public MBBindingList<CustomBattleSiegeMachineVM> AttackerMeleeMachines
    {
      get
      {
        return this._attackerMeleeMachines;
      }
      set
      {
        if (value == this._attackerMeleeMachines)
          return;
        this._attackerMeleeMachines = value;
        this.OnPropertyChanged(nameof (AttackerMeleeMachines));
      }
    }

    [DataSourceProperty]
    public MBBindingList<CustomBattleSiegeMachineVM> AttackerRangedMachines
    {
      get
      {
        return this._attackerRangedMachines;
      }
      set
      {
        if (value == this._attackerRangedMachines)
          return;
        this._attackerRangedMachines = value;
        this.OnPropertyChanged(nameof (AttackerRangedMachines));
      }
    }

    [DataSourceProperty]
    public MBBindingList<CustomBattleSiegeMachineVM> DefenderMachines
    {
      get
      {
        return this._defenderMachines;
      }
      set
      {
        if (value == this._defenderMachines)
          return;
        this._defenderMachines = value;
        this.OnPropertyChanged(nameof (DefenderMachines));
      }
    }

    private IEnumerable<SiegeEngineType> GetAllDefenderRangedMachines()
    {
            // ISSUE: object of a compiler-generated type is created
            //return new CustomBattleMenuVM.\u003CGetAllDefenderRangedMachines\u003Ed__74(-2);
            return (IEnumerable<SiegeEngineType>) new CustomBattleMenuVM(_customBattleState)._defenderMachines;
    }

    private IEnumerable<SiegeEngineType> GetAllAttackerRangedMachines()
    {
            // ISSUE: object of a compiler-generated type is created
            //return (IEnumerable<SiegeEngineType>) new CustomBattleMenuVM.\u003CGetAllAttackerRangedMachines\u003Ed__75(-2);
            return (IEnumerable<SiegeEngineType>)new CustomBattleMenuVM(_customBattleState)._attackerRangedMachines;
        }

    private IEnumerable<SiegeEngineType> GetAllAttackerMeleeMachines()
    {
            // ISSUE: object of a compiler-generated type is created
            //return (IEnumerable<SiegeEngineType>) new CustomBattleMenuVM.\u003CGetAllAttackerMeleeMachines\u003Ed__76(-2);
            return (IEnumerable<SiegeEngineType>)new CustomBattleMenuVM(_customBattleState)._attackerMeleeMachines;
        }

    private static SiegeEngineType GetSiegeWeaponType(SiegeEngineType siegeWeaponType)
    {
      if (siegeWeaponType == DefaultSiegeEngineTypes.Ladder)
        return DefaultSiegeEngineTypes.Ladder;
      if (siegeWeaponType == DefaultSiegeEngineTypes.Ballista)
        return DefaultSiegeEngineTypes.Ballista;
      if (siegeWeaponType == DefaultSiegeEngineTypes.FireBallista)
        return DefaultSiegeEngineTypes.FireBallista;
      if (siegeWeaponType == DefaultSiegeEngineTypes.Ram || siegeWeaponType == DefaultSiegeEngineTypes.ImprovedRam)
        return DefaultSiegeEngineTypes.Ram;
      if (siegeWeaponType == DefaultSiegeEngineTypes.SiegeTower)
        return DefaultSiegeEngineTypes.SiegeTower;
      if (siegeWeaponType == DefaultSiegeEngineTypes.Onager || siegeWeaponType == DefaultSiegeEngineTypes.Catapult)
        return DefaultSiegeEngineTypes.Onager;
      if (siegeWeaponType == DefaultSiegeEngineTypes.FireOnager || siegeWeaponType == DefaultSiegeEngineTypes.FireCatapult)
        return DefaultSiegeEngineTypes.FireOnager;
      return siegeWeaponType == DefaultSiegeEngineTypes.Trebuchet || siegeWeaponType == DefaultSiegeEngineTypes.Bricole ? DefaultSiegeEngineTypes.Trebuchet : siegeWeaponType;
    }
  }
}
