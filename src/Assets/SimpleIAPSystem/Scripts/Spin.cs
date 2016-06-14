using UnityEngine;

namespace SIS
{
	public class Spin : MonoBehaviour
	{
		public float rotationsPerSecond = 0.1f;

		void Update ()
		{
			Vector3 euler = transform.localEulerAngles;
			euler.z -= rotationsPerSecond * 360f * Time.deltaTime;
			transform.localEulerAngles = euler;
		}
	}
}
