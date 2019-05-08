using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class BattleFunc
{
	
	/// <summary>
	/// 指定した２つの座標が同じ水平線上にあるかどうか
	///   ・なお、deltaYに・・・
	///     0を設定すると、basePos.y == targetPos.y の時に、２つの座標は同じ水平線上に存在すると判定される
	///     正の数を設定すると、basePos.y <= targetPos.y <= basePos.y + deltaY - 1 で、２つの座標は同じ水平線上に存在すると判定される
	///     負の数を設定すると、basePos.y + deltaY - 1 <= targetPos.y <= basePos.y で、２つの座標は同じ水平線上に存在すると判定される
	/// </summary>
	public static bool IsSameHorizontalLine(Vector2 basePos, Vector2 targetPos, float deltaY = 0f)
	{
		if (deltaY == 0) {
			return basePos.Equals (targetPos);
		} else if (deltaY > 0) {
			return basePos.y <= targetPos.y && targetPos.y <= (basePos.y + deltaY - 1f);
		} else {
			return (basePos.y + deltaY - 1f) <= targetPos.y && targetPos.y <= basePos.y;
		}
	}

	/// <summary>
	/// 指定した２つの座標が同じ鉛直線上にあるかどうか
	///   ・なお、deltaYに・・・
	///     0を設定すると、basePos.x == targetPos.x の時に、２つの座標は同じ垂直線上に存在すると判定される
	///     正の数を設定すると、basePos.x <= targetPos.x <= basePos.x + deltaX で、２つの座標は同じ垂直線上に存在すると判定される
	///     負の数を設定すると、basePos.x + deltaX <= targetPos.x <= basePos.x で、２つの座標は同じ垂直線上に存在すると判定される
	/// </summary>
	public static bool IsSameVerticalLine(Vector2 basePos, Vector2 targetPos, float deltaX = 0f)
	{
		if (deltaX == 0) {
			return basePos.x.MyEquals (targetPos.x);
		} else if (deltaX > 0) {
			return basePos.x <= targetPos.x && targetPos.x <= (basePos.x + deltaX - 1f);
		} else {
			return (basePos.x + deltaX - 1f) <= targetPos.x && targetPos.x <= basePos.x;
		}
	}

	/// <summary>
	/// タップした座標から巨大ユニットのサイズを元に移動先を計算する
	///   →この関数では、実際に移動できる範囲と関係なく、タップした座標に移動した場合を想定して、ユニットの位置を計算する）
	///    実際に移動できるかどうかは、関数の予備元で判断する必要がある
	///   →戻り値の座標はユニットの左下隅の座標を返す
	/// </summary>
	public Vector2 CalcHugeUnitMovePosFromTapPos(BattleUnit unit, Vector2 basePos/* 基準座標 */, Vector2 tapPos)
	{
		Vector2 movePos = tapPos;
		bool isSameHorizontal = BattleFieldManager.IsSameHorizontalLine (basePos, tapPos, unit.objSize.y);
		bool isSameVertical = BattleFieldManager.IsSameVerticalLine (basePos, tapPos, unit.objSize.x);
		if (isSameHorizontal || isSameVertical) {
			if (isSameHorizontal) {
				/* ユニットに対して、タップ座標が左右方向に存在する場合 */
				float x = tapPos.x < basePos.x ?
					tapPos.x /* ユニットの左側をタップした場合 */ :
					tapPos.x - (unit.objSize.x - 1)/* ユニットの右側をタップした場合 */;

				movePos = new Vector2 (x, basePos.y);
			} else {
				/* ユニットに対して、タップ座標が上下方向に存在する場合 */
				float y = tapPos.y < basePos.y ?
					tapPos.y /* ユニットの下側をタップした場合 */ :
					tapPos.y - (unit.objSize.y - 1)/* ユニットの上側をタップした場合 */;

				movePos = new Vector2 (basePos.x, y);
			}

			int distance = BattleFieldManager.CalcBoxDistance (turnStartPos, movePos);
			if (unit.Move < distance) {
				//movePosが移動範囲外なので、タップ座標方向の単位ベクトル * diff 分だけ平行移動させる
				float diff = distance - unit.Move;

				Vector2 vec = Vector2.zero;
				vec.x = isSameVertical ? (movePos.x < tapPos.x ? diff : -diff) : 0f;
				vec.y = isSameHorizontal ? (movePos.y < tapPos.y ? diff : -diff) : 0f;

				movePos = movePos + vec;
			}
			return movePos;
		} else {
			/* ユニットに対して、タップ座標が斜め方向に存在する場合 */
			Vector2 disXY = BattleFieldManager.CalcBoxDistanceXY (basePos, unit.objSize, tapPos, Vector2.one);
			Vector2 vec = tapPos - basePos;
			vec = new Vector2 (vec.x.MyEquals (0f) ? 0f : Math.Abs (vec.x) / vec.x, vec.y.MyEquals (0f) ? 0f : Math.Abs (vec.y) / vec.y);
			return new Vector2 (basePos.x + vec.x * disXY.x, basePos.y + vec.y * disXY.y);
		}
	}

	/// <summary>
	/// 目的の場所まで移動するのに必要なマス数を計算する
	/// </summary>
	public static int CalcBoxDistance(Vector2 fromPos, Vector2 fromSize, Vector2 toPos, Vector2 toSize)
	{
		bool isFromOne = fromSize.Equals (Vector2.one);
		bool isToOne = toSize.Equals (Vector2.one);

		if (isFromOne && isToOne) {
			return (int)Math.Abs (toPos.x - fromPos.x) + (int)Math.Abs (toPos.y - fromPos.y);
		} else if (isFromOne || isToOne) {
			//点と矩形のマンハッタン距離を求める
			Vector2 dot = isFromOne ? fromPos : toPos;
			Vector2[] vertex = isToOne ?
				//矩形の頂点座標は左下から時計回りに設定する
				new Vector2[] {
				fromPos,
				new Vector2 (fromPos.x, fromPos.y + fromSize.y - 1),
				new Vector2 (fromPos.x + fromSize.x - 1, fromPos.y + fromSize.y - 1),
				new Vector2 (fromPos.x + fromSize.x - 1, fromPos.y)
			} :
				new Vector2[] {
				toPos,
				new Vector2 (toPos.x, toPos.y + toSize.y - 1),
				new Vector2 (toPos.x + toSize.x - 1, toPos.y + toSize.y - 1),
				new Vector2 (toPos.x + toSize.x - 1, toPos.y)
			};

			//点と矩形のX軸方向の距離を求める
			float disX = 0f;
			if (dot.x < vertex [0].x) {
				//このとき、点は矩形の左辺が近くなる
				if (vertex [0].Equals (vertex [1])) {
					//vertex[0]とvertex[1]は重なるので、dotとvertex[0]のX軸方向の距離を求める
					disX = Mathf.Abs (dot.x - vertex [0].x);
				} else {
					//点と直線の距離を求める
					disX = calcDotLineDistance (dot, vertex [0], vertex [1]);
				}
			} else if (vertex [3].x < dot.x) {
				//このとき、点は矩形の右辺が近くなる
				if (vertex [2].Equals (vertex [3])) {
					//vertex[2]とvertex[3]は重なるので、dotとvertex[2]のX軸方向の距離を求める
					disX = Mathf.Abs (dot.x - vertex [2].x);
				} else {
					//点と直線の距離を求める
					disX = calcDotLineDistance (dot, vertex [2], vertex [3]);
				}
			} else {
				//このとき、点は上辺または下辺が近くなる
				disX = 0f;
			}

			//点と矩形のY軸方向の距離を求める
			float disY = 0f;
			if (dot.y < vertex [0].y) {
				//このとき、点は矩形の下辺が近くなる
				if (vertex [0].Equals (vertex [3])) {
					//vertex[0]とvertex[3]は重なるので、dotとvertex[0]のY軸方向の距離を求める
					disX = Mathf.Abs (dot.y- vertex [0].y);
				} else {
					//点と直線の距離を求める
					disY = calcDotLineDistance (dot, vertex [3], vertex [0]);
				}
			} else if (vertex [1].y < dot.y) {
				//このとき、点は矩形の上辺が近くなる
				if (vertex [1].Equals (vertex [2])) {
					//vertex[1]とvertex[2]は重なるので、dotとvertex[1]のY軸方向の距離を求める
					disX = Mathf.Abs (dot.y- vertex [1].y);
				} else {
					//点と直線の距離を求める
					disY = calcDotLineDistance (dot, vertex [1], vertex [2]);
				}
			} else {
				//このとき、点は左辺または右辺が近くなる
				disY = 0f;
			}

			return (int)(disX + disY);
		} else {
			//矩形どうしのマンハッタン距離の求め方がややこしいので、一旦 考慮しない
			BattleException.Throw ("fromSize is not Vector2.one, and toSize is not Vector2.one");
			return int.MaxValue;
		}
	}


	/// <summary>
	/// 吹き飛ばし処理
	///   →実装アルゴリズムは、https://qiita.com/nao-key_0504/private/06dc3d7d351c6f9716a7 を御参照ください。
	/// </summary>
	/// <returns>ユニットと吹き飛ばし後の座標の連想配列</returns>
	/// <param name="center">吹き飛ばしの中心</param>
	/// <param name="thrustAwayUnits">吹き飛ばし対象のユニット</param>
	public static Dictionary<BattleUnit, Vector2> ThrustAway(Vector2 center, BattleUnit[] thrustAwayUnits)
	{
		//マップの４隅の座標（corners[0] = (0, 0)として、時計周りにcorners[1], corners[2], corners[3]とする）
		Vector2[] corners = new Vector2[4];
		corners [0] = Vector2.zero;
		corners [1] = new Vector2 (0f, mapSize.y - 1);
		corners [2] = new Vector2 (mapSize.x - 1, mapSize.y - 1);
		corners [3] = new Vector2 (mapSize.x - 1, 0f);

		//吹き飛ばしの中心座標
		Vector2 O = center;

		//ステップ１：吹き飛ばしの中心座標から遠いユニット順に並べる
		//  →吹き飛ばしで座標を更新するユニットは、中心座標から遠いユニットから行わないといけない
		BattleUnit[] orderedUnits = thrustAwayUnits.OrderByDescending(unit => CalcBoxDistance(center, BattleFieldManager.Instance.GetUnitPos (unit.UniqueTag))).ToArray ();

		Dictionary<BattleUnit, Vector2> output = new Dictionary<BattleUnit, Vector2>();
		foreach (BattleUnit unit in orderedUnits) {
			//ステップ２：中心座標から吹き飛ばすユニットへのベクトル（以下、ベクトルOP）が、マップの一番端のどの辺に接するかを求める
			//  →吹き飛ばし中心とマップの４隅を結び、マップの４辺で三角形を４つ作り、
			//   各三角形とユニットの座標の内外判定で、どの辺に接するかを求める。
			Vector2 P = GetUnitPos (unit.UniqueTag);
			Vector2 A = Vector2.zero;
			Vector2 B = Vector2.zero;
			for (int idx = 0; idx < corners.Length; ++idx) {
				A = corners [idx];
				B = corners [(idx + 1) % corners.Length];
				if (isPointWithinTriangle (P, O, A, B)) {
					break;
				}
			}

			//ステップ３：OPと正方向に交わるマップ端のベクトル（以下、AB）の交点Iを求める
			Vector2 PO = O - P;
			Vector2 AB = B - A;
			Vector2 I = Vector2.zero;
			if (!crossProduct(AB, PO).Equals (0f)) {
				/* 吹き飛ばす側と吹き飛ばされる側どちら片方がマップ端の４辺に居ない場合 */
				I = calcLineIntersection (O, P, A, B);
			} else {
				/* 吹き飛ばす側と吹き飛ばされる側が共にマップ端の４辺に居る場合 */
				float cos = Vector2.Dot (AB, PO) / AB.magnitude * PO.magnitude;
				if (0 < cos) { //座標Aの方向に飛ばされる
					//交点IをAとする
					I = A;
				} else { //座標Bの方向に飛ばされる
					//交点IをBとする
					I = B;
				}
			}

			//ステップ４：座標Iに移動できない場合（ユニットがいる・障害物がある、等）、座標IからベクトルPO（吹き飛ばすユニット→中心座標）の単位ベクトルuだけ戻す
			Vector2 pos = Vector2.zero;
			Vector2 u = PO / PO.magnitude;
			float k = 0f;
			do {
				pos = I + k * u;

				//座標の値を整数化する
				pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
				if (IsPosFieldOut (pos)) {
					//マップ外なら、これ以上 whileループを回さない
					break;
				}

				k += 1f;
			} while (!isEnableThrustAway (unit, pos));

			output.Add (unit, pos);
		}
		return output;
	}

	/// <summary>
	/// 線分PQとユニットが交差するかどうか（線分の端点も含む）
	/// </summary>
	public static bool IsSegmentClossingUnit(BattleUnit unit, Vector2 P, Vector2 Q)
	{
		Vector2 A = GetUnitPos (unit.UniqueTag);
		Vector2 B = A + new Vector2 (0f, unit.objSize.y - 1f);
		Vector2 C = B + new Vector2 (unit.objSize.x - 1f, 0f);
		Vector2 D = C - new Vector2 (0f, unit.objSize.y - 1f);
		return isSegmentCrossingRect (P, Q, A, B, C, D);
	}

	/// <summary>
	/// 点Pと三角形ABCの内外判定
	///（true:三角形ABCの中である(辺を含む)、false：三角形ABCの外である）
	/// </summary>
	private static bool isPointWithinTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
	{
		return isPointWithinPolygon (P, new Vector2[] { A, B, C });
	}

	/// <summary>
	/// 点Pと凸四角形ABCDの内外判定
	///（true:凸四角形ABCDの中である(辺を含む)、false：凸四角形ABCDの外である）
	/// </summary>
	private static bool isPointWithinRect(Vector2 P, Vector2 A, Vector2 B, Vector2 C, Vector2 D)
	{
		return isPointWithinPolygon (P, new Vector2[] { A, B, C, D });
	}

	/// <summary>
	/// 線分PQと凸四角形ABCDの交差判定
	///（true:凸四角形ABCDと交差する(辺上を含む)、false：凸四角形ABCDと交差しない）
	/// </summary>
	private static bool isSegmentCrossingRect(Vector2 P, Vector2 Q, Vector2 A, Vector2 B, Vector2 C, Vector2 D)
	{
		Vector2[,] rectSegments = new Vector2[4, 2] {
			{A, B},
			{B, C},
			{C, D},
			{D, A},
		};
		for (int idx = 0; idx < rectSegments.GetLength (0); ++idx) {
			if (Utility.IsClossingSegments (P, Q, rectSegments [idx, 0], rectSegments [idx, 1])) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 回転していない長方形の内外判定
	/// (lbPos:左下の座標, rectSize:マス単位のサイズ)
	/// </summary>
	private static bool isPointWithinRectagle(Vector2 P, Vector2 lbPos, Vector2 rectSize)
	{
		if (rectSize.x.MyEquals (1f)) {
			//Y軸方向の点と線分の線上判定で処理する
			return Utility.IsDotOnSegment (P, lbPos, lbPos + new Vector2(0f, rectSize.y - 1));
		} else if (rectSize.y.MyEquals (1f)) {
			//X軸方向の点と線分の線上判定で処理する
			return Utility.IsDotOnSegment (P, lbPos, lbPos + new Vector2(rectSize.x - 1, 0f));
		} else if (!rectSize.Equals (Vector2.one)) {
			//点と凸多角形の内外判定で処理する
			Vector2[] vertex = new Vector2[] {
				lbPos,
				new Vector2 (lbPos.x, lbPos.y + rectSize.y - 1),
				new Vector2 (lbPos.x + rectSize.x - 1, lbPos.y + rectSize.y - 1),
				new Vector2 (lbPos.x + rectSize.x - 1, lbPos.y),
			};
			return isPointWithinPolygon (P, vertex);
		} else {
			return P.Equals (lbPos);
		}
	}

	/// <summary>
	/// 点Pと（時計回り or 反時計回りの順番に並ぶ頂点vertexをもつ）凸多角形の内外判定
	///（true:多角形の中である(辺を含む)、false：多角形の外である）
	/// </summary>
	private static bool isPointWithinPolygon(Vector2 P, params Vector2[] vertex)
	{
		//多角形ではない
		int vertexLen = vertex.Length;
		if (vertexLen <= 2) {
			return false;
		}

		int[] crossSign = new int [vertexLen];
		for (int idx = 0; idx < vertexLen; ++idx) {
			int nextIdx = (idx + 1) % vertexLen;        //隣の頂点
			Vector2 vecSide = vertex [nextIdx] - vertex [idx];  //多角形の辺ベクトル
			Vector2 vecP = P - vertex [idx];                    //各頂点から点Pへのベクトル

			//辺ベクトルと、各頂点から点Pへのベクトルの外積を計算する
			float cross = crossProduct (vecSide, vecP);

			//外積の値の符号を取得する
			crossSign [idx] = System.Math.Sign (cross);
		}

		int prevSign = 0;
		for (int idx = 0; idx < vertexLen; ++idx) {
			if (crossSign [idx] == 0) {
				//ゼロの場合はスルー
				continue;
			} else if (prevSign != 0 && prevSign != crossSign [idx]) {
				//符号が異なる場合、凸多角形の外側にある
				return false;
			}
			prevSign = crossSign [idx];
		}
		return true;
	}

	/// <summary>
	/// 点Pと２点A, Bを通る直線との距離を計算する
	/// </summary>
	private static float calcDotLineDistance(Vector2 P, Vector2 A, Vector2 B)
	{
		return Mathf.Abs (crossProduct (B - A, P - A)) / (B - A).magnitude;
	}

	/// <summary>
	/// ２点A, Bを通る直線と２点P, Qを通る直線の交点を求める
	/// </summary>
	private static Vector2 calcLineIntersection(Vector2 A, Vector2 B, Vector2 P, Vector2 Q)
	{
		Vector2 AB = B - A;
		Vector2 PQ = Q - P;
		return A + AB * crossProduct (PQ, P - A) / crossProduct (PQ, AB);
	}

	/// <summary>
	/// ２つのベクトルの外積を計算する
	/// </summary>
	private static float crossProduct(Vector2 vec1, Vector2 vec2)
	{
		return vec1.x * vec2.y - vec2.x * vec1.y;
	}
}
