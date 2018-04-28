using UnityEngine;

[ExecuteInEditMode]
public class EditorScripts : MonoBehaviour
{
	public Texture stylisticFog, desaturationGradient;

	void Awake()
	{
		if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
			Debug.Log("Clearing Fog Shaders");
			Shader.SetGlobalTexture("_MK_FOG_DESATURATE", desaturationGradient);
			Shader.SetGlobalTexture("_MK_FOG_STYLISTIC", stylisticFog);	
		}
	}
}
