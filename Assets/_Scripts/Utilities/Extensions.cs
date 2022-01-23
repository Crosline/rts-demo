using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Utilities {
	public static class Extensions {
		public static void Fade(this SpriteRenderer renderer, float alpha) {
			var color = renderer.color;
			color.a = alpha;
			renderer.color = color;
		}
		public static void Fade(this Image image, float alpha) {
			var color = image.color;
			color.a = alpha;
			image.color = color;
		}

		public static void ChangeColor(this SpriteRenderer renderer, Color color) {
			renderer.color = color;
		}

		public static bool Equal(this float a, float b) {
			return Mathf.Approximately(a, b);
		}

		public static T Rand<T>(this IList<T> list) {
			return list[Random.Range(0, list.Count)];
		}

		public static T Last<T>(this IList<T> list, int index = 0) {
			return list[list.Count - 1 - index];
		}

		public static void SetLayersRecursively(this GameObject g, int layer) {
			g.layer = layer;
			foreach (Transform t in g.transform) {
				t.gameObject.SetLayersRecursively(layer);
			}
		}
		
		public static void DestroyRecursively(this Transform t, float time = 0f) {
			foreach (Transform child in t) { 
				DestroyRecursively(child, time);
			}
			Object.Destroy(t.gameObject, time);
		}
		
		public static void DestroyChildren(this Transform t, float time = 0f) {
			foreach (Transform child in t) { 
				Object.Destroy(child.gameObject, time);
			}
		}

		public static Vector2 ToVector2(this Vector3 vec3) {
			return new Vector2(vec3.x, vec3.z);
		}
		public static Vector2 ToVector3(this Vector2 vec2) {
			return new Vector3(vec2.x, 0, vec2.y);
		}

		public static Vector3 Flat(this Vector3 vec3) {
			return new Vector3(vec3.x, 0, vec3.z);
		}
		
		public static Vector3Int ToVector3Int(this Vector3 vec3) {
			return new Vector3Int((int) vec3.x, (int) vec3.y, (int) vec3.z);
		}



	}

}