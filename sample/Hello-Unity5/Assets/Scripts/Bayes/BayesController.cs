using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bayes
{
	public class BayesController : MonoBehaviour
	{
		private const int kTreasureMax = 30;
		private const int kLockedMax = 15;
		private const int kTrappedMax = 30;//15;

		private enum Phase
		{
			None = 0,

			Init,
			Set,
			Start,
			Result,
		}
		private Phase m_Phase = Phase.None;

		private Treasure m_TreasurePrefab;
		private Transform m_TreasureList;
		private int m_TreasureNum = 0, m_LockedNum = 0, m_TrappedNum = 0;

		private Treasure[] m_TreasureArray = new Treasure[kTreasureMax];
		private Thief m_Thief;
		private BayesUI m_BayesUI;

		private int m_StealIdx = 0;

		private void Start ()
		{
			m_TreasurePrefab = Resources.Load<Treasure> ("Prefabs/Bayes/Treasure");

			Transform canvas = transform.Find ("Canvas");
			m_TreasureList = canvas.Find ("TreasureList");
			m_Thief = canvas.Find ("Thief").GetComponent<Thief> ();
			m_BayesUI = canvas.Find ("BayesUI").GetComponent<BayesUI> ();
			m_BayesUI.AddListenerStartBtnOnClick (() => {
				m_Phase = Phase.Start;
				m_StealIdx = 0;
				changeThiefParentToTreasure (m_StealIdx);
				foreach (Treasure t in m_TreasureArray) {
					t.RemoveAllListenersPadlockBtn ();
					t.RemoveAllListenersTrapBtn ();
				}
				m_BayesUI.SetActiveStartBtn (false);
				m_BayesUI.SetActiveNextBtn (true);
				m_BayesUI.SetExperienceText (m_Thief.GetTrappedSum (), m_Thief.GetTrappedLockedNum (), m_Thief.GetNotTrappedSum (), m_Thief.GetNotTrappedLockedNum ());
			});
			m_BayesUI.AddListenerResultBtnOnClick (() => {
				m_Phase = Phase.Init;
				m_BayesUI.SetActiveResultBtn (false);
			});
			m_BayesUI.AddListenerNextBtnOnClick (() => {
				Treasure treasure = m_TreasureArray [m_StealIdx];
				bool tryOpening = m_Thief.Infer (treasure.isLocked);
				if (tryOpening) {
					treasure.Opened ();
					m_Thief.SetExperience (treasure.isTrapped, treasure.isLocked);
					m_BayesUI.SetExperienceText (m_Thief.GetTrappedSum (), m_Thief.GetTrappedLockedNum (), m_Thief.GetNotTrappedSum (), m_Thief.GetNotTrappedLockedNum ());
				}

				if (m_StealIdx < kTreasureMax - 1) {
					++m_StealIdx;
					changeThiefParentToTreasure (m_StealIdx);
				} else {
					m_Phase = Phase.Result;
					m_BayesUI.SetActiveResultBtn (true);
					m_BayesUI.SetActiveNextBtn (false);
					m_Thief.ChangeParent (canvas.transform);
				}
			});

			m_Phase = Phase.Init;
		}
		
		// Update is called once per frame
		private void Update ()
		{
			switch (m_Phase) {
			case Phase.Init:
				m_Phase = Phase.Set;
				createTreasureList ();
				m_BayesUI.SetActiveStartBtn (true);
				break;

			case Phase.Set:
				m_LockedNum = m_TrappedNum = 0;
				for (int i = 0; i < kTreasureMax; ++i) {
					m_LockedNum = m_TreasureArray [i].isLocked ? ++m_LockedNum : m_LockedNum;
					m_TrappedNum = m_TreasureArray [i].isTrapped ? ++m_TrappedNum : m_TrappedNum;
				}
				m_BayesUI.SetLockedNum (m_LockedNum, kLockedMax);
				m_BayesUI.SetTrappedNum (m_TrappedNum, kTrappedMax);
				break;

			case Phase.Start:				
				break;

			case Phase.Result:
				break;
			}
		}

		private void createTreasureList()
		{
			for (int i = m_TreasureList.childCount - 1; 0 <= i; --i) {
				GameObject.Destroy (m_TreasureList.GetChild (i).gameObject);
			}

			m_TreasureNum = 0;
			for (int i = 0; i < kTreasureMax; ++i) {
				m_TreasureArray [i] = createTreasure ();
			}
		}

		private Treasure createTreasure()
		{
			Treasure obj = GameObject.Instantiate<Treasure> (m_TreasurePrefab, m_TreasureList);
			obj.name = "Treasure" + m_TreasureNum;
			obj.SetIsEnableLockedFunc (() => {
				return m_LockedNum < kLockedMax;
			});
			obj.SetIsEnableTrapFunc (() => {
				return m_TrappedNum < kTrappedMax;
			});
			++m_TreasureNum;
			return obj;
		}

		private void changeThiefParentToTreasure(int treasureIdx)
		{
			m_Thief.ChangeParent (m_TreasureArray [treasureIdx].GetTheifParent ());
		}
	}
}