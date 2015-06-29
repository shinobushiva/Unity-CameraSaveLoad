 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DataSaveLoad;

namespace CameraSaveLoad {
	public class CameraSaveLoadManager : MonoBehaviour {

		public Camera target;

		private DataSaveLoadMaster dataSaveLoad;

		[System.Serializable] 
		public class SavedCamera
		{
			public string name;
			public string cameraName;
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 localScale;
			public string createdDate;
		}

		public void ShowSaveUI(){
			SavedCamera sc = new SavedCamera ();
			sc.cameraName = target.name;
			sc.position = target.transform.position;
			sc.rotation = target.transform.rotation;
			sc.localScale = target.transform.localScale;

			dataSaveLoad.ShowSaveDialog (sc);
		}

		public void ShowLoadUI(){
			dataSaveLoad.ShowLoadDialog (typeof(SavedCamera));
		}

		// Use this for initialization
		void Awake () {
			dataSaveLoad = GetComponent<DataSaveLoadMaster> ();
		}

		void Start () {
			dataSaveLoad.dataLoadHandler += DataLoadCallback;
		}
		
		public void DataLoadCallback(object o){
			SavedCamera sc = o as SavedCamera;
			target.transform.position = sc.position;
			target.transform.rotation = sc.rotation;
			target.transform.localScale = sc.localScale;
		}
		
		// Update is called once per frame
		void Update () {
		
		}

	}
}