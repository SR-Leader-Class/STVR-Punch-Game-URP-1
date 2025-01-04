using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.XR;

namespace Arnold.VTracker {
	public class VTrackerFollower : MonoBehaviour {

		// 紀錄追蹤器是否已被追蹤
		public bool IsTracked = false;
		// 用來顯示/隱藏物件的 MeshRenderer 組件
		[SerializeField] private MeshRenderer meshRenderer;
		// 參考物件，用於校準偏移量
		[SerializeField] private GameObject offsetReference;

		// 虛擬現實追蹤器的輸入裝置
		//private InputDevice tracker;
		// 用於追蹤器的唯一標識符
		public string Index; 

		// 預設位置的向量
		private Vector3 presetPos;

		// Awake 方法在物件生成時執行一次
		private void Awake() {
			// 設置追蹤狀態為 false，表明尚未被追蹤
			IsTracked = false;
			// 設置物件的初始位置
			presetPos = this.transform.position;
		}

		// 每一幀都會執行 Update 方法
		private void Update() {
			// 如果物件的位置為原點或初始位置，則表示尚未被追蹤
			if (this.transform.position == Vector3.zero || this.transform.position == presetPos) {
				IsTracked = false;
			} else {
				// 否則標記為已追蹤
				IsTracked = true;
			}
			// 此部分被註解，未啟用的追蹤器位置及旋轉更新程式碼
			// tracker.TryGetFeatureValue(CommonUsages.devicePosition, out var pos);
			// tracker.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot);
			// this.transform.position = pos;
			// this.transform.rotation = rot;
		}

		// 設定 MeshRenderer 是否顯示
		public void SetVisible (bool flag) {
			meshRenderer.enabled = flag;
		}

		// 校準追蹤器偏移量的方法
		[ContextMenu("CalibrateOffset")]
		private void CalibrateOffset() {
			// 檢查偏移參考物件是否存在
			if (offsetReference == null) {
				Debug.Log("OffsetReference is null");
				return;
			}
			// 計算追蹤器和參考物件之間的偏移量
			Vector3 offset = this.transform.position - offsetReference.transform.position;
			// 使用 Index 生成唯一的鍵值
			string saveKey = string.Format("Tracker{0}_Offset", Index);
			// 將偏移量序列化為 JSON 字符串
			string saveString = JsonUtility.ToJson(offset);
			Debug.Log("Offset for " + Index + " is saved.");
			// 將偏移量保存到 PlayerPrefs 中
			PlayerPrefs.SetString(saveKey, saveString);
		}
	}
}
