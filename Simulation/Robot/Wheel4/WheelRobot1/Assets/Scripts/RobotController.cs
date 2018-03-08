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
	public float MaxMotorTorque = 400;
	public float MaxSteeringAngle = 30;

	public void ApplyLocalPosiitonToVisuals (WheelCollider collider)
	{
		// Wheelコライダーの子要素がない場合は処理終了。
		if (collider.transform.childCount == 0)
			return;

		// タイヤビジュアルを取得。
		Transform visual = collider.transform.GetChild (0);

		// コライダーの位置と回転を取得。
		Vector3 pos;
		Quaternion q;
		collider.GetWorldPose (out pos, out q);

		// タイヤビジュアルに、コライダーの値を設定。
		// 単純な車の場合、Z軸を90f回転させる必要がある
		visual.transform.position = pos;
		visual.transform.rotation = q * Quaternion.Euler (0f, 0f, 90f);
	}

	void FixedUpdate ()
	{
		float steering = MaxSteeringAngle * Input.GetAxis ("Horizontal");
		float motor = MaxMotorTorque * Input.GetAxis ("Vertical");

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
