using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

[System.Serializable]
public class AxleInfo
{
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}

public class RobotController : MonoBehaviour
{
	public List<AxleInfo> axleInfos;
	public float MaxMotorTorque = 400;  // モーターの最大トルク
	public float MaxSteeringAngle = 30; // ステアリングの最大回転角度

	public void ApplyLocalPosiitonToVisuals (WheelCollider collider)
	{
		// エラー処理（車輪用コライダーの子要素がなければ終了）
		if (collider.transform.childCount == 0)
			return;

		// 車輪ビジュアルの子要素を取得
		Transform wheel = collider.transform.GetChild (0);

		// コライダーの位置・回転角を取得
		Vector3 pos;
		Quaternion q;
		collider.GetWorldPose (out pos, out q);

		// 車輪ビジュアルの位置と回転角を設定
		wheel.transform.position = pos;
		wheel.transform.rotation = q * Quaternion.Euler (0f, 0f, 90f);
	}

	void FixedUpdate ()
	{
	    // 水平方向キーの入力に応じてステアリング角度を変化
		float steering = MaxSteeringAngle * Input.GetAxis ("Horizontal");

		// 垂直方向キーの入力に応じてモータートルクを変化
		float motor = MaxMotorTorque * Input.GetAxis ("Vertical");

        // ステアリング角とモータトルクの値を更新
		foreach (var axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			ApplyLocalPosiitonToVisuals (axleInfo.leftWheel);
			ApplyLocalPosiitonToVisuals (axleInfo.rightWheel);
		}
	}
}
