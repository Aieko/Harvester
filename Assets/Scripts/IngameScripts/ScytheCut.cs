using UnityEngine;
using EzySlice;

public class ScytheCut : MonoBehaviour
{
    private GameObject objToCut;
    private Material cuttedObjMat;
    [SerializeField] LayerMask whatIsCuttable;
    [SerializeField] GameObject wheatCutParticle;

    private void Update()
    {
        var colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 1, 0.02f), transform.rotation, whatIsCuttable);

        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                cuttedObjMat = collider.GetComponent<MeshRenderer>().material;
                objToCut = collider.gameObject;
                if (objToCut == null) return;

                SlicedHull cuttedObj = Slice(objToCut, cuttedObjMat);

                if (cuttedObj == null) return;

                GameObject cuttedObjectBot = cuttedObj.CreateUpperHull(objToCut, cuttedObjMat);
                var parent = objToCut.transform.parent.gameObject;
                cuttedObjectBot.transform.SetParent(parent.transform);
                cuttedObjectBot.transform.position = objToCut.transform.position;

                if(wheatCutParticle)
                {
                    Instantiate(wheatCutParticle, collider.transform.position, Quaternion.identity);
                }
                

                var wheatScript = cuttedObjectBot.AddComponent<WheatScript>();
                wheatScript.StartRespawn();

                Destroy(objToCut);
                objToCut = null;

            }
        }
    }


    public SlicedHull Slice(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }
}
