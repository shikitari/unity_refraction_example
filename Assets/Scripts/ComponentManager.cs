using UnityEngine;
using System.Collections;

public class ComponentManager
{
    private MonoBehaviour owner;
    public ComponentManager(MonoBehaviour owner)
    {
        this.owner = owner;
    }

	private MeshFilter meshFilter_;
	public MeshFilter meshFilter
	{
		get
		{
			if (meshFilter_ == null)
			{
				meshFilter_ = owner.GetComponent<MeshFilter>();
			}
			return meshFilter_;
		}
	}

	private MeshRenderer meshRenderer_;
	public MeshRenderer meshRenderer
	{
		get
		{
			if (meshRenderer_ == null)
			{
				meshRenderer_ = owner.GetComponent<MeshRenderer>();
			}
			return meshRenderer_;
		}
	}

	private MeshCollider meshCollider_;
	public MeshCollider meshCollider
	{
		get
		{
			if (meshCollider_ == null)
			{
				meshCollider_ = owner.GetComponent<MeshCollider>();
			}
			return meshCollider_;
		}
	}

    private Camera camera_;
    public Camera camera
    {
        get
        {
            if (camera_ == null)
            {
                camera_ = owner.GetComponent<Camera>();
            }
            return camera_;
        }
    }
}
