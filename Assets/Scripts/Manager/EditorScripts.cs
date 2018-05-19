using UnityEngine;

[ExecuteInEditMode]
public class EditorScripts : MonoBehaviour
{
	public Texture emptyGradient;
	public Vector3 worldOrigin;

	void Awake()
	{
		if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
			Debug.Log("Clearing Fog Shaders");
			Shader.SetGlobalTexture("_MK_FOG_DESATURATE", emptyGradient);
			Shader.SetGlobalTexture("_MK_FOG_STYLISTIC", emptyGradient);	
			Shader.SetGlobalTexture("_MK_FOG_SPIRIT_MODE_GRADIENT", emptyGradient);
			Shader.SetGlobalVector("_MK_FOG_SPIRIT_MODE_ORIGIN", worldOrigin);Shader.SetGlobalInt("_MK_FOG_SPIRIT_MODE_ENABLED", 0);
		}
	}
}
