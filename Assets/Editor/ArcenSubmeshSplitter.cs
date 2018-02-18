using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ArcenSubmeshSplitter
{
	//http://answers.unity3d.com/questions/1213025/separating-submeshes-into-unique-meshes.html
	[MenuItem( "Arcen/Submesh Splitter" )]
	public static void BuildWindowsAssetBundle()
	{
		GameObject[] objects = Selection.gameObjects;
		for ( int i = 0; i < objects.Length; i++ )
		{
			ProcessGameObject( objects[i] );
		}
		Debug.Log( "Done splitting meshes into submeshes!  " + System.DateTime.Now );
	}

	public class MeshFromSubmesh
	{
		public Mesh mesh;
		public int id; // Represent the ID of the sub mesh from with the new 'mesh' has been created
	}

	private static void ProcessGameObject( GameObject go )
	{
		// Isolate Sub Meshes
		MeshFilter meshFilterComponent = go.GetComponent<MeshFilter>();
		if ( !meshFilterComponent )
		{
			Debug.LogError( "MeshFilter null for '" + go.name + "'!" );
			return;
		}
		MeshRenderer meshRendererComponent = go.GetComponent<MeshRenderer>();
		if ( !meshRendererComponent )
		{
			Debug.LogError( "MeshRenderer null for '" + go.name + "'!" );
			return;
		}
		Mesh mesh = go.GetComponent<MeshFilter>().sharedMesh;
		if ( !mesh )
		{
			Debug.LogError( "Mesh null for '" + go.name + "'!" );
			return;
		}
		List<MeshFromSubmesh> meshFromSubmeshes = GetAllSubMeshAsIsolatedMeshes( mesh );
		if ( meshFromSubmeshes == null || meshFromSubmeshes.Count == 0 )
		{
			Debug.LogError( "List<MeshFromSubmesh> empty or null for '" + go.name + "'!" );
			return;
		}
		string goName = go.name;
		for ( int i = 0; i < meshFromSubmeshes.Count; i++ )
		{
			string meshFromSubmeshName = goName + "_sub_" + i;
			GameObject meshFromSubmeshGameObject = new GameObject();
			meshFromSubmeshGameObject.name = meshFromSubmeshName;
			meshFromSubmeshGameObject.transform.SetParent( meshFilterComponent.transform );
			meshFromSubmeshGameObject.transform.localPosition = Vector3.zero;
			meshFromSubmeshGameObject.transform.localRotation = Quaternion.identity;
			MeshFilter meshFromSubmeshFilter = meshFromSubmeshGameObject.AddComponent<MeshFilter>();
			meshFromSubmeshFilter.sharedMesh = meshFromSubmeshes[i].mesh;
			MeshRenderer meshFromSubmeshMeshRendererComponent = meshFromSubmeshGameObject.AddComponent<MeshRenderer>();
			if ( meshRendererComponent != null )
			{
				// To use the same mesh renderer properties of the initial mesh
				EditorUtility.CopySerialized( meshRendererComponent, meshFromSubmeshMeshRendererComponent );
				// We just need the only one material used by the sub mesh in its renderer
				Material material = meshFromSubmeshMeshRendererComponent.sharedMaterials[meshFromSubmeshes[i].id];
				meshFromSubmeshMeshRendererComponent.sharedMaterials = new[] { material };
			}
			// Don't forget to save the newly created mesh in the asset database (on disk)
			string path = "Assets/SplitMeshes/" + meshFromSubmeshName + ".asset";
			AssetDatabase.CreateAsset( meshFromSubmeshes[i].mesh, path );
			Debug.Log( "Created: " + path );
		}
	}

	private static List<MeshFromSubmesh> GetAllSubMeshAsIsolatedMeshes( Mesh mesh )
	{
		List<MeshFromSubmesh> meshesToReturn = new List<MeshFromSubmesh>();
		if ( !mesh )
		{
			Debug.LogError( "No mesh passed into GetAllSubMeshAsIsolatedMeshes!" );
			return meshesToReturn;
		}
		int submeshCount = mesh.subMeshCount;
		if ( submeshCount < 2 )
		{
			Debug.LogError( "Only " + submeshCount + " submeshes in mesh passed to GetAllSubMeshAsIsolatedMeshes" );
			return meshesToReturn;
		}
		MeshFromSubmesh m1;
		for ( int i = 0; i < submeshCount; i++ )
		{
			m1 = new MeshFromSubmesh();
			m1.id = i;
			m1.mesh = mesh.GetSubmesh( i );
			meshesToReturn.Add( m1 );
		}
		return meshesToReturn;
	}
}

public static class MeshExtension
{
	private class Vertices
	{
		List<Vector3> verts = null;
		List<Vector2> uv1 = null;
		List<Vector2> uv2 = null;
		List<Vector2> uv3 = null;
		List<Vector2> uv4 = null;
		List<Vector3> normals = null;
		List<Vector4> tangents = null;
		List<Color32> colors = null;
		List<BoneWeight> boneWeights = null;

		public Vertices()
		{
			verts = new List<Vector3>();
		}
		public Vertices( Mesh aMesh )
		{
			verts = CreateList( aMesh.vertices );
			uv1 = CreateList( aMesh.uv );
			uv2 = CreateList( aMesh.uv2 );
			uv3 = CreateList( aMesh.uv3 );
			uv4 = CreateList( aMesh.uv4 );
			normals = CreateList( aMesh.normals );
			tangents = CreateList( aMesh.tangents );
			colors = CreateList( aMesh.colors32 );
			boneWeights = CreateList( aMesh.boneWeights );
		}

		private List<T> CreateList<T>( T[] aSource )
		{
			if ( aSource == null || aSource.Length == 0 )
				return null;
			return new List<T>( aSource );
		}
		private void Copy<T>( ref List<T> aDest, List<T> aSource, int aIndex )
		{
			if ( aSource == null )
				return;
			if ( aDest == null )
				aDest = new List<T>();
			aDest.Add( aSource[aIndex] );
		}
		public int Add( Vertices aOther, int aIndex )
		{
			int i = verts.Count;
			Copy( ref verts, aOther.verts, aIndex );
			Copy( ref uv1, aOther.uv1, aIndex );
			Copy( ref uv2, aOther.uv2, aIndex );
			Copy( ref uv3, aOther.uv3, aIndex );
			Copy( ref uv4, aOther.uv4, aIndex );
			Copy( ref normals, aOther.normals, aIndex );
			Copy( ref tangents, aOther.tangents, aIndex );
			Copy( ref colors, aOther.colors, aIndex );
			Copy( ref boneWeights, aOther.boneWeights, aIndex );
			return i;
		}
		public void AssignTo( Mesh aTarget )
		{
			aTarget.SetVertices( verts );
			if ( uv1 != null ) aTarget.SetUVs( 0, uv1 );
			if ( uv2 != null ) aTarget.SetUVs( 1, uv2 );
			if ( uv3 != null ) aTarget.SetUVs( 2, uv3 );
			if ( uv4 != null ) aTarget.SetUVs( 3, uv4 );
			if ( normals != null ) aTarget.SetNormals( normals );
			if ( tangents != null ) aTarget.SetTangents( tangents );
			if ( colors != null ) aTarget.SetColors( colors );
			if ( boneWeights != null ) aTarget.boneWeights = boneWeights.ToArray();
		}
	}

	public static Mesh GetSubmesh( this Mesh aMesh, int aSubMeshIndex )
	{
		if ( aSubMeshIndex < 0 || aSubMeshIndex >= aMesh.subMeshCount )
			return null;
		int[] indices = aMesh.GetTriangles( aSubMeshIndex );
		Vertices source = new Vertices( aMesh );
		Vertices dest = new Vertices();
		Dictionary<int, int> map = new Dictionary<int, int>();
		int[] newIndices = new int[indices.Length];
		for ( int i = 0; i < indices.Length; i++ )
		{
			int o = indices[i];
			int n;
			if ( !map.TryGetValue( o, out n ) )
			{
				n = dest.Add( source, o );
				map.Add( o, n );
			}
			newIndices[i] = n;
		}
		Mesh m = new Mesh();
		dest.AssignTo( m );
		m.triangles = newIndices;
		return m;
	}
}
