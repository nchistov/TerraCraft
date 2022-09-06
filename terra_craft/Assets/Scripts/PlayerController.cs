using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 60.0f;
    public float jumpForce = 12.0f;

    private Rigidbody2D _body;
    private BoxCollider2D _box;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movment = new Vector2(deltaX * 10, _body.velocity.y);
        _body.velocity = movment;

        Vector3 max = _box.bounds.max;
        Vector2 min = _box.bounds.min;

        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hit != null) {
            grounded = true;
        }

        _body.gravityScale = grounded && deltaX == 0 ? 0 : 1;
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow)) {
            _body.AddForce(Vector2.up * jumpForce * 5, ForceMode2D.Impulse);
        }

        if (!Mathf.Approximately(deltaX, 0)) {
			transform.localScale = new Vector3(Mathf.Sign(deltaX) * -1, 1, -1);
        }
    }
}
