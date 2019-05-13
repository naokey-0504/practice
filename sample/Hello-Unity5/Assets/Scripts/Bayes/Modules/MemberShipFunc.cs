using System;

namespace Bayes
{
	public static class MemberShipFunc
	{
		/// 傾斜型(右上り)のメンバーシップ関数
		public static float Grade(float value, float x0, float x1)
		{
			float result = 0.0f;
			float x = value;

			if (x <= x0) {
				result = 0.0f;
			} else if (x >= x1) {
				result = 1.0f;
			} else {
				result = (x - x0) / (x1 - x0);  
			}
			return result;
		}

		/// 逆傾斜型(右下り)のメンバーシップ関数
		public static float ReverseGrade(float value, float x0, float x1)
		{
			float result = 0.0f;
			float x = value;

			if (x <= x0) {
				result = 1.0f;
			} else if (x >= x1) {
				result = 0.0f;
			} else {
				result = (-x + x1) / (x1 - x0);  
			}
			return result;
		}

		/// 三角形のメンバーシップ関数
		public static float Triangle(float value, float x0, float x1, float x2)
		{
			float result = 0.0f;
			float x = value;

			if (x <= x0 || x2 <= x) {
				result = 0.0f;
			} else if (x == x1) {
				result = 1.0f;
			} else if ((x > x0) && (x < x1)) {
				result = (x - x0) / (x1 - x0);
			} else {
				result = (-x + x2) / (x2 - x1);
			}
			return result;
		}

		/// 台形型のメンバーシップ関数
		public static float Trapezoid(float value, float x0, float x1, float x2, float x3)
		{
			float result = 0.0f;
			float x = value;

			if (x <= x0 || x3 <= x) {
				result = 0.0f;
			} else if ((x >= x1) && (x <= x2)) {
				result = 1.0f;
			} else if ((x > x0) && (x < x1)) {
				result = (x - x0) / (x1 - x0);
			} else {
				result = (-x + x3) / (x3 - x2);
			}
			return result;
		}
	}
}