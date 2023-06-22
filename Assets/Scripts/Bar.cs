using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public float maxLength = 1f;
    public Vector2 startPos;
    public SpriteRenderer barSpriteRenderer;
    public BoxCollider2D boxCollider2D;
    public HingeJoint2D startJoint;
    public HingeJoint2D endJoint;

    float startJointCurrentLoad = 0;
    float endJointCurrentLoad = 0;

    MaterialPropertyBlock propertyBlock;

    public void UpdateCreatingBar(Vector2 toPosition)
    {
        transform.position = (toPosition + startPos) / 2;

        Vector2 dir = toPosition - startPos;
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float length = dir.magnitude;
        barSpriteRenderer.size = new Vector2(length, barSpriteRenderer.size.y);

        boxCollider2D.size = barSpriteRenderer.size;
    }

    public void UpdateMaterial()
    {
        if(startJoint != null) startJointCurrentLoad = startJoint.reactionForce.magnitude / startJoint.breakForce;
        if(endJoint != null) endJointCurrentLoad = endJoint.reactionForce.magnitude / endJoint.breakForce;

        float maxLoad = Mathf.Max(startJointCurrentLoad, endJointCurrentLoad);

        propertyBlock = new MaterialPropertyBlock();
        barSpriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Load", maxLoad);
        barSpriteRenderer.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        if (Time.timeScale == 1) UpdateMaterial();
    }
}
