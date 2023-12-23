using UnityEngine;

namespace BattlefieldSimulator
{
    public class TestPointInsideHexagon : MonoBehaviour
    {
        [SerializeField] private MeshRenderer mouseMeshRenderer;
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material redMaterial;

        private Hexagon hexagon;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 testPosition = Input.mousePosition;

            mouseMeshRenderer.material = redMaterial;
            if (testPosition.x < hexagon.upperRightCorner.x &&
                testPosition.x > hexagon.upperLeftCorner.x &&
                testPosition.z < hexagon.upperCorner.z &&
                testPosition.z > hexagon.lowerCorner.z)
            {
                // now you are in the box!

                Vector3 dirFromURCorner2UCorner = hexagon.upperCorner - hexagon.upperRightCorner;
                Vector3 dotDirURCorner =
                    UtilsClass.ApplyRotation2VectorXZ(dirFromURCorner2UCorner, hexagon.upperRightCorner, -90);

                Vector3 dirFromUCorner2TestPoint = testPosition - hexagon.upperCorner;
                Vector3 dirFromLCorner2TestPoint = testPosition - hexagon.lowerCorner;


                if (Vector3.Dot(dotDirURCorner, dirFromUCorner2TestPoint) > 0   // upper right corner
                    && Vector3.Dot(dotDirURCorner, dirFromLCorner2TestPoint) < 0) // lower left corner
                {
                    Vector3 dirFromULCorner2UCorner = hexagon.upperCorner - hexagon.upperLeftCorner;
                    Vector3 dotDirULCorner =
                        UtilsClass.ApplyRotation2VectorXZ(dirFromULCorner2UCorner, hexagon.upperLeftCorner, 90);

                    if (Vector3.Dot(dotDirULCorner, dirFromUCorner2TestPoint) > 0   // upper left corner
                        && Vector3.Dot(dotDirULCorner, dirFromLCorner2TestPoint) < 0) // lower right corner
                    {
                        // now you are in the hexagon!
                    }

                }
            }
        }
    }
}