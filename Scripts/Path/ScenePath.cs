using System;
using UnityEngine;

namespace MiniSceneManager {
	[Serializable]
	public struct ScenePath : IEquatable<ScenePath> {
		[SerializeField] private ScenePathReference _reference;
		[SerializeField] private string _value;

		public readonly string path => _reference.path ?? _value;

		public ScenePath(ScenePathReference path) {
			_reference = path;
			_value = default;
		}

		public ScenePath(in string path) {
			_reference = default;
			_value = path;
		}

		// MARK: - IEquatable

		public readonly bool Equals(ScenePath other) => path == other.path;

		public static implicit operator string(ScenePath field) => field.path;
		public static implicit operator ScenePath(string field) => new ScenePath { _value = field };

		public static bool operator ==(ScenePath lhs, ScenePath rhs) => lhs.Equals(rhs);
		public static bool operator !=(ScenePath lhs, ScenePath rhs) => !lhs.Equals(rhs);

		// MARK: - Override

		public readonly override bool Equals(object other) => other switch {
			ScenePath _other => this.Equals(_other),
			_ => throw new ArgumentException()
		};

		public readonly override int GetHashCode() => (_reference, _value).GetHashCode();
	}
}
