using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DataSaveLoad;
using Shiva.CameraSwitch;
using System;
using System.IO;

namespace CameraSaveLoad {
	public class CameraSaveLoadManager : MonoBehaviour {

		public string folderName = "CameraSaveLoad";
		public string screenShotFolder = "Screenshots";

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

		// Update is called once per frame
		void Update () {
			
		}

		private IEnumerator _TakeScreenshot(){
			Canvas[] canvses = FindObjectsOfType<Canvas> ();
			foreach (Canvas c in canvses) {
				c.enabled = false;
			}
			yield return new WaitForEndOfFrame ();

			string fname = "screenshot_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".png";
			string path = dataSaveLoad.GetFolderPath ( folderName + "/" + screenShotFolder);
			if (!Directory.Exists (path)) {
				Directory.CreateDirectory (path);
			}
			print (path + "/" + fname);


			float maxScale = 4096;

			Camera camera = cameraSwitcher.CurrentActive.c;

			var max = Mathf.Max(Screen.width, Screen.height);
			int width = (int)(maxScale * Screen.width / max );
			int height = (int)(maxScale * Screen.height / max);

			var rt = RenderTexture.GetTemporary(width, height);
			var tex = new Texture2D(width, height, TextureFormat.ARGB32, false, false);

			camera.targetTexture = rt;
			camera.Render();

			RenderTexture.active = rt;
			tex.ReadPixels( new Rect(0, 0, width, height), 0, 0);
			tex.Apply();

			camera.targetTexture = null;
			RenderTexture.active = null;

			System.IO.File.WriteAllBytes(path+"/"+fname, tex.EncodeToPNG());


//			float max = Mathf.Max(Screen.width, Screen.height);
//			int scale = Mathf.RoundToInt( 4096 / max );
//			scale = 1;
//			print("Captuer scale : " + scale);
//			Application.CaptureScreenshot(path+"/"+fname, scale);

			Helper.OpenInFileBrowser (path);

			foreach (Canvas c in canvses) {
				c.enabled = true;
			}
		}


		public void TakeScreenshot(){

			StartCoroutine (_TakeScreenshot ());
		}

		public void ShowSaveUI(){
			SavedCamera sc = new SavedCamera ();
			sc.cameraName = cameraSwitcher.CurrentActive.c.name;
			sc.position = cameraSwitcher.CurrentActive.transform.position;
			sc.rotation = cameraSwitcher.CurrentActive.transform.rotation;
			sc.localScale = cameraSwitcher.CurrentActive.transform.localScale;

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
			
			SavedCamera sc = o as SavedCamera;

			if (sc.cameraName == cameraSwitcher.CurrentActive.c.name) {
				cameraSwitcher.CurrentActive.transform.position = sc.position;
				cameraSwitcher.CurrentActive.transform.rotation = sc.rotation;
				cameraSwitcher.CurrentActive.transform.localScale = sc.localScale;
			}
		}

	}
}