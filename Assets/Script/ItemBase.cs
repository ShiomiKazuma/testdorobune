using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] Item _item;
    [SerializeField] AcitiveType _acitiveType;

    public abstract void Active();

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag.Equals("Player"))
        {
            if(_acitiveType == AcitiveType.Use)
            {
                Active();
                Destroy(this.gameObject);
            }
            else if(_acitiveType == AcitiveType.PickUp)
            {
                //‚±‚±‚ÉƒCƒ“ƒxƒ“ƒgƒŠ‚É“ü‚ê‚éˆ—‚ğ‘‚­
            }
        }
    }

    public enum AcitiveType
    {
        Use,
        PickUp,
    }
}
