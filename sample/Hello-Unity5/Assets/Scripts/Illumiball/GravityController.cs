using UnityEngine;
using System.Collections;

public class GravityController : MonoBehaviour
{
	//重力加速度
	private const float kGravity = 9.81f;

	//重力の適用具合
	public float kGravityScale = 1.0f;
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 vector = new Vector3 ();

		//Unityエディタと実機で処理を切り分ける
		#if false
		if (Application.isEditor)
		{
			//キーの入力を検知しベクトルを設定
			vector.x = Input.GetAxis ("Horizontal");
			vector.z = Input.GetAxis ("Vertical");

			//高さ方向の判定はキーのzとする
			if (Input.GetKey ("z")) {
				vector.y = 1.0f;
			} else {
				vector.y = -1.0f;
			}
		} else {
			//加速度センサーの入力をUnity空間軸にマッピングする
			vector.x = Input.acceleration.x;
			vector.z = Input.acceleration.y;
			vector.y = Input.acceleration.z;
		}
		#else
		//加速度センサーの入力をUnity空間軸にマッピングする
		vector.x = Input.acceleration.x;
		vector.z = Input.acceleration.y;
		vector.y = Input.acceleration.z;
		#endif

		//シーンの重力を入力ベクトルの方向に合わせて変化させる
		Physics.gravity = kGravity * vector.normalized * kGravityScale;
	}
}
