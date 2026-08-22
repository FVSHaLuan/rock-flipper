using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesLookAtMovingDirection : ParticleManipulator
{
    [SerializeField]
    private TransformVector transformVector = TransformVector.Up;
    [SerializeField]
    private float factor = -1;
    [SerializeField]
    private float addition = 0;
    [SerializeField]
    private bool useParticleVelocity;

    protected override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
    {
        var cd = GetCustomData(ParticleSystemCustomData.Custom1);
        var lastPos = (Vector2)cd;
        var currentPos = (Vector2)particle.position;

        ///
        Vector2 velocity = useParticleVelocity ? (Vector2)particle.velocity : (currentPos - lastPos);
        particle.rotation = GetRotation(velocity);

        ///
        if (!useParticleVelocity)
        {
            SetCustomData(new Vector4(particle.position.x, particle.position.y, 0, 0), ParticleSystemCustomData.Custom1);
        }

        ///
        return particle;
    }

    private float GetRotation(Vector2 velocityVector)
    {
        ///
        var particleVector = GetParticleVector();

        ///
        return Vector2.SignedAngle(particleVector, velocityVector) * factor + addition;
    }

    private Vector2 GetParticleVector()
    {
        switch (transformVector)
        {
            case TransformVector.Up:
                return Vector2.up;
            case TransformVector.Right:
                return Vector2.right;
            case TransformVector.Forward:
                throw new System.ArgumentException("only Up or Right vector are valid");
            default:
                throw new System.NotImplementedException();
        }
    }
}
