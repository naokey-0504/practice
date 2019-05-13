using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Bayes
{
	public class Thief : MonoBehaviour
	{
		public int m_TrappedSum = 37;
		public int m_TrappedLockedNum = 29;
		public int m_NotTrappedSum = 63;
		public int m_NotTrappedLockedNum = 18;

		private Image m_Image;

		public Thief ()
		{
		}

		public void Start()
		{
			m_Image = transform.GetComponent<Image> ();
		}

		/// <summary>
		/// 宝箱を開けるかどうかを推理する
		/// </summary>
		/// <return>宝箱を開けるかどうか(true:開ける / false:開けない)</return>
		private int num = 0;

		public bool Infer(bool locked)
		{
			float sum = (float)(m_TrappedSum + m_NotTrappedSum);
			float pTrapped = m_TrappedSum / sum;
			float pNotTrapped = m_NotTrappedSum / sum;
			float pLocked_Trapped = m_TrappedLockedNum / (float)m_TrappedSum;
			float pLocked_NotTrapped = m_NotTrappedLockedNum / (float)m_NotTrappedSum;

			float pLocked = pLocked_Trapped * pTrapped + pLocked_NotTrapped * pNotTrapped;

			float estimateTrapped = 0f;
			if (locked) {
				float pTrapped_Locked = pLocked_Trapped * pTrapped / pLocked;
				estimateTrapped = pTrapped_Locked;
			} else {
				float pNotLocked_Trapeed = 1f - pLocked_Trapped;
				float pNotLocked = 1f - pLocked;
				float pTrapped_NotLocked = pNotLocked_Trapeed * pTrapped / pNotLocked;
				estimateTrapped = pTrapped_NotLocked;
			}

			float high = MemberShipFunc.Grade (estimateTrapped, 0.3f, 0.7f);
			float middle = MemberShipFunc.Triangle (estimateTrapped, 0.2f, 0.5f, 0.8f);
			float low = MemberShipFunc.ReverseGrade (estimateTrapped, 0.3f, 0.7f);

			float open = 1f;
			float notOpen = -1f;

			float fuzzyRule1 = high;
			float fuzzyRule2 = low;
			float openRate = (notOpen * fuzzyRule1 + open * fuzzyRule2) / (fuzzyRule1 + fuzzyRule2);
			saveExperience ("FileName", string.Format ("{0}, {1:0.00}", ++num, openRate));
			return 0f < openRate;
		}

		public int GetTrappedSum()
		{
			return m_TrappedSum;
		}

		public int GetTrappedLockedNum()
		{
			return m_TrappedLockedNum;
		}

		public int GetNotTrappedSum()
		{
			return m_NotTrappedSum;
		}

		public int GetNotTrappedLockedNum()
		{
			return m_NotTrappedLockedNum;
		}

		public void ChangeParent(Transform parent)
		{
			m_Image.transform.parent = parent;
			m_Image.transform.localPosition = Vector3.zero;
		}

		public void SetExperience(bool isTrapped, bool isLocked)
		{
			if (isTrapped){
				++m_TrappedSum;
				if (isLocked) {
					++m_TrappedLockedNum;
				}
			} else {
				++m_NotTrappedSum;
				if (isLocked) {
					++m_NotTrappedLockedNum;
				}
			}
		}

		public void saveExperience(string fileName, string txt){
			StreamWriter sw;
			FileInfo fi;
			fi = new FileInfo (Application.dataPath + "/" + fileName + ".csv");
			sw = fi.AppendText();
			sw.WriteLine(txt);
			sw.Flush();
			sw.Close();
		}
	}
}