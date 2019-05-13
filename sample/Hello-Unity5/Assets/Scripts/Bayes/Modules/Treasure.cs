using UnityEngine;
using UnityEngine.UI;

namespace Bayes
{
	public class Treasure : MonoBehaviour
	{
		public delegate bool IsEnableFunc();

		private Image m_Open, m_Close;

		private Image m_Locked, m_Unlocked;
		private Button m_PadlockBtn;

		private Image m_Trap;
		private Button m_TrapBtn;

		private Transform m_ThiefParent;

		private IsEnableFunc m_IsEnableLockedFunc = null, m_IsEnableTrappedFunc = null;

		private bool m_IsLocked = false;
		public bool isLocked { get { return m_IsLocked; } }

		private bool m_IsTrapped = true;
		public bool isTrapped { get { return m_IsTrapped; } }

		public Treasure ()
		{
		}

		public void Awake()
		{
			m_Open = transform.Find ("Box/Open").GetComponent<Image> ();
			m_Close = transform.Find ("Box/Close").GetComponent<Image> ();

			Transform padlock = transform.Find ("Padlock");
			m_Locked = padlock.Find("Locked").GetComponent<Image> ();
			m_Unlocked = padlock.Find("Unlocked").GetComponent<Image> ();
			m_PadlockBtn = padlock.Find ("Button").GetComponent<Button> ();
			m_PadlockBtn.onClick.AddListener (switchPadlock);

			Transform trap = transform.Find ("Trap");
			m_Trap = trap.GetComponent<Image> ();
			m_TrapBtn = trap.Find ("Button").GetComponent<Button> ();
			m_TrapBtn.onClick.AddListener (switchTrap);

			m_ThiefParent = transform.Find ("ThiefParent");
		}

		public void Start()
		{
			m_Open.enabled = false;
			m_Close.enabled = true;
			setEnabledPadlock ();
			setEnabledTrap ();
		}

		public void RemoveAllListenersPadlockBtn()
		{
			m_PadlockBtn.onClick.RemoveAllListeners ();
		}

		public void SetIsEnableLockedFunc(IsEnableFunc func)
		{
			m_IsEnableLockedFunc = func;
		}

		private void switchPadlock()
		{
			//ロックする場合
			if (!m_IsLocked) {
				if (m_IsEnableLockedFunc != null && !m_IsEnableLockedFunc ()) {
					return;
				}
			}
			m_IsLocked = !m_IsLocked;
			setEnabledPadlock ();
		}

		private void setEnabledPadlock()
		{
			m_Locked.enabled = m_IsLocked;
			m_Unlocked.enabled = !m_IsLocked;
		}

		public void RemoveAllListenersTrapBtn()
		{
			m_TrapBtn.onClick.RemoveAllListeners ();
		}

		public void SetIsEnableTrapFunc(IsEnableFunc func)
		{
			m_IsEnableTrappedFunc = func;
		}

		private void switchTrap()
		{
			//罠を設置する場合
			if (!m_IsTrapped) {
				if (m_IsEnableTrappedFunc != null && !m_IsEnableTrappedFunc ()) {
					return;
				}
			}
			m_IsTrapped = !m_IsTrapped;
			setEnabledTrap ();
		}

		private void setEnabledTrap()
		{
			m_Trap.enabled = m_IsTrapped;
		}

		public Transform GetTheifParent()
		{
			return m_ThiefParent;
		}

		public void Opened()
		{
			m_Open.enabled = true;
			m_Close.enabled = false;
		}
	}
}