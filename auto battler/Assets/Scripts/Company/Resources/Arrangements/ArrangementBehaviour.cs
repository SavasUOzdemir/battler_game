using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrangementBehaviour : MonoBehaviour
{
    protected Company company;
    protected CompanyMover companyMover;
    protected BoxCollider collider;

    

    void Awake()
    {
        company = GetComponent<Company>();
        companyMover = GetComponent<CompanyMover>();
        collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        UpdateBannerPosition();
        if (!company.InMelee)
        {
            UpdateColliderSize();
            collider.size = new Vector3(collider.size.x + company.MeleeRange * 2, 0f,
                collider.size.z + company.MeleeRange * 2);
            UpdateColliderCenter();
            UpdateCompanyRotation();
        }
    }

    public abstract void ArrangeModels(Vector3 companyPos, Vector3 direction);
    protected abstract void UpdateBannerPosition();
    protected abstract void UpdateColliderSize();

    protected virtual void UpdateColliderCenter()
    {
        Vector3 sum = Vector3.zero;
        foreach (GameObject model in company.models)
        {
            sum += model.transform.position;
        }

        collider.center = transform.InverseTransformPoint(sum / company.models.Count);
    }

    void UpdateCompanyRotation()
    {
        Vector3 forward = Vector3.Cross(Vector3.up, transform.forward);
        var angleToRotate = Vector3.SignedAngle(forward, companyMover.CurrentCompanyDir, Vector3.up);
        transform.Rotate(0f, angleToRotate, 0f);
    }
}
