using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Stone_and_Wood_Generator
{
    public class InputTriggerZone : MonoBehaviour
    {
        public enum InputType { Resource, Catalyst }
        public InputType inputType;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            StoneWoodGenerator smelter = GetComponentInParent<StoneWoodGenerator>();
            if (smelter != null)
            {
                smelter.SetPlayerZone(other.gameObject, inputType);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            StoneWoodGenerator smelter = GetComponentInParent<StoneWoodGenerator>();
            if (smelter != null)
            {
                smelter.ClearPlayerZone(other.gameObject);
            }
        }
    }
}
