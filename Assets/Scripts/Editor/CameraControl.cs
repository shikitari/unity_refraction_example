using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class CameraControl : EditorWindow {
    private GameObject dummyCamera;
    private main.Cup3 _cup3;
    public main.Cup3 cup3
    {
        get {
            if (_cup3 == null)
            {
                _cup3 = main.ScriptManager.Get<main.Cup3>();
            }
            return _cup3;
        }
    }
    private Material _cup3TopMaterial;
    public Material cup3TopMaterial
    {
        get{
            if(_cup3TopMaterial ==null) {
                GameObject g = GameObject.Find("CupTop").gameObject;
                _cup3TopMaterial = g?.GetComponent<MeshRenderer>().sharedMaterial;
            }
            return _cup3TopMaterial;
        }
    }

    bool autoLookAt = true;

    [MenuItem("Window/Camera/CameraControl")]
	static void Init()
	{
        var window = EditorWindow.FindObjectOfType<CameraControl>();
		if( window != null )
			window.Close();

        window =  EditorWindow.CreateInstance<CameraControl>();
		window.Show();
	}

	public Camera sceneCamera
	{
		get{ return SceneView.lastActiveSceneView.camera; }
	}

	void OnGUI()
	{
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("scene debug on"))
        {
            cup3.sceneViewDebug = true;
            SetCamera();
        }
        else if (GUILayout.Button("off")) {
            cup3.sceneViewDebug = false;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("scene debug on"))
        {
            
        }

        if (GUILayout.Button("capture image"))
        {
            CaptureScreenshot();
        }

        if (GUILayout.Button("set c2"))
        {
            
            var c1 = Camera.main.gameObject;
            var c2 = GameObject.Find("Camera2");
            c2.transform.position = c1.transform.position;

            Undo.RecordObject(c2.transform, "camera");
            c2.transform.rotation = c1.transform.rotation;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Text look at camera"))
        {
            textLookAtCamera();
        }
        GUILayout.EndHorizontal();

        autoLookAt = EditorGUILayout.Toggle("Auto look at", autoLookAt);

	}

    void textLookAtCamera()
    {
        if (SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.camera != null)
        {
            Camera c = SceneView.lastActiveSceneView.camera;
            Vector3 direction = c.transform.position;
            GameObject[] gs = GameObject.FindGameObjectsWithTag("TextMesh");
            Quaternion q1 = Quaternion.LookRotation(-direction);
            foreach (GameObject g in gs)
            {
                TextMesh t = g.GetComponent<TextMesh>();
                //g.transform.rotation = q1;
                g.transform.LookAt(c.transform.position);
                //float distance = Vector3.Distance (c.transform.position, g.transform.position);
                //t.characterSize = 0.2f;//change size depend on distance
            }
        }
    }

    void SetCamera()
    {
        if (dummyCamera == null)
        {
            dummyCamera = new GameObject();
            dummyCamera.name = "scene debug camera";
            dummyCamera.AddComponent<Camera>();
        }

        Camera c = dummyCamera.GetComponent<Camera>();
        dummyCamera.transform.position = sceneCamera.transform.position;
        dummyCamera.transform.rotation = sceneCamera.transform.rotation;
        c.fieldOfView = sceneCamera.fieldOfView;
        c.cullingMask = (1 << 8) + (1 << 9);
        float w = (float)sceneCamera.pixelWidth / (float)sceneCamera.pixelHeight;
        RenderTexture rt = new RenderTexture((int)(w * 512), 512, 0, RenderTextureFormat.ARGB32);
        c.targetTexture = rt;
        c.projectionMatrix = sceneCamera.projectionMatrix;
        c.clearFlags = CameraClearFlags.Color;
        c.backgroundColor = Color.black;

        cup3TopMaterial.SetMatrix("_RTCameraProjection", c.projectionMatrix);
        cup3TopMaterial.SetMatrix("_RTCameraView", c.worldToCameraMatrix);
        cup3TopMaterial.SetTexture("_MainTex", rt);

        //EditorGUIUtility.systemCopyBuffer = "ok";
    }

    /// <summary>
    /// キャプチャを撮る
    /// </summary>
    /// <remarks>
    /// Edit > CaptureScreenshot に追加。
    /// HotKeyは Ctrl + Shift + F12。
    /// </remarks>
    [MenuItem("Edit/CaptureScreenshot #%F12")]
    private static void CaptureScreenshot()
    {
        // 現在時刻からファイル名を決定
        var filename = System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";

        // キャプチャを撮る
        ScreenCapture.CaptureScreenshot("./Recordings_img/" + filename);

        // GameViewを取得してくる
        var assembly = typeof(UnityEditor.EditorWindow).Assembly;
        var type = assembly.GetType("UnityEditor.GameView");
        var gameview = EditorWindow.GetWindow(type);
        // GameViewを再描画
        gameview.Repaint();

        Debug.Log("ScreenShot: " + filename);
    }

	void Update() {
        
        if (cup3?.sceneViewDebug == true) {
            SetCamera();
        }

        if (autoLookAt)
        {
            textLookAtCamera();
        }
	}
}