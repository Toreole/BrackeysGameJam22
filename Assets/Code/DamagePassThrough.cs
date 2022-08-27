using System.Collections;
using UnityEngine;

namespace Assets.Code
{
    public class DamagePassThrough : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private GameObject pass;

        public void Damage()
         => pass.GetComponent<IDamageable>().Damage();
    }
}