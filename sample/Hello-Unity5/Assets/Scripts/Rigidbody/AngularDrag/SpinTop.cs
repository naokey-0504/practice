using UnityEngine;

public class SpinTop : MonoBehaviour {
	public float m_TorqueRate = 25.0f;
	public float m_MaxAngularVelocity = 7f;

	void Start () {
		Rigidbody rb = this.GetComponent<Rigidbody> ();  // rigidbodyを取得
		rb.maxAngularVelocity = m_MaxAngularVelocity;
		rb.AddTorque (transform.up * m_TorqueRate);  // 力を加える
	}
}