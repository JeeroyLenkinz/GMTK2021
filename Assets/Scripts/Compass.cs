using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Compass : MonoBehaviour
{
    [SerializeField]
    private BoolReference isSevered;
    private SpriteRenderer sprite;
    [SerializeField]
    private GameObject human;
    [SerializeField]
    private GameObject ghost;
    public float displayOffset;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSevered.Value) {
            sprite.enabled = true;
            Vector2 humanPos = new Vector2(human.transform.position.x, human.transform.position.y + 1.5f);
            Vector2 ghostPos = ghost.transform.position;
            Vector2 direction = (ghostPos - humanPos).normalized;
            Vector2 stretchedDirection = direction * displayOffset;
            gameObject.transform.position = humanPos + new Vector2(stretchedDirection.x, stretchedDirection.y);
            gameObject.transform.up = direction;
        } else {
            sprite.enabled = false;
        }
    }
}
