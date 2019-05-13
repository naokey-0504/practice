using UnityEngine;
using UnityEngine.UI;

namespace Bayes
{
	public class BayesUI : MonoBehaviour
	{
		private const int kTrappedSumInit = 37;
		private const int kTrappedLockedInit = 29;
		private const int kNotTrappedSumInit = 63;
		private const int kNotTrappedLockedInit = 18;

		private Text m_LockedNum, m_TrapNum;
		private Text m_StolenText;
		private Button m_StartBtn, m_ResultBtn, m_NextBtn;

		private InputField m_TrappedSum, m_TrappedLocked, m_NotTrappedSum, m_NotTrappedLocked;

		public BayesUI ()
		{
		}

		public void Awake()
		{
			Transform textDisp = transform.Find ("TextDisp");
			m_LockedNum = textDisp.Find ("LockedNum").GetComponent<Text> ();
			m_TrapNum = textDisp.Find ("TrappedNum").GetComponent<Text> ();

			Transform btn = transform.Find ("Btn");
			m_StartBtn = btn.Find ("StartBtn").GetComponent<Button> ();
			m_ResultBtn = btn.Find ("ResultBtn").GetComponent<Button> ();
			m_NextBtn = btn.Find ("NextBtn").GetComponent<Button> ();

			m_StolenText = transform.Find ("StolenText/Text").GetComponent<Text> ();

			Transform experience = transform.Find ("Experience");
			Transform trapped = experience.Find ("Trapped");
			m_TrappedSum = trapped.Find ("SumInputField").GetComponent<InputField> ();
			m_TrappedSum.interactable = false;
			m_TrappedLocked = trapped.Find ("LockedInputField").GetComponent<InputField> ();
			m_TrappedLocked.interactable = false;
			Transform notTrapped = experience.Find ("NotTrapped");
			m_NotTrappedSum = notTrapped.Find ("SumInputField").GetComponent<InputField> ();
			m_NotTrappedSum.interactable = false;
			m_NotTrappedLocked = notTrapped.Find ("LockedInputField").GetComponent<InputField> ();
			m_NotTrappedLocked.interactable = false;

			SetActiveStartBtn (false);
			SetActiveResultBtn (false);
			SetActiveNextBtn (false); 
		}

		public void SetLockedNum(int lockedNum, int lockedMax)
		{
			m_LockedNum.text = string.Format ("Locked Num = {0} / {1}", lockedNum, lockedMax);
		}

		public void SetTrappedNum(int trappedNum, int trappedMax)
		{
			m_TrapNum.text = string.Format ("Trap Num = {0} / {1}", trappedNum, trappedMax);
		}

		public void SetStolenText(string text)
		{
			m_StolenText.text = text;
		}
		
		public void SetActiveStartBtn(bool flg)
		{
			m_StartBtn.gameObject.SetActive (flg);
		}

		public void AddListenerStartBtnOnClick(UnityEngine.Events.UnityAction onClick)
		{
			m_StartBtn.onClick.AddListener (onClick);
		}

		public void SetActiveResultBtn(bool flg)
		{
			m_ResultBtn.gameObject.SetActive (flg);
		}

		public void AddListenerResultBtnOnClick(UnityEngine.Events.UnityAction onClick)
		{
			m_ResultBtn.onClick.AddListener (onClick);
		}

		public void SetActiveNextBtn(bool flg)
		{
			m_NextBtn.gameObject.SetActive (flg);
		}

		public void AddListenerNextBtnOnClick(UnityEngine.Events.UnityAction onClick)
		{
			m_NextBtn.onClick.AddListener (onClick);
		}

		public void SetExperienceText(int trappedSum, int trappedLocked, int notTrappedSum, int notTrappedLocked)
		{
			m_TrappedSum.text = trappedSum.ToString ();
			m_TrappedLocked.text = trappedLocked.ToString ();
			m_NotTrappedSum.text = notTrappedSum.ToString ();
			m_NotTrappedLocked.text = notTrappedLocked.ToString ();
		}
	}
}