using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 定義命名空間 Arnold.VTracker
namespace Arnold.VTracker {
	// VTrackerManager 類，繼承自 MonoBehaviour
	public class VTrackerManager : MonoBehaviour {
		// 隱藏在檢視器中，定義一個靜態的 VTrackerManager 實例
		[HideInInspector] public static VTrackerManager instance;
		
		// 定義 UI 元素：面板的 Canvas、錄製開關、時間文本和計數文本
		[SerializeField] private Canvas panelCanvas;
		[SerializeField] private Toggle recordingToggle;
		[SerializeField] private Text timeText;
		[SerializeField] private Text tickText;

		// 定義一個列表來存儲多個 VTrackerStatusBar
		[SerializeField] private List<VTrackerStatusBar> vTrackerStatusBars = new List<VTrackerStatusBar>();

		// 定義 VTrackerRecorder 物件，負責錄製數據
		[SerializeField] private VTrackerRecorder vTrackerRecorder;

		// 定義多個 VTrackerFollower 和跟隨者偏移物件的列表
		[SerializeField] private List<VTrackerFollower> vTrackerFollowers = new List<VTrackerFollower>();
		[SerializeField] private List<GameObject> vTrackerOffsetFollowers = new List<GameObject>();

		// 私有變數來記錄時間，公開的 getter 和 setter 用來更新時間顯示
		private float recordedTime = 0f;
		public float RecordedTime {
			get { return recordedTime; }
			set {
				// 更新錄製時間並在 UI 中顯示格式化的時間
				recordedTime = value;
				string minute = Mathf.Floor(recordedTime / 60f).ToString("00");
				string second = Mathf.Floor(recordedTime % 60f).ToString("00");
				timeText.text = string.Format("{0}:{1}", minute, second);
			}
		}

		// 私有變數記錄錄製的幀數和計數，公開的 getter 和 setter 用來更新 UI 顯示的計數
		private int recordingFrame = 0;
		private int recordedTick = 0;
		public int RecordedTick {
			get { return recordedTick; }
			set {
				// 更新錄製的計數並顯示在 UI 上
				recordedTick = value;
				tickText.text = recordedTick.ToString("0000");
			}
		}

		// 私有變數來控制是否正在錄製，公開的 getter 和 setter 用來控制錄製狀態
		private bool isRecording = false;
		public bool IsRecording {
			get {
				return isRecording;
			}
			set {
				// 當設置錄製狀態時，切換 UI 和狀態
				isRecording = value;
				recordingToggle.isOn = value;
				if (isRecording == true) {
					// 如果開始錄製，則重置並初始化錄製
					Reset();
					List<string> vTrackerIndexs = new List<string>();
					for (int i = 0; i < vTrackerFollowers.Count; i++) {
						vTrackerIndexs.Add(vTrackerFollowers[i].Index);
					}
					vTrackerRecorder.InitiateRecord(vTrackerIndexs);
				} else {
					// 停止錄製並保存數據
					Save();
				}
			}
		}

		// 控制追蹤器是否可見的私有變數，公開的 getter 和 setter 用來切換可見性
		private bool isTrackerVisible = true;
		public bool IsTrackerVisible {
			get { return isTrackerVisible; }
			set {
				// 設置每個追蹤物件的可見性
				isTrackerVisible = value;
				for (int i = 0; i < vTrackerFollowers.Count; i++) {
					vTrackerFollowers[i].SetVisible(isTrackerVisible);
				}
			}
		}

		// Awake 方法，確保 VTrackerManager 單例模式的實現
		private void Awake() {
			if (instance == null) {
				instance = this;
				DontDestroyOnLoad(this); // 保持對象在場景加載時不被銷毀
			} else {
				GameObject.Destroy(this); // 如果已有實例，銷毀當前對象
				return;
			}
			// 初始化錄製狀態為 false，並將每個狀態欄背景顏色與偏移物件的顏色同步
			IsRecording = false;
			for (int i = 0; i < vTrackerStatusBars.Count; i++) {
				vTrackerStatusBars[i].TrackerBackground.color = vTrackerOffsetFollowers[i].GetComponent<MeshRenderer>().materials[0].color;
			}
		}

		// Update 方法，每幀檢查按鍵輸入，控制面板的顯示/隱藏
		private void Update() {
			if (UnityEngine.Input.GetKeyDown(KeyCode.V)) {
				panelCanvas.gameObject.SetActive(!panelCanvas.gameObject.activeSelf);
			}
		}

		// FixedUpdate 方法，每固定幀更新追蹤信息並進行錄製
		private void FixedUpdate() {
			// 調試用途：手動保存錄製數據
			//if (Input.GetKeyDown(KeyCode.Space)) {
			//	vTrackerRecorder.Save();
			//}
			UpdateTrackerInfo(); // 更新追蹤器的狀態信息
			RecordTrackers();    // 錄製追蹤數據
		}

		// 重置錄製數據
		private void Reset() {
			vTrackerRecorder.Reset(); // 重置錄製器
			RecordedTick = 0;
			recordingFrame = 0;
			RecordedTime = 0;
		}

		// 更新每個追蹤器的狀態信息並顯示在 UI 中
		private void UpdateTrackerInfo() {
			for (int i = 0; i < vTrackerFollowers.Count; i++) {
				// 格式化顯示位置、旋轉和歐拉角信息，並顯示在對應的狀態欄
				string contentText = string.Format("Pos: {0}<br>Rot: {1}<br>Eul: {2}", vTrackerFollowers[i].transform.position, vTrackerFollowers[i].transform.rotation, vTrackerFollowers[i].transform.eulerAngles);
				string untrackedText = "Untracked"; // 如果未追蹤，顯示 "Untracked"
				if (vTrackerFollowers[i].IsTracked == true) {
					vTrackerStatusBars[i].ContentText.text = contentText;
				} else {
					vTrackerStatusBars[i].ContentText.text = untrackedText;
				}
				vTrackerStatusBars[i].IndexText.text = vTrackerFollowers[i].Index; // 顯示追蹤器索引
			}
		}

		// 錄製追蹤器數據
		public void RecordTrackers() {
			if (IsRecording == false) return; // 如果未錄製，直接返回
			recordingFrame++; // 錄製幀數增加
			RecordedTime += Time.fixedDeltaTime; // 更新錄製時間
			if (recordingFrame % 2 == 0) return; // 每隔幀進行一次錄製
			RecordedTick++; // 錄製的計數增加

			for (int i = 0; i < vTrackerFollowers.Count; i++) {
				// 錄製每個追蹤器的數據，使用偏移物件進行錄製
				vTrackerRecorder.RecordTracker(vTrackerFollowers[i].Index, vTrackerOffsetFollowers[i].gameObject);
				// 在 UI 中顯示錄製的索引和計數
				vTrackerStatusBars[i].IndexResultText.text = string.Format("T:{0} @ {1}", RecordedTick, vTrackerFollowers[i].Index);
			}
		}

		// 錄製 Avatar 的 Punch 動作數據
		public void RecordAvatarPunchData(int index) {
			vTrackerRecorder.RecordAvatarPunchData(index);
		}

		// 保存錄製的數據
		public void Save() {
			vTrackerRecorder.Save();
		}
	}
}
