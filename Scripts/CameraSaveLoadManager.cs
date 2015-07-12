 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DataSaveLoad;
using Shiva.CameraSwitch;

namespace CameraSaveLoad {
	public class CameraSaveLoadManager : MonoBehaviour {

		public string folderName = "CameraSaveLoad";

		public CameraSwitcher cameraSwitcher;

		public DataSaveLoadMaster dataSaveLoad;

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
			sc.cameraName = cameraSwitcher.CurrentActive.c.name;
			sc.position = cameraSwitcher.CurrentActive.c.transform.position;
			sc.rotation = cameraSwitcher.CurrentActive.c.transform.rotation;
			sc.localScale = cameraSwitcher.CurrentActive.c.transform.localScale;

			dataSaveLoad.ShowSaveDialog (sc, folderName);
		}

		public void ShowLoadUI(){
			dataSaveLoad.ShowLoadDialog (typeof(SavedCamera), folderName);
		}

		// Use this for initialization
		void Awake () {
//			dataSaveLoad = GetComponent<DataSaveLoadMaster> ();
		}

		void Start () {
			dataSaveLoad.AddHandler(DataLoadCallback, typeof(SavedCamera));
		}
		
		public void DataLoadCallback(object o){

			print (o);

			SavedCamera sc = o as SavedCamera;
			cameraSwitcher.CurrentActive.c.transform.position = sc.position;
			cameraSwitcher.CurrentActive.c.transform.rotation = sc.rotation;
			cameraSwitcher.CurrentActive.c.transform.localScale = sc.localScale;
		}
		
		// Update is called once per frame
		void Update () {
		
		}

	}
}