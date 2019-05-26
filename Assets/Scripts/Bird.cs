using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    public delegate void OnHit(Collision2D collision);
    public OnHit OnHittingSurface;
    private bool HasSentHitInformation = false;

    // Use this for initialization
    void Start()
    {
        //trailrenderer is not visible until we throw the bird
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        //no gravity at first
        GetComponent<Rigidbody2D>().isKinematic = true;
        //make the collider bigger to allow for easy touching
        GetComponent<CircleCollider2D>().radius = Constants.BirdColliderRadiusBig;
        State = BirdState.BeforeThrown;
    }


    /*
    void FixedUpdate()
    {
        //if we've thrown the bird
        //and its speed is very small
        if (State == BirdState.Thrown &&
            GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.MinVelocity)
        {
            //destroy the bird after 2 seconds
            StartCoroutine(DestroyAfter(0));
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //avoid double calls when bird hits two objects in the same frame
        if (HasSentHitInformation)
            return;

        OnHittingSurface(collision);
        Destroy(gameObject);
        HasSentHitInformation = true;
    }

    public void OnThrow()
    {
        //play the sound
        GetComponent<AudioSource>().Play();
        //show the trail renderer
        GetComponent<TrailRenderer>().enabled = true;
        //allow for gravity forces
        GetComponent<Rigidbody2D>().isKinematic = false;
        //make the collider normal size
        GetComponent<CircleCollider2D>().radius = Constants.BirdColliderRadiusNormal;
        State = BirdState.Thrown;
    }

    public BirdState State
    {
        get;
        private set;
    }
}
