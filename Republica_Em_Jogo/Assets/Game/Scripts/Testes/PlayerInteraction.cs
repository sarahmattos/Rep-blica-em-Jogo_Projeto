using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Game.Networking.Data.CustomNetworkData;


namespace Game.Test
{
    public class PlayerInteraction : NetworkBehaviour
    {

        private NetworkVariable<NetworkColor> randomColor = new NetworkVariable<NetworkColor>(
            new NetworkColor
            {
                r = 1,
                g = 1,
                b = 1,
            },
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



        //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private Rigidbody characterRigidBody;
        private MeshRenderer meshRenderer;
        [SerializeField] private float speed;
        [SerializeField] private float jump;





        private void Awake()
        {
            characterRigidBody = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();

        }

        private void OnEnable()
        {
            //randomColor.OnValueChanged += (Color prevColor, Color nextColor) =>
            //{
            //    meshRenderer.material.color = randomColor.Value;
            //};

            randomColor.OnValueChanged += UpdateColor;
        }


        private void FixedUpdate()
        {
            if (!IsOwner) return;

            characterRigidBody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, characterRigidBody.velocity.y, Input.GetAxis("Vertical") * speed);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                characterRigidBody.AddForce(Vector3.up * jump, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                //randomNumber.Value = Random.Range(0, 100);
                //Debug.Log(OwnerClientId +" NUM:"+randomNumber.Value);

                randomColor.Value = new NetworkColor
                {
                    r = Random.Range(0.00f, 1.00f),
                    g = Random.Range(0.00f, 1.00f),
                    b = Random.Range(0.00f, 1.00f),

                };
            }

        }

        private void UpdateColor(NetworkColor previousColor, NetworkColor nextColor)
        {
            meshRenderer.material.color = new Color(nextColor.r, nextColor.g, nextColor.b);

        }

    }


}
