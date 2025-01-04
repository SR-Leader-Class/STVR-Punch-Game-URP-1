using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arnold.VTracker {
	// VTrackerRecorder 是一個負責記錄追蹤器資料的類別
	public class VTrackerRecorder : MonoBehaviour {

		// 儲存每個追蹤器資料的清單
		public List<VTrackerData> VTrackerDatas = new List<VTrackerData>();
		// 儲存Avatar動作（例如出拳）的數據清單
		public List<int> AvatarPunchData = new List<int>();

		// 清空所有已儲存的追蹤器與Avatar出拳資料
		public void Reset() {
			VTrackerDatas.Clear();
			AvatarPunchData.Clear();
		}

		// 初始化追蹤器資料
		// trackerIndexs 是包含所有追蹤器索引的字串清單
		public void InitiateRecord(List<string> trackerIndexs) {
			VTrackerDatas.Clear(); // 清空先前的追蹤器資料
			for (int i = 0; i < trackerIndexs.Count; i++) {
				VTrackerData data = new VTrackerData(); // 新增一個追蹤器資料物件
				data.TrackerIndex = trackerIndexs[i];   // 設定追蹤器索引
				VTrackerDatas.Add(data);                // 加入清單中
			}
		}

		// 記錄特定追蹤器的位移與旋轉資訊
		// trackerIndex 是追蹤器的識別碼
		// tracker 是追蹤器的遊戲物件
		public void RecordTracker (string trackerIndex, GameObject tracker) {
			// 根據trackerIndex尋找對應的追蹤器資料
			VTrackerData data = VTrackerDatas.FirstOrDefault(x => x.TrackerIndex == trackerIndex);
			if (data == null) { // 若找不到該索引，顯示錯誤訊息
				Debug.Log("VTracker " + trackerIndex + ": index not found in VTrackerDatas.");
				return;
			}
			// 獲取追蹤器的位置信息
			Vector3 trackerPos = tracker.transform.position;

			// 註釋掉的程式碼部分：載入位移的偏移值
			// string saveKey = string.Format("Tracker{0}_Offset", trackerIndex);
			// string offsetSave = PlayerPrefs.GetString(saveKey, string.Empty);
			// if (string.IsNullOrEmpty(offsetSave) == false) {
			// 	Vector3 offset = JsonUtility.FromJson<Vector3>(offsetSave);
			// 	trackerPos -= offset;
			// }

			// 將位置、旋轉四元數和歐拉角加入到data對應的清單中
			data.TrackerPositions.Add(trackerPos);
			data.TrackerQuaternions.Add(tracker.transform.rotation);
			data.TrackerEulerAngles.Add(tracker.transform.eulerAngles);
		}

		// 記錄Avatar的出拳資料
		public void RecordAvatarPunchData(int index) {
			AvatarPunchData.Add(index);
		}

		// 保存所有的追蹤資料與Avatar出拳資料
		public void Save() {
			// 建立一個新的VTrackerMatchRecord物件
			VTrackerMatchRecord matchRecord = new VTrackerMatchRecord();
			// 設定時間戳記，用於命名
			matchRecord.TimeStamp = string.Format("{0}{1}{2}_{3}{4}{5}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"), DateTime.Now.Second.ToString("00"));
			
			Debug.Log("Pre: " + VTrackerDatas.Count); // 儲存前追蹤資料的數量
			matchRecord.VTrackerDatas = new List<VTrackerData>(VTrackerDatas); // 複製追蹤資料
			Debug.Log("Post: " + matchRecord.VTrackerDatas.Count); // 儲存後追蹤資料的數量
			matchRecord.AvatarPunchData = new List<int>(AvatarPunchData); // 複製Avatar出拳資料

			// 將資料序列化為 JSON 格式
			string savingString = JsonUtility.ToJson(matchRecord);
			// 設定檔案名稱
			string fileName = string.Format("VTrackerData_{0}.json", matchRecord.TimeStamp);
			// 檢查資料夾是否存在，不存在則創建
			if (System.IO.Directory.Exists(Application.dataPath + "/StreamingAssets/VTrackerData/") == false) {
				System.IO.Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/VTrackerData/");
			}
			// 將資料寫入到指定路徑的 JSON 檔案中
			System.IO.File.WriteAllText(Application.dataPath + "/StreamingAssets/VTrackerData/" + fileName, savingString);
			Debug.Log("VTrakerMatchRecord Saved to " + Application.dataPath + "/StreamingAssets/VTrackerData/" + fileName); // 保存成功的訊息
		}

	}

	// VTrackerMatchRecord 類別用於保存一場比賽的追蹤記錄
	[Serializable]
	public class VTrackerMatchRecord {
		public string TimeStamp;                       // 時間戳記
		public List<VTrackerData> VTrackerDatas = new List<VTrackerData>(); // 儲存追蹤器資料
		public List<int> AvatarPunchData = new List<int>(); // 儲存Avatar出拳資料
	}

		// VTrackerData 類別用於保存單一追蹤器的數據
	[Serializable]
	public class VTrackerData {
		public string TrackerIndex;                           // 追蹤器的識別碼
		public List<Vector3> TrackerPositions = new List<Vector3>(); // 儲存追蹤器的位置信息
		public List<Quaternion> TrackerQuaternions = new List<Quaternion>(); // 儲存追蹤器的旋轉四元數
		public List<Vector3> TrackerEulerAngles = new List<Vector3>();       // 儲存追蹤器的歐拉角
	}
}
