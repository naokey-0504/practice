using UnityEngine;

public class ShootBall : MonoBehaviour {
	void Start () {
		Rigidbody rb = this.GetComponent<Rigidbody> ();  // rigidbodyを取得
		Vector3 force = new Vector3 (5.0f, 0.0f, 0.0f);    // 力を設定
		rb.AddForce (force, ForceMode.Impulse);  // 力を加える
	}
}