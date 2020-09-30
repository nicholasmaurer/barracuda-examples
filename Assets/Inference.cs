using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class Inference : MonoBehaviour
{

	public NNModel modelAsset;
	public Texture2D texture;
	private Model m_RuntimeModel;
	private IWorker m_Worker;
	public RenderTexture rt;
	
	void Start()
	{   
		m_RuntimeModel = ModelLoader.Load(modelAsset);
		m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel, true);
		rt = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.RGB565);
	}

	void Process()
	{
		var channelCount = 3;
		var tensor = new Tensor(texture, channelCount);
		m_Worker.Execute(tensor);
		tensor = m_Worker.PeekOutput();
		BarracudaTextureUtils.TensorToRenderTexture(tensor, rt);
		tensor.Dispose();
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Process"))
		{
			Process();
		}
	}
}
