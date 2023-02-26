/*
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 */
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Pyehouse.Editors
{
	public class RagdollMassAdjuster : EditorWindow
	{
		// Can be anything, make it smaller if you like...
		// just preventing divide by zero
		private const float SMALL_MASS_EPSILON = 0.00001f;
		private static bool TooSmall(float m)
		{
			return System.Math.Abs(m) < SMALL_MASS_EPSILON;
		}
		private static RagdollMassAdjuster editor;
		private static int width = 350;
		private static int height = 300;
		private static int x = 0;
		private static int y = 0;
		private bool noRigidbodies = false;
		private bool currentMassTooSmall = false;
		private float newMass = 20f;
		private GameObject ragdollParent;
		/*
		private void Awake()
		{
			ragdollParent = Selection.activeGameObject;
			Rigidbody[] rigidbodies = ragdollParent.GetComponentsInChildren<Rigidbody>();
			if (rigidbodies != null && rigidbodies.Length > 0)
			{
				float mass = 0f;
				foreach (Rigidbody rigidbody in rigidbodies)
				{
					mass += rigidbody.mass;
				}
				currentMassTooSmall = TooSmall(mass);
				newMass = mass;
				noRigidbodies = false;
			}
			else
			{
				newMass = 20f;
				noRigidbodies = true;
		}
		*/
		[MenuItem("Whumpus/Ragdolls/Mass Adjuster")]
		static void ShowEditor()
		{
			CenterWindow();
		}
		private static void CenterWindow()
		{
			editor = EditorWindow.GetWindow<RagdollMassAdjuster>();
			x = (Screen.currentResolution.width - width) / 2;
			y = (Screen.currentResolution.height - height) / 2;
			editor.position = new Rect(x, y, width, height);
			editor.maxSize = new Vector2(width, height);
			editor.minSize = editor.maxSize;
		}
		private void OnGUI()
		{
			if (currentMassTooSmall)
			{
				EditorGUILayout.HelpBox(string.Format("Current mass of selected ragdoll is too small: {0}", newMass), MessageType.Error);
			}
			else if (noRigidbodies)
			{
				EditorGUILayout.HelpBox("No rigidbodies present in selected GameObject or children.", MessageType.Error);
			}
			else
			{
				EditorStyles.label.wordWrap = true;
				EditorGUILayout.HelpBox("The new mass will be applied, maintaining the same per-object ratio as currently exists.", MessageType.Info);
				EditorGUILayout.HelpBox("So if your head is currently 3 out of a total 20 and you want a total mass of 100, your head will become 15.", MessageType.Info);
				ragdollParent = (GameObject)EditorGUILayout.ObjectField("Ragdoll Parent", ragdollParent, typeof(GameObject), true);
				newMass = EditorGUILayout.FloatField("New Mass for Ragdoll:", newMass);
				if (GUILayout.Button("Update Mass"))
				{
					UpdateMass(newMass);
				}
			}
		}
		private void UpdateMass(float toMass)
		{
			if (ragdollParent == null)
            {
				Debug.LogErrorFormat("No ragdoll parent");

				return;
			}
			Rigidbody[] rigidbodies = ragdollParent.GetComponentsInChildren<Rigidbody>();
			if (rigidbodies == null || rigidbodies.Length < 1)
			{
				Debug.LogErrorFormat("No rigidbodies found on selected object.");
				return;
			}
			float fromMass = 0f;
			foreach (Rigidbody rigidbody in rigidbodies)
			{
				fromMass += rigidbody.mass;
			}
			if (TooSmall(fromMass))
			{
				Debug.LogErrorFormat("Current mass too small: {0}", fromMass);
				return;
			}
			float ratio = toMass / fromMass;
			foreach (Rigidbody rigidbody in rigidbodies)
			{
				rigidbody.mass *= ratio;
				EditorUtility.SetDirty(rigidbody);
				EditorSceneManager.MarkSceneDirty(rigidbody.transform.gameObject.scene);
			}
		}
	}
}