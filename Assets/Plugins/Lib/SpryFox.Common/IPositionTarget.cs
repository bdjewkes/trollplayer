using UnityEngine;
using System;

namespace SpryFox.Common {

    public interface IPositionTarget : IEquatable<IPositionTarget> {
        bool IsValid {
            get;
        }
        Vector3 GetTargetPosition();

        // may return null if this target doesn't have components, or doesn't have T.
        T GetComponent<T>() where T : Component; 
    }

    public class TransformPositionTarget : IPositionTarget {

        public TransformPositionTarget() {
        }
        
        public TransformPositionTarget(Transform target) {
            Assert.IsNotNull(target,
                             "TransformPositionTarget expected a non-null transform");
            Target = target;
        }

        public bool IsValid {
            get { return Target != null; }
        }

        public Vector3 GetTargetPosition() {
            return Target.position;
        }

        public T GetComponent<T>() where T : Component {
            return IsValid ? Target.GetComponent<T>() : null;
        }

        public Transform Target { get; set; }

        public bool Equals(IPositionTarget otherTarget) {
            return otherTarget is TransformPositionTarget
                && (otherTarget as TransformPositionTarget).Target == Target;
        }

        public override bool Equals(object other) {
            return other is IPositionTarget
                && Equals(other as IPositionTarget);
        }

        public override int GetHashCode() {
            return Target.GetHashCode();
        }

        public override string ToString() {
            return IsValid == false ? "NULL" 
                : (Target.gameObject.name + ":" + (Vector2)GetTargetPosition());
        }
    }

    public class StaticPositionTarget : IPositionTarget {

        public StaticPositionTarget(Vector3 target) {
            Target = target;
        }

        public bool IsValid {
            get { return true; }
        }

        public Vector3 GetTargetPosition() {
            return Target;
        }

        // no components on this type
        public T GetComponent<T>() where T : Component { 
            return null; 
        }

        public Vector3 Target { get; set; }

        public bool Equals(IPositionTarget otherTarget) {
            return otherTarget is StaticPositionTarget
                && (otherTarget as StaticPositionTarget).Target == Target;
        }

        public override bool Equals(object other) {
            return other is IPositionTarget
                && Equals(other as IPositionTarget);
        }

        public override int GetHashCode() {
            return Target.GetHashCode();
        }

        public override string ToString() {
            return ((Vector2)Target).ToString();
        }
    }

    public static class TargetExtensions {

        public static IPositionTarget AsTarget(this Transform target) {
            return new TransformPositionTarget(target);
        }

        public static IPositionTarget AsTarget(this Vector3 target) {
            return new StaticPositionTarget(target);
        }

        public static IPositionTarget AsTarget(this Vector2 target) {
            return new StaticPositionTarget(target);
        }
    }
}