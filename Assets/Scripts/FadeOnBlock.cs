using System.Collections.Generic;
using UnityEngine;

public class FadeOnBlock : MonoBehaviour
{
    public Transform target;
    public LayerMask wallLayer;
    public Material fadeMaterial;

    [Range(0.01f, 20f)] public float fadeSpeed = 8f;

    // lưu renderer + material gốc
    private Dictionary<Renderer, Material[]> originalMats = new Dictionary<Renderer, Material[]>();
    private HashSet<Renderer> currentFaded = new HashSet<Renderer>();

    void Update()
    {
        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position;

        Vector3 dir = (targetPos - camPos).normalized;
        float distance = Vector3.Distance(camPos, targetPos);

        RaycastHit[] hits = Physics.RaycastAll(camPos, dir, distance, wallLayer);

        HashSet<Renderer> newFaded = new HashSet<Renderer>();

        foreach (var hit in hits)
        {
            Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();

            foreach (var r in renderers)
            {
                newFaded.Add(r);

                // nếu chưa từng fade
                if (!originalMats.ContainsKey(r))
                {
                    originalMats[r] = r.materials;
                    ApplyFadeMaterial(r);
                }
            }
        }

        // reset những object không còn bị che
        foreach (var r in currentFaded)
        {
            if (!newFaded.Contains(r))
            {
                RestoreMaterial(r);
            }
        }

        currentFaded = newFaded;
    }

    void ApplyFadeMaterial(Renderer r)
    {
        Material[] mats = new Material[r.materials.Length];

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = fadeMaterial;
        }

        r.materials = mats;
    }

    void RestoreMaterial(Renderer r)
    {
        if (originalMats.ContainsKey(r))
        {
            r.materials = originalMats[r];
            originalMats.Remove(r);
        }
    }
}