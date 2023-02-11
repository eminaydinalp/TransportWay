using _GAME.__Scripts.Stack;
using DG.Tweening;
using UnityEngine;

namespace _GAME.__Scripts.Package
{
    public class PackageBox : MonoBehaviour
    {
        public Rigidbody rigidbody;
        
        public BoxCollider boxCollider;
        public void Move(Vector3 target, StackController stackController, float duration)
        {
            
            transform.parent = stackController.transform;
               
            transform.DOLocalJump(target, 1f, 1, duration);
            transform.DOScale(Vector3.one, 1f);
            transform.localEulerAngles = Vector3.zero;

            boxCollider.enabled = false;
            
            stackController.stackObjects.Add(this);
        }
    }
}