using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarAlgorithm
{
	//マップ情報
	public class MapSpot
	{
		public int idx { get; set; }        //インデックス値
		public Vector3 pos { get; set; }    //座標
		public bool isNoEntry { get; set; } //進入禁止かどうか

		//接続スポット
		public int[] linkIdx { get; set; }  //接続しているスポットのインデックス値
		public int linkNum { //接続しているスポット数
			get { return linkIdx != null ? linkIdx.Length : 0; }
		}

		public MapSpot(int idx, Vector3 pos, bool isNoEntry, int[] linkIdx)
		{
			this.idx = idx;
			this.pos = pos;
			this.isNoEntry = isNoEntry;
			this.linkIdx = linkIdx;
		}
	}

	//探索結果
	public class Result
	{
		public enum Kind {
			None = 0,

			Reachable,      //到達可能
			Unreachable,    //到達不可能（袋小路など）
		}
		public Kind kind { get; set; }
		public Vector3[] path { get; set; }

		public Result()
		{
			this.kind = Kind.None;
			this.path = new Vector3 [0];
		}
	}
	private Result m_Result = new Result();

	//距離計算の種類
	public enum DisCalcKind
	{
		None = 0,

		Euclidean,      //ユークリッド距離による計算（２点の直線距離）
		Manhattan,      //マンハッタン距離による計算（格子上の距離）
	}

	//距離関数テーブル
	private delegate float calcDistanceFunc(Vector3 pos0, Vector3 pos1);
	private static Dictionary<DisCalcKind, calcDistanceFunc> kCalcDistanceFunc = new Dictionary<DisCalcKind, calcDistanceFunc> {
		{ DisCalcKind.Euclidean,        calcEuclideanDistance },
		{ DisCalcKind.Manhattan,        calcManhattanDistance },
	};

	//探索ノード
	private class Node
	{
		public int idx { get; set; }        //インデックス値
		public Vector3 pos { get; set; }    //座標
		public bool isNoEntry { get; set; } //進入禁止かどうか
		public float cost { get; set; }         //コスト値（スタートから当該のノードまでのコスト）
		public float heuristic { get; set; }    //ヒューリスティック値（当該のノードからゴールまでの到達コストの推定値）
		public float score { //スコア
			get { return cost + heuristic; }
		}

		//接続ノード
		public int[] linkIdx { get; set; }  //接続しているノードのインデックス値
		public int linkNum { //接続しているノード数
			get { return linkIdx != null ? linkIdx.Length : 0; }
		}

		//親ノード（探索結果で どのノードから辿ってきたか）
		public int parenIdx { get; set; }

		public Node(int idx, Vector3 pos, bool isNoEntry, int[] linkNo)
		{
			this.idx = idx;
			this.pos = pos;
			this.isNoEntry = isNoEntry;
			this.cost = 0f;
			this.heuristic = 0f;
			this.linkIdx = linkNo;
			this.parenIdx = -1;
		}
	}

	private List<Node> m_OpenList = new List<Node> ();      //オープンリスト
	private List<Node> m_ClosedList = new List<Node> ();    //クローズドリスト

	public AstarAlgorithm ()
	{
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Init()
	{
		m_OpenList.Clear ();
		m_ClosedList.Clear ();
	}

	/// <summary>
	/// A*アルゴリズム探索
	/// （同一フレーム内で探索処理を行う）
	/// </summary>
	public void Search(DisCalcKind disCalcKind, MapSpot[] mapSpot, int startIdx, int goalIdx)
	{
		//初期化
		Node[] nodeMap = searchInit (disCalcKind, mapSpot);

		//オープンリストに開始ノードを追加する
		m_OpenList.Add (nodeMap.Where(n => n.idx == startIdx).First ());

		//探索開始
		while (0 < m_OpenList.Count/* オープンリストが空ではない */) {
			bool isGoal = searchLocal (disCalcKind, nodeMap, goalIdx);
			if (isGoal) {
				break;
			}
		}

		//終了処理
		searchFinish (nodeMap, startIdx, goalIdx);
	}

	/// <summary>
	/// A*アルゴリズム探索
	/// （指定したフレーム数ごとに探索処理を行う）
	/// </summary>
	public IEnumerator SearchPerFrame(DisCalcKind disCalcKind, MapSpot[] mapSpot, int startIdx, int goalIdx, int loopPerFrame)
	{
		//初期化
		Node[] nodeMap = searchInit (disCalcKind, mapSpot);

		//オープンリストに開始ノードを追加する
		m_OpenList.Add (nodeMap.Where(n => n.idx == startIdx).First ());

		//探索開始
		int loopCnt = 0;
		while (0 < m_OpenList.Count/* オープンリストが空ではない */) {
			bool isGoal = searchLocal (disCalcKind, nodeMap, goalIdx);
			if (isGoal) {
				break;
			}

			//一定回数 探索を行うと、yield return nullする
			if (loopPerFrame <= loopCnt++) {
				loopCnt = 0;    //カウンタをリセットする
				yield return null;
			}
		}

		//終了処理
		searchFinish (nodeMap, startIdx, goalIdx);
	}

	/// <summary>
	/// 探索初期化
	/// </summary>
	private Node[] searchInit(DisCalcKind disCalcKind, MapSpot[] mapSpot)
	{
		//エラーチェック
		if (disCalcKind == DisCalcKind.None) {
			//距離計算の種類が正しくない
			BattleException.Throw (string.Format ("disCalcKind == DisCalcKind.None"));
		} else if (!kCalcDistanceFunc.ContainsKey (disCalcKind)) {
			//距離計算関数が関数テーブルに設定されていない
			BattleException.Throw (string.Format ("!kCalcDistanceFunc.ContainsKey (disCalcKind(={0}))", disCalcKind));
		} else if (mapSpot == null || mapSpot.Length <= 0) {
			//マップ情報が正しくない
			BattleException.Throw (string.Format ("mapSpot == null || mapSpot.Length <= 0"));
		}

		m_OpenList.Clear ();
		m_ClosedList.Clear ();

		//マップ情報を探索ノードに置き換える
		List<Node> nodeMapList = new List<Node> ();
		foreach (MapSpot spot in mapSpot) {
			nodeMapList.Add (new Node (
				idx: spot.idx,
				pos: spot.pos,
				isNoEntry: spot.isNoEntry,
				linkNo: spot.linkIdx
			));
		}
		return nodeMapList.ToArray ();
	}

	/// <summary>
	/// 経路探索ローカル
	/// </summary>
	private bool searchLocal(DisCalcKind disCalcKind, Node[] nodeMap, int goalIdx)
	{
		//オープンリストの中で、スコアが一番低いノードを現在のノードとする
		Node nowNode = m_OpenList.OrderBy (node => node.score).ToArray () [0];

		if (nowNode.idx == goalIdx) {
			//目的地に達成した
			return true;
		} else {
			//現在のノードをオープリストから削除して、クローズドリストに追加する
			int openIdx = m_OpenList.IndexOf (node => node.idx == nowNode.idx);
			m_OpenList.RemoveAt (openIdx);
			m_ClosedList.Add (nowNode);

			//現在のノードに隣接する各ノードを調べる
			foreach (int idx in nowNode.linkIdx) {
				int linkIdx = nodeMap.IndexOf (node => node.idx == idx);
				if (linkIdx < 0) {
					//mapSpotに存在しないノードと隣接している
					BattleException.Throw (string.Format ("nodeMapList.IndexOf (node => node.idx == idx(={0})) < 0", idx));
				}

				//進入禁止ではない かつ オープンリストに含まれていない かつ クローズドリストに含まれていない
				if (!nodeMap [linkIdx].isNoEntry && !m_OpenList.Any (node => node.idx == idx) && !m_ClosedList.Any (node => node.idx == idx)) {
					//隣接ノードのコスト、ヒューリスティックを計算する
					nodeMap [linkIdx].cost = nowNode.cost + kCalcDistanceFunc [disCalcKind] (nowNode.pos, nodeMap [linkIdx].pos);
					nodeMap [linkIdx].heuristic = kCalcDistanceFunc [disCalcKind] (nowNode.pos, nodeMap [goalIdx].pos);

					//隣接ノードの親ノードを設定する
					nodeMap [linkIdx].parenIdx = nowNode.idx;

					//オープンリストに追加する
					m_OpenList.Add (nodeMap [linkIdx]);
				}
			}
		}
		return false;
	}

	/// <summary>
	/// 探索終了処理
	/// </summary>
	private void searchFinish(Node[] nodeMap, int startIdx, int goalIdx)
	{
		//探索結果を判定する
		if (0 < m_OpenList.Count) {
			//ゴールに到達した
			m_Result.kind = Result.Kind.Reachable;

			//経路を取得する
			Node node = null;
			int nowIdx = goalIdx;
			List<Vector3> pathList = new List<Vector3> ();
			do {
				//ゴールの親ノードから経路を辿っていく
				node = nodeMap.Where(n => n.idx == nowIdx).First ();
				pathList.Add (node.pos);
				nowIdx = node.parenIdx;
			} while (node.idx != startIdx);
			pathList.Reverse ();
			m_Result.path = pathList.ToArray ();
		} else {
			//ゴールに到達できなかった
			m_Result.kind = Result.Kind.Unreachable;
			m_Result.path = new Vector3[0];
		}
	}

	/// <summary>
	/// 探索結果を取得する
	/// </summary>
	public Result GetResult()
	{
		return m_Result;
	}

	/// <summary>
	/// ユークリッド距離を計算する
	/// </summary>
	private static float calcEuclideanDistance(Vector3 pos0, Vector3 pos1)
	{
		return (pos0 - pos1).magnitude;
	}

	/// <summary>
	/// マンハッタン距離を計算する
	/// </summary>
	private static float calcManhattanDistance(Vector3 pos0, Vector3 pos1)
	{
		return Mathf.Abs (pos0.x - pos1.x) + Mathf.Abs (pos0.y - pos1.y) + Mathf.Abs (pos0.z - pos1.z);
	}
}